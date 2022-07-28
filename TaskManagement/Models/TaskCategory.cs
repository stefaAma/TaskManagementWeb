using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace TaskManagement.Models
{
    public class TaskCategory
    {
        [Key]
        public Guid Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public float Effort { get; set; }
        [Required]
        [DisplayName("Rgba Color")]
        public string RgbaColor { get; set; }

        public ICollection<DailyTask> DailyTasks;
    }
}
