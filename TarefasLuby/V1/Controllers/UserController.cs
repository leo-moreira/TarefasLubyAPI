using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using TarefasLuby.V1.Models;
using TarefasLuby.V1.Repositories.Contracts;

namespace TarefasLuby.V1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly ITokenRepository _tokenRepository;
        private readonly UserManager<AppUser> _userManager;

        public StringBuilder StringBuilder { get; private set; }

        public UserController(IUserRepository userRepository, ITokenRepository tokenRepository, SignInManager<AppUser> signInManager, UserManager<AppUser> userManager)
        {
            _userRepository = userRepository;
            _signInManager = signInManager;
            _userManager = userManager;
            _tokenRepository = tokenRepository;
        }

        [HttpPost("login")]
        public ActionResult Login([FromBody] User user)
        {
            if (ModelState.IsValid)
            {
                AppUser appUser = _userRepository.GetUser(user.Username, user.Password);
                if (appUser != null)
                {
                    return GenerateToken(appUser);
                }
                else
                {
                    return NotFound("User not found");
                }

            }
            else
            {
                return UnprocessableEntity(ModelState);
            }
        }

        [HttpPost("renew")]
        public ActionResult Renew([FromBody]TokenDTO tokenDTO)
        {
            var refreshTokenDb = _tokenRepository.GetToken(tokenDTO.RefreshToken);

            if(refreshTokenDb == null)
                return NotFound();

            refreshTokenDb.Refreshed = DateTime.Now;
            refreshTokenDb.Used = true;
            _tokenRepository.Refresh(refreshTokenDb);

            var user = _userRepository.GetUser(refreshTokenDb.UserId);

            return GenerateToken(user);
        }

        [HttpPost("")]
        public ActionResult CreateUser([FromBody] User user)
        {
            if (ModelState.IsValid)
            {
                AppUser appUser = new AppUser();
                appUser.UserName = user.Username;
                var result = _userManager.CreateAsync(appUser, user.Password).Result;

                if (!result.Succeeded)
                {
                    StringBuilder sb = new StringBuilder();
                    foreach (var error in result.Errors)
                    {
                        sb.AppendLine(error.Description);
                    }
                    return UnprocessableEntity(sb);
                }

                return Ok(appUser);
            }
            else
            {
                return UnprocessableEntity(ModelState);
            }
        }

        public TokenDTO BuildToken(AppUser appUser)
        {
            var claims = new[]
            {
                new Claim(Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames.UniqueName, appUser.UserName),
                new Claim(Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames.Sub, appUser.Id)
            };
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("tarefas-luby-07-2020-avaliacao"));
            var sign = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var exp = DateTime.UtcNow.AddHours(1);

            JwtSecurityToken token = new JwtSecurityToken(
                issuer: null,
                audience: null,
                claims: claims,
                expires: exp,
                signingCredentials: sign);

            var tokenString = new JwtSecurityTokenHandler().WriteToken(token);

            var refreshToken = Guid.NewGuid().ToString();
            var expRefreshToken = DateTime.UtcNow.AddHours(2);

            var tokenDto = new TokenDTO
            {
                Token = tokenString,
                Expiration = exp,
                RefreshToken = refreshToken,
                ExpirationRefreshToken = expRefreshToken
            };

            return tokenDto;
        }

        private ActionResult GenerateToken(AppUser appUser)
        {
            var token = BuildToken(appUser);

            var tokenModel = new Token()
            {
                Expiration = token.Expiration,
                RefreshToken = token.RefreshToken,
                ExpirationRefreshToken = token.ExpirationRefreshToken,
                User = appUser,
                Created = DateTime.Now,
                Used = false
            };

            _tokenRepository.Create(tokenModel);
            return Ok(token);
        }
    }
}
