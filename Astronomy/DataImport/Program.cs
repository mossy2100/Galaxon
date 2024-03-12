using Galaxon.Astronomy.Data;
using Galaxon.Astronomy.Data.Repositories;
using Galaxon.Astronomy.DataImport.Services;
using Galaxon.Core.Files;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace Galaxon.Astronomy.DataImport;

public class Program
{
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
        _serviceProvider = new ServiceCollection()
            .AddDbContext<AstroDbContext>()
            .AddSingleton<SeasonalMarkerImportService>()
            .AddSingleton<AstroObjectRepository>()
            .AddSingleton<AstroObjectGroupRepository>()
            .AddSingleton<LeapSecondImportService>()
            .AddSingleton<LeapSecondRepository>()
            .AddSingleton<EasterDateImportService>()
            .AddSingleton<VSOP87ImportService>()
            .AddSingleton<AstroObjectGroupImportService>()
            .AddSingleton<PlanetImportService>()
            .AddLogging(loggingBuilder =>
            {
                // Dispose Serilog logger when disposing of ILoggerFactory.
                loggingBuilder.AddSerilog(dispose: true);
            })
            .BuildServiceProvider();

        try
        {
            await ImportData();
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

    public static async Task ImportData()
    {
        // await ImportLeapSeconds();
        // ImportEasterDates();

        // Import groups.
        AstroObjectGroupImportService astroObjectGroupImportService =
            _serviceProvider!.GetRequiredService<AstroObjectGroupImportService>();
        astroObjectGroupImportService.InitAstroObjectGroups();
        //
        // // Import planets.
        // PlanetImportService planetImportService =
        //     _serviceProvider!.GetRequiredService<PlanetImportService>();
        // planetImportService.ImportPlanets();
        //
        // // Import VSOP87 data.
        // VSOP87ImportService vsop87ImportService =
        //     _serviceProvider!.GetRequiredService<VSOP87ImportService>();
        // vsop87ImportService.ParseAllVSOP87DataFiles();
    }

    /// <summary>
    /// Parse leap seconds and copy into database.
    /// </summary>
    public static async Task ImportLeapSeconds()
    {
        LeapSecondImportService leapSecondImportService =
            _serviceProvider!.GetRequiredService<LeapSecondImportService>();
        await leapSecondImportService.ParseNistWebPage();
        await leapSecondImportService.ImportIersBulletins();
    }

    /// <summary>
    /// Parse leap seconds and copy into database.
    /// </summary>
    public static void ImportEasterDates()
    {
        EasterDateImportService easterDateImportService =
            _serviceProvider!.GetRequiredService<EasterDateImportService>();
        easterDateImportService.ParseEasterDates1600_2099();
        easterDateImportService.ParseEasterDates1700_2299();
    }
}
