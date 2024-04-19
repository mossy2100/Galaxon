using System.Globalization;
using Galaxon.Astronomy.Data;
using Galaxon.Astronomy.Data.Repositories;
using Galaxon.Astronomy.DataImport.Services;
using Galaxon.Core.Files;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace Galaxon.Astronomy.DataImport;

public class Program
{
    /// <summary>
    /// Reference to the ServiceProvider.
    /// </summary>
    private static ServiceProvider? _serviceProvider;

    public static async Task Main()
    {
        SetupLogging();
        SetupServices();

        try
        {
            // await ImportDwarfPlanets();
            // await ImportNaturalSatellites();
            await ImportLunarPhases();
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

    public static void SetupLogging()
    {
        string? solnDir = DirectoryUtility.GetSolutionDirectory();
        if (solnDir == null)
        {
            throw new InvalidOperationException("Could not find solution directory.");
        }

        // Set up logging.
        Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Debug()
            .WriteTo.Console()
            .WriteTo.File(Path.Combine(solnDir, "logs/Astronomy.DataImport.log"))
            .CreateLogger();
    }

    public static void SetupServices()
    {
        // Setup DI container.
        IServiceCollection serviceCollection = new ServiceCollection()
            .AddDbContext<AstroDbContext>();

        // Add repositories.
        serviceCollection
            .AddSingleton<AstroObjectRepository>()
            .AddSingleton<AstroObjectGroupRepository>()
            .AddSingleton<LeapSecondRepository>();

        // Add import services.
        serviceCollection
            .AddSingleton<AstroObjectGroupImportService>()
            .AddSingleton<DwarfPlanetImportService>()
            .AddSingleton<EasterDateImportService>()
            .AddSingleton<LeapSecondImportService>()
            .AddSingleton<LunarPhaseDataImportService>()
            .AddSingleton<NaturalSatelliteImportService>()
            .AddSingleton<PlanetImportService>()
            .AddSingleton<SeasonalMarkerImportService>()
            .AddSingleton<SunImportService>()
            .AddSingleton<VSOP87ImportService>();

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
        VSOP87ImportService vsop87ImportService =
            _serviceProvider!.GetRequiredService<VSOP87ImportService>();
        vsop87ImportService.Import();
    }

    public static async Task ImportLunarPhases()
    {
        LunarPhaseDataImportService lunarPhaseDataImportService =
            _serviceProvider!.GetRequiredService<LunarPhaseDataImportService>();
        // await lunarPhaseDataImportService.ImportAstroPixels();
        await lunarPhaseDataImportService.ImportUsno();
    }

    public static async Task ImportSeasonalMarkers()
    {
        SeasonalMarkerImportService seasonalMarkerImportService =
            _serviceProvider!.GetRequiredService<SeasonalMarkerImportService>();
        await seasonalMarkerImportService.Import();
    }

    public static async Task ImportLeapSeconds()
    {
        LeapSecondImportService leapSecondImportService =
            _serviceProvider!.GetRequiredService<LeapSecondImportService>();
        await leapSecondImportService.ImportNistWebPage();
        await leapSecondImportService.ImportIersBulletins();
    }

    public static void ImportEasterDates()
    {
        EasterDateImportService easterDateImportService =
            _serviceProvider!.GetRequiredService<EasterDateImportService>();
        easterDateImportService.ImportEasterDates1600_2099();
        easterDateImportService.ImportEasterDates1700_2299();
    }
}
