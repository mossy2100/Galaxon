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
            double tropicalYearLengthInEphemerisDays = EarthService.CalcTropicalYearLength(T);
            double solarDayLengthInSeconds = EarthService.CalcSolarDayLength(T);
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

    public static void GetAverageTropicalLengthPerMillennium()
    {
        for (int c = 2000; c < 10000; c += 1000)
        {
            double totalSolarDaysInMillennium = 0;
            for (int e = 0; e < 1000; e++)
            {
                int y = c + e;
                DateTime dtMidYear = GregorianCalendarExtensions.YearMidPoint(y);
                double jdut = JulianDateService.DateTimeToJulianDateUT(dtMidYear);
                double jdtt = JulianDateService.JulianDateUniversalTimeToTerrestrialTime(jdut);
                double T = JulianDateService.JulianCenturiesSinceJ2000(jdtt);
                double yearLengthInEphemerisDays = EarthService.CalcTropicalYearLength(T);
                double solarDayLengthInSeconds = EarthService.CalcSolarDayLength(T);
                double yearLengthInSolarDays = yearLengthInEphemerisDays
                    * TimeConstants.SECONDS_PER_DAY
                    / solarDayLengthInSeconds;

                // if (y % 100 == 0)
                // {
                //     string strYearLengthEphemeris =
                //         TimeSpanConversion.GetTimeString(
                //             TimeSpan.FromDays(yearLengthInEphemerisDays));
                //     Console.WriteLine($"Year {y} is {strYearLengthEphemeris} long (ephemeris days).");
                //     Console.WriteLine($"Solar day length in {y} = {solarDayLengthInSeconds} seconds");
                //     string strYearLengthSolar =
                //         TimeSpanConversion.GetTimeString(
                //             TimeSpan.FromDays(yearLengthInSolarDays));
                //     Console.WriteLine($"Year {y} is {strYearLengthSolar} long (solar days).");
                //     Console.WriteLine();
                // }

                totalSolarDaysInMillennium += yearLengthInSolarDays;
            }
            double avg = totalSolarDaysInMillennium / 1000;
            string timeString = TimeSpanExtensions.GetTimeString(TimeSpan.FromDays(avg));
            Console.WriteLine($"Average tropical year length in millennium {c}-{c + 999} = {avg:F9} ({timeString})");

            // Look for a fraction.
            double maxDiffInDays = 1.0 / TimeConstants.MINUTES_PER_DAY;
            (int num, int den) = FractionFinder.FindFraction(avg, maxDiffInDays, ETimeUnit.Day);

            // Look for possible rules.
            RuleFinder.FindRuleWith2Mods(num, den);
            RuleFinder.FindRuleWith3Mods(num, den);

            // int nLeapDays = (int)Math.Round(DoubleExtensions.Frac(avg) * 1000);
            // Console.WriteLine($"Number of leap days needed = {nLeapDays}");
            Console.WriteLine();
        }
    }
}
