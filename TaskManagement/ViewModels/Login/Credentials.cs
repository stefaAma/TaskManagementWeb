using System.ComponentModel.DataAnnotations;

namespace TaskManagement.ViewModels.Login
{
    public class Credentials
    {
        [Required]
        public string Username { get; set; } = "";
        [Required]
        [MinLength(4)]
        public string Password { get; set; } = "";
    }
}
