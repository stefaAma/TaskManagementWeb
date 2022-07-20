using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace TaskManagement.Controllers
{
    [Authorize(Policy = "AdminAccount")]
    public class TaskCategoriesController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
