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
    }
}
