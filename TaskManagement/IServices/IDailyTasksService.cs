using TaskManagement.Models;
using TaskManagement.ViewModels.DailyTasks;

namespace TaskManagement.IServices
{
    public interface IDailyTasksService
    {
        public Task<IEnumerable<DailyTask>> GetTasksByUserAndDate(string username, string date);
        public Report GenerateReport(IEnumerable<DailyTask> tasks);
        public Task<bool> CreateTask(DailyTask task, string username);
        public bool CheckTotalEffortOverflow(IEnumerable<DailyTask> tasks);
        public Task<DailyTask> GetTaskById(Guid id);
        public IEnumerable<DailyTask> ReplaceTask(IEnumerable<DailyTask> dailyTasks, DailyTask dailyTask);
        public Task<bool> EditTask(DailyTask task, string username);
    }
}
