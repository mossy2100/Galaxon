using System.Text.Json;
using Galaxon.Astronomy.Algorithms.Services;
using Galaxon.Astronomy.Data;
using Galaxon.Astronomy.Data.Repositories;
using Galaxon.Quantities;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace Galaxon.Astronomy.AstroAPI;

/// <summary>
/// The main entry point class for the AstroAPI web application.
/// This class sets up the logging, services, web application configurations and starts the
/// application.
/// </summary>
public class Program
{
    /// <summary>
    /// Main entry point for the application.
    /// Sets up logging, builds and configures the web application, and runs it.
    /// </summary>
    /// <param name="args">Command line arguments passed to the application.</param>
    public static void Main(string[] args)
    {
        try
        {
            SetupLogger();

            Log.Information("Creating web application builder...");
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
        catch (Exception ex)
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
    private static void SetupLogger()
    {
        IConfigurationRoot configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json")
            .Build();

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
        builder.Services.AddAuthorization();
        builder.Services.AddRazorPages();
        builder.Services.AddEndpointsApiExplorer();

        // JsonConverters.
        builder.Services.AddControllers().AddJsonOptions(options =>
        {
            options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
            options.JsonSerializerOptions.Converters.Add(new QuantityJsonConverter());
        });

        // Swagger.
        builder.Services.AddSwaggerGen(c =>
        {
            c.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, "AstroAPI.xml"));
        });

        // Configure DbContext.
        builder.Services.AddScoped<AstroDbContext>();

        // Dependency injection for repositories and services.
        builder.Services.AddScoped<AstroObjectGroupRepository>();
        builder.Services.AddScoped<AstroObjectRepository>();
        builder.Services.AddScoped<PlanetService>();
        builder.Services.AddScoped<EarthService>();
        builder.Services.AddScoped<MoonService>();
        builder.Services.AddScoped<SunService>();
        builder.Services.AddScoped<SeasonalMarkerService>();
        builder.Services.AddScoped<LeapSecondRepository>();
        builder.Services.AddScoped<LeapSecondService>();
        builder.Services.AddScoped<ApsideService>();
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
            app.UseSwagger();
            app.UseSwaggerUI();
            app.UseDeveloperExceptionPage();
        }
        else
        {
            app.UseExceptionHandler("/Error");
            app.UseHsts();
        }

        // Use things.
        app.UseHttpsRedirection();
        app.UseStaticFiles();
        app.UseRouting();
        app.UseAuthentication();
        app.UseAuthorization();

        // Map things.
        app.MapControllers();
        app.MapRazorPages();
    }

    /// <summary>
    /// Provides a method to handle exceptions and log them appropriately.
    /// Returns a standardized error response in the event of an exception.
    /// </summary>
    /// <param name="controller">The controller instance handling the request.</param>
    /// <param name="error">The error message to be logged and returned.</param>
    /// <param name="ex">The exception object, if any, associated with the error.</param>
    /// <returns>A standardized error response with a status code of 500.</returns>
    public static ObjectResult ReturnException(ControllerBase controller, string error, Exception? ex = null)
    {
        Log.Error("Error: {Error}", error);
        if (ex != null)
        {
            Log.Error("Exception: {Exception}", ex.Message);
            if (ex.InnerException != null)
            {
                Log.Error("Inner exception: {InnerException}", ex.InnerException.Message);
            }
        }

        return controller.StatusCode(500, error);
    }
}
