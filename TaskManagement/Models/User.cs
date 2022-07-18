using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TaskManagement.Models
{
    public class User
    {
        [Key]
        //[DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public Guid Id { get; set; }
        [Required]
        public string Username { get; set; } = "";
        [Required]
        [MinLength(4)]
        public string Password { get; set; } = "";
        [Required]
        public string Role { get; set; } = "";

        public ICollection<DailyTask> DailyTasks;
    }
}
