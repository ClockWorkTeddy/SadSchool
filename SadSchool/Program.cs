using Microsoft.EntityFrameworkCore;
using SadSchool.Models;
using Azure.Identity;
using Microsoft.AspNetCore.Identity;
using SadSchool.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

var connStrSad = "Data Source=.\\sad_school.db";
var connStrAuth = "Data Source=.\\auth.db";

builder.Services.AddDbContext<SadSchoolContext>(_ => _.UseSqlite(connStrSad));
builder.Services.AddDbContext<AuthDbContext>(_ => _.UseSqlite(connStrAuth));

builder.Services.AddIdentity<IdentityUser, IdentityRole>(opts => {
    opts.Password.RequiredLength = 1;  
    opts.Password.RequireNonAlphanumeric = false;
    opts.Password.RequireLowercase = false; 
    opts.Password.RequireUppercase = false; 
    opts.Password.RequireDigit = false; 
}).AddEntityFrameworkStores<AuthDbContext>();

builder.Services.AddSingleton<INavigationService, NavigationService>();

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
