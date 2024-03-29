﻿using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace TaskManagement.Models
{
    public class DailyTask
    {
        [Key]
        public Guid Id { get; set; }
        [Required]
        public string Name { get; set; } = "";
        [Required]
        public string Date { get; set; } = "";
        public string? Description { get; set; }
        [Required]
        [DisplayName("Duration (1 ~ 4)")]
        public int Duration { get; set; }
        [Required]
        public float Effort { get; set; }
        [Required]
        public bool IsCompleted { get; set; }

        public Guid UserId { get; set; }
        public User User { get; set; }
        [DisplayName("Category")]
        public Guid CategoryId { get; set; }
        public TaskCategory Category { get; set; } 
    }
}
