// <copyright file="Program.cs" company="ClockWorkTeddy">
// Written by ClockWorkTeddy.
// </copyright>

using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SadSchool.Controllers.Contracts;
using SadSchool.Models;
using SadSchool.Services;
using SadSchool.Services.ApiServices;
using SadSchool.Services.Cache;
using SadSchool.Services.ClassBook;
using StackExchange.Redis;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

var connStrSad = "Data Source=.\\sad_school.db";
var connStrAuth = "Data Source=.\\auth.db";

builder.Services.AddDbContext<SadSchoolContext>(_ => _.UseSqlite(connStrSad).UseLazyLoadingProxies());
builder.Services.AddDbContext<AuthDbContext>(_ => _.UseSqlite(connStrAuth));

builder.Services.AddIdentity<IdentityUser, IdentityRole>(opts =>
{
    opts.Password.RequiredLength = 1;
    opts.Password.RequireNonAlphanumeric = false;
    opts.Password.RequireLowercase = false;
    opts.Password.RequireUppercase = false;
    opts.Password.RequireDigit = false;
}).AddEntityFrameworkStores<AuthDbContext>();

SelectCacheSource(builder);

builder.Services.AddSingleton<INavigationService, NavigationService>();
builder.Services.AddTransient<IAuthService, AuthService>();
builder.Services.AddTransient<IClassBookService, ClassBookService>();
builder.Services.AddTransient<IMarksAnalyticsService, MarksAnalyticsService>();

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
app.UseAuthentication();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();

void SelectCacheSource(WebApplicationBuilder builder)
{
    try
    {
        builder.Services.AddSingleton<IConnectionMultiplexer>(ConnectionMultiplexer.Connect("localhost"));
        builder.Services.AddScoped<ICacheService, RedisCacheService>();
    }
    catch
    {
        builder.Services.AddScoped<ICacheService, MemoryCacheService>();
    }
}
