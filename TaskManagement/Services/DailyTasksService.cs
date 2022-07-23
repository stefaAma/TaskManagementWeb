using Microsoft.EntityFrameworkCore;
using TaskManagement.Data;
using TaskManagement.IServices;
using TaskManagement.Models;
using TaskManagement.ViewModels.DailyTasks;

namespace TaskManagement.Services
{
    public class DailyTasksService : IDailyTasksService
    {
        private TaskManagementContext TaskManagementContext { get; set; }

        public DailyTasksService(TaskManagementContext taskManagementContext)
        {
            TaskManagementContext = taskManagementContext;
        }

        public async Task<IEnumerable<DailyTask>> GetTasksByUserAndDate(string username, string date)
        {
            //DateTime dateTime = DateTime.ParseExact(date, "yyyy-mm-dd", CultureInfo.InvariantCulture);
            var dailyTasks = await TaskManagementContext.DailyTasks
                .Include(dt => dt.User)
                .Include(dt => dt.Category)
                .Where(dt => dt.User.Username == username && dt.Date.Equals(date))
                .OrderBy(dt => dt.Category.Name)
                .ToListAsync();

            return dailyTasks;
        }

        public Report GenerateReport(IEnumerable<DailyTask> tasks)
        {
            Report report = new Report();

            foreach (var task in tasks)
            {
                if (!report.Categories.ContainsKey(task.Category.Name))
                {
                    report.Categories[task.Category.Name] = 1;
                }
                else
                {
                    report.Categories[task.Category.Name]++;
                }

                if (task.IsCompleted)
                    report.PercentageCompletion++;
                else
                    report.TotalEffort = report.TotalEffort + (task.Duration * (task.Effort + task.Category.Effort - 1));
            }
            int tasksNum = tasks.Count();
            foreach (var category in report.Categories)
            {
                report.Categories[category.Key] = (category.Value / tasksNum) * 100;
            }
            report.PercentageCompletion = (report.PercentageCompletion / tasksNum) * 100;

            return report;
        }

    }
}
