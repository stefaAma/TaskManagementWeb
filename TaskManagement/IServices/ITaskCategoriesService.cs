using TaskManagement.Models;

namespace TaskManagement.IServices
{
    public interface ITaskCategoriesService
    {
        public Task<IEnumerable<TaskCategory>> GetCategories();
        public Task<bool> CreateTaskCategory(TaskCategory category);
    }
}
