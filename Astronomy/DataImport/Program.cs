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
        string solnDir = DirectoryUtility.GetSolutionDirectory() ?? ".";

        // Set up logging.
        Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Debug()
            .WriteTo.Console()
            .WriteTo.File(Path.Combine(solnDir, "logs/Astronomy.DataImport.log"))
            .CreateLogger();

        // Setup DI container.
        _serviceProvider = new ServiceCollection()
            .AddDbContext<AstroDbContext>()
            .AddSingleton<DataImportService>()
            .AddSingleton<AstroObjectRepository>()
            .AddSingleton<AstroObjectGroupRepository>()
            .AddSingleton<LeapSecondImportService>()
            .AddSingleton<LeapSecondRepository>()
            .AddSingleton<EasterDateImportService>()
            .AddLogging(loggingBuilder =>
            {
                // Dispose Serilog logger when disposing of ILoggerFactory.
                loggingBuilder.AddSerilog(dispose: true);
            })
            .BuildServiceProvider();

        try
        {
            // await ImportLeapSeconds();
            await ImportEasterDates();
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

    public static async Task ImportLeapSeconds()
    {
        // Parse leap seconds and copy into database.
        LeapSecondImportService leapSecondImportService =
            _serviceProvider!.GetRequiredService<LeapSecondImportService>();
        await leapSecondImportService.ParseNistWebPage();
        await leapSecondImportService.ImportIersBulletins();
    }

    public static async Task ImportEasterDates()
    {
        // Parse leap seconds and copy into database.
        EasterDateImportService easterDateImportService =
            _serviceProvider!.GetRequiredService<EasterDateImportService>();
        easterDateImportService.ParseEasterDates1600_2099();
        easterDateImportService.ParseEasterDates1700_2299();
    }
}
