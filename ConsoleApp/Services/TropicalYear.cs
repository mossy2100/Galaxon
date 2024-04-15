using Galaxon.Astronomy.Algorithms.Services;
using Galaxon.Numerics.Extensions;
using Galaxon.Time;

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
            double tropicalYearLengthInEphemerisDays =
                EarthService.GetTropicalYearLengthInEphemerisDays(y);
            double solarDayLengthInSeconds = EarthService.GetSolarDayLength(y);
            double tropicalYearLengthInSolarDays = EarthService.GetTropicalYearLengthInSolarDays(y);

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
                    $"Approx. tropical year length at commencement of year {y} is {tropicalYearLengthInEphemerisDays} ephemeris days ({sTime}).");
                Console.WriteLine(
                    $"Approx. solar day length at commencement of year {y} is {solarDayLengthInSeconds} seconds.");
                Console.WriteLine(
                    $"Thus, the approx. tropical year length at commencement of year {y} is {tropicalYearLengthInSolarDays} solar days.");
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
            double yearLengthInSolarDays = EarthService.GetTropicalYearLengthInSolarDays(y);
            totalSolarDays += yearLengthInSolarDays;
        }
        return totalSolarDays / (maxYear - minYear);
    }

    public static void GetAverageLengthInSolarDaysPerMillennium()
    {
        Console.WriteLine($"Average tropical year length in solar days:");
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
        double maxDiffInDays = 1.0 / TimeConstants.MINUTES_PER_DAY;
        (int num, int den) =
            FractionFinder.FindFraction(avgYearLength, maxDiffInDays, ETimeUnit.Day);

        // Look for possible rules.
        RuleFinder.FindRuleWith2Mods(num, den);
        RuleFinder.FindRuleWith3Mods(num, den);

        // int nLeapDays = (int)Math.Round(DoubleExtensions.Frac(avg) * 1000);
        // Console.WriteLine($"Number of leap days needed = {nLeapDays}");
        Console.WriteLine();
    }

    public static void CalculateTotalDrift(double avgCalendarYearLengthInDays)
    {
        Console.WriteLine($"Average calendar year length = {avgCalendarYearLengthInDays} days:");
        double totalDriftInSeconds = 0;
        bool oneDayShiftYearReported = false;
        for (int y = 2025; y < 10000; y++)
        {
            if (y % 1000 == 0 || totalDriftInSeconds > TimeConstants.SECONDS_PER_DAY && !oneDayShiftYearReported)
            {
                string timeString = TimeSpanExtensions.GetTimeString(TimeSpan.FromSeconds(totalDriftInSeconds), 0);
                Console.WriteLine(
                    $"By year {y} cumulative drift = {timeString}.");

                if (totalDriftInSeconds > TimeConstants.SECONDS_PER_DAY && !oneDayShiftYearReported)
                {
                    oneDayShiftYearReported = true;
                }
            }

            // Calculate the approximate drift in seconds for this year.
            double avgCalendarYearLengthInSeconds =
                avgCalendarYearLengthInDays * EarthService.GetSolarDayLength(y);
            double tropicalYearLengthInSeconds =
                EarthService.GetTropicalYearLengthInEphemerisDays(y)
                * TimeConstants.SECONDS_PER_DAY;
            double driftInSeconds = avgCalendarYearLengthInSeconds - tropicalYearLengthInSeconds;
            totalDriftInSeconds += driftInSeconds;
        }
    }
}
