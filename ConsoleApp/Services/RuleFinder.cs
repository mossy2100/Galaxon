namespace Galaxon.ConsoleApp.Services;

public class RuleFinder
{
    /// <summary>
    /// Find the best formula using the module chain formula with 2 operations.
    /// </summary>
    public static void FindRuleWith2Mods(int num, int den)
    {
        bool isLeapYear(int y, int a, int r)
        {
            return y % den % a == r;
        }

        // Invert fraction to find the min and max gaps.
        double avgGap = (double)den / num;
        int minGap = (int)double.Truncate(avgGap);
        int maxGap = minGap + 1;

        for (int a2 = 2; a2 < den; a2++)
        {
            for (int r2 = 0; r2 < a2; r2++)
            {
                // Test the formula.
                int count = 0;
                int gap = 0;
                List<int> gaps = new ();
                for (int y2 = 0; y2 < den; y2++)
                {
                    if (isLeapYear(y2, a2, r2))
                    {
                        count++;
                        gaps.Add(gap);
                        gap = 0;
                    }
                    gap++;
                }
                // Add any remaining gap to the first one.
                if (gaps.Count > 0)
                {
                    gaps[0] += gap;
                }
                gaps = gaps.Distinct().ToList();

                if (count == num && gaps.Count <= 2)
                {
                    Console.WriteLine($"Found solution: y % {den} % {a2} == {r2};");
                    Console.WriteLine(string.Join(", ", gaps.Distinct()));
                    Console.WriteLine($"Minimum gap: {gaps.Min()}");
                    Console.WriteLine($"Maximum gap: {gaps.Max()}");
                    if (gaps.Min() == minGap && gaps.Max() == maxGap)
                    {
                        Console.WriteLine("OPTIMAL GAPS");
                    }
                }
            }
        }
    }

    /// <summary>
    /// Find the best formula using the module chain formula with 3 operations.
    /// </summary>
    public static void FindRuleWith3Mods(int num, int den)
    {
        bool isLeapYear(int y, int a, int b, int r)
        {
            return y % den % a % b == r;
        }

        // Invert fraction to find the min and max gaps.
        double avgGap = (double)den / num;
        int minGap = (int)double.Truncate(avgGap);
        int maxGap = minGap + 1;

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
                    for (int y2 = 0; y2 < den; y2++)
                    {
                        if (isLeapYear(y2, a2, b2, r2))
                        {
                            count++;
                            gaps.Add(gap);
                            gap = 0;
                        }
                        gap++;
                    }
                    // Add any remaining gap to the first one.
                    if (gaps.Count > 0)
                    {
                        gaps[0] += gap;
                    }
                    gaps = gaps.Distinct().ToList();

                    if (count == num && gaps.Count <= 2)
                    {
                        Console.WriteLine($"Found solution: y % {den} % {a2} % {b2} == {r2};");
                        Console.WriteLine(string.Join(", ", gaps.Distinct()));
                        Console.WriteLine($"Minimum gap: {gaps.Min()}");
                        Console.WriteLine($"Maximum gap: {gaps.Max()}");
                        if (gaps.Min() == minGap && gaps.Max() == maxGap)
                        {
                            Console.WriteLine("OPTIMAL GAPS");
                        }
                    }
                }
            }
        }
    }

    /// <summary>
    /// Find the best formula using the module chain formula with 3 operations.
    /// </summary>
    public static void FindRuleWith4Mods(int num, int den)
    {
        bool isLeapYear(int y, int a, int b, int c, int r)
        {
            return y % den % a % b % c == r;
        }

        // Invert fraction to find the min and max gaps.
        double avgGap = (double)den / num;
        int minGap = (int)double.Truncate(avgGap);
        int maxGap = minGap + 1;

        for (int a2 = 2; a2 < den; a2++)
        {
            for (int b2 = 2; b2 < a2; b2++)
            {
                for (int c2 = 2; c2 < b2; c2++)
                {
                    for (int r2 = 0; r2 < c2; r2++)
                    {
                        // Test the formula.
                        int count = 0;
                        int gap = 0;
                        List<int> gaps = new ();
                        for (int y2 = 0; y2 < den; y2++)
                        {
                            if (isLeapYear(y2, a2, b2, c2, r2))
                            {
                                count++;
                                gaps.Add(gap);
                                gap = 0;
                            }
                            gap++;
                        }
                        // Add any remaining gap to the first one.
                        if (gaps.Count > 0)
                        {
                            gaps[0] += gap;
                        }
                        gaps = gaps.Distinct().ToList();

                        bool optimalGaps = gaps.Min() == minGap && gaps.Max() == maxGap;
                        if (count == num && optimalGaps)
                        {
                            Console.WriteLine($"Found solution: y % {den} % {a2} % {b2} % {c2} == {r2};");
                            Console.WriteLine(string.Join(", ", gaps.Distinct()));
                            Console.WriteLine($"Minimum gap: {gaps.Min()}");
                            Console.WriteLine($"Maximum gap: {gaps.Max()}");
                            if (optimalGaps)
                            {
                                Console.WriteLine("OPTIMAL GAPS");
                            }
                        }
                    }
                }
            }
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
