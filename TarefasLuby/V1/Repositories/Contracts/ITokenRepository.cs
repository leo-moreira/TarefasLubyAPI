using TarefasLuby.V1.Models;

namespace TarefasLuby.V1.Repositories.Contracts
{
    public interface ITokenRepository
    {
        void Create(Token token);

        Token GetToken(string refreshToken);

        void Refresh(Token token);
    }
}
