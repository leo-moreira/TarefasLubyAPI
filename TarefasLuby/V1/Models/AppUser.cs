using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace TarefasLuby.V1.Models
{
    public class AppUser : IdentityUser
    {
        [ForeignKey("UserId")]
        public ICollection<Task> Tasks { get; set; }

        [ForeignKey("UserId")]
        public ICollection<Token> Tokens { get; set; }
    }
}
