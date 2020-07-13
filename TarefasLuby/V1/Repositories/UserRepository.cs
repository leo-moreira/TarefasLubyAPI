using Microsoft.AspNetCore.Identity;
using System;
using System.Text;
using TarefasLuby.V1.Models;
using TarefasLuby.V1.Repositories.Contracts;

namespace TarefasLuby.V1.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly UserManager<AppUser> _userManager;

        public UserRepository(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }

        public void CreateUser(AppUser user, string password)
        {
            var result = _userManager.CreateAsync(user, password).Result;
            if (!result.Succeeded)
            {
                StringBuilder sb = new StringBuilder();
                foreach(var error in result.Errors)
                {
                    sb.Append(error.Description);
                }
                throw new Exception($"User not created! {sb}");
            }
        }

        public AppUser GetUser(string username, string password)
        {
            var user = _userManager.FindByNameAsync(username).Result;
            if(_userManager.CheckPasswordAsync(user, password).Result)
            {
                return user;
            } else
            {
                throw new Exception("User not found");
            }
        }

        public AppUser GetUser(string id)
        {
            return _userManager.FindByIdAsync(id).Result;
        }
    }
}
