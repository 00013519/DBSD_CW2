using ScooterManagementApp.DAL.Infrastructure;
using Microsoft.EntityFrameworkCore;
using ScooterManagementApp.DAL.Repositories;

var builder = WebApplication.CreateBuilder(args);


IConfiguration config = builder.Configuration;
string connStr = config.GetConnectionString("Scooter")
    .Replace("|DataDirectory|", builder.Environment.ContentRootPath);

builder.Services.AddDbContext<ScooterDbContext>(options =>
    options.UseSqlServer(connStr)
);

builder.Services.AddSingleton<IEmployeeRepository>(
    x => new EmployeeStoredProcDapperRepository(connStr)
    );
// Add services to the container.
builder.Services.AddControllersWithViews();

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

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
