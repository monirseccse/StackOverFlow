using Autofac;
using Autofac.Extensions.DependencyInjection;
using DemoWebApp.Infrastructure.Services;
using FluentNHibernate.AspNetCore.Identity;
using log4net;
using Microsoft.AspNetCore.Identity;
using Serilog;
using StackOverFlowClone.Infrastructure;
using StackOverFlowClone.Infrastructure.DbContexts;
using StackOverFlowClone.Infrastructure.Entities;
using StackOverFlowClone.Infrastructure.Services;
using StackOverFlowClone.Web;
using System.Reflection;

    var builder = WebApplication.CreateBuilder(args);
    var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
    var migrationAssemblyName = Assembly.GetExecutingAssembly().FullName;

    //Autofac Configure
    builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());
    builder.Host.ConfigureContainer<ContainerBuilder>(containerBuilder =>
    {
        containerBuilder.RegisterModule(new WebModule());
        containerBuilder.RegisterModule(new InfrastructureModule());
    });

    //NHibernate
    builder.Services.GetSession(connectionString);

    //AutoMapper Configure
    builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

    //Identity Configurations

    builder.Services
   .AddIdentity<ApplicationUser, ApplicationRole>()
   .ExtendConfiguration()
   .AddNHibernateStores(t => t.SetSessionAutoFlush(true))
   .AddUserManager<ApplicationUserManager>()
   .AddRoleManager<ApplicationRoleManager>()
   .AddSignInManager<ApplicationSignInManager>()
   .AddDefaultTokenProviders();

    builder.Services.Configure<IdentityOptions>(options =>
    {
        // Password settings.
        options.Password.RequireDigit = true;
        options.Password.RequireLowercase = false;
        options.Password.RequireNonAlphanumeric = false;
        options.Password.RequireUppercase = false;
        options.Password.RequiredLength = 6;
        options.Password.RequiredUniqueChars = 0;

        // Lockout settings.
        options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
        options.Lockout.MaxFailedAccessAttempts = 5;
        options.Lockout.AllowedForNewUsers = true;

        // User settings.
        options.User.AllowedUserNameCharacters =
        "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
        options.User.RequireUniqueEmail = true;
    });

    builder.Services.AddSession(options =>
    {
        options.IdleTimeout = TimeSpan.FromMinutes(30);
        options.Cookie.HttpOnly = true;
        options.Cookie.IsEssential = true;
    });


    builder.Logging.AddLog4Net();

    var Log = LogManager.GetLogger(typeof(Program));
    builder.Services.AddControllersWithViews();

try
{
    var app = builder.Build();

    Log.Info("Application Starting up...");

    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        // app.UseMigrationsEndPoint();
    }
    else
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
    app.UseSession();

    app.MapControllerRoute(
        name: "areas",
        pattern: "{area:exists}/{controller=Question}/{action=Index}/{id?}");

    app.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}");

    app.Run();
}
catch (Exception ex)
{
    Log.Fatal($"Error Message: {ex.Message}\n Exception: {ex}\n\n");
}