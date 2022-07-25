using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskManagement.IServices;
using TaskManagement.Models;
using TaskManagement.ViewModels.DailyTasks;

namespace TaskManagement.Controllers
{
    [Authorize(Policy = "BasicUserAccount")]
    public class DailyTasksController : Controller
    {

        private IDailyTasksService DailyTasksService { get; set; }
        private ITaskCategoriesService TaskCategoriesService { get; set; }

        public DailyTasksController(IDailyTasksService dailyTasksService, ITaskCategoriesService taskCategoriesService)
        {
            DailyTasksService = dailyTasksService;
            TaskCategoriesService = taskCategoriesService;
        }

        public async Task<IActionResult> Index(string date)
        {
            var dailyTasks = await DailyTasksService.GetTasksByUserAndDate(User.Identity.Name, date);
            // manage the case in which there are no daily tasks!
            Report? report = null;
            if(dailyTasks != null && dailyTasks.Count() > 0)
                report = DailyTasksService.GenerateReport(dailyTasks);
            ViewData["date"] = date;
            ViewData["report"] = report;
            if (report != null && report.TotalEffort > 18)
            {
                ModelState.AddModelError("", "Too much effort, please remove one or more tasks!");
            }
                
            return View(dailyTasks);
        }

        public async Task<IActionResult> Create(string date)
        {
            ViewData["date"] = date;
            ViewData["categories"] = await TaskCategoriesService.GetCategories();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(DailyTask dailyTask)
        {
            if (!await DailyTasksService.CreateTask(dailyTask, User.Identity.Name))
                return BadRequest();
            return RedirectToAction("Index", new {date = dailyTask.Date});
        }
    }
}
