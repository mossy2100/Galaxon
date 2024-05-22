using Galaxon.Astronomy.Algorithms.Services;
using Galaxon.Numerics.Extensions.Integers;
using Galaxon.Time;
using Galaxon.Time.Extensions;

namespace Galaxon.ConsoleApp.Services;

public static class TropicalYear
{
    public static void GetTropicalYearLength()
    {
        // Let's see how much the tropical year changes.
        double totalEphemerisDays = 0;
        double totalSolarDays = 0;
        int minYear = 2000;
        int maxYear = 5000;
        int nYears = maxYear - minYear;
        double yearLengthAtStart = 0;
        double yearLengthAtEnd = 0;
        for (int y = minYear; y <= maxYear; y++)
        {
            double tropicalYearLength_ephemerisDays =
                EarthService.GetTropicalYearInEphemerisDaysForYear(y);
            double solarDayLength_s = EarthService.GetSolarDayInSeconds(y);
            double tropicalYearLength_solarDays =
                EarthService.GetTropicalYearInSolarDaysForYear(y);

            if (y < maxYear)
            {
                totalEphemerisDays += tropicalYearLength_ephemerisDays;
                totalSolarDays += tropicalYearLength_solarDays;
            }

            if (y == minYear)
            {
                yearLengthAtStart = tropicalYearLength_ephemerisDays;
            }

            if (y == maxYear)
            {
                yearLengthAtEnd = tropicalYearLength_ephemerisDays;
            }

            if (y == minYear || y == maxYear)
            {
                TimeSpan tsLength = TimeSpan.FromDays(tropicalYearLength_ephemerisDays);
                string sTime = TimeSpanExtensions.GetTimeString(tsLength);
                Console.WriteLine(
                    $"Approx. tropical year length at commencement of year {y} is {tropicalYearLength_ephemerisDays} ephemeris days ({sTime}).");
                Console.WriteLine(
                    $"Approx. solar day length at commencement of year {y} is {solarDayLength_s} seconds.");
                Console.WriteLine(
                    $"Thus, the approx. tropical year length at commencement of year {y} is {tropicalYearLength_solarDays} solar days.");
                Console.WriteLine();
            }
        }

        double avgTropicalYearLengthInEphemerisDays = totalEphemerisDays / nYears;
        TimeSpan tsAvg = TimeSpan.FromDays(avgTropicalYearLengthInEphemerisDays);
        string sAvgTimeEphemeris = TimeSpanExtensions.GetTimeString(tsAvg);
        Console.WriteLine(
            $"Average tropical year length over {nYears} years is {avgTropicalYearLengthInEphemerisDays} ephemeris days ({sAvgTimeEphemeris}).");

        double avgTropicalYearLengthInSolarDays = totalSolarDays / nYears;
        Console.WriteLine(
            $"This equals {avgTropicalYearLengthInSolarDays} solar days.");

        Console.WriteLine();

        double totalChange_d = yearLengthAtEnd - yearLengthAtStart;
        double totalChange_s = totalChange_d * TimeConstants.SECONDS_PER_DAY;
        Console.WriteLine(
            $"The tropical year changes in length by {totalChange_s} seconds in {nYears} years.");
        double changePerYear_s = totalChange_s / nYears;
        Console.WriteLine(
            $"Thus, it changes by about {changePerYear_s} seconds ({changePerYear_s * 1000} ms) per year.");
        double changePerCentury_s = changePerYear_s * 100;
        Console.WriteLine($"Or about {changePerCentury_s} seconds per century.");
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
            double yearLengthInSolarDays = EarthService.GetTropicalYearInSolarDaysForYear(y);
            totalSolarDays += yearLengthInSolarDays;
        }
        return totalSolarDays / (maxYear - minYear);
    }

    public static void GetAverageLengthInSolarDaysPerMillennium()
    {
        Console.WriteLine("Average tropical year length in solar days:");
        for (int c = 2; c < 10; c++)
        {
            int minYear = c * 1000;
            int maxYear = minYear + 1000;
            double avg = GetAverageLengthInSolarDays(minYear, maxYear);
            int millenniumNum = c + 1;
            string millenniumNumSuffix = Int64Extensions.GetOrdinalSuffix(millenniumNum);
            Console.WriteLine(
                $"{millenniumNum}{millenniumNumSuffix} millennium ({minYear}-{maxYear - 1}): {avg:F6} solar days.");
        }
    }

    public static void FindLeapYearRule(int minYear, int maxYear)
    {
        double avgYearLength = GetAverageLengthInSolarDays(minYear, maxYear);
        Console.WriteLine($"Average year length: {avgYearLength} solar days");

        // Look for a fraction.
        double maxDiff_d = 1.0 / TimeConstants.MINUTES_PER_DAY;
        (int num, int den) = FractionFinder.FindFraction(avgYearLength, maxDiff_d, ETimeUnit.Day);

        // Look for possible rules.
        RuleFinder.FindRuleWith2Mods(num, den);
        RuleFinder.FindRuleWith3Mods(num, den);

        // int nLeapDays = (int)Round(DoubleExtensions.Frac(avg) * 1000);
        // Console.WriteLine($"Number of leap days needed = {nLeapDays}");
        Console.WriteLine();
    }

    public static void CalculateTotalDrift(int num, int den)
    {
        double avgCalendarYearLength_d = 365 + (double)num / den;
        Console.WriteLine($"Average calendar year length = {avgCalendarYearLength_d} days:");
        double totalDrift_s = 0;
        bool oneDayShiftYearReported = false;
        for (int y = 2025; y <= 12000; y++)
        {
            if (y % 1000 == 0
                || totalDrift_s > TimeConstants.SECONDS_PER_DAY && !oneDayShiftYearReported)
            {
                string timeString =
                    TimeSpanExtensions.GetTimeString(TimeSpan.FromSeconds(totalDrift_s), 0);
                Console.WriteLine(
                    $"By year {y} cumulative drift = {timeString}.");

                if (totalDrift_s > TimeConstants.SECONDS_PER_DAY && !oneDayShiftYearReported)
                {
                    oneDayShiftYearReported = true;
                }
            }

            // Calculate the approximate drift in seconds for this year.
            double avgCalendarYearLength_s =
                avgCalendarYearLength_d * EarthService.GetSolarDayInSeconds(y);
            double tropicalYearLength_s = EarthService.GetTropicalYearInEphemerisDaysForYear(y)
                * TimeConstants.SECONDS_PER_DAY;
            double drift_s = avgCalendarYearLength_s - tropicalYearLength_s;
            totalDrift_s += drift_s;
        }
    }
}
