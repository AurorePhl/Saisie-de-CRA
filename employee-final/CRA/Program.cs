using CRA.DataAccess;
using Microsoft.AspNetCore.Localization;
using Microsoft.EntityFrameworkCore;
using System.Globalization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<IAssignmentRepository, AssignmentRepository>();

builder.Services.AddScoped<IScheduleRepository, ScheduleRepository>();

builder.Services.AddScoped<IEmployeeRepository, EmployeeRepository>();

builder.Services.AddScoped<IAdminRepository,AdminRepository>();

builder.Services.AddScoped<ITimeSlotRepository, TimeSlotRepository>();

builder.Services.AddScoped<IPeriodRepository, PeriodRepository>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "EmployeeRoute",
    pattern: "{controller=HomeEmployee}/{action=Index}/{id?}",
    new { controller = "HomeEmployee", action = "Index" });

app.MapControllerRoute(
    name: "AdminRoute",
    pattern: "{controller=HomeAdmin}/{action=Index}/{id?}",
    new { controller = "HomeAdmin", action = "Index" });




app.Run();
