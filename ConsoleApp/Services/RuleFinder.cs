namespace Galaxon.ConsoleApp.Services;

public class RuleFinder
{
    /// <summary>
    /// Find the best formula using the module chain formula with 2 operations.
    /// </summary>
    public static void FindRuleWith2Mods(int num, int den)
    {
        bool isLeapYear(int n, int a, int r)
        {
            return n % den % a == r;
        }

        // Invert fraction to find the min and max gaps.
        double avgGap = (double)den / num;
        int minGap = (int)double.Truncate(avgGap);
        int maxGap = minGap + 1;
        Console.WriteLine($"Gaps between leap years should be {minGap} or {maxGap}");

        for (int a2 = 2; a2 < den; a2++)
        {
            for (int r2 = 0; r2 < a2; r2++)
            {
                // Test the formula.
                int count = 0;
                int gap = 0;
                List<int> gaps = new ();
                for (int n2 = 0; n2 < den; n2++)
                {
                    if (isLeapYear(n2, a2, r2))
                    {
                        count++;
                        gaps.Add(gap);
                        gap = 0;
                    }
                    gap++;
                }

                if (count == num && gaps.Min() == minGap && gaps.Max() == maxGap)
                {
                    Console.WriteLine($"Found solution: y % {den} % {a2} == {r2};");
                    Console.WriteLine(string.Join(", ", gaps));
                }
            }
        }
    }

    /// <summary>
    /// Find the best formula using the module chain formula with 3 operations.
    /// </summary>
    public static void FindRuleWith3Mods(int num, int den)
    {
        bool isLeapYear(int n, int a, int b, int r)
        {
            return n % den % a % b == r;
        }

        // Invert fraction to find the min and max gaps.
        double avgGap = (double)den / num;
        int minGap = (int)double.Truncate(avgGap);
        int maxGap = minGap + 1;
        Console.WriteLine($"Gaps between leap years should be {minGap} or {maxGap}");

        for (int a2 = 2; a2 < den; a2++)
        {
            for (int b2 = 2; b2 < a2; b2++)
            {
                for (int r2 = 0; r2 < b2; r2++)
                {
                    // Test the formula.
                    int count = 0;
                    int gap = 0;
                    List<int> gaps = new ();
                    for (int n2 = 0; n2 < den; n2++)
                    {
                        if (isLeapYear(n2, a2, b2, r2))
                        {
                            count++;
                            gaps.Add(gap);
                            gap = 0;
                        }
                        gap++;
                    }
                    // Add any remaining gap to the first one.
                    gaps[0] += gap;

                    if (count == num && gaps.Min() == minGap && gaps.Max() == maxGap)
                    {
                        Console.WriteLine($"Found solution: n % {den} % {a2} % {b2} == {r2};");
                        Console.WriteLine(string.Join(", ", gaps));
                    }
                }
            }
        }
    }

    public static void PrintLeapYearPattern(int n, int d, int a, int b, int c)
    {
        bool isLeapYear(int y) => y % d % a % b == c;
        int count = 0;
        Console.WriteLine();
        int gap = 0;
        List<int> gaps = new ();
        for (int yy = 0; yy < d; yy++)
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
        if (count == n)
        {
            Console.WriteLine($"Total number of leap years = {count}");
            Console.WriteLine($"Gaps: {string.Join(", ", gaps)} = {gaps.Sum()}");
            Console.WriteLine($"Minimum gap: {gaps.Min()}");
            Console.WriteLine($"Maximum gap: {gaps.Max()}");
        }
        else
        {
            Console.WriteLine($"Total number of leap years = {count}");
            Console.WriteLine($"Not equal to {n}");
        }
    }
}
