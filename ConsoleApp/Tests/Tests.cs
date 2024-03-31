using Galaxon.ConsoleApp.Services;

namespace Galaxon.ConsoleApp.Tests;

public static class Tests
{
    public static void TestLeapYearPatterns(int a, int b)
    {
        int n = 0;
        int nLeapYears = 0;
        for (int y = 0; y < a; y++)
        {
            if (SolarCalendar.IsLeapYear1(y))
            {
                Console.Write(" 1");
                nLeapYears++;
            }
            else
            {
                Console.Write(" 0");
            }

            n++;
            if (n == b)
            {
                Console.WriteLine();
                n = 0;
            }
        }

        Console.WriteLine();
        Console.WriteLine($"Number of leap years in {a} years is {nLeapYears}");
    }

    public static void TestAlternate31In128Formula()
    {
        var n = 0;
        for (int i = 0; i < 128; i++)
        {
            if (i % 29 % 4 == 1)
            {
                Console.WriteLine(" 1");
                n++;
            }
            else
            {
                Console.Write(" 0");
            }
        }
        Console.WriteLine();
        Console.WriteLine($"{n} leap years.");
    }
}
