using project_ver1.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Builder;
using static project_ver1.Controllers.HomeController;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
//builder.Services.AddControllersWithViews();
builder.Services.AddControllersWithViews();
var connectionString = builder.Configuration.GetConnectionString("AppDbContext");
builder.Services.AddDbContext<HomeDbContext>(x => x.UseSqlServer(connectionString));
builder.Services.AddDbContext<RoomDbContext>(y => y.UseSqlServer(connectionString));
builder.Services.AddSession();
builder.Services.AddScoped<EmailService>();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.UseSession();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
