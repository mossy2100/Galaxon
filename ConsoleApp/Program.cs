using Galaxon.Astronomy.Algorithms.Services;
using Galaxon.Astronomy.Data;
using Galaxon.Astronomy.Data.Repositories;
using Galaxon.ConsoleApp.Services;
using Galaxon.Core.Files;
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
        SolarCalendar solarCalendar =
            _serviceProvider!.GetRequiredService<SolarCalendar>();

        double avg = TropicalYear.GetAverageLengthInSolarDays(2000, 5000);
        Console.WriteLine($"Average tropical year length 2000-5000: {avg:F6} solar days.");

        solarCalendar.PrintMillennialYearInfo();

        // solarCalendar.FindYearWithIdealLength();
        Console.WriteLine();
        solarCalendar.CountLeapYears();

        Console.WriteLine();
        // solarCalendar.FindSynchronisationPoints();
        solarCalendar.FindOptimalPeriod();

        // RuleFinder.PrintLeapYearPattern(121, 500, 33, 4, 2);

        // Console.WriteLine($"Checking solar day.");
        // double T = -1;
        // double len1 = EarthService.GetSolarDayLengthInSeconds(T) * EarthService.GetTropicalYearLengthInEphemerisDays(T);
        // double len2 = 31_556_925.9747;
        // Console.WriteLine($"Solar day length at J1900: {len1} c.f. {len2}");
    }

    static void LeapWeek()
    {
        // LeapWeekCalendar.FindIntercalationFraction();
        // LeapWeekCalendar.FindIntercalationRule();
        // LeapWeekCalendar.VerifyIntercalationRule();
        // LeapWeekCalendar.PrintLeapWeekPattern();
        // LeapWeekCalendar.PrintCalendarPages13();


        RuleFinder.PrintLeapYearPattern(11, 62, 17, 6, 0);
    }

    static void Lunisolar()
    {
        LunisolarCalendar lunisolarCalendar =
            _serviceProvider!.GetRequiredService<LunisolarCalendar>();
        lunisolarCalendar.FindSynchronisationPoints();
    }
}
