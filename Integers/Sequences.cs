using Galaxon.Core.Functional;

namespace Galaxon.Numerics.Integers;

public static class Sequences
{
    public static readonly Func<long, List<long>> Collatz =
        Memoization.Memoize<long, List<long>>(_Collatz);

    /// <summary>
    /// Returns series of numbers in a Collatz series, starting at n and ending in 1.
    /// <see href="https://en.wikipedia.org/wiki/Collatz_conjecture"/>
    /// </summary>
    /// <param name="n">Starting number.</param>
    /// <returns>The series of numbers.</returns>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    private static List<long> _Collatz(long n)
    {
        // Guard.
        if (n < 1)
        {
            throw new ArgumentOutOfRangeException(nameof(n), "Cannot be less than 1.");
        }

        List<long> result = new () { n };

        if (n > 1)
        {
            // Get the next number.
            long m = n % 2 == 0 ? n / 2 : 3 * n + 1;

            // Append additional items in the chain.
            result.AddRange(Collatz(m));
        }

        return result;
    }
}
