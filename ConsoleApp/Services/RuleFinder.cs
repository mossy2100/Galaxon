namespace Galaxon.ConsoleApp.Services;

public class RuleFinder
{
    /// <summary>
    /// Test a leap year function.
    /// </summary>
    private static bool TestSolution(int num, int den, Func<int, bool> func, string funcString)
    {
        // Test the formula.
        int count = 0;
        int gap = 0;
        List<int> gaps = new ();
        for (int y = 0; y < den; y++)
        {
            if (func(y))
            {
                count++;
                gaps.Add(gap);
                gap = 0;
            }
            gap++;
        }

        // Invert fraction to find the min and max gaps.
        double avgGap = (double)den / num;
        int minGap = (int)double.Truncate(avgGap);
        int maxGap = minGap + 1;

        // Add any remaining gap to the first one.
        if (gaps.Count > 0)
        {
            gaps[0] += gap;
        }

        bool result = false;

        if (count == num && gaps.Min() == minGap && gaps.Max() == maxGap)
        {
            Console.WriteLine($"Found possible solution: {funcString}");
            Console.WriteLine($"Gaps: {string.Join(", ", gaps)}");
            Console.WriteLine($"Minimum gap: {gaps.Min()}");
            Console.WriteLine($"Maximum gap: {gaps.Max()}");
            Console.WriteLine();
            result = true;
        }

        return result;
    }

    /// <summary>
    /// Find the best formula using the modulo chain formula with 2 operations.
    /// </summary>
    public static bool FindRuleWith2Mods(int num, int den)
    {
        bool result = false;
        int a = (int)Round((double)den / num);
        for (int r = 0; r < a; r++)
        {
            Func<int, bool> isLeapYear = y => y % den % a == r;
            string funcString = $"y => y % {den} % {a} == {r}";
            if (TestSolution(num, den, isLeapYear, funcString))
            {
                result = true;
            }
        }
        return result;
    }

    /// <summary>
    /// Find the best formula using the modulo chain formula with 3 operations.
    /// </summary>
    public static bool FindRuleWith3Mods(int num, int den)
    {
        bool result = false;
        int b = (int)Round((double)den / num);
        for (int a = 2; a < den; a++)
        {
            for (int r = 0; r < b; r++)
            {
                Func<int, bool> isLeapYear = y => y % den % a % b == r;
                string funcString = $"y => y % {den} % {a} % {b} == {r}";
                if (TestSolution(num, den, isLeapYear, funcString))
                {
                    result = true;
                }
            }
        }
        return result;
    }

    /// <summary>
    /// Find the best formula using the modulo chain formula with 3 operations.
    /// </summary>
    public static bool FindRuleWith4Mods(int num, int den)
    {
        bool result = false;
        int c = (int)Round((double)den / num);
        for (int a = 2; a < den; a++)
        {
            for (int b = 2; b < a; b++)
            {
                for (int r = 0; r < c; r++)
                {
                    Func<int, bool> isLeapYear = y => y % den % a % b % c == r;
                    string funcString = $"y => y % {den} % {a} % {b} % {c} == {r}";
                    if (TestSolution(num, den, isLeapYear, funcString))
                    {
                        result = true;
                    }
                }
            }
        }
        return result;
    }

    /// <summary>
    /// Find solutions using the modulo chain formula.
    /// </summary>
    public static void FindModRule(int num, int den)
    {
        // Check for invalid input.
        if (num <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(num), "Numerator must be positive.");
        }
        if (den <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(den), "Denominator must be positive.");
        }
        if (num >= den)
        {
            throw new ArgumentOutOfRangeException(nameof(num),
                "Numerator must be less than denominator.");
        }

        // Check for simple solution.
        if (num == 1)
        {
            Console.WriteLine($"Solution: y => y % {den} == 0;");
            return;
        }

        // Check if we need to reverse the fraction.
        if ((double)num / den > 0.5)
        {
            num = den - num;
            Console.WriteLine("Fraction is reversed. Solution is for common years, not leap years.");
        }

        if (FindRuleWith2Mods(num, den))
        {
            return;
        }
        if (FindRuleWith3Mods(num, den))
        {
            return;
        }
        if (FindRuleWith4Mods(num, den))
        {
            return;
        }
    }

    public static void FindHumanRule(double leapYearFrac)
    {
        if (leapYearFrac is <= 0 or >= 1)
        {
            throw new ArgumentOutOfRangeException(nameof(leapYearFrac), "Must be between 0 and 1.");
        }

        Console.WriteLine($"Target fraction: {leapYearFrac}");

        double frac;
        double newFrac;
        string commonOrLeap;
        if (leapYearFrac <= 0.5)
        {
            frac = leapYearFrac;
            Console.WriteLine("By default, a year is a common year.");
            commonOrLeap = "leap";
        }
        else
        {
            frac = 1 - leapYearFrac;
            Console.WriteLine("By default, a year is a leap year.");
            commonOrLeap = "common";
        }

        // Start with the closest fraction 1/n greater than frac.
        int a = (int)Floor(1 / leapYearFrac);
        double fracPart = 1.0 / a;
        newFrac = fracPart;
        Console.WriteLine($"Unless the year is divisible by {a}, then it's a {commonOrLeap} year. This gives a fraction of {newFrac:F9}");
        // int nRules = 1;
        while (true)
        {
            double diff = Abs(frac - newFrac);
            Console.WriteLine($"Current difference: {diff:F9}.");
            if (diff < 1e-6)
            {
                return;
            }

            // We need to subtract a fraction part, roughly equal to rem.
            // So, find the closest value 1/b where b is a multiple of a, greater than rem.
            int b = a;
            while (true)
            {
                // Get next multiple of a.
                b += a;
                fracPart = 1.0 / b;
                if (fracPart <= diff)
                {
                    if (b == 2 * a)
                    {
                        Console.WriteLine("Possibly no solution of this form.");
                    }
                    else
                    {
                        // Next part found. Go back one step to get the closest below.
                        b -= a;
                        fracPart = 1.0 / b;
                    }
                    break;
                }
            }

            // Avoid numbers too large to be useful.
            if (b > 4000)
            {
                return;
            }

            a = b;
            if (commonOrLeap == "leap")
            {
                newFrac -= fracPart;
                commonOrLeap = "common";
            }
            else
            {
                newFrac += fracPart;
                commonOrLeap = "leap";
            }
            Console.WriteLine(
                $"Unless the year is divisible by {b}, then it's a {commonOrLeap} year. This gives a fraction of {newFrac:F9}");
        }
    }

    public static void PrintLeapYearPattern(int num, int den, int a, int b, int c, int r)
    {
        bool isLeapYear(int y) => y % den % a % b % c == r;
        int count = 0;
        Console.WriteLine();
        int gap = 0;
        List<int> gaps = new ();
        for (int yy = 0; yy < den; yy++)
        {
            if (isLeapYear(yy))
            {
                count++;
                Console.Write(" 1");

                // Add the gap.
                gaps.Add(gap);
                gap = 0;
            }
            else
            {
                Console.Write(" 0");
            }

            gap++;

            if (yy % a == a - 1)
            {
                Console.WriteLine();
            }
        }

        // Add any remaining gap to the first one.
        gaps[0] += gap;

        Console.WriteLine();
        if (count == num)
        {
            Console.WriteLine($"Total number of leap years = {count}");
            Console.WriteLine($"Gaps: {string.Join(", ", gaps)} = {gaps.Sum()}");
            Console.WriteLine($"Minimum gap: {gaps.Min()}");
            Console.WriteLine($"Maximum gap: {gaps.Max()}");
        }
        else
        {
            Console.WriteLine($"Total number of leap years = {count}");
            Console.WriteLine($"Not equal to {num}");
        }
    }
}
