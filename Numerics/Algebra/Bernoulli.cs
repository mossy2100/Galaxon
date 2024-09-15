using Galaxon.Core.Functional;
using Galaxon.Numerics.BigNumbers;
using Galaxon.Numerics.Integers;

namespace Galaxon.Numerics.Algebra;

public static class Bernoulli
{
    /// <summary>
    /// Calculate a Bernoulli number.
    /// NB: The Bernoulli number for n=1 can be ±1/2. This method returns 1/2.
    /// See: <see href="https://en.wikipedia.org/wiki/Bernoulli_number"/>
    /// </summary>
    /// <param name="n">The index of the Bernoulli number to calculate.</param>
    /// <returns>The Bernoulli number as a BigRational.</returns>
    private static BigRational _GetNumber(int n)
    {
        switch (n)
        {
            // Guard.
            case < 0:
                throw new ArgumentOutOfRangeException(nameof(n), "Cannot be negative.");

            // Optimizations.
            case 0:
                return 1;

            // For all odd indices greater than 1, the Bernoulli number is 0.
            case > 1 when int.IsOddInteger(n):
                return 0;
        }

        // Compute result.
        BigRational b = 1;
        for (var k = 0; k < n; k++)
        {
            b -= Combinatorial.BinomialCoefficient(n, k) * GetNumber(k) / (n - k + 1);
        }
        return b;
    }

    /// <summary>
    /// Calculate a Bernoulli number.
    /// </summary>
    /// <returns>The memoized version of the method.</returns>
    public static readonly Func<int, BigRational> GetNumber =
        Memoization.Memoize<int, BigRational>(_GetNumber);
}