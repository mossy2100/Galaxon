using System.Globalization;
using Galaxon.Astronomy.Algorithms.Services;
using Galaxon.Astronomy.Data;
using Galaxon.Astronomy.Data.Repositories;
using Galaxon.ConsoleApp.Services;
using Galaxon.Core.Files;
using Galaxon.Numerics.Extensions.FloatingPoint;
using Galaxon.Numerics.Geometry;
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
            // Solar();
            // Lunisolar();
            // LeapWeek();
            // TotalDrift();
            // LunisolarSynch();
            // CalcTicksInLongPeriod();
            // TestDecimalYearToJulianDateUniversal();
            // Eras();
            MyBirthMinutes();
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
            .AddSingleton<LunisolarCalendar>()
            .AddSingleton<BirthdayService>();

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
        TropicalYear.CalculateTotalDrift(97, 400);
        Console.WriteLine();

        Console.WriteLine("The 969/4000 solution:");
        TropicalYear.CalculateTotalDrift(969, 4000);
        Console.WriteLine();

        Console.WriteLine("The 31/128 solution:");
        TropicalYear.CalculateTotalDrift(31, 128);
        Console.WriteLine();

        Console.WriteLine("The 29/120 solution:");
        TropicalYear.CalculateTotalDrift(29, 120);
        Console.WriteLine();

        Console.WriteLine("The 121/500 solution:");
        TropicalYear.CalculateTotalDrift(121, 500);
        Console.WriteLine();
    }

    public static void CalcTicksInLongPeriod()
    {
        int minYear = -10000;
        ulong maxNumTicks = ulong.MaxValue;
        double nYears = (double)maxNumTicks / TimeConstants.TICKS_PER_TROPICAL_YEAR;
        Console.WriteLine(nYears);
        double maxYear = nYears + minYear;
        Console.WriteLine($"We can support a tick count from {minYear} to {maxYear}");
        double nTicks = 20000 * TimeConstants.DAYS_PER_TROPICAL_YEAR * TimeConstants.TICKS_PER_DAY;
        Console.WriteLine($"{nTicks:N0}");
    }

    public static void TestDecimalYearToJulianDateUniversal()
    {
        Random rnd = new ();
        for (int i = 1; i < 10000; i++)
        {
            double y = rnd.Next(1, 9999) + rnd.NextDouble();

            // double jdut1 = TimeScales.DecimalYearToJulianDateLimitedRange(y);
            double jdut2 = TimeScales.DecimalYearToJulianDate(y);
            // if (!jdut2.FuzzyEquals(jdut1))
            // {
            //     Console.WriteLine("Error in DecimalYearToJulianDate():");
            //     Console.WriteLine($"    Original value for decimal year: {y}");
            //     Console.WriteLine($"    JDUT, limited range method:      {jdut1}");
            //     Console.WriteLine($"    JDUT, unlimited range method:    {jdut2}");
            //     Console.WriteLine($"    Difference:                      {Math.Abs(jdut2 - jdut1)}");
            //     Console.WriteLine();
            //     break;
            // }

            // double y1 = TimeScales.JulianDateToDecimalYearLimitedRange(jdut1);
            // if (!y1.FuzzyEquals(y))
            // {
            //     Console.WriteLine("Error in JulianDateUniversalToDecimalYearLimitedRange().");
            //     Console.WriteLine($"    Original value for decimal year:    {y}");
            //     Console.WriteLine($"    Decimal year, limited range method: {y1}");
            //     Console.WriteLine($"    Difference:                         {Math.Abs(y1 - y)}");
            //     Console.WriteLine();
            //     break;
            // }

            double y2 = TimeScales.JulianDateToDecimalYear(jdut2);
            if (!y2.FuzzyEquals(y))
            {
                Console.WriteLine("Error in JulianDateUniversalToDecimalYear().");
                Console.WriteLine($"    Original value for decimal year:      {y}");
                Console.WriteLine($"    Decimal year, unlimited range method: {y2}");
                Console.WriteLine($"    Difference:                           {Math.Abs(y2 - y)}");
                Console.WriteLine();
                break;
            }

            // if (!y2.FuzzyEquals(y1))
            // {
            //     Console.WriteLine("Different values for decimal year.");
            //     Console.WriteLine($"    Original value for decimal year:      {y}");
            //     Console.WriteLine($"    Decimal year, limited range method:   {y1}");
            //     Console.WriteLine($"    Decimal year, unlimited range method: {y2}");
            //     Console.WriteLine($"    Difference:                           {Math.Abs(y2 - y1)}");
            //     Console.WriteLine();
            //     break;
            // }
        }
    }

    public static void Eras()
    {
        GregorianCalendar gcal = GregorianCalendarUtility.GregorianCalendarInstance;
        for (int i = 0; i < gcal.Eras.Length; i++)
        {
            Console.WriteLine($"Eras[{i}] = {gcal.Eras[i]}");
        }
    }

    public static void MyBirthMinutes()
    {
        BirthdayService birthdayService = _serviceProvider!.GetRequiredService<BirthdayService>();

        // Get the time zone for Melbourne.
        TimeZoneInfo melbourneTimeZone = TimeZoneInfo.FindSystemTimeZoneById("AUS Eastern Standard Time");

        // Create a DateTime in unspecified kind (assumed local time in context).
        DateTime localDateTime = new (1971, 10, 29, 23, 30, 0);

        // Convert it to DateTimeOffset considering the Melbourne time zone.
        DateTimeOffset dtoBirth = TimeZoneInfo.ConvertTimeToUtc(localDateTime, melbourneTimeZone);

        // Convert to UTC DateTime. Automatically adjusts for DST if applicable.
        DateTime dtBirthUtc = dtoBirth.UtcDateTime;
        Console.WriteLine($"I was born at {dtBirthUtc:yyyy-MM-dd HH:mm (zzz)}");
        DateTime dtBirthLocal = TimeZoneInfo.ConvertTimeFromUtc(dtBirthUtc, melbourneTimeZone);
        Console.WriteLine($"equal to {dtBirthLocal:yyyy-MM-dd HH:mm (zzz)}");

        // Get the Ls at birth.
        double Ls_rad = birthdayService.CalcLongitudeOfSunAtBirth(dtBirthUtc);
        double Ls_deg = Angles.RadiansToDegreesWithWrap(Ls_rad, false);
        Console.WriteLine($"The longitude of the Sun at my birth was {Ls_deg:F3}°");

        for (int y = 2024; y <= 2030; y++)
        {
            Console.WriteLine();
            DateTime dtBirthMinuteUtc = birthdayService.CalcBirthMinute(dtBirthUtc, y);
            DateTime dtBirthMinuteLocal =
                TimeZoneInfo.ConvertTimeFromUtc(dtBirthMinuteUtc, melbourneTimeZone);
            Console.WriteLine($"My birth minute in the year {y} was/will be {dtBirthMinuteUtc:yyyy-MM-dd HH:mm (zzz)}");
            Console.WriteLine($"equal to {dtBirthMinuteLocal:yyyy-MM-dd HH:mm (zzz)}");
        }
    }
}
