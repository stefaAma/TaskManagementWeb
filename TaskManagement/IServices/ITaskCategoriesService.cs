using TaskManagement.Models;
using TaskManagement.ViewModels.TaskCategories;

namespace TaskManagement.IServices
{
    public interface ITaskCategoriesService
    {
        public Task<IEnumerable<TaskCategory>> GetCategories();
        public Task<bool> CreateTaskCategory(TaskCategory category);
        public Task<TaskCategory> GetTaskCategoryById(Guid Id);
        public Task<bool> EditTaskCategory(TaskCategory category);
        public Task<bool> DeleteTaskCategory(Guid Id);
        public Report GenerateReport(IEnumerable<TaskCategory> categories);
    }
}
