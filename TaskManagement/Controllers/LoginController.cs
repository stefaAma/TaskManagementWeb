using Microsoft.AspNetCore.Mvc;
//using TaskManagement.Data;
using TaskManagement.IServices;
using TaskManagement.ViewModels.Login;

namespace TaskManagement.Controllers
{
    public class LoginController : Controller
    {
        private ILoginService LoginService { get; set; }

        public LoginController(ILoginService loginService)
        {
            LoginService = loginService;
        }

        [HttpGet]
        public IActionResult Index()
        {
            string? AccessDeniedError = TempData["Access Denied"] as string;
            if (AccessDeniedError != null)
                ModelState.AddModelError("", AccessDeniedError);
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Index(Credentials credentials)
        {
            if(!ModelState.IsValid || ! await LoginService.Login(credentials, HttpContext))
            {
                ModelState.AddModelError("", "Username or Password are not correct!");
                return View(credentials);
            }

            return Redirect("/Home");
        }

        public async Task<IActionResult> Logout()
        {
            await LoginService.Logout(HttpContext);
            return Redirect("/Home");
        }

        public async Task<IActionResult> AccessDenied()
        {
            await LoginService.Logout(HttpContext);
            TempData["Access Denied"] = "Access Denied!";
            return RedirectToAction("Index");
        }
    }
}
