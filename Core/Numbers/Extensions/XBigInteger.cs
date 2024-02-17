using System.Numerics;
using Galaxon.Core.Functional;
using Galaxon.Core.Strings;

namespace Galaxon.Core.Numbers.Extensions;

/// <summary>Extension methods for BigInteger.</summary>
public static class XBigInteger
{
    #region Fields

    /// <summary>
    /// Cache for GreatestCommonDivisor().
    /// </summary>
    private static readonly Dictionary<string, BigInteger> s_gcdCache = new ();

    #endregion Fields

    #region Miscellaneous other methods

    /// <summary>
    /// Get the unsigned, twos-complement version of the value, containing the fewest number of
    /// bytes.
    /// </summary>
    public static BigInteger ToUnsigned(this BigInteger n)
    {
        // Check if anything to do.
        if (n >= 0)
        {
            return n;
        }

        // Get the bytes.
        var bytes = n.ToByteArray().ToList();

        // Check the most-significant bit, and, if it's 1, add a zero byte to ensure the bytes are
        // interpreted as a positive value when constructing the result BigInteger.
        if ((bytes[^1] & 0b10000000) != 0)
        {
            bytes.Add(0);
        }

        // Construct a new unsigned value.
        return new BigInteger(bytes.ToArray());
    }

    #endregion Miscellaneous other methods

    #region Digit-related methods

    /// <summary>
    /// Reverse a BigInteger.
    /// e.g. 123 becomes 321.
    /// </summary>
    public static BigInteger Reverse(this BigInteger n)
    {
        return BigInteger.Parse(n.ToString().Reverse());
    }

    /// <summary>
    /// Check if a BigInteger is palindromic.
    /// </summary>
    public static bool IsPalindromic(this BigInteger n)
    {
        return n == n.Reverse();
    }

    /// <summary>
    /// Sum of the digits in a BigInteger.
    /// If present, a negative sign is ignored.
    /// </summary>
    public static BigInteger DigitSum(this BigInteger n)
    {
        return BigInteger.Abs(n).ToString().Sum(c => c - '0');
    }

    /// <summary>
    /// Get the number of digits in the BigInteger.
    /// The result will be the same for a positive or negative value.
    /// I tried doing this with double.Log() but because double is imprecise it gives wrong results
    /// for values close to but less than powers of 10.
    /// </summary>
    public static int NumDigits(this BigInteger n)
    {
        return BigInteger.Abs(n).ToString().Length;
    }

    #endregion Digit-related methods

    #region Methods relating to factors

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
        // Optimizations.
        if (a == 0 || b == 0)
        {
            return 0;
        }
        if (a == b)
        {
            return a;
        }

        a = BigInteger.Abs(a);
        b = BigInteger.Abs(b);
        BigInteger gcd = GreatestCommonDivisor(a, b);

        return a > b ? a / gcd * b : b / gcd * a;
    }

    /// <summary>
    /// Determine the greatest common divisor of two integers.
    /// Synonyms: greatest common factor, highest common factor.
    /// </summary>
    public static BigInteger GreatestCommonDivisor(BigInteger a, BigInteger b)
    {
        // Make a and b non-negative, since the result will be the same for negative values.
        a = BigInteger.Abs(a);
        b = BigInteger.Abs(b);

        // Make a < b.
        if (a > b)
        {
            (a, b) = (b, a);
        }

        // Optimization/terminating conditions.
        if (a == b || a == 0)
        {
            return b;
        }
        if (a == 1)
        {
            return 1;
        }

        // Check the cache.
        var key = $"{a}/{b}";
        if (s_gcdCache.TryGetValue(key, out BigInteger gcd))
        {
            return gcd;
        }

        // Get the result by recursion.
        gcd = GreatestCommonDivisor(a, b % a);

        // Store the result in the cache.
        s_gcdCache[key] = gcd;

        return gcd;
    }

    #endregion Methods relating to factors

    #region Methods relating to exponentation

    /// <summary>Compute 2 raised to a given integer power.</summary>
    /// <param name="y">The power to which 2 is raised.</param>
    /// <returns>2 raised to the given BigInteger value.</returns>
    public static BigInteger Exp2(int y)
    {
        return BigInteger.Pow(2, y);
    }

    /// <summary>Compute 10 raised to a given integer power.</summary>
    /// <param name="y">The power to which 10 is raised.</param>
    /// <returns>10 raised to the given BigInteger value.</returns>
    public static BigInteger Exp10(int y)
    {
        return BigInteger.Pow(10, y);
    }

    /// <summary>
    /// Calculated the truncated square root of a BigInteger value.
    /// Uses Newton's method.
    /// </summary>
    /// <param name="n">The BigInteger value.</param>
    /// <returns>The truncated square root.</returns>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    public static BigInteger TruncSqrt(BigInteger n)
    {
        // Guard.
        if (n < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(n), "Cannot be negative.");
        }
        if (n == 0)
        {
            // Sqrt(0) == 0
            return 0;
        }
        if (n == 1)
        {
            // Sqrt(1) == 1
            return 1;
        }

        // Compute using Newton's method.
        BigInteger x = n;
        BigInteger y = (x + 1) / 2;
        while (y < x)
        {
            x = y;
            y = (x + n / x) / 2;
        }
        return x;
    }

    /// <summary>Check if a BigInteger is a power of 2.</summary>
    /// <param name="bi">The BigInteger to inspect.</param>
    /// <returns>If the value is a power of 2.</returns>
    public static bool IsPowerOf2(BigInteger bi)
    {
        // A number is not a power of 2 if it's less than or equal to 0
        if (bi <= 0)
        {
            return false;
        }

        // A number is a power of 2 if it has exactly one bit set.
        // (number - 1) will have all the bits set to the right of the only set bit in number.
        // Anding number with (number - 1) should give 0 if number is a power of 2.
        // For example, 0b1000000 & 0b0111111 == 0
        return (bi & (bi - 1)) == 0;
    }

    #endregion Power methods

    #region Factorial

    /// <summary>
    /// Factorial of n, n >= 0.
    /// Private, uncached version.
    /// </summary>
    /// <param name="n"></param>
    /// <returns>The factorial of the parameter.</returns>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    private static BigInteger _Factorial(BigInteger n)
    {
        // Guard.
        if (n < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(n), "Cannot be negative.");
        }

        return n <= 1 ? 1 : n * Factorial(n - 1);
    }

    /// <summary>Public memoized version of the Factorial method.</summary>
    public static readonly Func<BigInteger, BigInteger> Factorial =
        Memoization.Memoize<BigInteger, BigInteger>(_Factorial);

    #endregion Factorial

    #region Methods relating to division

    /// <summary>
    /// Return the floored division of 2 BigInteger values.
    /// </summary>
    /// <param name="dividend"></param>
    /// <param name="divisor"></param>
    /// <returns></returns>
    /// <exception cref="DivideByZeroException"></exception>
    public static BigInteger FlooredDivision(BigInteger dividend, BigInteger divisor)
    {
        // Guard.
        if (divisor == 0)
        {
            throw new DivideByZeroException("Cannot divide by zero.");
        }

        // Adjust for negative result by adding 1 before division.
        if (dividend.Sign != divisor.Sign)
        {
            return (dividend - divisor + 1) / divisor;
        }

        // Positive result, arguments have the same sign.
        return dividend / divisor;
    }

    /// <summary>
    /// Return the floored division of 2 BigInteger values, with the remainder after division.
    /// </summary>
    /// <param name="dividend"></param>
    /// <param name="divisor"></param>
    /// <returns>A tuple containing the result of the floored division, and the remainder.</returns>
    /// <exception cref="DivideByZeroException"></exception>
    public static (BigInteger, BigInteger) DivMod(BigInteger dividend, BigInteger divisor)
    {
        BigInteger q = FlooredDivision(dividend, divisor);
        BigInteger r = XNumber.Mod(dividend, divisor);
        return (q, r);
    }

    #endregion Methods relating to division
}
