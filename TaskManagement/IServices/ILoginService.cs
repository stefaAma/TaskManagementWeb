using TaskManagement.ViewModels.Login;

namespace TaskManagement.IServices
{
    public interface ILoginService
    {
        public Task<bool> Login(Credentials credentials, HttpContext httpContext);
        public Task Logout(HttpContext httpContext);
    }
}
