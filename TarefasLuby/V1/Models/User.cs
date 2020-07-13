using System.ComponentModel.DataAnnotations;

namespace TarefasLuby.V1.Models
{
    public class User
    {
        [Required]
        [MinLength(3, ErrorMessage = "Username should have more than 3 characters")]
        public string Username { get; set; }
        [Required]
        [MinLength(3, ErrorMessage = "Password should have more than 3 characters")]
        public string Password { get; set; }
    }
}
