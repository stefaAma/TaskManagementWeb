using Microsoft.EntityFrameworkCore;
using System.Globalization;
using TaskManagement.Data;
using TaskManagement.IServices;
using TaskManagement.Models;

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
            DateTime dateTime = DateTime.ParseExact(date, "yyyy-mm-dd", CultureInfo.InvariantCulture);
            var dailyTasks = await TaskManagementContext.DailyTasks
                .Include(dt => dt.User)
                .Where(dt => dt.User.Username == username && DateTime.Compare(dt.Date, dateTime) == 0)
                .ToListAsync();

            return dailyTasks;
        }

    }
}
