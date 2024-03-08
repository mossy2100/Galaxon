namespace Galaxon.ConsoleApp;

public static class Tests
{
    public static void TestDivisionMethods()
    {
        var rnd = new Random();
        var x = rnd.Next(-10000, 10000);
        var y = rnd.Next(-100, 100);
        Console.WriteLine($"Remainder after {x} / {y}:");
        Console.WriteLine();

        var modop = Division.ModuloOperator(x, y);
        var modopposrem = Division.Mod(x, y);
        var remtrunc = Division.RemainderAfterTruncatedDivision(x, y);
        var remfloor = Division.RemainderAfterFlooredDivision(x, y);
        var remeuclid = Division.RemainderAfterEuclideanDivision(x, y);

        Console.WriteLine($"ModuloOperator: {modop}");
        Console.WriteLine($"RemainderAfterTruncatedDivision: {remtrunc}");
        Console.WriteLine();
        Console.WriteLine($"ModuloOperatorWithPositiveRemainder: {modopposrem}");
        Console.WriteLine($"RemainderAfterFlooredDivision: {remfloor}");
        Console.WriteLine();
        Console.WriteLine($"RemainderAfterEuclideanDivision: {remeuclid}");
        Console.WriteLine();

        if (modop == remtrunc)
        {
            Console.WriteLine("ModuloOperator produces same result as RemainderAfterTruncatedDivision");
        }
        else
        {
            Console.WriteLine("ModuloOperator DOES NOT produce same result as RemainderAfterTruncatedDivision");
        }

        if (modopposrem == remfloor)
        {
            Console.WriteLine("ModuloOperatorWithPositiveRemainder produces same result as RemainderAfterFlooredDivision");
        }
        else
        {
            Console.WriteLine("ModuloOperatorWithPositiveRemainder DOES NOT produce same result as RemainderAfterFlooredDivision");
        }
    }

    public static void TestLeapYearPatterns(int a, int b)
    {
        int n = 0;
        int nLeapYears = 0;
        for (int y = 0; y < a; y++)
        {
            if (LeapYearFormulaFinder.IsLeapYear(y))
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
