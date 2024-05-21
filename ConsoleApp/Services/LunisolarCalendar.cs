using Galaxon.Astronomy.Algorithms.Records;
using Galaxon.Astronomy.Algorithms.Services;
using Galaxon.Astronomy.Data.Enums;
using Galaxon.Core.Functional;
using Galaxon.Core.Strings;
using Galaxon.Numerics.Extensions.FloatingPoint;
using Galaxon.Numerics.Geometry;
using Galaxon.Time;
using Microsoft.OpenApi.Extensions;
using static Galaxon.Numerics.Extensions.NumberExtensions;

namespace Galaxon.ConsoleApp.Services;

public class LunisolarCalendar(
    SeasonalMarkerService seasonalMarkerService,
    MoonService moonService,
    SunService sunService)
{
    public static bool IsFullMonth(int m)
    {
        return Mod(m, 2) == 0 || Mod(Mod(m, 850), 32) == 25;
    }

    /// <summary>
    /// The function found by Program1.
    /// </summary>
    /// <param name="m"></param>
    /// <returns></returns>
    public static bool IsFullMonth2(int m)
    {
        return Mod(m, 2) == 1 || Mod(Mod(Mod(m, 5000), 49), 13) == 10;
    }

    /// <summary>
    /// Determine if the year is a leap year with 13 months or a common year with 12.
    /// </summary>
    /// <param name="y"></param>
    /// <returns></returns>
    public static bool IsLeapYear(int y)
    {
        return Mod(Mod(Mod(y, 1021), 19), 3) == 0;
    }

    private static int _MonthsInYear(int y)
    {
        return IsLeapYear(y) ? 13 : 12;
    }

    public static Func<int, int> MonthsInYear = Memoization.Memoize<int, int>(_MonthsInYear);

    // @todo Ensure the rule works with negative values for LN.
    public static void TestFullMonthRule()
    {
        var oddMonthsLong = 0;
        var monthsPerCycle = 850;
        var gapLengthMonths = 32;
        var offset = 25;
        Console.WriteLine(
            $"The rule: isLongMonth = (m % 2 == {oddMonthsLong}) || (m % {monthsPerCycle} % {gapLengthMonths} == {offset})");

        var longMonthCount = 0;
        List<int> monthNumbers = [];
        // LN = Lunation Number. LN = 0 started with the first New Moon of 2000, approximately 2000-01-06T18:14+0000.
        for (var month = 0; month < monthsPerCycle; month++)
        {
            if (!IsFullMonth(month))
            {
                continue;
            }

            if (month % 2 != oddMonthsLong)
            {
                monthNumbers.Add(month);
            }

            longMonthCount++;
        }

        Console.WriteLine(
            $"All even months are long, plus these months within the {monthsPerCycle} month cycle: {string.Join(", ", monthNumbers)}");
        double calMonthLengthDays = 29 + longMonthCount / (double)monthsPerCycle;
        Console.WriteLine(
            $"This gives {longMonthCount} long months per {monthsPerCycle} months, which is an average of {calMonthLengthDays} days per month.");
        double driftDaysPerMonth =
            Abs(calMonthLengthDays - TimeConstants.DAYS_PER_LUNATION);
        double driftSecondsPerMonth = driftDaysPerMonth * TimeConstants.SECONDS_PER_DAY;
        Console.WriteLine(
            $"The drift is about {driftDaysPerMonth:N9} days or {driftSecondsPerMonth:N3} seconds per month.");
        double driftSecondsPerDay = driftSecondsPerMonth / calMonthLengthDays;
        double driftSecondsPerYear = driftSecondsPerDay * TimeConstants.DAYS_PER_TROPICAL_YEAR;
        double cycleLengthDays = monthsPerCycle * calMonthLengthDays;
        double cycleLengthYears = cycleLengthDays / TimeConstants.DAYS_PER_TROPICAL_YEAR;
        Console.WriteLine($"A cycle length is about {cycleLengthYears:N2} years.");
        double driftSecondsPerCycle = driftSecondsPerMonth * monthsPerCycle;
        Console.WriteLine(
            $"Thus, the drift is about {driftSecondsPerYear:N3} seconds per year, or {driftSecondsPerCycle:N3} seconds per cycle (not counting the changing length of the synodic month, which is increasing by about 0.022 seconds per century.");
    }

    public static void LeapMonthCounter()
    {
        int cycleLength = 5000;
        int nLeapMonths = 0;
        int totalDays = 0;
        for (int i = 0; i < cycleLength; i++)
        {
            if (IsFullMonth2(i))
            {
                nLeapMonths++;
                totalDays += 29;
            }
            else
            {
                totalDays += 28;
            }
        }
        double avgMonthLength = (double)totalDays / cycleLength;
        Console.WriteLine($"Number of leap months in {cycleLength} months is {nLeapMonths}");
        Console.WriteLine($"Average calendar month length is {avgMonthLength} days.");
    }

    public static void LeapMonthCounterNegativeMonths()
    {
        int cycleLength = 5000;
        int nLeapMonths = 0;
        int totalDays = 0;
        for (int i = -cycleLength; i < 0; i++)
        {
            if (IsFullMonth2(i))
            {
                nLeapMonths++;
                totalDays += 28;
            }
            else
            {
                totalDays += 29;
            }
        }
        double avgMonthLength = (double)totalDays / cycleLength;
        Console.WriteLine($"Number of leap months in {cycleLength} months is {nLeapMonths}");
        Console.WriteLine($"Average calendar month length is {avgMonthLength} days.");
    }

    /// <summary>
    /// This program finds a formula for determining full months that closely matches the average
    /// lunation.
    /// This uses the shorter modulo chain formula.
    /// </summary>
    public static void FindFullMonthRule1(int nLeapMonthsWanted, int nMonthsInCycle)
    {
        // Function to determine if a month is a leap month.
        bool isLeapMonth(int m, int a, int b, int r) => m % 2 == a || m % nMonthsInCycle % b == r;

        bool foundSolution = false;

        // Test possible values for n and r.
        for (int a2 = 0; a2 <= 1; a2++)
        {
            for (int b2 = 2; b2 < nMonthsInCycle; b2++)
            {
                for (var r2 = 0; r2 < b2; r2++)
                {
                    // Test the formula with this combination of a, b, and r.
                    int nLeapMonths = 0;
                    List<int> gaps = new ();
                    int gap = 0;

                    for (var m2 = 0; m2 < nMonthsInCycle; m2++)
                    {
                        gap++;
                        if (isLeapMonth(m2, a2, b2, r2))
                        {
                            nLeapMonths++;
                            if (!gaps.Contains(gap))
                            {
                                gaps.Add(gap);
                            }
                            gap = 0;
                        }
                    }

                    if (b2 <= 20 && nLeapMonths == nLeapMonthsWanted)
                    {
                        double frac2 = (double)nLeapMonths / nMonthsInCycle;
                        double avgMonthLength = 29 + frac2;
                        Console.WriteLine($"{a2}, {b2}, {r2}");
                        Console.WriteLine(
                            $"The number of leap months in {nMonthsInCycle} months is {nLeapMonths}.");
                        Console.WriteLine(
                            $"The average calendar month length is {avgMonthLength} days.");
                        Console.Write("Gaps: ");
                        gaps.Sort();
                        Console.WriteLine(string.Join(", ", gaps));
                        Console.WriteLine(
                            $"isLeapMonth(m) => m % 2 == {a2} || m % {nMonthsInCycle} % {b2} == {r2}");
                        Console.WriteLine();

                        for (var m2 = 0; m2 < nMonthsInCycle; m2++)
                        {
                            Console.Write(isLeapMonth(m2, a2, b2, r2) ? 1 : 0);
                            if (m2 % 80 == 79)
                            {
                                Console.WriteLine();
                            }
                        }
                        return;
                    }
                }
            }
        }

        if (!foundSolution)
        {
            Console.WriteLine("No solution found.");
        }
    }

    /// <summary>
    /// This program finds a formula for determining full months that closely matches the average
    /// lunation.
    /// </summary>
    public static void FindFullMonthRule2(int nLeapMonthsWanted, int nMonthsInCycle)
    {
        // Function to determine if a month is a leap month.
        bool isLeapMonth(int m, int a, int b, int r) => m % nMonthsInCycle % a % b == r;

        bool foundSolution = false;

        // Test possible values for n and r.
        int aMin = 2;
        int aMax = nMonthsInCycle - 1;
        for (int a2 = aMin; a2 <= aMax; a2++)
        {
            for (int b2 = 2; b2 < a2; b2++)
            {
                for (var r2 = 0; r2 < b2; r2++)
                {
                    // Test the formula with this combination of a, b, and r.
                    int nLeapMonths = 0;
                    List<int> gaps = new ();
                    int gap = 0;

                    for (var m2 = 0; m2 < nMonthsInCycle; m2++)
                    {
                        gap++;
                        if (isLeapMonth(m2, a2, b2, r2))
                        {
                            nLeapMonths++;
                            if (!gaps.Contains(gap))
                            {
                                gaps.Add(gap);
                            }
                            gap = 0;
                        }
                    }

                    if (nLeapMonths == nLeapMonthsWanted)
                    {
                        foundSolution = true;
                        double frac2 = (double)nLeapMonths / nMonthsInCycle;
                        double avgMonthLength = 29 + frac2;
                        Console.WriteLine($"{a2}, {b2}, {r2}");
                        Console.WriteLine(
                            $"When a = {a2}, b = {b2}, and r = {r2}, the number of leap months in {nMonthsInCycle} months is {nLeapMonths}.");
                        Console.WriteLine(
                            $"The average calendar month length is {avgMonthLength} days.");
                        Console.Write("Gaps: ");
                        gaps.Sort();
                        Console.WriteLine(string.Join(", ", gaps));
                        Console.WriteLine(
                            $"isLeapMonth(m) => m % {nMonthsInCycle} % {a2} % {b2} == {r2}");
                        Console.WriteLine();
                        // return;
                    }
                }
            }
        }

        if (!foundSolution)
        {
            Console.WriteLine("No solution found.");
        }
    }

    public static void FindFullMonthFraction()
    {
        double frac = DoubleExtensions.Frac(TimeConstants.DAYS_PER_LUNATION);
        const int DAYS_IN_HOLLOW_MONTH = 29;

        // Ignore any solution that produces an error more than 1 hour.
        double maxDiff_s = 3600.0;
        // Keep track of the best solution we've found so far.
        double smallestDiffSoFar_s = double.MaxValue;

        // Show column headings.
        Console.WriteLine(
            $"{"Fraction",15}{"Fraction (d)",25}{"Avg. month length (d)",25}{"Error (sec)",25}");
        Console.WriteLine("=".Repeat(95));

        int den = 2;
        while (true)
        {
            int num = (int)Round(frac * den);
            double frac2 = (double)num / den;
            double avgMonthLength = DAYS_IN_HOLLOW_MONTH + frac2;
            double diff_d = Abs(frac - frac2);
            double diff_s = diff_d * TimeConstants.SECONDS_PER_DAY;

            if (diff_s <= maxDiff_s && diff_s < smallestDiffSoFar_s)
            {
                smallestDiffSoFar_s = diff_s;
                string rational = $"{num} / {den}";
                Console.WriteLine(
                    $"{rational,15}{frac2,25:F9}{avgMonthLength,25:F9}{diff_s,25:F9}");

                if (den > 12628)
                {
                    break;
                }
            }
            den++;
        }
    }

    public static void FindIntercalationFraction()
    {
        double lunationsPerTropicalYear = TimeConstants.DAYS_PER_TROPICAL_YEAR
            / TimeConstants.DAYS_PER_LUNATION;
        double frac = DoubleExtensions.Frac(lunationsPerTropicalYear);

        int num;
        int den = 2;
        // Ignore any solution that produces an error more than 1 hour.
        double maxDiff_s = 3600;
        // What's our target?
        double goalDiff_s = 1;
        // Keep track of the best we've found so far.
        double smallestDiffSoFar_s = double.MaxValue;

        // Show column headings.
        Console.WriteLine(
            $"{"Fraction",10}{"Fraction",25}{"Num. lunations",20}{"Num. days",25}{"Avg. year length (d)",25}{"Error (sec)",25}");
        Console.WriteLine("=".Repeat(105));

        while (true)
        {
            num = (int)Round(den * frac);
            double frac2 = (double)num / den;
            double avgYearLength_d = (12 + frac2) * TimeConstants.DAYS_PER_LUNATION;
            double diff_d = Abs(TimeConstants.DAYS_PER_TROPICAL_YEAR - avgYearLength_d);
            double diff_s = diff_d * 86400;

            // Output any result that is the best so far, and not more than maxDiff.
            if (diff_s <= maxDiff_s && diff_s < smallestDiffSoFar_s)
            {
                int numLunations = den * 12 + num;
                double numDays = numLunations * TimeConstants.DAYS_PER_LUNATION;
                double fracDays = DoubleExtensions.Frac(numDays);
                double fracDays_s = fracDays * TimeConstants.SECONDS_PER_DAY;
                string rational = $"{num} / {den}";
                Console.WriteLine(
                    $"{rational,10}{frac2,25:F9}{numLunations,20}{numDays,25:F9}{avgYearLength_d,25:F9}{diff_s,25:F9}");

                if (diff_s <= goalDiff_s && fracDays_s <= 3600)
                {
                    break;
                }

                smallestDiffSoFar_s = diff_s;
            }

            den++;
        }
    }

    public static void FindYearLengthRule()
    {
        double bestDiff_s = double.MaxValue;
        const double LUNATIONS_PER_TROPICAL_YEAR =
            TimeConstants.DAYS_PER_TROPICAL_YEAR / TimeConstants.DAYS_PER_LUNATION;

        for (int a = 0; a < 1000; a++)
        {
            for (int b = 0; b <= 0; b++)
            {
                for (int c = 0; c < 1000; c++)
                {
                    for (int d = 0; d < 1000; d++)
                    {
                        int numDays = a * 354 + b * 355 + c * 383 + d * 384;
                        int numYears = a + b + c + d;

                        // Get the difference in seconds from the tropical year.
                        double avgYearLength = (double)numDays / numYears;
                        double diffFromTropicalYear_s =
                            (avgYearLength - TimeConstants.DAYS_PER_TROPICAL_YEAR)
                            * TimeConstants.SECONDS_PER_DAY;

                        // How many lunations?
                        int numLunations =
                            (int)Round(numDays / TimeConstants.DAYS_PER_LUNATION);

                        // Get the difference in seconds from the lunation.
                        double avgMonthLength = (double)numDays / numLunations;
                        double diffFromLunation_s =
                            (avgMonthLength - TimeConstants.DAYS_PER_LUNATION)
                            * TimeConstants.SECONDS_PER_DAY;
                        double annualDiffFromLunations_s =
                            diffFromLunation_s * LUNATIONS_PER_TROPICAL_YEAR;

                        double avgAnnualDiffBetweenLunationsAndTropicalYears_s =
                            Abs(numYears * TimeConstants.DAYS_PER_TROPICAL_YEAR
                                - numLunations * TimeConstants.DAYS_PER_LUNATION)
                            / numYears
                            * TimeConstants.SECONDS_PER_DAY;

                        if (Abs(diffFromTropicalYear_s) <= 10
                            && Abs(diffFromLunation_s) <= 1
                            && avgAnnualDiffBetweenLunationsAndTropicalYears_s < bestDiff_s)
                        {
                            Console.WriteLine(
                                $"Found solution: a={a}, b={b}, c={c}, d={d}, numDays={numDays}, numYears={numYears}, numLunations={numLunations}");
                            Console.WriteLine(
                                $"Average annual error in seconds from tropical year = {diffFromTropicalYear_s}");
                            Console.WriteLine(
                                $"Average annual error in seconds from lunation = {annualDiffFromLunations_s}");
                            Console.WriteLine(
                                $"Average annual difference between tropical years and lunations = {avgAnnualDiffBetweenLunationsAndTropicalYears_s} seconds");
                            Console.WriteLine();
                            bestDiff_s = avgAnnualDiffBetweenLunationsAndTropicalYears_s;
                        }
                    }
                }
            }
        }
    }

    public static void FindMonthLengthFormula()
    {
        bool isFullMonth(int y, int m, int a, int b, int r)
        {
            int monthsInyear = MonthsInYear(y);

            if (m < 1 || m > monthsInyear)
                throw new ArgumentOutOfRangeException(nameof(m),
                    $"Month number must be in the range 1..{monthsInyear} for the year {y}.");

            // For the first 12 months of the year, just alternate 29 and 30.
            if (m <= 12) return m % 2 == 0;

            // If there's a 13th month, use a mod chain rule to get the result from the year.
            return y % a % b == r;
        }

        double calcAvgMonthLength(int nYears, int a, int b, int r)
        {
            // Count the number of days and months in the cycle.
            int nMonths = 0;
            int nDays = 0;
            for (int y = 0; y < nYears; y++)
            {
                int nMonthsInYear = MonthsInYear(y);
                nMonths += nMonthsInYear;
                for (int m = 1; m <= nMonthsInYear; m++)
                {
                    int nDaysInMonth = isFullMonth(y, m, a, b, r) ? 30 : 29;
                    nDays += nDaysInMonth;
                }
            }
            // Calculate the average.
            return (double)nMonths / nDays;
        }

        int nYears = 1021;
        double bestDiff = double.MaxValue;
        for (int a = 2; a < nYears; a++)
        {
            Console.WriteLine($"Trying a={a}...");
            for (int b = 2; b < a; b++)
            {
                for (int r = 0; r < b; r++)
                {
                    double avgMonthLength_d = calcAvgMonthLength(nYears, a, b, r);
                    double diff_s = Abs(avgMonthLength_d - TimeConstants.DAYS_PER_LUNATION)
                        * TimeConstants.SECONDS_PER_DAY;
                    if (diff_s < bestDiff)
                    {
                        Console.WriteLine($"a={a}, b={b}, r={r}, diff={diff_s}");
                        bestDiff = diff_s;
                        // break;
                    }
                }
            }
        }
    }

    public static void FindSynchronousCycles()
    {
        // for (int nYears = 1; nYears <= 10000; nYears++)
        // {
        int wholeYears = 2729;
        double tropicalYears_d = wholeYears * TimeConstants.DAYS_PER_TROPICAL_YEAR;
        int wholeDays = (int)Round(tropicalYears_d);
        double nLunations = tropicalYears_d / TimeConstants.DAYS_PER_LUNATION;
        int wholeLunations = (int)Round(nLunations);
        double lunations_d = wholeLunations * TimeConstants.DAYS_PER_LUNATION;

        double smallest = Min(wholeDays, lunations_d, tropicalYears_d);
        double largest = Max(wholeDays, lunations_d, tropicalYears_d);
        double diff_d = Abs(smallest - largest);
        double diff_s = diff_d * TimeConstants.SECONDS_PER_DAY;

        double avgYearLength_d = (double)wholeDays / wholeYears;
        double yearError_s = Abs(avgYearLength_d - TimeConstants.DAYS_PER_TROPICAL_YEAR)
            * TimeConstants.SECONDS_PER_DAY;

        double avgMonthLength_d = (double)wholeDays / wholeLunations;
        double monthError_s = Abs(avgMonthLength_d - TimeConstants.DAYS_PER_LUNATION)
            * TimeConstants.SECONDS_PER_DAY;

        // if (diffInSeconds <= 3600)
        // {
        Console.WriteLine(
            $"nYears={wholeYears}, wholeDays={wholeDays}, wholeLunations={wholeLunations}");
        Console.WriteLine($"{wholeYears} tropical years = {tropicalYears_d} days");
        Console.WriteLine($"{wholeLunations} lunations = {lunations_d} days");
        Console.WriteLine($"diff = {diff_d} days = {diff_s} seconds");
        Console.WriteLine($"avg year length = {avgYearLength_d} days");
        Console.WriteLine($"year error = {yearError_s} seconds");
        Console.WriteLine($"month error = {monthError_s} seconds");
        Console.WriteLine();
        // }
        // }
    }

    /// <summary>
    /// Find a date when a New Moon occurs and a year begins.
    /// </summary>
    public void FindEpoch()
    {
        // Find all the New Moons in a 25-year period.
        DateTime start = new (2025, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        DateTime end = new (2050, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        List<LunarPhaseEvent> newMoons =
            moonService.GetPhasesInPeriod(start, end, ELunarPhaseType.NewMoon);
        foreach (LunarPhaseEvent newMoon in newMoons)
        {
            // Get Ls.
            (double Ls, double Bs, double Rs) = sunService.CalcPosition(newMoon.DateTimeUtc);
            double LsDeg = Angles.RadiansToDegrees(Ls);

            // Check for New Moon within 1° of the northward equinox.
            double diff = Abs(LsDeg);
            if (diff < 1)
            {
                DateTime dtEquinox =
                    seasonalMarkerService.GetSeasonalMarkerAsDateTime(newMoon.DateTimeUtc.Year,
                        ESeasonalMarkerType.NorthwardEquinox);
                double diffDays =
                    Abs(dtEquinox.GetTotalDays() - newMoon.DateTimeUtc.GetTotalDays());
                Console.WriteLine();
                Console.WriteLine($"The New Moon of {newMoon.DateTimeUtc} occurred at Ls={LsDeg}°");
                Console.WriteLine($"The northward equinox occurred at {dtEquinox}");
                Console.WriteLine("Close match to northward equinox (northern spring equinox).");
                Console.WriteLine($"Difference = {diff}° or {diffDays} days.");
            }

            // Check for New Moon within 1° of the southern solstice.
            diff = Abs(LsDeg + 90);
            if (diff < 1)
            {
                DateTime dtSolstice =
                    seasonalMarkerService.GetSeasonalMarkerAsDateTime(newMoon.DateTimeUtc.Year,
                        ESeasonalMarkerType.SouthernSolstice);
                double diffDays =
                    Abs(dtSolstice.GetTotalDays() - newMoon.DateTimeUtc.GetTotalDays());
                Console.WriteLine();
                Console.WriteLine($"The New Moon of {newMoon.DateTimeUtc} occurred at Ls={LsDeg}°");
                Console.WriteLine($"The southern solstice occurred at {dtSolstice}");
                Console.WriteLine("Close match to southern solstice (northern winter solstice).");
                Console.WriteLine($"Difference = {diff}° or {diffDays} days.");
            }

            // Check for New Moon within 1° of the Besselian new year.
            diff = Abs(LsDeg + 80);
            if (diff < 1)
            {
                Console.WriteLine();
                Console.WriteLine($"The New Moon of {newMoon.DateTimeUtc} occurred at Ls={LsDeg}°");
                Console.WriteLine("Close match to Besselian New Year.");
                Console.WriteLine($"Difference = {diff}°");
            }

            // Check for New Moon within 1 day of the Gregorian New Year.
            DateTime nextNewYear =
                new (newMoon.DateTimeUtc.Year, 12, 31, 0, 0, 0, DateTimeKind.Utc);
            DateTime prevNewYear = new (newMoon.DateTimeUtc.Year - 1, 12, 31, 0, 0, 0,
                DateTimeKind.Utc);
            double diff1 =
                Abs(newMoon.DateTimeUtc.GetTotalDays() - nextNewYear.GetTotalDays());
            double diff2 =
                Abs(newMoon.DateTimeUtc.GetTotalDays() - prevNewYear.GetTotalDays());
            if (diff1 < 1)
            {
                Console.WriteLine();
                Console.WriteLine(
                    $"The New Moon of {newMoon.DateTimeUtc} is a close match to Gregorian New Year.");
                Console.WriteLine($"Difference = {diff1} days");
            }
            else if (diff2 < 1)
            {
                Console.WriteLine();
                Console.WriteLine(
                    $"The New Moon of {newMoon.DateTimeUtc} is a close match to Gregorian New Year.");
                Console.WriteLine($"Difference = {diff2} days");
            }
        }
    }

    public void FindSynchronisationPoints()
    {
        // For some reason this breaks after year 5000. Some error in the lunar phase calculator;
        // perhaps the Meeus algorithm doesn't support years later than that, although there's no
        // year range specified.
        for (int y = 2052; y < 5000; y++)
        {
            // Get the southern solstice.
            DateTime solstice =
                seasonalMarkerService.GetSeasonalMarkerAsDateTime(y,
                    ESeasonalMarkerType.SouthernSolstice);

            // Check if there's also a New Moon at this time.
            LunarPhaseEvent newMoon = moonService.GetPhaseNearDateTimeHumble(solstice);
            if (newMoon.PhaseType != ELunarPhaseType.NewMoon)
            {
                continue;
            }

            // Check the difference isn't too large.
            TimeSpan diff = solstice - newMoon.DateTimeUtc;
            diff = TimeSpanExtensions.Abs(diff);
            if (diff.Ticks >= TimeConstants.TICKS_PER_HOUR)
            {
                continue;
            }

            // Print candidate.
            Console.WriteLine();
            Console.WriteLine($"Alignment in year {y}:");
            Console.WriteLine($"{"Southern Solstice",20}: {solstice:R}");
            Console.WriteLine($"{newMoon.PhaseType.GetDisplayName(),20}: {newMoon.DateTimeUtc:R}");
            Console.WriteLine(
                $"{"Difference",20}: {TimeSpanExtensions.GetTimeString(diff, ETimeUnit.Minute)}");
        }
    }
}
