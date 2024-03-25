using Galaxon.Numerics.BigNumbers;

namespace Galaxon.ConsoleApp;

public class FractionFinder
{
    public static void FindFraction(double avg, double maxDiff, double mult)
    {
        int whole = (int)Math.Floor(avg);
        double frac = avg - whole;
        Console.WriteLine($"Average = {avg}");
        Console.WriteLine($"Fraction = {frac}");

        for (int den = 2; den <= 1000; den++)
        {
            int num = (int)Math.Round(den * frac);
            double frac2 = (double)num / den;
            double diff = Math.Abs(frac - frac2);

            // See if this fraction is worth reporting.
            if (diff <= maxDiff && !BigRational.IsReducible(num, den))
            {
                double diffInSeconds = diff * mult;
                Console.WriteLine($"Found fraction {num}/{den} = {frac2}. Difference = {diffInSeconds} seconds per year.");
                RuleFinder.FindRuleWith2Mods(num, den);
            }
        }
    }
}
