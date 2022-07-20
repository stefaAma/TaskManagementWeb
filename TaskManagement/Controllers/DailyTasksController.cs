using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace TaskManagement.Controllers
{
    [Authorize(Policy = "BasicUserAccount")]
    public class DailyTasksController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
