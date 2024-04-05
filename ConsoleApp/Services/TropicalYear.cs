using Azure.Core;
using Galaxon.Astronomy.Algorithms.Services;
using Galaxon.Time;

namespace Galaxon.ConsoleApp.Services;

public static class TropicalYear
{
    public static void GetTropicalYearLength()
    {
        // Let's see how much the tropical year changes in one century.
        double T = 0;
        double totalEphemerisDays = 0;
        double totalSolarDays = 0;
        int minYear = 2000;
        int maxYear = 5000;
        int nYears = maxYear - minYear;
        double yearLengthAtStart = 0;
        double yearLengthAtEnd = 0;
        for (int y = minYear; y <= maxYear; y++)
        {
            double tropicalYearLengthInEphemerisDays = EarthService.GetTropicalYearLengthInEphemerisDays(T);
            double solarDayLengthInSeconds = EarthService.GetSolarDayLengthInSeconds(T);
            double tropicalYearLengthInSolarDays = tropicalYearLengthInEphemerisDays
                * TimeConstants.SECONDS_PER_DAY
                / solarDayLengthInSeconds;

            if (y < maxYear)
            {
                totalEphemerisDays += tropicalYearLengthInEphemerisDays;
                totalSolarDays += tropicalYearLengthInSolarDays;
            }

            if (y == minYear)
            {
                yearLengthAtStart = tropicalYearLengthInEphemerisDays;
            }

            if (y == maxYear)
            {
                yearLengthAtEnd = tropicalYearLengthInEphemerisDays;
            }

            if (y == minYear || y == maxYear)
            {
                TimeSpan tsLength = TimeSpan.FromDays(tropicalYearLengthInEphemerisDays);
                string sTime = TimeSpanExtensions.GetTimeString(tsLength);
                Console.WriteLine(
                    $"Tropical year length at commencement of year {y} is {tropicalYearLengthInEphemerisDays} ephemeris days ({sTime}).");
                Console.WriteLine(
                    $"Solar day length at commencement of year {y} is {solarDayLengthInSeconds} seconds.");
                Console.WriteLine();
            }

            // Go to start of next year.
            T += tropicalYearLengthInEphemerisDays / TimeConstants.DAYS_PER_JULIAN_CENTURY;
        }

        Console.WriteLine();

        double avgTropicalYearLengthInEphemerisDays = totalEphemerisDays / nYears;
        TimeSpan tsAvg = TimeSpan.FromDays(avgTropicalYearLengthInEphemerisDays);
        string sAvgTimeEphemeris = TimeSpanExtensions.GetTimeString(tsAvg);
        Console.WriteLine(
            $"Average tropical year length over {nYears} years is {avgTropicalYearLengthInEphemerisDays} ephemeris days ({sAvgTimeEphemeris}).");

        double avgTropicalYearLengthInSolarDays = totalSolarDays / nYears;
        Console.WriteLine(
            $"Average tropical year length over {nYears} years is {avgTropicalYearLengthInSolarDays} solar days.");

        Console.WriteLine();

        double totalChangeInDays = yearLengthAtEnd - yearLengthAtStart;
        double totalChangeInSeconds = totalChangeInDays * TimeConstants.SECONDS_PER_DAY;
        Console.WriteLine(
            $"The tropical year changes in length by {totalChangeInSeconds} seconds in {nYears} years.");
        double changePerYearInSeconds = totalChangeInSeconds / nYears;
        Console.WriteLine(
            $"Thus, it changes by about {changePerYearInSeconds} seconds ({changePerYearInSeconds * 1000} ms) per year.");
        double changePerCenturyInSeconds = changePerYearInSeconds * 100;
        Console.WriteLine($"Or about {changePerCenturyInSeconds} seconds per century.");
    }

    /// <summary>
    /// Get the average tropical year length in solar days from minYear (inclusive) to maxYear
    /// (exclusive).
    /// </summary>
    /// <param name="minYear"></param>
    /// <param name="maxYear"></param>
    /// <returns></returns>
    public static double GetAverageLengthInSolarDays(int minYear, int maxYear)
    {
        double totalSolarDays = 0;
        for (int y = minYear; y < maxYear; y++)
        {
            DateTime dt = GregorianCalendarExtensions.YearStart(y);
            double jdtt = JulianDateService.DateTimeUniversalToJulianDateTerrestrial(dt);
            double T = JulianDateService.JulianCenturiesSinceJ2000(jdtt);
            double yearLengthInSolarDays = EarthService.GetTropicalYearLengthInSolarDays(T);
            totalSolarDays += yearLengthInSolarDays;
        }
        return totalSolarDays / (maxYear - minYear);
    }

    public static void GetAverageLengthInSolarDaysPerMillennium()
    {
        for (int c = 2; c < 10; c++)
        {
            int minYear = c * 1000;
            int maxYear = minYear + 1000;
            double avg = GetAverageLengthInSolarDays(minYear, maxYear);
            string timeString = TimeSpanExtensions.GetTimeString(TimeSpan.FromDays(avg));
            Console.WriteLine($"Average tropical year length in millennium {minYear}-{maxYear} = {avg:F9} ({timeString})");
        }
    }

    public static void FindLeapYearRule(int minYear, int maxYear)
    {
        double avgYearLength = GetAverageLengthInSolarDays(minYear, maxYear);
        Console.WriteLine($"Average year length: {avgYearLength} solar days");

        // Look for a fraction.
        double maxDiffInDays = 1.0 / TimeConstants.MINUTES_PER_DAY;
        (int num, int den) = FractionFinder.FindFraction(avgYearLength, maxDiffInDays, ETimeUnit.Day);

        // Look for possible rules.
        RuleFinder.FindRuleWith2Mods(num, den);
        RuleFinder.FindRuleWith3Mods(num, den);

        // int nLeapDays = (int)Math.Round(DoubleExtensions.Frac(avg) * 1000);
        // Console.WriteLine($"Number of leap days needed = {nLeapDays}");
        Console.WriteLine();
    }
}
