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
        await CompleteDatabaseRebuild();
        // LunarPhaseImportService lunarPhaseImportService =
        //     _serviceProvider!.GetRequiredService<LunarPhaseImportService>();
        // await lunarPhaseImportService.CacheCalculations();
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
            .AddScoped<SunService>()
            .AddScoped<LunarPhaseService>()
            .AddScoped<SeasonalMarkerService>();

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

    /// <summary>
    /// This asynchronous method is responsible for rebuilding the entire database.
    /// It calls the methods responsible for importing data related to the Sun and planets,
    /// and for caching and importing astronomical events.
    /// </summary>
    public static async Task CompleteDatabaseRebuild()
    {
        // Get services.
        SunImportService sunImportService =
            _serviceProvider!.GetRequiredService<SunImportService>();
        PlanetImportService planetImportService =
            _serviceProvider!.GetRequiredService<PlanetImportService>();
        DwarfPlanetImportService dwarfPlanetImportService =
            _serviceProvider!.GetRequiredService<DwarfPlanetImportService>();
        NaturalSatelliteImportService naturalSatelliteImportService =
            _serviceProvider!.GetRequiredService<NaturalSatelliteImportService>();
        AstroObjectGroupImportService astroObjectGroupImportService =
            _serviceProvider!.GetRequiredService<AstroObjectGroupImportService>();
        Vsop87ImportService vsop87ImportService =
            _serviceProvider!.GetRequiredService<Vsop87ImportService>();
        SeasonalMarkerImportService seasonalMarkerImportService =
            _serviceProvider!.GetRequiredService<SeasonalMarkerImportService>();
        LunarPhaseImportService lunarPhaseImportService =
            _serviceProvider!.GetRequiredService<LunarPhaseImportService>();
        ApsideImportService apsideImportService =
            _serviceProvider!.GetRequiredService<ApsideImportService>();
        EasterDateImportService easterDateImportService =
            _serviceProvider!.GetRequiredService<EasterDateImportService>();

        // Import groups.
        // await astroObjectGroupImportService.InitAstroObjectGroups();

        // Import Sun.
        // await sunImportService.Import();

        // Import planets.
        // await planetImportService.Import();

        // Import dwarf planets.
        // await dwarfPlanetImportService.Import();

        // Import natural satellites.
        // await naturalSatelliteImportService.Import();

        // Import Easter dates.
        // await easterDateImportService.Import();

        // Import VSOP87 data.
        // await vsop87ImportService.Import();

        // Import leap second data.
        // TODO

        // Import delta-T data.
        // TODO

        // Compute and import seasonal markers.
        // await seasonalMarkerImportService.CacheCalculations();
        // await seasonalMarkerImportService.ImportFromUsno();
        // await seasonalMarkerImportService.ImportFromAstroPixels();

        // Compute and import lunar phases.
        await lunarPhaseImportService.CacheCalculations();
        // await lunarPhaseImportService.ImportFromUsno();
        // await lunarPhaseImportService.ImportFromAstroPixels();

        // Compute and import apsides.
        // await apsideImportService.CacheCalculations();
        // await apsideImportService.ImportFromUsno();
        // await apsideImportService.ImportFromAstroPixels();
        // await apsideImportService.CacheCalculations("Mars");
    }
}
