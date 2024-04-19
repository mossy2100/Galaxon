using System.Globalization;
using Galaxon.Astronomy.Algorithms.Services;
using Galaxon.Astronomy.Data;
using Galaxon.Astronomy.Data.Repositories;
using Galaxon.ConsoleApp.Services;
using Galaxon.Core.Files;
using Galaxon.Numerics.Extensions;
using Galaxon.Time;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace Galaxon.ConsoleApp;

class Program
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
            Solar();
            // Lunisolar();
            // LeapWeek();
            // TotalDrift();
            // LunisolarSynch();
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

        // Add algorithm services.
        serviceCollection
            .AddSingleton<PlanetService>()
            .AddSingleton<EarthService>()
            .AddSingleton<SunService>()
            .AddSingleton<SeasonalMarkerService>()
            .AddSingleton<MoonService>();

        // Add calendar services.
        serviceCollection
            .AddSingleton<FractionFinder>()
            .AddSingleton<RuleFinder>()
            .AddSingleton<SolarCalendar>()
            .AddSingleton<LeapWeekCalendar>()
            .AddSingleton<LunisolarCalendar>();

        // Add logging.
        serviceCollection.AddLogging(loggingBuilder =>
        {
            // Dispose Serilog logger when disposing of ILoggerFactory.
            loggingBuilder.AddSerilog(dispose: true);
        });

        // Build.
        _serviceProvider = serviceCollection.BuildServiceProvider();
    }

    static void Solar()
    {
        SolarCalendar solarCalendar = _serviceProvider!.GetRequiredService<SolarCalendar>();
        // solarCalendar.FindSynchronisationPoints();
        solarCalendar.CalculateYearLengths();
    }

    private static void SolarCalendarFindIntercalationRules()
    {
        // RuleFinder.FindRule(8, 33);
        // RuleFinder.FindRule(11, 62);
        // RuleFinder.FindRule(31, 128);
        // RuleFinder.FindRule(121, 500);
        // RuleFinder.FindModRule(330, 896);
        // Console.WriteLine("======================================================================");
        // RuleFinder.FindRule(358, 566);
        // Console.WriteLine("======================================================================");
        // RuleFinder.FindRule(34, 330);

        RuleFinder.FindHumanRule(0.242189);
        Console.WriteLine();
        RuleFinder.FindHumanRule(0.2422);
        Console.WriteLine();
        RuleFinder.FindHumanRule(0.242);
        Console.WriteLine();

        Console.WriteLine("======================================================================");
        Console.WriteLine("Leap week:");
        double nWeeks = TimeConstants.DAYS_PER_TROPICAL_YEAR / 7;
        RuleFinder.FindHumanRule(nWeeks.Frac());

        Console.WriteLine("======================================================================");
        Console.WriteLine("Lunisolar:");
        double nMonths = TimeConstants.DAYS_PER_TROPICAL_YEAR / TimeConstants.DAYS_PER_LUNATION;
        RuleFinder.FindHumanRule(nMonths.Frac());
        Console.WriteLine();
        RuleFinder.FindHumanRule(0.36825);
    }

    static void LeapWeek()
    {
        // LeapWeekCalendar.FindIntercalationFraction();
        // LeapWeekCalendar.FindIntercalationRule();
        // LeapWeekCalendar.VerifyIntercalationRule();
        // LeapWeekCalendar.PrintLeapWeekPattern();
        // LeapWeekCalendar.PrintIsoWeekCalendarLeapYearPattern();
        // LeapWeekCalendar.PrintExampleIsoWeekCalendar();
        // LeapWeekCalendar.PrintCalendarPages12b();
        // LeapWeekCalendar.PrintYearsEndOnSunday();
        // RuleFinder.PrintLeapYearPattern(11, 62, 17, 6, 0);
    }

    static void LunisolarSynch()
    {
        LunisolarCalendar lunisolarCalendar =
            _serviceProvider!.GetRequiredService<LunisolarCalendar>();
        lunisolarCalendar.FindSynchronisationPoints();
    }

    public static void TotalDrift()
    {
        Console.WriteLine("The 97/400 solution:");
        TropicalYear.CalculateTotalDrift(365.2425);
        Console.WriteLine();

        Console.WriteLine("The 969/4000 solution:");
        TropicalYear.CalculateTotalDrift(365.24225);
        Console.WriteLine();

        Console.WriteLine("The 31/128 solution:");
        TropicalYear.CalculateTotalDrift(365.2421875);
        Console.WriteLine();

        Console.WriteLine("The 121/500 solution:");
        TropicalYear.CalculateTotalDrift(365.242);
        Console.WriteLine();
    }
}
