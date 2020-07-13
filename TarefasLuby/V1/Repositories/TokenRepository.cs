using System.Linq;
using TarefasLuby.Database;
using TarefasLuby.V1.Models;
using TarefasLuby.V1.Repositories.Contracts;

namespace TarefasLuby.V1.Repositories
{
    public class TokenRepository : ITokenRepository
    {
        private readonly TarefasLubyContext _context;
        public TokenRepository(TarefasLubyContext context)
        {
            _context = context;
        }
        public void Create(Token token)
        {
            _context.Tokens.Add(token);
            _context.SaveChanges();
        }

        public void Refresh(Token token)
        {
            _context.Tokens.Update(token);
            _context.SaveChanges();
        }

        public Token GetToken(string refreshToken)
        {
            return _context.Tokens.FirstOrDefault(a => a.RefreshToken == refreshToken && a.Used == false);
        }
    }
}
