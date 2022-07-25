using TaskManagement.Models;
using TaskManagement.ViewModels.DailyTasks;

namespace TaskManagement.IServices
{
    public interface IDailyTasksService
    {
        public Task<IEnumerable<DailyTask>> GetTasksByUserAndDate(string username, string date);
        public Report GenerateReport(IEnumerable<DailyTask> tasks);
        public Task<bool> CreateTask(DailyTask task, string username);
    }
}
