// <copyright file="Program.cs" company="ClockWorkTeddy">
// Written by ClockWorkTeddy.
// </copyright>

using Hangfire;
using Hangfire.AspNetCore;
using Hangfire.SqlServer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MongoDB.Driver;
using SadSchool.Contracts;
using SadSchool.Controllers.GraphQl;
using SadSchool.DbContexts;
using SadSchool.Mappers;
using SadSchool.Services;
using SadSchool.Services.ApiServices;
using SadSchool.Services.Cache;
using SadSchool.Services.ClassBook;
using SadSchool.Services.HangFire;
using SadSchool.Services.Secrets;
using SadSchool.Services.SignalR;
using Serilog;
using StackExchange.Redis;

SetupLogger();

Log.Information("Welcome to Hell.");

var builder = WebApplication.CreateBuilder(args);
SetUpConfiguration(builder);

// Add services to the container.
builder.Services.AddControllersWithViews();

var connStrAuth = "Data Source=.\\auth.db";

builder.Services.AddDbContext<SadSchoolContext>(_ => _.UseSqlServer(builder.Configuration["sad_school_conn_str"]).UseLazyLoadingProxies());
builder.Services.AddDbContext<AuthDbContext>(_ => _.UseSqlite(connStrAuth));

builder.Services.AddIdentity<IdentityUser, IdentityRole>(opts =>
{
    opts.Password.RequiredLength = 1;
    opts.Password.RequireNonAlphanumeric = false;
    opts.Password.RequireLowercase = false;
    opts.Password.RequireUppercase = false;
    opts.Password.RequireDigit = false;
}).AddEntityFrameworkStores<AuthDbContext>();

var secretService = new SecretService(builder.Configuration);

SelectCacheSource(builder, secretService);
SetUpMongo(builder, secretService);

builder.Services.AddHangfire(configuration => configuration
        .SetDataCompatibilityLevel(CompatibilityLevel.Version_180)
        .UseSimpleAssemblyNameTypeSerializer()
        .UseRecommendedSerializerSettings()
        .UseSqlServerStorage(builder.Configuration["hangfire_conn_str"]));

builder.Services
    .AddGraphQLServer()
    .RegisterDbContext<SadSchoolContext>()
    .AddQueryType<Query>() // Register Query class
    .AddMutationType<Mutation>()
    .AddSorting()
    .AddFiltering();

builder.Services.AddRazorPages();
builder.Services.AddSignalR();

builder.Services.AddSingleton<INavigationService, NavigationService>();
builder.Services.AddTransient<IAuthService, AuthService>();
builder.Services.AddTransient<IClassBookService, ClassBookService>();
builder.Services.AddTransient<IMarksAnalyticsService, MarksAnalyticsService>();
builder.Services.AddSingleton<ICommonMapper, CommonMapper>();
builder.Services.AddSingleton<IScheduledLessonMapper, ScheduleLessonMapper>();
builder.Services.AddHangfireServer();

var graphQlKey = builder.Configuration["graphql-key"];

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");

    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHangfireDashboard();

RecurringJob.AddOrUpdate<LoggerJobService>("LoggerJobService", x => x.LogJob(), Cron.Minutely);
RecurringJob.AddOrUpdate<LessonCheckService>("LessonCheckService", x => x.DeleteLessonWithoutDate(), Cron.Minutely);

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();
app.UseAuthentication();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.UseWhen(context => context.Request.Path.StartsWithSegments("/graphql"), appBuilder =>
{
    appBuilder.Use(async (context, next) =>
    {
        var requestGraphQlKey = context.Request.Headers["graphql-key"].FirstOrDefault();

        if (!context!.User!.Identity!.IsAuthenticated && requestGraphQlKey != graphQlKey)
        {
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            await context.Response.WriteAsync("Unauthorized");
            return;
        }

        await next.Invoke();
    });
});

app.MapGraphQL("/graphql");
app.MapRazorPages();
app.MapHub<SignalRChatHub>("/chatHub");
app.MapHub<BlackboardHub>("/blackboardHub");

app.Run();

void SetupLogger()
{
    Log.Logger = new LoggerConfiguration()
        .WriteTo.Console()
        .WriteTo.File("logs\\log.txt", rollingInterval: RollingInterval.Day)
        .CreateLogger();
}

void SelectCacheSource(WebApplicationBuilder builder, SecretService secretService)
{
    try
    {
        var redisConnStr = secretService?.GetSecret("RedisSecretName");

        if (redisConnStr == null)
        {
            throw new Exception("Redis connection string is null");
        }

        builder.Services.AddSingleton<IConnectionMultiplexer>(ConnectionMultiplexer.Connect(redisConnStr));
        builder.Services.AddScoped<ICacheService, RedisCacheService>();

        Log.Information("Redis cache service selected.");
    }
    catch
    {
        builder.Services.AddScoped<ICacheService, MemoryCacheService>();

        Log.Information("Memory cache service selected.");
    }
}

void SetUpMongo(WebApplicationBuilder builder, SecretService secretService)
{
    try
    {
        var mongoConnStr = secretService?.GetSecret("MongoConnStr");

        if (mongoConnStr == null)
        {
            throw new Exception("Mongo connection string is null");
        }

        var client = new MongoClient(mongoConnStr);
        var bd = MongoContext.Create(client.GetDatabase("SadSchool"));

        builder.Services.AddSingleton(bd);
    }
    catch
    {
        // Nothing yet
    }
}

void SetUpConfiguration(WebApplicationBuilder builder)
{
    IConfiguration configuration = new ConfigurationBuilder()
        .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
        .Build();

    ConfigurationManager confManager = new ConfigurationManager();
    confManager.AddConfiguration(configuration);

    builder.Configuration.AddConfiguration(confManager);
}
