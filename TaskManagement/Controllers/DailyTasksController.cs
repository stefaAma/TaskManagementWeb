using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskManagement.IServices;

namespace TaskManagement.Controllers
{
    [Authorize(Policy = "BasicUserAccount")]
    public class DailyTasksController : Controller
    {

        private IDailyTasksService DailyTasksService { get; set; }

        public DailyTasksController(IDailyTasksService dailyTasksService)
        {
            DailyTasksService = dailyTasksService;
        }

        public async Task<IActionResult> Index(string date)
        {
            var dailyTasks = await DailyTasksService.GetTasksByUserAndDate(User.Identity.Name, date);
            // manage the case in which there are no daily tasks!
            var report = DailyTasksService.GenerateReport(dailyTasks);
            ViewData["date"] = date;
            ViewData["report"] = report;
            if (report.TotalEffort > 18)
            {
                ModelState.AddModelError("", "Too much effort, please remove one or more tasks!");
            }
                
            return View(dailyTasks);
        }
    }
}
