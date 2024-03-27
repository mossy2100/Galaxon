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
            .AddSingleton<EasterDateImportService>()
            .AddSingleton<LeapSecondImportService>()
            .AddSingleton<LunarPhaseDataImportService>()
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

        try
        {
            //======================================================================================
            // await ImportSeasonalMarkers();
            //======================================================================================
        }
        catch (Exception ex)
        {
            // Log any unhandled exceptions
            Log.Error(ex, "An error occurred while importing data.");
        }
        finally
        {
            // Dispose the service provider to clean up resources
            await _serviceProvider.DisposeAsync();
        }
    }

    //==============================================================================================

    public static void ImportGroups()
    {
        AstroObjectGroupImportService astroObjectGroupImportService =
            _serviceProvider!.GetRequiredService<AstroObjectGroupImportService>();
        astroObjectGroupImportService.InitAstroObjectGroups();
    }

    public static void ImportSun()
    {
        SunImportService sunImportService = _serviceProvider!.GetRequiredService<SunImportService>();
        sunImportService.ImportSun();
    }

    public static void ImportPlanets()
    {
        PlanetImportService planetImportService =
            _serviceProvider!.GetRequiredService<PlanetImportService>();
        planetImportService.ImportPlanets();
    }

    public static void ImportDwarfPlanets()
    {
    }

    public static void ImportMoons()
    {
    }

    public static void ImportVsop87Data()
    {
        VSOP87ImportService vsop87ImportService =
            _serviceProvider!.GetRequiredService<VSOP87ImportService>();
        vsop87ImportService.ParseAllVSOP87DataFiles();
    }

    public static async Task ImportLunarPhases()
    {
        LunarPhaseDataImportService lunarPhaseDataImportService =
            _serviceProvider!.GetRequiredService<LunarPhaseDataImportService>();
        await lunarPhaseDataImportService.ParseLunarPhaseData();
    }

    public static async Task ImportSeasonalMarkers()
    {
        SeasonalMarkerImportService seasonalMarkerImportService =
            _serviceProvider!.GetRequiredService<SeasonalMarkerImportService>();
        await seasonalMarkerImportService.ImportSeasonalMarkerData();
    }

    public static async Task ImportLeapSeconds()
    {
        LeapSecondImportService leapSecondImportService =
            _serviceProvider!.GetRequiredService<LeapSecondImportService>();
        await leapSecondImportService.ParseNistWebPage();
        await leapSecondImportService.ImportIersBulletins();
    }

    public static void ImportEasterDates()
    {
        EasterDateImportService easterDateImportService =
            _serviceProvider!.GetRequiredService<EasterDateImportService>();
        easterDateImportService.ParseEasterDates1600_2099();
        easterDateImportService.ParseEasterDates1700_2299();
    }
}
