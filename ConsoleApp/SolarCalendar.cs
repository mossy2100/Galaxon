using Galaxon.Numerics.Extensions;
using Galaxon.Time;
using static Galaxon.Numerics.Extensions.NumberExtensions;

namespace Galaxon.ConsoleApp;

public class SolarCalendar
{
    /// <summary>
    /// 31/128 leap year methods.
    /// </summary>
    /// <param name="year"></param>
    /// <returns></returns>
    public static bool IsLeapYear1(int year)
    {
        return Mod(year, 4) == 0 && Mod(year, 128) != 0;
    }

    /// <summary>
    /// Formula: 5000 % 95 % 4 == 3
    /// </summary>
    public static bool IsLeapYear2(int y)
    {
        return Mod(Mod(Mod(y, 5000), 95), 4) == 3;
    }

    /// <summary>
    /// Formula: 5000 % 128 % 29 % 4 == 3
    /// </summary>
    public static bool IsLeapYear3(int y)
    {
        return Mod(Mod(Mod(Mod(y, 5000), 128), 29), 4) == 3;
    }

    /// <summary>
    /// This program finds a formula for determining leap years that closely matches the mean
    /// tropical year.
    /// </summary>
    public static void FindSolution(int nLeapYearsWanted, int nYearsInCycle)
    {
        // Function to determine if a year is a leap year.
        bool isLeapYear(int y, int n, int r) => y % nYearsInCycle % n % 4 == r;

        bool foundSolution = false;

        // Test possible values for n and r.
        for (int n = 2; n < nYearsInCycle; n++)
        {
            for (var r = 0; r < 4; r++)
            {
                int nLeapYears = 0;
                List<int> gaps = new ();
                int gap = 0;

                for (var y = 0; y < nYearsInCycle; y++)
                {
                    gap++;
                    if (isLeapYear(y, n, r))
                    {
                        nLeapYears++;
                        if (!gaps.Contains(gap))
                        {
                            gaps.Add(gap);
                        }
                        gap = 0;
                    }
                }

                if (nLeapYears == nLeapYearsWanted)
                {
                    foundSolution = true;
                    double avgYearLength = 365 + (double)nLeapYears / nYearsInCycle;
                    Console.WriteLine(
                        $"When n = {n} and r = {r}, the number of leap years in {nYearsInCycle} years is {nLeapYears}.");
                    Console.WriteLine($"The average calendar year length is {avgYearLength} days.");
                    Console.Write("Gaps: ");
                    gaps.Sort();
                    Console.WriteLine(string.Join(", ", gaps));
                    Console.WriteLine($"isLeapYear(y) => {nYearsInCycle} % {n} % 4 == {r}");
                    Console.WriteLine();
                }
            }
        }

        if (!foundSolution)
        {
            Console.WriteLine("No solution found.");
        }
    }

    public static void LeapYearCounter()
    {
        int cycleLength = 5000;
        int nLeapYears = 0;
        int totalDays = 0;
        for (int i = 0; i < cycleLength; i++)
        {
            if (IsLeapYear2(i))
            {
                nLeapYears++;
                totalDays += 366;
            }
            else
            {
                totalDays += 365;
            }
        }
        double avgYearLength = (double)totalDays / cycleLength;
        Console.WriteLine($"Number of leap years in {cycleLength} years is {nLeapYears}");
        Console.WriteLine($"Average calendar year length is {avgYearLength} days.");
    }

    public static void LeapYearCounterNegativeYears()
    {
        int cycleLength = 5000;
        int nLeapYears = 0;
        int totalDays = 0;
        for (int i = -cycleLength; i < 0; i++)
        {
            if (IsLeapYear2(i))
            {
                nLeapYears++;
                totalDays += 366;
            }
            else
            {
                totalDays += 365;
            }
        }
        double avgYearLength = (double)totalDays / cycleLength;
        Console.WriteLine($"Number of leap years in {cycleLength} years is {nLeapYears}");
        Console.WriteLine($"Average calendar year length is {avgYearLength} days.");
    }

    /// <summary>
    /// This program finds a formula for determining leap years that closely matches the mean
    /// tropical year.
    /// </summary>
    public static void FindSolution2(int regularYearLength, int nLeapYearsWanted, int nYearsInCycle)
    {
        // Function to determine if a year is a leap year.
        bool isLeapYear(int y, int n, int m, int p, int r) => y % nYearsInCycle % n % m % p == r;

        // Calculate the average year length desired:
        double avgYearLengthWanted = regularYearLength + (double)nLeapYearsWanted / nYearsInCycle;
        Console.WriteLine($"Average year length wanted = {avgYearLengthWanted} days.");

        // Calculate p, being the last term in the modulo chain.
        double frac = (double)nYearsInCycle / nLeapYearsWanted;
        int p = (int)Math.Round(frac);
        Console.WriteLine($"p = {p}");

        // Calculate the min and max gap.
        int minGap = (int)Math.Floor(frac);
        int maxGap = (int)Math.Ceiling(frac);

        bool foundSolution = false;

        // Test possible parameters for the formula.
        for (int n = 2; n < nYearsInCycle; n++)
        {
            for (int m = 2; m < n; m++)
            {
                for (int r = 0; r < p; r++)
                {
                    int nLeapYears = 0;
                    List<int> gaps = new ();
                    int gap = 0;

                    for (var y = 0; y < nYearsInCycle; y++)
                    {
                        gap++;
                        if (isLeapYear(y, n, m, p, r))
                        {
                            nLeapYears++;
                            if (!gaps.Contains(gap))
                            {
                                gaps.Add(gap);
                            }
                            gap = 0;
                        }
                    }

                    if (nLeapYears == nLeapYearsWanted
                        && gaps.Min() == minGap
                        && gaps.Max() == maxGap)
                    {
                        double avgYearLength =
                            regularYearLength + (double)nLeapYears / nYearsInCycle;
                        Console.WriteLine("SOLUTION FOUND");
                        Console.WriteLine(
                            $"When n = {n}, m = {m}, p = {p}, and r = {r}, the number of leap years in {nYearsInCycle} years is {nLeapYears}.");
                        Console.WriteLine(
                            $"The average calendar year length is {avgYearLength} days.");
                        Console.Write("Gaps: ");
                        gaps.Sort();
                        Console.WriteLine(string.Join(", ", gaps));
                        Console.WriteLine();
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

    public static void FractionFinder(double frac, int maxDen)
    {
        double? minDiff = null;
        int bestNum = 0;
        int bestDen = 0;

        for (int den = 2; den <= maxDen; den++)
        {
            // Console.WriteLine($"den = {den}");
            // Calculate an approximation for the numerator.
            double num = den * frac;

            // Try the integer below.
            int num1 = (int)Math.Floor(num);
            TestFraction(frac, num1, den, ref minDiff, ref bestNum, ref bestDen);

            // Try the integer above.
            int num2 = (int)Math.Ceiling(num);
            TestFraction(frac, num2, den, ref minDiff, ref bestNum, ref bestDen);
        }
    }

    private static void TestFraction(double targetFrac, int num, int den, ref double? minDiff,
        ref int bestNum, ref int bestDen)
    {
        double frac = (double)num / den;
        // Console.WriteLine($"Test frac = {frac}");
        double diff = Math.Abs(targetFrac - frac);

        // Convert difference to seconds.
        double seconds = diff * 89428.3286475439;

        // If the difference is worse then 10 seconds, ignore.
        if (seconds > 10) return;

        // See if this is the best solution so far.
        if (minDiff == null || diff < minDiff)
        {
            Console.WriteLine(
                $"New best: {num}/{den}, gives a fraction of {frac}, drift is {seconds} seconds per year.");
            minDiff = diff;
            bestNum = num;
            bestDen = den;
            FindSolution2(342, num, den);
            Console.WriteLine();
        }
    }

    public static void FindLeapYearFraction()
    {
        double frac = DoubleExtensions.Frac(TimeConstants.DAYS_PER_TROPICAL_YEAR);
        List<double> fractions = [];
        for (var den = 1; den <= 10000; den++)
        {
            // Console.WriteLine($"Testing d = {d}...");
            var num = (int)Math.Round(frac * den);
            double frac2 = num / (double)den;

            // Eliminate duplicates.
            if (fractions.Contains(frac2))
            {
                continue;
            }
            fractions.Add(frac2);

            double diffSeconds = Math.Abs(frac - frac2) * TimeConstants.SECONDS_PER_DAY;
            if (diffSeconds <= 1)
            {
                Console.WriteLine(
                    $"Match: {num} / {den} = {frac2} (drift is about {diffSeconds:N3} seconds per year)");
            }
        }

        Console.WriteLine("Done.");
    }

    public static void TestLeapYearRule1()
    {
        var leapYearCount = 0;
        var yearsPerCycle = 128;
        var gapLengthYears = 4;
        Console.WriteLine(
            $"The rule: isLeapYear = (m % {gapLengthYears} == 0) && (m % {yearsPerCycle} != 0)");
        List<int> yearNumbers = [];
        for (var y = 0; y < yearsPerCycle; y++)
        {
            if (IsLeapYear1(y))
            {
                yearNumbers.Add(y);
                leapYearCount++;
            }
        }

        Console.WriteLine(
            $"The following years within the {yearsPerCycle} year cycle are leap years: {string.Join(", ", yearNumbers)}");
        double calYearLengthDays = 365 + leapYearCount / (double)yearsPerCycle;
        Console.WriteLine(
            $"This gives {leapYearCount} leap years per {yearsPerCycle} years, which is an average of {calYearLengthDays} days per year.");
        double driftDaysPerYear =
            Math.Abs(calYearLengthDays - TimeConstants.DAYS_PER_TROPICAL_YEAR);
        double driftSecondsPerYear = driftDaysPerYear * TimeConstants.SECONDS_PER_DAY;
        double driftSecondsPerCycle = driftSecondsPerYear * yearsPerCycle;
        Console.WriteLine(
            $"The drift is about {driftSecondsPerYear:N3} seconds per year, or {driftSecondsPerCycle:N3} seconds per cycle (not counting the changing length of the tropical year, which is decreasing by about 0.53 seconds per century).");
    }

    public static void TestLeapYearRule2()
    {
        var leapYearCount = 0;
        var a = 9005;
        var b = 4;
        var c = 128;
        Console.WriteLine($"The rule: isLeapYear = (y % {a} % {b} == 0) && (y % {a} % {c} != 0)");
        List<int> yearNumbers = [];
        for (var y = 0; y < a; y++)
        {
            if (IsLeapYear2(y % a))
            {
                yearNumbers.Add(y);
                leapYearCount++;
            }
        }

        Console.WriteLine(
            $"The following years within the {a} year cycle are leap years: {string.Join(", ", yearNumbers)}");
        double calYearLengthDays = 365 + leapYearCount / (double)a;
        Console.WriteLine(
            $"This gives {leapYearCount} leap years per {a} years, which is an average of {calYearLengthDays} days per year.");
        double driftDaysPerYear =
            Math.Abs(calYearLengthDays - TimeConstants.DAYS_PER_TROPICAL_YEAR);
        double driftSecondsPerYear = driftDaysPerYear * TimeConstants.SECONDS_PER_DAY;
        double driftSecondsPerCycle = driftSecondsPerYear * a;
        Console.WriteLine(
            $"The drift is about {driftSecondsPerYear:N3} seconds per year, or {driftSecondsPerCycle:N3} seconds per cycle (not counting the changing length of the tropical year, which is decreasing by about 0.53 seconds per century).");
    }
}
