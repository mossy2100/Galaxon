namespace Galaxon.ConsoleApp;

public class RuleFinder
{
    /// <summary>
    /// Find the best formula using the module chain formula with 2 operations.
    /// </summary>
    public static void FindRuleWith2Mods(int numerator, int denominator)
    {
        bool isLeapYear(int n, int a, int r)
        {
            return n % denominator % a == r;
        }

        for (int a2 = 2; a2 < denominator; a2++)
        {
            for (int r2 = 0; r2 < a2; r2++)
            {
                // Test the formula.
                int count = 0;
                for (int n2 = 0; n2 < denominator; n2++)
                {
                    if (isLeapYear(n2, a2, r2))
                    {
                        count++;
                    }
                }

                if (count == numerator)
                {
                    Console.WriteLine($"Found solution: y % {denominator} % {a2} == {r2};");
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
            return n % a % b == r;
        }

        for (int a2 = 2; a2 < den; a2++)
        {
            for (int b2 = 2; b2 < a2; b2++)
            {
                for (int r2 = 0; r2 < b2; r2++)
                {
                    // Test the formula.
                    int count = 0;
                    for (int n2 = 0; n2 < den; n2++)
                    {
                        if (isLeapYear(n2, a2, b2, r2))
                        {
                            count++;
                        }
                    }

                    if (count == num)
                    {
                        Console.WriteLine($"Found solution: n % {den} % {a2} % {b2} == {r2};");
                    }
                }
            }
        }
    }
}
