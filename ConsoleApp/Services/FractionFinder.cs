using Galaxon.Numerics.BigNumbers;
using Galaxon.Time;

namespace Galaxon.ConsoleApp.Services;

public class FractionFinder
{
    public static (int, int) FindFraction(double avg, double maxDiff, ETimeUnit timeUnit)
    {
        int whole = (int)Math.Floor(avg);
        double frac = avg - whole;
        Console.WriteLine($"Average = {avg}");
        Console.WriteLine($"Fraction = {frac}");
        int bestNum = 0;
        int bestDen = 0;

        for (int den = 2; den <= 500; den++)
        {
            int num = (int)Math.Round(den * frac);
            double frac2 = (double)num / den;
            double diff = Math.Abs(frac - frac2);

            // See if this fraction is worth reporting.
            if (diff <= maxDiff && !BigRational.IsReducible(num, den))
            {
                double diff_s = TimeSpanExtensions.Convert(diff, timeUnit, ETimeUnit.Second);
                Console.WriteLine(
                    $"Found fraction {num}/{den} = {frac2}. Difference = {diff_s} seconds per year.");
                bestNum = num;
                bestDen = den;
            }
        }

        return (bestNum, bestDen);
    }
}
