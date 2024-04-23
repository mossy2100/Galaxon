using System.Numerics;
using Galaxon.Core.Functional;
using Galaxon.Numerics.Extensions;
using Galaxon.Numerics.Extensions.Integers;

namespace Galaxon.Numerics.Integers;

public static class Divisors
{
    /// <summary>
    /// Get the proper divisors of an integer.
    /// </summary>
    /// <exception cref="ArgumentOutOfRangeException">If the argument is negative.</exception>
    private static List<BigInteger> _GetProperDivisors(BigInteger n)
    {
        // Guard.
        if (n < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(n), "Cannot be negative.");
        }

        // Initialize the result.
        List<BigInteger> divisors = [];

        // Optimization.
        if (n <= 1)
        {
            return divisors;
        }

        // Get the truncated square root of the argument.
        BigInteger sqrt = BigIntegerExtensions.TruncSqrt(n);

        // Look for divisors up to the square root.
        for (BigInteger factor = 1; factor <= sqrt; factor++)
        {
            // Check if factor is a divisor.
            if (n % factor != 0)
            {
                continue;
            }

            // Add the factor.
            divisors.Add(factor);

            // Calculate the complement.
            BigInteger complement = n / factor;

            // If it's different from the factor and less than the argument, add the complement.
            if (complement != factor && complement < n)
            {
                divisors.Add(complement);
            }
        }

        // Order the divisors.
        divisors.Sort();

        return divisors;
    }

    /// <summary>
    /// Public memoized version of _GetProperDivisors().
    /// </summary>
    public static readonly Func<BigInteger, List<BigInteger>> GetProperDivisors =
        Memoization.Memoize<BigInteger, List<BigInteger>>(_GetProperDivisors);

    /// <summary>
    /// Get all the divisors of an integer, including itself.
    /// </summary>
    /// <exception cref="ArgumentOutOfRangeException">If the argument is negative.</exception>
    public static List<BigInteger> GetDivisors(BigInteger n)
    {
        List<BigInteger> divisors = GetProperDivisors(n);
        divisors.Add(n);
        return divisors;
    }

    /// <summary>
    /// Get the sum of an integer's divisors.
    /// </summary>
    /// <exception cref="ArgumentOutOfRangeException">If the argument is negative.</exception>
    public static BigInteger SumDivisors(BigInteger n)
    {
        return GetDivisors(n).Sum();
    }

    /// <summary>
    /// Get the sum of an integer's proper divisors.
    /// </summary>
    /// <see href="https://en.wikipedia.org/wiki/Aliquot_sum"/>
    /// <exception cref="ArgumentOutOfRangeException">If the argument is negative.</exception>
    public static BigInteger Aliquot(BigInteger n)
    {
        return GetProperDivisors(n).Sum();
    }

    /// <summary>
    /// Check if a number is perfect, deficient, or abundant.
    /// <see href="https://en.wikipedia.org/wiki/Perfect_number"/>
    /// </summary>
    public static sbyte PerfectNumber(long n)
    {
        BigInteger aliquot = Aliquot(n);

        // Check if the number is perfect.
        if (aliquot == n)
        {
            return 0;
        }

        // Check if the number is deficient or abundant.
        return (sbyte)(aliquot < n ? -1 : 1);
    }

    /// <summary>
    /// Prepare values for GreatestCommonDivisor() or LeastCommonMultiple() calculations.
    /// In the resulting values, both will be made positive and 'a' will be less than 'b'.
    /// </summary>
    /// <param name="a">The first integer.</param>
    /// <param name="b">The second integer.</param>
    /// <returns>The two integers, positive and ordered.</returns>
    private static (BigInteger, BigInteger) _PrepareValues(BigInteger a, BigInteger b)
    {
        // Make a and b non-negative, since the result will be the same for negative values.
        a = BigInteger.Abs(a);
        b = BigInteger.Abs(b);

        // Make a <= b.
        if (a > b)
        {
            (a, b) = (b, a);
        }

        return (a, b);
    }

    /// <summary>
    /// Determines the greatest common divisor (GCD) of two integers.
    /// </summary>
    /// <param name="a">The first integer.</param>
    /// <param name="b">The second integer.</param>
    /// <returns>The greatest common divisor of the two integers.</returns>
    private static BigInteger _GreatestCommonDivisor(BigInteger a, BigInteger b)
    {
        // Prepare the values.
        (a, b) = _PrepareValues(a, b);

        // Optimizations and terminating conditions.
        if (a == b || a == 0)
        {
            return b;
        }
        if (a == 1)
        {
            return 1;
        }

        // Get the result by recursion.
        return _GreatestCommonDivisor(a, b % a);
    }

    /// <summary>
    /// Determine the greatest common divisor of two integers.
    /// Synonyms: greatest common factor, highest common factor.
    /// Memoized version of _GreatestCommonDivisor().
    /// </summary>
    /// <returns>The greatest common divisor of two integers.</returns>
    public static readonly Func<BigInteger, BigInteger, BigInteger> GreatestCommonDivisor =
        Memoization.Memoize<BigInteger, BigInteger, BigInteger>(_GreatestCommonDivisor);

    /// <summary>
    /// Find the smallest integer which is a multiple of both arguments.
    /// Synonyms: lowest common multiple, smallest common multiple.
    /// For example, the LCM of 4 and 6 is 12.
    /// When adding fractions, the lowest common denominator is equal to the LCM of the
    /// denominators.
    /// </summary>
    /// <param name="a">First integer.</param>
    /// <param name="b">Second integer.</param>
    /// <returns>The least common multiple.</returns>
    public static BigInteger LeastCommonMultiple(BigInteger a, BigInteger b)
    {
        // Prepare values.
        (a, b) = _PrepareValues(a, b);

        // Terminating conditions.
        if (a == 0 && b == 0)
        {
            return 0;
        }
        if (a == 0 || a == b)
        {
            return b;
        }

        // Calculate LCM.
        return b / GreatestCommonDivisor(a, b) * a;
    }
}
