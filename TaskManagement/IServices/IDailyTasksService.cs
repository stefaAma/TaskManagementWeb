using TaskManagement.Models;

namespace TaskManagement.IServices
{
    public interface IDailyTasksService
    {
        public Task<IEnumerable<DailyTask>> GetTasksByUserAndDate(string username, string date);
    }
}
