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
                    }
                }
            }
        }
    }
}
