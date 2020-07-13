using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TarefasLuby.V1.Models
{
    public class Task
    {
        [Key]
        public int Id { get; set; }
        public string Description { get; set; }
        public bool Concluded { get; set; }
        public string UserId { get; set; }
    }
}
