using Galaxon.Astronomy.Algorithms.Services;
using Galaxon.Astronomy.Data;
using Galaxon.Astronomy.Data.Models;
using Galaxon.Astronomy.Data.Repositories;
using Galaxon.Astronomy.DataImport.Services;
using Galaxon.Development.Application;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace Galaxon.Astronomy.DataImport;

public class Program
{
    /// <summary>
    /// Reference to the configuration.
    /// </summary>
    private static IConfiguration? _configuration;

    /// <summary>
    /// Reference to the ServiceProvider.
    /// </summary>
    private static ServiceProvider? _serviceProvider;

    public static async Task Main(string[] args)
    {
        // Initialize.
        _configuration = SetupTools.Initialize();

        // Set up services.
        SetupServices();

        try
        {
            // await ImportDwarfPlanets();
            // await ImportNaturalSatellites();
            // await ImportLunarPhases();
            // await ImportLeapSeconds();
            // TestDbContext();
            // await ImportSeasonalMarkers();

            // PrepopulateApsides();
            // await ImportApsidesFromUsno();
            await ImportApsidesFromAstroPixels();

            // ImportVsop87Data();
        }
        catch (Exception ex)
        {
            // Log any unhandled exceptions
            Log.Error(ex, "An error occurred.");
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
            .AddScoped<PlanetService>();

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

    public static void ImportGroups()
    {
        AstroObjectGroupImportService astroObjectGroupImportService =
            _serviceProvider!.GetRequiredService<AstroObjectGroupImportService>();
        astroObjectGroupImportService.InitAstroObjectGroups();
    }

    public static void ImportSun()
    {
        SunImportService sunImportService =
            _serviceProvider!.GetRequiredService<SunImportService>();
        sunImportService.Import();
    }

    public static void ImportPlanets()
    {
        PlanetImportService planetImportService =
            _serviceProvider!.GetRequiredService<PlanetImportService>();
        planetImportService.Import();
    }

    public static async Task ImportDwarfPlanets()
    {
        DwarfPlanetImportService dwarfPlanetImportService =
            _serviceProvider!.GetRequiredService<DwarfPlanetImportService>();
        await dwarfPlanetImportService.Import();
    }

    public static async Task ImportNaturalSatellites()
    {
        NaturalSatelliteImportService naturalSatelliteImportService =
            _serviceProvider!.GetRequiredService<NaturalSatelliteImportService>();
        await naturalSatelliteImportService.Import();
    }

    public static void ImportVsop87Data()
    {
        Vsop87ImportService vsop87ImportService =
            _serviceProvider!.GetRequiredService<Vsop87ImportService>();
        vsop87ImportService.Import();
    }

    public static async Task ImportLunarPhases()
    {
        LunarPhaseImportService lunarPhaseImportService =
            _serviceProvider!.GetRequiredService<LunarPhaseImportService>();
        // await lunarPhaseImportService.ImportAstroPixels();
        await lunarPhaseImportService.ImportUsno();
    }

    public static async Task ImportSeasonalMarkers()
    {
        SeasonalMarkerImportService seasonalMarkerImportService =
            _serviceProvider!.GetRequiredService<SeasonalMarkerImportService>();
        await seasonalMarkerImportService.ImportFromUsno();
    }

    public static async Task ImportLeapSeconds()
    {
        LeapSecondImportService leapSecondImportService =
            _serviceProvider!.GetRequiredService<LeapSecondImportService>();
        // await leapSecondImportService.ImportNistWebPage();
        await leapSecondImportService.ImportIersBulletins(true);
    }

    public static void ImportEasterDates()
    {
        EasterDateImportService easterDateImportService =
            _serviceProvider!.GetRequiredService<EasterDateImportService>();
        easterDateImportService.ImportEasterDates1600_2099();
        easterDateImportService.ImportEasterDates1700_2299();
    }

    public static void TestDbContext()
    {
        AstroObjectRepository astroObjectRepository =
            _serviceProvider!.GetRequiredService<AstroObjectRepository>();
        List<AstroObjectRecord> planets = astroObjectRepository.LoadByGroup("Planet");
        foreach (AstroObjectRecord planet in planets)
        {
            Console.WriteLine($"{planet.Number}: {planet.Name}");
        }
    }

    private static void PrepopulateApsides()
    {
        ApsideImportService apsideImportService =
            _serviceProvider!.GetRequiredService<ApsideImportService>();
        apsideImportService.CacheCalculations();
    }

    private static async Task ImportApsidesFromUsno()
    {
        ApsideImportService apsideImportService =
            _serviceProvider!.GetRequiredService<ApsideImportService>();
        await apsideImportService.ImportApsidesFromUsno();
    }
    private static async Task ImportApsidesFromAstroPixels()
    {
        ApsideImportService apsideImportService =
            _serviceProvider!.GetRequiredService<ApsideImportService>();
        await apsideImportService.ImportApsidesFromAstroPixels();
    }
}
