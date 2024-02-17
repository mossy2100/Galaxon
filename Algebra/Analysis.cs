using Galaxon.BigNumbers;
using Galaxon.Core.Functional;
using Galaxon.Numerics.Integers;

namespace Galaxon.Numerics.Algebra;

public static class Analysis
{
    #region Bernoulli numbers

    /// <summary>Calculate a Bernoulli number.</summary>
    /// <see href="https://en.wikipedia.org/wiki/Bernoulli_number"/>
    /// <param name="n">The index of the Bernoulli number to calculate.</param>
    /// <returns>The Bernoulli number as a BigRational.</returns>
    private static BigRational _Bernoulli(int n)
    {
        // Guard.
        if (n < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(n), "Cannot be negative.");
        }

        // Optimizations.
        if (n == 0)
        {
            return 1;
        }

        // For all odd indices greater than 1, the Bernoulli number is 0.
        if (n > 1 && int.IsOddInteger(n))
        {
            return 0;
        }

        // Compute result.
        BigRational b = 1;
        for (var k = 0; k < n; k++)
        {
            b -= Combinatorial.BinomialCoefficient(n, k) * Bernoulli(k) / (n - k + 1);
        }
        return b;
    }

    /// <summary>Calculate a Bernoulli number.</summary>
    /// <returns>The memoized version of the method.</returns>
    public static readonly Func<int, BigRational> Bernoulli =
        Memoization.Memoize<int, BigRational>(_Bernoulli);

    #endregion Bernoulli numbers
}
