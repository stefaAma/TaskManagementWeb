using Microsoft.EntityFrameworkCore;
using TaskManagement.Models;

namespace TaskManagement.Data
{
    public class TaskManagementContext : DbContext
    {
        public DbSet<DailyTask> DailyTasks { get; set; }
        public DbSet<TaskCategory> TaskCategories { get; set; }
        public DbSet<User> Users { get; set; }

        public TaskManagementContext(DbContextOptions<TaskManagementContext> dbContextOptions): base(dbContextOptions)
        {}

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TaskCategory>()
                .HasIndex(tc => tc.Name)
                .IsUnique();

            modelBuilder.Entity<User>()
                .HasIndex(u => u.Username)
                .IsUnique();

            modelBuilder.Entity<DailyTask>()
                .HasOne(dt => dt.User)
                .WithMany(u => u.DailyTasks)
                .HasForeignKey(dt => dt.UserId);

            modelBuilder.Entity<DailyTask>()
                .HasOne(dt => dt.Category)
                .WithMany(c => c.DailyTasks)
                .HasForeignKey(dt => dt.CategoryId);
        }
    }
}
