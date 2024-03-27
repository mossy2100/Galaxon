using Galaxon.Astronomy.Algorithms.Services;
using Galaxon.Astronomy.Data;
using Galaxon.Astronomy.Data.Enums;
using Galaxon.Astronomy.Data.Models;
using Galaxon.Astronomy.Data.Repositories;
using Galaxon.Core.Functional;
using Galaxon.Core.Strings;
using Galaxon.Numerics.Extensions;
using Galaxon.Time;
using static Galaxon.Numerics.Extensions.NumberExtensions;

namespace Galaxon.ConsoleApp;

public static class LunisolarCalendar
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
            Math.Abs(calMonthLengthDays - TimeConstants.DAYS_PER_LUNATION);
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
                        foundSolution = true;
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
        int daysInHollowMonth = (int)Math.Truncate(TimeConstants.DAYS_PER_LUNATION);

        // Ignore any solution that produces an error more than 1 hour.
        double maxDiffInSeconds = 3600.0;
        // Keep track of the best solution we've found so far.
        double smallestDiffSoFarInSeconds = double.MaxValue;

        // Show column headings.
        Console.WriteLine(
            $"{"Fraction",15}{"Fraction (d)",25}{"Avg. month length (d)",25}{"Error (sec)",25}");
        Console.WriteLine("=".Repeat(95));

        int den = 2;
        while (true)
        {
            int num = (int)Math.Round(frac * den);
            double frac2 = (double)num / den;
            double avgMonthLength = daysInHollowMonth + frac2;
            double diffInDays = Math.Abs(frac - frac2);
            double diffInSeconds = diffInDays * TimeConstants.SECONDS_PER_DAY;

            if (diffInSeconds <= maxDiffInSeconds && diffInSeconds < smallestDiffSoFarInSeconds)
            {
                smallestDiffSoFarInSeconds = diffInSeconds;
                string rational = $"{num} / {den}";
                Console.WriteLine(
                    $"{rational,15}{frac2,25:F9}{avgMonthLength,25:F9}{diffInSeconds,25:F9}");

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
        double maxDiffInSeconds = 3600;
        // What's our target?
        double goalDiffInSeconds = 1;
        // Keep track of the best we've found so far.
        double smallestDiffSoFarInSeconds = double.MaxValue;

        // Show column headings.
        Console.WriteLine(
            $"{"Fraction",10}{"Fraction",25}{"Num. lunations",20}{"Num. days",25}{"Avg. year length (d)",25}{"Error (sec)",25}");
        Console.WriteLine("=".Repeat(105));

        while (true)
        {
            num = (int)Math.Round(den * frac);
            double frac2 = (double)num / den;
            double avgYearLengthInDays = (12 + frac2) * TimeConstants.DAYS_PER_LUNATION;
            double diffInDays =
                Math.Abs(TimeConstants.DAYS_PER_TROPICAL_YEAR - avgYearLengthInDays);
            double diffInSeconds = diffInDays * 86400;

            // Output any result that is the best so far, and not more than maxDiffInSeconds.
            if (diffInSeconds <= maxDiffInSeconds && diffInSeconds < smallestDiffSoFarInSeconds)
            {
                int numLunations = den * 12 + num;
                double numDays = numLunations * TimeConstants.DAYS_PER_LUNATION;
                double fracDays = DoubleExtensions.Frac(numDays);
                double fracDaysInSeconds = fracDays * TimeConstants.SECONDS_PER_DAY;
                string rational = $"{num} / {den}";
                Console.WriteLine(
                    $"{rational,10}{frac2,25:F9}{numLunations,20}{numDays,25:F9}{avgYearLengthInDays,25:F9}{diffInSeconds,25:F9}");

                if (diffInSeconds <= goalDiffInSeconds && fracDaysInSeconds <= 3600)
                {
                    break;
                }

                smallestDiffSoFarInSeconds = diffInSeconds;
            }

            den++;
        }
    }

    public static void FindYearLengthRule()
    {
        double bestDiff = double.MaxValue;
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
                        double diffFromTropicalYearInSeconds =
                            (avgYearLength - TimeConstants.DAYS_PER_TROPICAL_YEAR)
                            * TimeConstants.SECONDS_PER_DAY;

                        // How many lunations?
                        int numLunations =
                            (int)Math.Round(numDays / TimeConstants.DAYS_PER_LUNATION);

                        // Get the difference in seconds from the lunation.
                        double avgMonthLength = (double)numDays / numLunations;
                        double diffFromLunationInSeconds =
                            (avgMonthLength - TimeConstants.DAYS_PER_LUNATION)
                            * TimeConstants.SECONDS_PER_DAY;
                        double annualDiffFromLunationsInSeconds =
                            diffFromLunationInSeconds * LUNATIONS_PER_TROPICAL_YEAR;

                        double avgAnnualdiffBetweenLunationsAndTropicalYears =
                            Math.Abs(numYears * TimeConstants.DAYS_PER_TROPICAL_YEAR
                                - numLunations * TimeConstants.DAYS_PER_LUNATION)
                            / numYears
                            * TimeConstants.SECONDS_PER_DAY;

                        if (Math.Abs(diffFromTropicalYearInSeconds) <= 10
                            && Math.Abs(diffFromLunationInSeconds) <= 1
                            && avgAnnualdiffBetweenLunationsAndTropicalYears < bestDiff)
                        {
                            Console.WriteLine(
                                $"Found solution: a={a}, b={b}, c={c}, d={d}, numDays={numDays}, numYears={numYears}, numLunations={numLunations}");
                            Console.WriteLine(
                                $"Average annual error in seconds from tropical year = {diffFromTropicalYearInSeconds}");
                            Console.WriteLine(
                                $"Average annual error in seconds from lunation = {annualDiffFromLunationsInSeconds}");
                            Console.WriteLine(
                                $"Average annual difference between tropical years and lunations = {avgAnnualdiffBetweenLunationsAndTropicalYears} seconds");
                            Console.WriteLine();
                            bestDiff = avgAnnualdiffBetweenLunationsAndTropicalYears;
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
                    double avgMonthLengthInDays = calcAvgMonthLength(nYears, a, b, r);
                    double diffInSeconds =
                        Math.Abs(avgMonthLengthInDays - TimeConstants.DAYS_PER_LUNATION)
                        * TimeConstants.SECONDS_PER_DAY;
                    if (diffInSeconds < bestDiff)
                    {
                        Console.WriteLine($"a={a}, b={b}, r={r}, diff={diffInSeconds}");
                        bestDiff = diffInSeconds;
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
        double tropicalYearsInDays = wholeYears * TimeConstants.DAYS_PER_TROPICAL_YEAR;
        int wholeDays = (int)Math.Round(tropicalYearsInDays);
        double nLunations = tropicalYearsInDays / TimeConstants.DAYS_PER_LUNATION;
        int wholeLunations = (int)Math.Round(nLunations);
        double lunationsInDays = wholeLunations * TimeConstants.DAYS_PER_LUNATION;

        double smallest = Min(wholeDays, lunationsInDays, tropicalYearsInDays);
        double largest = Max(wholeDays, lunationsInDays, tropicalYearsInDays);
        double diffInDays = Math.Abs(smallest - largest);
        double diffInSeconds = diffInDays * TimeConstants.SECONDS_PER_DAY;

        double avgYearLength = (double)wholeDays / wholeYears;
        double yearErrorInSeconds =
            Math.Abs(avgYearLength - TimeConstants.DAYS_PER_TROPICAL_YEAR)
            * TimeConstants.SECONDS_PER_DAY;

        double avgMonthLength = (double)wholeDays / wholeLunations;
        double monthErrorInSeconds =
            Math.Abs(avgMonthLength - TimeConstants.DAYS_PER_LUNATION)
            * TimeConstants.SECONDS_PER_DAY;

        // if (diffInSeconds <= 3600)
        // {
        Console.WriteLine(
            $"nYears={wholeYears}, wholeDays={wholeDays}, wholeLunations={wholeLunations}");
        Console.WriteLine($"{wholeYears} tropical years = {tropicalYearsInDays} days");
        Console.WriteLine($"{wholeLunations} lunations = {lunationsInDays} days");
        Console.WriteLine($"diffInDays = {diffInDays}");
        Console.WriteLine($"diffInSeconds = {diffInSeconds}");
        Console.WriteLine($"avgYearLength = {avgYearLength} days");
        Console.WriteLine($"yearErrorInSeconds = {yearErrorInSeconds} seconds");
        Console.WriteLine($"monthErrorInSeconds = {monthErrorInSeconds} seconds");
        Console.WriteLine();
        // }
        // }
    }

    /// <summary>
    /// Find a date when a New Moon occurs and a year begins.
    /// </summary>
    public static void FindEpoch()
    {
        // Construct a SunService.
        AstroDbContext astroDbContext = new ();
        AstroObjectGroupRepository astroObjectGroupRepository = new (astroDbContext);
        AstroObjectRepository astroObjectRepository =
            new (astroDbContext, astroObjectGroupRepository);
        PlanetService planetService = new (astroDbContext);
        EarthService earthService = new (astroObjectRepository, planetService);
        var sunService = new SunService(earthService);

        // Find all the New Moons in a 25-year period.
        DateTime start = new (2000, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        DateTime end = new (2025, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        List<LunarPhase> newMoons =
            LunaService.GetPhasesInPeriod(start, end, ELunarPhaseType.NewMoon);
        foreach (LunarPhase newMoon in newMoons)
        {
            // Get Ls.
            (double Ls, double Bs, double Rs) = sunService.CalcPosition(newMoons[0].DateTimeUTC);
            Console.WriteLine($"The New Moon of {newMoons[0].DateTimeUTC} occurred at Ls={Ls}");
        }
    }
}
