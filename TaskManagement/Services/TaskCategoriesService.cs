using Microsoft.EntityFrameworkCore;
using TaskManagement.Data;
using TaskManagement.IServices;
using TaskManagement.Models;

namespace TaskManagement.Services
{
    public class TaskCategoriesService : ITaskCategoriesService
    {
        private TaskManagementContext TaskManagementContext { get; set; }

        public TaskCategoriesService(TaskManagementContext taskManagementContext)
        {
            TaskManagementContext = taskManagementContext;
        }

        public async Task<IEnumerable<TaskCategory>> GetCategories()
        {
            return await TaskManagementContext
                .TaskCategories
                .OrderBy(tc => tc.Name) 
                .ToListAsync();
        }

        public async Task<bool> CreateTaskCategory(TaskCategory category)
        {
            if (category == null || string.IsNullOrEmpty(category.Name) 
                || (category.Effort != 1 && category.Effort != 1.25 && category.Effort != 1.5
                && category.Effort != 1.75 && category.Effort != 2))
                return false;
            // convert hex color to rgba format
        }
    }
}
