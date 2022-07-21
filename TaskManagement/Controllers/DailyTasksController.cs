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
            ViewData["date"] = date;
            return View(dailyTasks);
        }
    }
}
