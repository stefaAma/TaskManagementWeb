using Microsoft.AspNetCore.Authentication;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using TaskManagement.Data;
using TaskManagement.Data.StaticData;
using TaskManagement.IServices;
using TaskManagement.ViewModels.Login;

namespace TaskManagement.Services
{
    public class LoginService : ILoginService
    {
        private TaskManagementContext TaskManagementContext { get; set; }

        public LoginService(TaskManagementContext taskManagementContext)
        {
            TaskManagementContext = taskManagementContext;
        }

        public async Task<bool> Login(Credentials credentials, HttpContext httpContext)
        {
            var user = await TaskManagementContext
                .Users
                .Where(u => u.Username == credentials.Username && u.Password == credentials.Password)
                .FirstOrDefaultAsync();

            if (user == null)
                return false;

            var claims = new List<Claim>() {
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Role, user.Role)
            };

            var identity = new ClaimsIdentity(claims, Constant.AuthenticationScheme);
            var claimsPrincipal = new ClaimsPrincipal(identity);

            await httpContext.SignInAsync(Constant.AuthenticationScheme, claimsPrincipal);

            return true;
        }
    }
}
