using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskManagement.IServices;
using TaskManagement.Models;
using TaskManagement.ViewModels.TaskCategories;

namespace TaskManagement.Controllers
{
    [Authorize(Policy = "AdminAccount")]
    public class TaskCategoriesController : Controller
    {
        private ITaskCategoriesService TaskCategoriesService { get; set; }
        public TaskCategoriesController(ITaskCategoriesService taskCategoriesService)
        {
            TaskCategoriesService = taskCategoriesService;
        }

        public async Task<IActionResult> Index()
        {
            var categories = await TaskCategoriesService.GetCategories();
            Report report = TaskCategoriesService.GenerateReport(categories);
            ViewData["report"] = report;
            return View(categories);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(TaskCategory taskCategory)
        {
            if (!await TaskCategoriesService.CreateTaskCategory(taskCategory))
                return BadRequest(ModelState);
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Edit(Guid Id)
        {
            var taskCategory = await TaskCategoriesService.GetTaskCategoryById(Id);
            return View(taskCategory);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(TaskCategory category)
        {
            if(! await TaskCategoriesService.EditTaskCategory(category))
                return BadRequest(ModelState);
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Delete(Guid Id)
        {
            var taskCategory = await TaskCategoriesService.GetTaskCategoryById(Id);
            return View(taskCategory);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeletePost(Guid Id)
        {
            if (!await TaskCategoriesService.DeleteTaskCategory(Id))
                return BadRequest(ModelState);
            return RedirectToAction("Index");
        }
    }
}
