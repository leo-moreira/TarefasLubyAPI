using TarefasLuby.V1.Models;

namespace TarefasLuby.V1.Repositories.Contracts
{
    public interface IUserRepository
    {
        void CreateUser(AppUser user, string password);

        AppUser GetUser(string username, string password);
        AppUser GetUser(string id);
    }
}
