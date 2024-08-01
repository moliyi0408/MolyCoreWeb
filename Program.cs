using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using MolyCoreWeb.Datas;
using MolyCoreWeb.Models.DTOs;
using MolyCoreWeb.Repositorys;
using MolyCoreWeb.Services;
using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// SQLite configuration
// using Microsoft.EntityFrameworkCore.Sqlite
builder.Services.AddDbContext<WebDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("WebDbContext")));

builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IUserAuthenticationService, UserAuthenticationService>();
builder.Services.AddScoped<IUserProfileService, UserProfileService>();
builder.Services.AddScoped<IStockService, StockService>();

//LINE API
builder.Services.AddHttpClient<ILineNotifyService, LineNotifyService>(client =>
{
    client.BaseAddress = new Uri("https://notify-api.line.me/");
    client.DefaultRequestHeaders.Add("Authorization", $"Bearer {builder.Configuration["LineNotify:AccessToken"]}");
});

builder.Services.AddHttpClient(); 
builder.Services.AddScoped<IDownloadService, DownloadService>();

builder.Services.AddScoped<IDataTableService, DataTableService>();

builder.Services.AddControllers()
        .AddJsonOptions(opts => {
            opts.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
        });

//automapper configuration
builder.Services.AddAutoMapper(typeof(Program));

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
            .AddCookie(options =>
            {
                options.LoginPath = "/User/Login";
                options.LogoutPath = "/User/Logout";
            });

// 注册 IHttpContextAccessor
builder.Services.AddHttpContextAccessor();
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
