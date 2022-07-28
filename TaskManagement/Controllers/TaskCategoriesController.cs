using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskManagement.IServices;
using TaskManagement.Models;

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
            return View(await TaskCategoriesService.GetCategories());
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(TaskCategory taskCategory)
        {

        }
    }
}
