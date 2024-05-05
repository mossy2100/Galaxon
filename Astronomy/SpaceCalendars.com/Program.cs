using Galaxon.Astronomy.SpaceCalendars.com.Repositories;
using Galaxon.Astronomy.SpaceCalendars.com.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace Galaxon.Astronomy.SpaceCalendars.com;

/// <summary>
/// The main entry point class for the SpaceCalendars.com website.
/// This class sets up the logging, services, web application configurations and starts the
/// application.
/// </summary>
public class Program
{
    public static void Main(string[] args)
    {
        try
        {
            SetupLogger();

            Log.Information("Creating wep application builder...");
            WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

            Log.Information("Configuring services...");
            ConfigureServices(builder);

            Log.Information("Building web application...");
            WebApplication app = builder.Build();

            Log.Information("Configuring web application...");
            ConfigureApp(app);

            Log.Information("Running web application...");
            app.Run();
        }
        catch (Exception ex) when (ex is not HostAbortedException
            && ex.Source
            != "Microsoft.EntityFrameworkCore.Design") // see https://github.com/dotnet/efcore/issues/29923
        {
            Log.Fatal(ex, "Application terminated unexpectedly.");
        }
        finally
        {
            Log.CloseAndFlush();
        }
    }

    /// <summary>
    /// Configures the Serilog logging service.
    /// </summary>
    public static void SetupLogger()
    {
        // Build configuration
        IConfigurationRoot configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json")
            .Build();

        // Set up Serilog
        Log.Logger = new LoggerConfiguration()
            .ReadFrom.Configuration(configuration)
            .CreateLogger();
    }

    /// <summary>
    /// Adds and configures services for the application.
    /// Includes controllers, Razor pages, API explorers, and custom services.
    /// </summary>
    /// <param name="builder">The application builder to which services are added.</param>
    private static void ConfigureServices(WebApplicationBuilder builder)
    {
        builder.Services.AddDefaultIdentity<IdentityUser>(options =>
            {
                options.SignIn.RequireConfirmedAccount = true;
            })
            .AddEntityFrameworkStores<ApplicationDbContext>();

        builder.Services.AddControllersWithViews(options =>
            options.Filters.Add(new AuthorizeFilter()));
        // builder.Services.AddControllersWithViews();
        // builder.Services.AddRazorPages();

        builder.Services.AddDatabaseDeveloperPageExceptionFilter();

        // Configure database.
        builder.Services.AddDbContext<ApplicationDbContext>(options =>
        {
            string? connString = builder.Configuration.GetConnectionString("SpaceCalendars");
            options.UseMySql(connString, ServerVersion.AutoDetect(connString));
            // options.UseMySql(connString, new MySqlServerVersion("8.3.0"));
        });

        // Dependency injection for repositories and services.
        builder.Services.AddScoped<IDocumentRepository, DocumentRepository>();
        builder.Services.AddScoped<DocumentService>();
        builder.Services.AddScoped<BufferedFileUploadService>();
        builder.Services.AddScoped<MessageBoxService>();
    }

    /// <summary>
    /// Configures the web application's request pipeline based on environment settings.
    /// </summary>
    /// <param name="app">The configured web application.</param>
    private static void ConfigureApp(WebApplication app)
    {
        // Configure the HTTP request pipeline for different environments.
        if (app.Environment.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
            app.UseMigrationsEndPoint();
        }
        else
        {
            app.UseExceptionHandler("/Home/Error");
            // The default HSTS value is 30 days. You may want to change this for production scenarios,
            // see https://aka.ms/aspnetcore-hsts.
            app.UseHsts();
        }

        // Use things.
        app.UseHttpsRedirection();
        app.UseStaticFiles();
        app.UseRouting();
        app.UseAuthentication();
        app.UseAuthorization();

        // Map things.
        app.MapControllerRoute(
            "default",
            "{controller=Home}/{action=Index}/{id?}");
        app.MapRazorPages();
        app.MapFallbackToController(@"{**alias}", "DisplayFromPathAlias", "Document");
    }

    /// <summary>
    /// Provides a method to handle exceptions and log them appropriately.
    /// Returns a standardized error response in the event of an exception.
    /// </summary>
    /// <param name="controller">The controller instance handling the request.</param>
    /// <param name="error">The error message to be logged and returned.</param>
    /// <param name="ex">The exception object, if any, associated with the error.</param>
    /// <returns>A standardized error response with a status code of 500.</returns>
    public static ObjectResult ReturnException(ControllerBase controller, string error,
        Exception? ex = null)
    {
        // Log the error and exception details.
        Log.Error("Error: {Error}", error);
        if (ex != null)
        {
            Log.Error("Exception: {Exception}", ex.Message);
            if (ex.InnerException != null)
            {
                Log.Error("Inner exception: {InnerException}", ex.InnerException.Message);
            }
        }

        // Send response.
        return controller.StatusCode(500, error);
    }
}
