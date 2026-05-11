using CustomAuthorizationLoginMVC.Database.DataAccess;
using CustomAuthorizationLoginMVC.Domain.Features.Login;
using CustomAuthorizationLoginMVC.Middleware;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging.Abstractions;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<LoginService>();
builder.Services.AddHttpContextAccessor();



Log.Logger = new LoggerConfiguration().WriteTo.Console().CreateLogger();
Log.Information("Application started with connection string: {ConnectionString}", builder.Configuration.GetConnectionString("DefaultConnection"));


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseMiddleware<CustomAuthMiddleware>();
app.UseStaticFiles();

app.UseRouting();



app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Login}/{action=Index}/{id?}");

app.Run();
