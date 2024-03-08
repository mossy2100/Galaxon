using Galaxon.Time;

namespace Galaxon.ConsoleApp;

public class SolarCalendar
{
    public static void FindLeapYearFraction()
    {
        double frac = TimeConstants.DAYS_PER_TROPICAL_YEAR
            - (int)TimeConstants.DAYS_PER_TROPICAL_YEAR;
        List<double> fractions = [];
        for (var d = 1; d <= 10000; d++)
        {
            // Console.WriteLine($"Testing d = {d}...");
            var n = (int)Math.Round(frac * d);
            if (n == 0)
            {
                continue;
            }

            double frac2 = n / (double)d;

            // Eliminate duplicates.
            if (fractions.Contains(frac2))
            {
                continue;
            }
            fractions.Add(frac2);

            double diffDays = Math.Abs(frac - frac2);
            double diffSeconds = diffDays * TimeConstants.SECONDS_PER_DAY;
            if (diffSeconds <= 1)
            {
                Console.WriteLine(
                    $"Match: {n} / {d} = {frac2} (drift is about {diffSeconds:N3} seconds per year)");
            }
        }

        Console.WriteLine("Done.");
    }

    public static bool IsLeapYear(int year)
    {
        var yearsPerCycle = 128;
        var gapLengthYears = 4;
        return year % gapLengthYears == 0 && year % yearsPerCycle != 0;
    }

    public static void TestLeapYearRule()
    {
        var leapYearCount = 0;
        var yearsPerCycle = 128;
        var gapLengthYears = 4;
        Console.WriteLine(
            $"The rule: isLeapYear = (m % {gapLengthYears} == 0) && (m % {yearsPerCycle} != 0)");
        List<int> yearNumbers = [];
        for (var y = 0; y < yearsPerCycle; y++)
        {
            if (IsLeapYear(y))
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
            if (IsLeapYear(y % a))
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
