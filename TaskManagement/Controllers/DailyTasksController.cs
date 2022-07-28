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
            var dailyTasks = await DailyTasksService.GetTasksByUserAndDate(User.Identity.Name, dailyTask.Date);
            if (DailyTasksService.CheckTotalEffortOverflow(dailyTasks))
            {
                ModelState.AddModelError("", "The effort is already beyond the limit! Please " +
                    "try to edit, delete or mark as completed other tasks");
                ViewData["date"] = dailyTask.Date;
                ViewData["categories"] = await TaskCategoriesService.GetCategories();
                return View(dailyTask);
            }
            if (!await DailyTasksService.CreateTask(dailyTask, User.Identity.Name))
                return BadRequest();
            return RedirectToAction("Index", new {date = dailyTask.Date});
        }

        public async Task<IActionResult> Edit(Guid Id)
        {
            
            var dailyTask = await DailyTasksService.GetTaskById(Id);
            ViewData["categories"] = await TaskCategoriesService.GetCategories();
            return View(dailyTask);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(DailyTask dailyTask)
        {
            var dailyTasks = await DailyTasksService.GetTasksByUserAndDate(User.Identity.Name, dailyTask.Date);
            if (!dailyTask.IsCompleted && DailyTasksService.CheckTotalEffortOverflow(dailyTasks))
            {
                var newDailyTasks = DailyTasksService.ReplaceTask(dailyTasks, dailyTask);
                if (DailyTasksService.CheckTotalEffortOverflow(newDailyTasks))
                {
                    ModelState.AddModelError("", "The effort is already beyond the limit! Please " +
                    "try to edit, delete or mark as completed other tasks");
                    ViewData["categories"] = await TaskCategoriesService.GetCategories();
                    return View(dailyTask);
                }
            }
            if (!await DailyTasksService.EditTask(dailyTask, User.Identity.Name))
                return BadRequest();
            return RedirectToAction("Index", new { date = dailyTask.Date });
        }

        public async Task<IActionResult> Delete(Guid Id)
        {
            var dailyTask = await DailyTasksService.GetTaskById(Id);
            ViewData["categories"] = await TaskCategoriesService.GetCategories();
            return View(dailyTask);
        }

        [HttpPost]
        [ActionName("Delete")]
        public async Task<IActionResult> DeletePost(Guid Id)
        {
            var dailyTask = await DailyTasksService.GetTaskById(Id);
            if (!await DailyTasksService.DeleteTask(dailyTask))
                return BadRequest();
            return RedirectToAction("Index", new { date = dailyTask.Date });
        }
    }
}
