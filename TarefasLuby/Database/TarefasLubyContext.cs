using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TarefasLuby.V1.Models;

namespace TarefasLuby.Database
{
    public class TarefasLubyContext : IdentityDbContext<AppUser>
    {
        public TarefasLubyContext(DbContextOptions<TarefasLubyContext> options) : base(options)
        {
        }

        public DbSet<Task> Tasks{ get; set; }
        public DbSet<Token> Tokens{ get; set; }
    }
}
