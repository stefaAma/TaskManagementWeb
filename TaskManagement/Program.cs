using TaskManagement.Data;
using Microsoft.EntityFrameworkCore;
using TaskManagement.Data.StaticData;
using TaskManagement.IServices;
using TaskManagement.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddEntityFrameworkNpgsql().AddDbContext<TaskManagementContext>(options => options.UseNpgsql(
    builder.Configuration.GetConnectionString(Constant.PostreConnectionString)));
// Authentication setup
builder.Services.AddAuthentication(Constant.AuthenticationScheme).AddCookie(Constant.AuthenticationScheme,
    options =>
    {
        options.Cookie.Name = Constant.AuthenticationScheme;
    });
builder.Services.AddSingleton<ILoginService, LoginService>();

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
