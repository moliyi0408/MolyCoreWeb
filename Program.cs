using Microsoft.EntityFrameworkCore;
using MolyCoreWeb.Datas;
using MolyCoreWeb.Models.DTOs;
using MolyCoreWeb.Repositorys;
using MolyCoreWeb.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// SQLite configuration
// using Microsoft.EntityFrameworkCore.Sqlite
builder.Services.AddDbContext<WebDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("WebDbContext")));


builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
//builder.Services.AddScoped<IService<UserDto>, UserService>();
builder.Services.AddScoped<IUserService, UserService>();
//automapper configuration
builder.Services.AddAutoMapper(typeof(Program));

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