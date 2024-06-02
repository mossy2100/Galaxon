using System.Globalization;
using Galaxon.Astronomy.Algorithms.Records;
using Galaxon.Astronomy.Algorithms.Services;
using Galaxon.Astronomy.Algorithms.Utilities;
using Galaxon.Astronomy.Data;
using Galaxon.Astronomy.Data.Enums;
using Galaxon.Astronomy.Data.Repositories;
using Galaxon.ConsoleApp.Services;
using Galaxon.Core.Files;
using Galaxon.Numerics.Extensions.FloatingPoint;
using Galaxon.Numerics.Geometry;
using Galaxon.Quantities.Kinds;
using Galaxon.Time;
using Galaxon.Time.Extensions;
using Humanizer;
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
            // MyBirthMinutes();
            // EarthDistance();
            // HaumeaCalculations();
            // QuaoarCalculations();
            // ProgressiveLeapYearRule3();
            MondaysNearSouthernSolstice();
        }
        catch (Exception ex)
        {
            // Log any unhandled exceptions
            Slog.Error(ex, "An error occurred.");
        }
        finally
        {
            // Dispose the service provider to clean up resources
            await _serviceProvider!.DisposeAsync();
        }
    }

    private static void HaumeaCalculations()
    {
        // Compute surface area.
        double a = 1050e3;
        double b = 840e3;
        double c = 537e3;
        var e = new Ellipsoid(a, b, c);
        Console.WriteLine($"Surface area = {e.SurfaceArea} m2 = {e.SurfaceArea / 1e6} km2");

        // Compute gravity.
        double G = 6.67430e-11;
        double m_kg = 4.006e21;
        double ga = G * m_kg / Pow(a, 2);
        double gb = G * m_kg / Pow(b, 2);
        double gc = G * m_kg / Pow(c, 2);
        Console.WriteLine(
            $"Gravity a = {ga:F3} m/s2, Gravity b = {gb:F3} m/s2, Gravity c = {gc:F3} m/s2");
        Console.WriteLine($"Average = {(ga + gb + gc) / 3:F3} m/s2");

        // Compute escape velocity.
        double eva = Sqrt(2 * ga * a);
        double evb = Sqrt(2 * gb * b);
        double evc = Sqrt(2 * gc * c);
        Console.WriteLine(
            $"Escape velocity a = {eva:F3} m/s, Escape velocity b = {evb:F3} m/s, Escape velocity c = {evc:F3} m/s");
        Console.WriteLine($"Average = {(eva + evb + evc) / 3:F3} m/s");
    }

    private static void QuaoarCalculations()
    {
        // Compute surface area.
        double a = 643e3;
        double b = 540e3;
        double c = 466e3;
        var e = new Ellipsoid(a, b, c);
        Console.WriteLine($"Surface area = {e.SurfaceArea} m2 = {e.SurfaceArea / 1e6} km2");

        // Compute gravity.
        double G = 6.67430e-11;
        double m_kg = 1.2e21;
        double ga = G * m_kg / Pow(a, 2);
        double gb = G * m_kg / Pow(b, 2);
        double gc = G * m_kg / Pow(c, 2);
        Console.WriteLine(
            $"Gravity a = {ga:F3} m/s2, Gravity b = {gb:F3} m/s2, Gravity c = {gc:F3} m/s2");
        Console.WriteLine($"Average = {(ga + gb + gc) / 3:F3} m/s2");

        // Compute escape velocity.
        double eva = Sqrt(2 * ga * a);
        double evb = Sqrt(2 * gb * b);
        double evc = Sqrt(2 * gc * c);
        Console.WriteLine(
            $"Escape velocity a = {eva:F3} m/s, Escape velocity b = {evb:F3} m/s, Escape velocity c = {evc:F3} m/s");
        Console.WriteLine($"Average = {(eva + evb + evc) / 3:F3} m/s");
    }

    private static void EarthDistance()
    {
        double aphelion_km = 152097597;
        double aphelion_AU = aphelion_km / Length.KILOMETRES_PER_ASTRONOMICAL_UNIT;
        Console.WriteLine($"{aphelion_km} km = {aphelion_AU:F8} AU");

        double perihelion_km = 147098450;
        double perihelion_AU = perihelion_km / Length.KILOMETRES_PER_ASTRONOMICAL_UNIT;
        Console.WriteLine($"{perihelion_km} km = {perihelion_AU:F9} AU");

        double semiMajorAxis_km = 149598023;
        double semiMajorAxis_AU = semiMajorAxis_km / Length.KILOMETRES_PER_ASTRONOMICAL_UNIT;
        Console.WriteLine($"{semiMajorAxis_km} km = {semiMajorAxis_AU:F8} AU");
    }

    public static void SetupLogging()
    {
        string? solnDir = DirectoryUtility.GetSolutionDirectory();
        if (solnDir == null)
        {
            throw new InvalidOperationException("Could not find solution directory.");
        }

        // Set up logging.
        Slog.Logger = new LoggerConfiguration()
            .MinimumLevel
            .Debug()
            .WriteTo
            .Console()
            .WriteTo
            .File(Path.Combine(solnDir, "logs/Astronomy.DataImport.log"))
            .CreateLogger();
    }

    public static void SetupServices()
    {
        // Setup DI container.
        IServiceCollection serviceCollection =
            new ServiceCollection().AddDbContext<AstroDbContext>();

        // Add repositories.
        serviceCollection
            .AddSingleton<AstroObjectRepository>()
            .AddSingleton<AstroObjectGroupRepository>()
            .AddSingleton<LeapSecondRepository>();

        // Add algorithm services.
        serviceCollection
            .AddSingleton<PlanetService>()
            .AddSingleton<SunService>()
            .AddSingleton<SeasonalMarkerService>()
            .AddSingleton<LunarPhaseService>();

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

            // double jdut1 = TimeScalesUtility.DecimalYearToJulianDateLimitedRange(y);
            double jdut2 = JulianDateUtility.FromDecimalYear(y);
            // if (!jdut2.FuzzyEquals(jdut1))
            // {
            //     Console.WriteLine("Error in FromDecimalYear():");
            //     Console.WriteLine($"    Original value for decimal year: {y}");
            //     Console.WriteLine($"    JDUT, limited range method:      {jdut1}");
            //     Console.WriteLine($"    JDUT, unlimited range method:    {jdut2}");
            //     Console.WriteLine($"    Difference:                      {Abs(jdut2 - jdut1)}");
            //     Console.WriteLine();
            //     break;
            // }

            // double y1 = TimeScalesUtility.JulianDateToDecimalYearLimitedRange(jdut1);
            // if (!y1.FuzzyEquals(y))
            // {
            //     Console.WriteLine("Error in JulianDateUniversalToDecimalYearLimitedRange().");
            //     Console.WriteLine($"    Original value for decimal year:    {y}");
            //     Console.WriteLine($"    Decimal year, limited range method: {y1}");
            //     Console.WriteLine($"    Difference:                         {Abs(y1 - y)}");
            //     Console.WriteLine();
            //     break;
            // }

            double y2 = JulianDateUtility.ToDecimalYear(jdut2);
            if (!y2.FuzzyEquals(y))
            {
                Console.WriteLine("Error in JulianDateUniversalToDecimalYear().");
                Console.WriteLine($"    Original value for decimal year:      {y}");
                Console.WriteLine($"    Decimal year, unlimited range method: {y2}");
                Console.WriteLine($"    Difference:                           {Abs(y2 - y)}");
                Console.WriteLine();
                break;
            }

            // if (!y2.FuzzyEquals(y1))
            // {
            //     Console.WriteLine("Different values for decimal year.");
            //     Console.WriteLine($"    Original value for decimal year:      {y}");
            //     Console.WriteLine($"    Decimal year, limited range method:   {y1}");
            //     Console.WriteLine($"    Decimal year, unlimited range method: {y2}");
            //     Console.WriteLine($"    Difference:                           {Abs(y2 - y1)}");
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
        TimeZoneInfo melbourneTimeZone =
            TimeZoneInfo.FindSystemTimeZoneById("AUS Eastern Standard Time");

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
        double Ls_deg = RadiansToDegreesWithWrap(Ls_rad, false);
        Console.WriteLine($"The longitude of the Sun at my birth was {Ls_deg:F3}°");

        for (int y = 2024; y <= 2030; y++)
        {
            Console.WriteLine();
            DateTime dtBirthMinuteUtc = birthdayService.CalcBirthMinute(dtBirthUtc, y);
            DateTime dtBirthMinuteLocal =
                TimeZoneInfo.ConvertTimeFromUtc(dtBirthMinuteUtc, melbourneTimeZone);
            Console.WriteLine(
                $"My birth minute in the year {y} was/will be {dtBirthMinuteUtc:yyyy-MM-dd HH:mm (zzz)}");
            Console.WriteLine($"equal to {dtBirthMinuteLocal:yyyy-MM-dd HH:mm (zzz)}");
        }
    }

    public static void ProgressiveLeapYearRule()
    {
        int prevBestD = 0;

        for (int y = 2025; y <= 10000; y++)
        {
            double tropicalYearLength_d = DurationUtility.GetTropicalYearInSolarDaysForYear(y);
            double targetFrac = tropicalYearLength_d.Frac();

            // Find the closest fraction.
            int bestN = 0;
            int bestD = 0;
            double diff;
            double minDiff = double.MaxValue;
            double frac = 0;

            for (int n = 32; n > 20; n--)
            {
                int d = n * 4 + 4;
                frac = (double)n / d;
                diff = Abs(frac - targetFrac);
                if (diff < minDiff)
                {
                    bestN = n;
                    bestD = d;
                    minDiff = diff;
                }
                if (diff < 1e-7)
                {
                    Console.WriteLine(
                        $"In year {y}, the average calendar length of {365 + frac:F6} days is virtually equal to the tropical year length of {tropicalYearLength_d:F6} solar days.");
                }
            }

            // Report.
            if (bestD != prevBestD)
            {
                Console.WriteLine($"From year {y}: {bestN}/{bestD}");
                prevBestD = bestD;
            }
        }
    }

    public static void ProgressiveLeapYearRule2()
    {
        double totalDrift = 0;
        int maxN = 35;
        Dictionary<int, (int, int, int, int)> results = new ();

        for (int millennium = 3; millennium <= 10; millennium++)
        {
            int maxYear = millennium * 1000;
            int minYear = maxYear - 999;

            Console.WriteLine(
                $"Results for the {millennium.Ordinalize()} millennium ({minYear}-{maxYear}).");

            // Get total solar days in this tropical millennium (e.g. 2001-3000).
            double tropicalMillennium_d = 0;
            for (int y = minYear; y <= maxYear; y++)
            {
                tropicalMillennium_d += DurationUtility.GetTropicalYearInSolarDaysForYear(y);
            }
            double avgTropicalYear_d = tropicalMillennium_d / 1000;
            double actualFrac = avgTropicalYear_d.Frac();

            // Consider each fraction and calculate the total drift by the end.
            int bestN = 0;
            int bestD = 0;
            int bestNumLeapYears = 0;
            double minDrift = double.MaxValue;
            double bestTotalDriftForMillennium = 0;
            double minFracDiff = double.MaxValue;
            double bestAvgCalendarYear_d = 0;

            for (int n = maxN; n >= 25; n--)
            {
                int d = n * 4 + 4;
                double ruleFrac = (double)n / d;
                double avgCalendarYear_d = 365 + ruleFrac;
                double fracDiff = Abs(ruleFrac - actualFrac);

                // Get the length of the calendar millennium in solar days.
                int calendarMillennium_d = 0;
                int nLeapYears = 0;
                for (int y = minYear; y <= maxYear; y++)
                {
                    bool isLeapYear = y % 4 == 0 && y % d != 0;

                    int calendarYear_d = isLeapYear ? 366 : 365;
                    calendarMillennium_d += calendarYear_d;

                    if (isLeapYear)
                    {
                        nLeapYears++;
                    }
                }

                // Compute the drift at the end of the millennium.
                double totalDriftForMillennium = calendarMillennium_d - tropicalMillennium_d;
                double testTotalDrift = Abs(totalDrift + totalDriftForMillennium);

                // Console.WriteLine($"    At end of millennium {millennium}, with rule {n} / {d}, total drift would be {testTotalDrift}");

                // Choose the result that produces the lower total drift from the tropical year;
                // or, if it produces the same, choose the result with closer alignment between the
                // average calendar year length and the tropical year, as this rule will last longer.
                // Console.WriteLine($"Rule {n}/{d} => drift {testTotalDrift:F6} days, frac diff {fracDiff:F6}");
                // if (testTotalDrift < minDrift || (testTotalDrift == minDrift && fracDiff < minFracDiff))
                if (testTotalDrift < minDrift
                    || (testTotalDrift == minDrift && fracDiff < minFracDiff))
                {
                    // Console.WriteLine("This is better");
                    minDrift = testTotalDrift;
                    minFracDiff = fracDiff;
                    bestN = n;
                    bestD = d;
                    bestNumLeapYears = nLeapYears;
                    bestTotalDriftForMillennium = totalDriftForMillennium;
                    bestAvgCalendarYear_d = avgCalendarYear_d;
                }
            }

            // Console.WriteLine($"Best total drift = {minDrift:F3} days ({minDrift * 24:F3} hours).");
            Console.WriteLine($"Best rule: {bestN} leap years per {bestD} years.");
            Console.WriteLine($"Actual: {bestNumLeapYears} leap years in the millennium.");
            Console.WriteLine($"Average tropical year length {avgTropicalYear_d:F6} solar days.");
            Console.WriteLine(
                $"Average calendar year length {bestAvgCalendarYear_d:F6} solar days.");
            Console.WriteLine(
                $"Difference between average tropical year and average calendar year = {minFracDiff * 86400:F3} seconds.");

            // Update the total drift.
            totalDrift += bestTotalDriftForMillennium;
            Console.WriteLine(
                $"Total drift by the end of the millennium is {Abs(totalDrift):F3} days ({Abs(totalDrift) * 24:F3} hours).");
            Console.WriteLine();

            results[millennium] = (minYear, maxYear, bestN, bestD);
        }

        foreach (KeyValuePair<int, (int, int, int, int)> result in results)
        {
            (int minYear, int maxYear, int n, int d) = result.Value;
            Console.WriteLine($"For years {minYear}-{maxYear}: {n} leap years in {d} years.");
        }
    }

    public static void ProgressiveLeapYearRule3()
    {
        // Compute the length of a century in calendar days given a century number and value for n.
        int calendarCenturyInDays(int century, int n)
        {
            int maxYear = century * 100;
            int minYear = maxYear - 99;
            int d = 4 * (n + 1);
            int total_d = 0;
            for (int y = minYear; y <= maxYear; y++)
            {
                bool isLeapYear = y % 4 == 0 && y % d != 0;
                total_d += isLeapYear ? 366 : 365;
            }
            return total_d;
        }

        // Step 1. Find the best fit rule of the form n/(4(n+1)) for the 21st century.
        int century = 21;
        int maxYear = century * 100;
        int minYear = maxYear - 99;

        // Get the total length of the tropical century in solar days.
        double tropicalCentury_d = DurationUtility.GetTropicalCenturyInSolarDays(century);
        double targetFrac = (tropicalCentury_d / 100).Frac();
        int currentN = 0;

        // Find which value for n produces the best fit for the 21st century.
        double minDiff = double.MaxValue;
        int currentD;
        double frac;
        for (int n = 1; n <= 40; n++)
        {
            frac = (double)n / (4 * (n + 1));
            double diff = Abs(frac - targetFrac);
            if (diff < minDiff)
            {
                minDiff = diff;
                currentN = n;
            }
        }
        currentD = 4 * (currentN + 1);
        frac = (double)currentN / currentD;

        Console.WriteLine(targetFrac);

        Console.WriteLine(frac);

        // Compute the drift at the end of the 21st century.
        // Note, this presupposes that the century started at 0 drift.
        // In fact, we'll probably start halfway through the century.
        int calendarCentury_d = calendarCenturyInDays(century, currentN);
        double currentDrift = tropicalCentury_d - calendarCentury_d;

        // Report.
        Console.WriteLine(
            $"For century {century} ({minYear}-{maxYear}), n = {currentN}, d = {currentD}, frac = {frac:F6}, drift at end of century is {currentDrift:F3} days ({currentDrift * 24:F1} hours)");

        Dictionary<int, (int minYear, int maxYear, int nLeapYears)> result = new ();
        int nLeapYears = calendarCentury_d % 365;
        result[currentN] = (minYear, maxYear, nLeapYears);

        // Loop through remaining centuries and see when it makes sense to decrement n.
        for (century = 22; century <= 120; century++)
        {
            maxYear = century * 100;
            minYear = maxYear - 99;

            // Console.WriteLine(
            //     $"Results for the {century.Ordinalize()} century ({minYear}-{maxYear}).");

            // Get total solar days in this tropical century.
            tropicalCentury_d = DurationUtility.GetTropicalCenturyInSolarDays(century);

            // Get total calendar days in this tropical century for current value of n and next value of n.
            int calendarCentury1_d = calendarCenturyInDays(century, currentN);
            int nLeapYears1 = calendarCentury1_d % 365;
            int calendarCentury2_d = calendarCenturyInDays(century, currentN - 1);
            int nLeapYears2 = calendarCentury2_d % 365;

            // Calculate accumulated drift at the end of the century for the current value of n and next value of n.
            double newDrift1 = currentDrift + (tropicalCentury_d - calendarCentury1_d);
            double newDrift2 = currentDrift + (tropicalCentury_d - calendarCentury2_d);

            // if switching produces a better result, switch.
            if (Abs(newDrift2) < Abs(newDrift1))
            {
                Console.WriteLine();
                currentN--;
                currentDrift = newDrift2;
                result[currentN] = (minYear, maxYear, nLeapYears2);
            }
            else
            {
                currentDrift = newDrift1;
                result[currentN] = (result[currentN].minYear, maxYear, result[currentN].nLeapYears + nLeapYears1);
            }

            currentD = 4 * (currentN + 1);
            frac = (double)currentN / currentD;

            Console.WriteLine(
                $"For century {century} ({minYear}-{maxYear}), n = {currentN}, d = {currentD}, frac = {frac:F6}, drift at end of century is {currentDrift:F3} days ({currentDrift * 24:F1} hours)");
        }

        foreach (var item in result)
        {
            int nYears = item.Value.maxYear - item.Value.minYear + 1;
            string label = $"For years {item.Value.minYear}-{item.Value.maxYear}:";
            Console.WriteLine($"    {label,-22} {item.Key} leap years per {4 * (item.Key + 1)} years (actually {item.Value.nLeapYears} leap years in {nYears} years).");
        }
    }

    public static void MondaysNearSouthernSolstice()
    {
        Console.WriteLine("Years with a southern solstice close to start of Monday:");
        SeasonalMarkerService seasonalMarkerService = _serviceProvider!.GetRequiredService<SeasonalMarkerService>();
        Console.WriteLine();
        Console.WriteLine("Southern Solstice                     Possible start of new leap week calendar");
        Console.WriteLine("------------------------------------------------------------------------------");
        for (int y = 2025; y <= 2100; y++)
        {
            SeasonalMarkerEvent solstice =
                seasonalMarkerService.GetSeasonalMarker(y, ESeasonalMarkerType.SouthernSolstice);
            DateOnly startDate = solstice.DateTimeUtc.RoundToNearestDay().GetDateOnly();
            if (startDate.DayOfWeek == DayOfWeek.Monday)
            {
                Console.WriteLine($"{solstice.DateTimeUtc.ToString("dddd dd MMMM yyyy HH:mm")} UTC     {startDate.ToString("dddd dd MMMM yyyy")}");
            }
        }
    }
}
