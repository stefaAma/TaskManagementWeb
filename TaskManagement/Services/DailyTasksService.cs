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
                    report.TotalEffort = TotalEffortExpression(report.TotalEffort, task);
            }
            int tasksNum = tasks.Count();
            foreach (var category in report.Categories)
            {
                report.Categories[category.Key] = (category.Value / tasksNum) * 100;
            }
            report.PercentageCompletion = (report.PercentageCompletion / tasksNum) * 100;

            return report;
        }

        public async Task<bool> CreateTask(DailyTask task, string username)
        {
            if (!CheckTaskValues(task)) 
                return false;
            User? user = await TaskManagementContext.Users.Where(u => u.Username == username)
                .FirstOrDefaultAsync();
            task.User = user;
            TaskManagementContext.DailyTasks.Add(task);
            int changes = await TaskManagementContext.SaveChangesAsync();
            if (changes != 1)
                return false;
            return true;
        }

        private float TotalEffortExpression(float totalEffort, DailyTask task)
        {
            if (task.Category == null)
                task.Category = TaskManagementContext.TaskCategories.Find(task.CategoryId);
            return totalEffort + (task.Duration * (task.Effort + task.Category.Effort - 1));
        }

        public bool CheckTotalEffortOverflow(IEnumerable<DailyTask> tasks)
        {
            bool overflow = false;
            float totalEffort = 0;
            foreach (var task in tasks)
            {
                totalEffort = TotalEffortExpression(totalEffort, task);
                if (totalEffort > 18)
                    overflow = true;
            }
            return overflow;
        }

        public async Task<DailyTask> GetTaskById(Guid id)
        {
            var dailyTask = await TaskManagementContext.DailyTasks.FindAsync(id);
            if (dailyTask == null)
                throw new KeyNotFoundException("Task cannot be found!");
            return dailyTask;
        }

        public IEnumerable<DailyTask> ReplaceTask(IEnumerable<DailyTask> dailyTasks, DailyTask dailyTask)
        {
            var tasksList = dailyTasks.ToList();
            int id = tasksList.FindIndex(task => task.Id == dailyTask.Id);
            tasksList.RemoveAt(id);
            tasksList.Add(dailyTask);
            return tasksList;
        }

        private bool CheckTaskValues(DailyTask task)
        {
            if (task == null || string.IsNullOrEmpty(task.Name) || task.Duration < 1 ||
                task.Duration > 4 || (task.Effort != 1 && task.Effort != 1.5 && task.Effort != 2))
                return false;
            return true;
        } 

        public async Task<bool> EditTask(DailyTask task, string username)
        {
            if (!CheckTaskValues(task))
                return false;
            User? user = await TaskManagementContext.Users.Where(u => u.Username == username)
                .FirstOrDefaultAsync();
            task.User = user;
            TaskManagementContext.Entry<DailyTask>(TaskManagementContext.DailyTasks.Find(task.Id)).State = EntityState.Detached;
            TaskManagementContext.DailyTasks.Update(task);
            int changes = await TaskManagementContext.SaveChangesAsync();
            if (changes != 1)
                return false;
            return true;
        }

        public async Task<bool> DeleteTask(DailyTask task)
        {
            TaskManagementContext.DailyTasks.Remove(task);
            int changes = await TaskManagementContext.SaveChangesAsync();
            if (changes != 1)
                return false;
            return true;
        }

    }
}
