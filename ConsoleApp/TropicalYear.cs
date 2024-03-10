using Galaxon.Astronomy.Algorithms.Utilities;
using Galaxon.Time;

namespace Galaxon.ConsoleApp;

public static class TropicalYear
{
    public static void GetTropicalYearLength()
    {
        // Let's see how much the tropical year changes in one century.
        double T = 0;
        double totalEphemerisDays = 0;
        double totalSolarDays = 0;
        int minYear = 2000;
        int maxYear = 12000;
        int nYears = maxYear - minYear;
        double yearLengthAtStart = 0;
        double yearLengthAtEnd = 0;
        for (int y = minYear; y <= maxYear; y++)
        {
            double tropicalYearLengthInEphemerisDays = EarthUtility.CalcTropicalYearLength(T);
            double solarDayLengthInSeconds = EarthUtility.CalcSolarDayLength(T);
            double tropicalYearLengthInSolarDays = tropicalYearLengthInEphemerisDays
                * TimeConstants.SECONDS_PER_DAY / solarDayLengthInSeconds;

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
                string sTime = TimeSpanConversion.GetTimeString(tsLength);
                Console.WriteLine($"Tropical year length at commencement of year {y} is {tropicalYearLengthInEphemerisDays} ephemeris days ({sTime}).");
                Console.WriteLine($"Solar day length at commencement of year {y} is {solarDayLengthInSeconds} seconds.");
                Console.WriteLine();
            }

            // Go to start of next year.
            T += tropicalYearLengthInEphemerisDays / TimeConstants.DAYS_PER_JULIAN_CENTURY;
        }

        Console.WriteLine();

        double avgTropicalYearLengthInEphemerisDays = totalEphemerisDays / nYears;
        TimeSpan tsAvg = TimeSpan.FromDays(avgTropicalYearLengthInEphemerisDays);
        string sAvgTimeEphemeris = TimeSpanConversion.GetTimeString(tsAvg);
        Console.WriteLine($"Average tropical year length over {nYears} years is {avgTropicalYearLengthInEphemerisDays} ephemeris days ({sAvgTimeEphemeris}).");

        double avgTropicalYearLengthInSolarDays = totalSolarDays / nYears;
        Console.WriteLine($"Average tropical year length over {nYears} years is {avgTropicalYearLengthInSolarDays} solar days.");

        Console.WriteLine();

        double totalChangeInDays = yearLengthAtEnd - yearLengthAtStart;
        double totalChangeInSeconds = totalChangeInDays * TimeConstants.SECONDS_PER_DAY;
        Console.WriteLine($"The tropical year changes in length by {totalChangeInSeconds} seconds in {nYears} years.");
        double changePerYearInSeconds = totalChangeInSeconds / nYears;
        Console.WriteLine($"Thus, it changes by about {changePerYearInSeconds} seconds ({changePerYearInSeconds * 1000} ms) per year.");
        double changePerCenturyInSeconds = changePerYearInSeconds * 100;
        Console.WriteLine($"Or about {changePerCenturyInSeconds} seconds per century.");
    }
}
