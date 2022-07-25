using TaskManagement.Data;
using Microsoft.EntityFrameworkCore;
using TaskManagement.Data.StaticData;
using TaskManagement.IServices;
using TaskManagement.Services;
using System.Security.Claims;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<TaskManagementContext>(options => options.UseNpgsql(
    builder.Configuration.GetConnectionString(Constant.PostreConnectionString)));
// Authentication setup
builder.Services.AddAuthentication(Constant.AuthenticationScheme).AddCookie(Constant.AuthenticationScheme,
    options =>
    {
        options.Cookie.Name = Constant.CookieName;
        options.LoginPath = "/Login";
        options.AccessDeniedPath = "/Login/AccessDenied";
    });
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminAccount", 
        policy => { 
            policy.RequireClaim(ClaimTypes.Role, "Admin");
            policy.RequireClaim(ClaimTypes.Name);
        });
    options.AddPolicy("BasicUserAccount",
        policy => {
            policy.RequireClaim(ClaimTypes.Role, new string[2] {
                "Admin",
                "Normal User"
            });
            policy.RequireClaim(ClaimTypes.Name);
        });
});
builder.Services.AddScoped<ILoginService, LoginService>();
builder.Services.AddScoped<IDailyTasksService, DailyTasksService>();
builder.Services.AddScoped<ITaskCategoriesService, TaskCategoriesService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
