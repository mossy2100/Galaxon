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
            await CompleteDatabaseRebuild();
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

    /// <summary>
    /// This asynchronous method is responsible for rebuilding the entire database.
    /// It calls the methods responsible for importing data related to the Sun and planets,
    /// and for caching and importing astronomical events.
    /// </summary>
    public static async Task CompleteDatabaseRebuild()
    {
        // // Import groups.
        // AstroObjectGroupImportService astroObjectGroupImportService = _serviceProvider!.GetRequiredService<AstroObjectGroupImportService>();
        // await astroObjectGroupImportService.InitAstroObjectGroups();
        //
        // // Import Sun.
        // SunImportService sunImportService = _serviceProvider!.GetRequiredService<SunImportService>();
        // await sunImportService.Import();
        //
        // // Import planets.
        // PlanetImportService planetImportService = _serviceProvider!.GetRequiredService<PlanetImportService>();
        // await planetImportService.Import();
        //
        // Import dwarf planets.
        // DwarfPlanetImportService dwarfPlanetImportService = _serviceProvider!.GetRequiredService<DwarfPlanetImportService>();
        // await dwarfPlanetImportService.Import();

        // Import natural satellites.
        NaturalSatelliteImportService naturalSatelliteImportService = _serviceProvider!.GetRequiredService<NaturalSatelliteImportService>();
        await naturalSatelliteImportService.Import();

        // // Import Easter dates.
        // EasterDateImportService easterDateImportService = _serviceProvider!.GetRequiredService<EasterDateImportService>();
        // await easterDateImportService.Import();
        //
        // // Import VSOP87 data.
        // VSOP87ImportService vsop87ImportService = _serviceProvider!.GetRequiredService<VSOP87ImportService>();
        // await vsop87ImportService.Import();
        //
        // // Import leap second data.
        // LeapSecondImportService leapSecondImportService = _serviceProvider!.GetRequiredService<LeapSecondImportService>();
        // await leapSecondImportService.Import();

        // Import delta-T data.
        // DeltaTImportService deltaTImportService = _serviceProvider!.GetRequiredService<DeltaTImportService>();
        // await deltaTImportService.Import();

        // // Compute and import seasonal markers.
        // SeasonalMarkerImportService seasonalMarkerImportService = _serviceProvider!.GetRequiredService<SeasonalMarkerImportService>();
        // await seasonalMarkerImportService.CacheCalculations();
        // await seasonalMarkerImportService.ImportFromUsno();
        // await seasonalMarkerImportService.ImportFromAstroPixels();

        // // Compute and import lunar phases.
        // LunarPhaseImportService lunarPhaseImportService = _serviceProvider!.GetRequiredService<LunarPhaseImportService>();
        // await lunarPhaseImportService.CacheCalculations();
        // await lunarPhaseImportService.ImportFromUsno();
        // await lunarPhaseImportService.ImportFromAstroPixels();

        // Compute and import apsides.
        // ApsideImportService apsideImportService = _serviceProvider!.GetRequiredService<ApsideImportService>();
        // await apsideImportService.CacheCalculations();
        // await apsideImportService.CacheCalculations("Mars");
        // await apsideImportService.ImportFromUsno();
        // await apsideImportService.ImportFromAstroPixels();
    }

    #region Services stuff

    private static ServiceProvider? _serviceProvider;

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
            .AddScoped<DeltaTImportService>()
            .AddScoped<DwarfPlanetImportService>()
            .AddScoped<EasterDateImportService>()
            .AddScoped<LeapSecondImportService>()
            .AddScoped<LunarPhaseImportService>()
            .AddScoped<NaturalSatelliteImportService>()
            .AddScoped<PlanetImportService>()
            .AddScoped<SeasonalMarkerImportService>()
            .AddScoped<SunImportService>()
            .AddScoped<VSOP87ImportService>();

        // Add other services.
        serviceCollection.AddScoped<HttpClient>();

        // Add logging.
        serviceCollection.AddLogging(loggingBuilder =>
        {
            // Dispose Serilog logger when disposing of ILoggerFactory.
            loggingBuilder.AddSerilog(dispose: true);
        });

        // Build.
        _serviceProvider = serviceCollection.BuildServiceProvider();
    }

    #endregion Services
}
