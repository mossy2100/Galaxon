using Galaxon.Astronomy.Algorithms.Services;
using Galaxon.Astronomy.Data;
using Galaxon.Astronomy.Data.Repositories;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using Serilog.Events;

namespace Galaxon.Astronomy.AstroAPI;

public class Program
{
    public static void Main(string[] args)
    {
        Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
            .Enrich.FromLogContext()
            .WriteTo.Console()
            .CreateBootstrapLogger();

        try
        {
            Log.Information("Starting web application");
            ConstructApp(args);
        }
        catch (Exception ex)
        {
            Log.Fatal(ex, "Application terminated unexpectedly");
        }
        finally
        {
            Log.CloseAndFlush();
        }
    }

    private static void ConstructApp(string[] args)
    {
        WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

        // Add logger.
        builder.Services.AddSerilog((services, lc) => lc
            .ReadFrom.Configuration(builder.Configuration)
            .ReadFrom.Services(services)
            .Enrich.FromLogContext()
            .WriteTo.Console());

        // Usual stuff.
        builder.Services.AddAuthorization();
        builder.Services.AddControllers();
        builder.Services.AddRazorPages();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        // Add services to the container.
        AddServices(builder);

        // Build.
        WebApplication app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            ConfigureForDevelopment(app);
        }
        else
        {
            ConfigureForProduction(app);
        }

        app.UseHttpsRedirection();
        app.UseStaticFiles();
        app.UseRouting();
        app.UseAuthentication();
        app.UseAuthorization();

        app.MapControllers();
        app.MapRazorPages();

        // Start the web server, host the website, and wait for requests.
        app.Run();
    }

    private static void ConfigureForProduction(WebApplication app)
    {
        app.UseExceptionHandler("/Error");
        // The default HSTS value is 30 days. You may want to change this for production
        // scenarios, see https://aka.ms/aspnetcore-hsts.
        app.UseHsts();
    }

    private static void ConfigureForDevelopment(WebApplication app)
    {
        app.UseSwagger();
        app.UseSwaggerUI();
        app.UseDeveloperExceptionPage();
    }

    /// <summary>
    /// Add services to the container.
    /// </summary>
    /// <param name="builder">The WebApplicationBuilder instance.</param>
    private static void AddServices(WebApplicationBuilder builder)
    {
        builder.Services.AddScoped<AstroDbContext>();
        builder.Services.AddScoped<AstroObjectGroupRepository>();
        builder.Services.AddScoped<AstroObjectRepository>();
        builder.Services.AddScoped<PlanetService>();
        builder.Services.AddScoped<EarthService>();
        builder.Services.AddScoped<SunService>();
        builder.Services.AddScoped<SeasonalMarkerService>();
        builder.Services.AddScoped<LeapSecondRepository>();
        builder.Services.AddScoped<LeapSecondService>();
    }

    /// <summary>
    /// Return exception details in JSON format.
    /// </summary>
    /// <param name="controller">The controller where the error occurred.</param>
    /// <param name="error">The error message.</param>
    /// <param name="ex">The exception that occured.</param>
    /// <returns></returns>
    public static ObjectResult ReturnException(ControllerBase controller, string error, Exception? ex)
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
