using Microsoft.AspNetCore.Mvc;

namespace TaskManagement.Controllers
{
    public class SignUpController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
