using Galaxon.Astronomy.Algorithms.Services;
using Galaxon.Astronomy.Data;
using Galaxon.Astronomy.Data.Repositories;
using Galaxon.Astronomy.DataImport.Services;
using Galaxon.Development.Application;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace Galaxon.Astronomy.DataImport;

public class Program
{
    /// <summary>
    /// Reference to the ServiceProvider.
    /// </summary>
    private static ServiceProvider? _serviceProvider;

    /// <summary>
    /// Main method that does the stuff.
    /// </summary>
    private static async Task DoStuff()
    {
        LunarPhaseImportService lunarPhaseImportService =
            _serviceProvider!.GetRequiredService<LunarPhaseImportService>();
        await lunarPhaseImportService.CacheCalculations();
    }

    /// <summary>
    /// Main wrapper method that handles setup and exceptions.
    /// </summary>
    /// <param name="args"></param>
    public static async Task Main(string[] args)
    {
        // Initialize.
        SetupTools.Initialize();
        SetupServices();

        try
        {
            await DoStuff();
        }
        catch (Exception ex)
        {
            // Log any unhandled exceptions
            Slog.Error("{Exception}", ex.Message);
        }
        finally
        {
            // Dispose the service provider to clean up resources
            await _serviceProvider!.DisposeAsync();
        }
    }

    public static void SetupServices()
    {
        // Setup DI container.
        IServiceCollection serviceCollection = new ServiceCollection();

        // Add database.
        serviceCollection.AddDbContext<AstroDbContext>();

        // Add repositories.
        serviceCollection
            .AddScoped<AstroObjectRepository>()
            .AddScoped<AstroObjectGroupRepository>()
            .AddScoped<LeapSecondRepository>();

        // Add astronomical algorithms services.
        serviceCollection
            .AddScoped<ApsideService>()
            .AddScoped<PlanetService>()
            .AddScoped<LunarPhaseService>();

        // Add import services.
        serviceCollection
            .AddScoped<ApsideImportService>()
            .AddScoped<AstroObjectGroupImportService>()
            .AddScoped<DwarfPlanetImportService>()
            .AddScoped<EasterDateImportService>()
            .AddScoped<LeapSecondImportService>()
            .AddScoped<LunarPhaseImportService>()
            .AddScoped<NaturalSatelliteImportService>()
            .AddScoped<PlanetImportService>()
            .AddScoped<SeasonalMarkerImportService>()
            .AddScoped<SunImportService>()
            .AddScoped<Vsop87ImportService>();

        // Add logging.
        serviceCollection.AddLogging(loggingBuilder =>
        {
            // Dispose Serilog logger when disposing of ILoggerFactory.
            loggingBuilder.AddSerilog(dispose: true);
        });

        // Build.
        _serviceProvider = serviceCollection.BuildServiceProvider();
    }
}
