using System.Numerics;
using Galaxon.Core.Functional;

namespace Galaxon.Numerics.Extensions;

/// <summary>
/// Extension methods for BigInteger.
/// </summary>
public static class BigIntegerExtensions
{
    #region Bit-related methods

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
        // interpreted as a positive value when reconstructing the result BigInteger.
        if ((bytes[^1] & 0b10000000) != 0)
        {
            bytes.Add(0);
        }

        // Construct a new unsigned value.
        return new BigInteger(bytes.ToArray());
    }

    #endregion Bit-related methods

    #region Digit-related methods

    /// <summary>
    /// Reverse a BigInteger.
    /// e.g. 123 becomes 321.
    /// </summary>
    public static BigInteger Reverse(this BigInteger n)
    {
        return BigInteger.Parse(n.ToString().Reverse().ToString()!);
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

    #region Exponentation

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

        // Sqrt(0) == 0
        if (n == 0)
        {
            return 0;
        }

        // Sqrt(1) == 1
        if (n == 1)
        {
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
    /// Calculates the factorial of a non-negative integer.
    /// Private version.
    /// </summary>
    /// <param name="n">The non-negative integer for which to calculate the factorial.</param>
    /// <returns>The factorial of the specified non-negative integer.</returns>
    /// <exception cref="ArgumentOutOfRangeException">When the argument is negative.</exception>
    private static BigInteger _Factorial(BigInteger n)
    {
        // Guard.
        if (n < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(n), "Cannot be negative.");
        }

        return n <= 1 ? 1 : n * _Factorial(n - 1);
    }

    /// <summary>
    /// Calculates the factorial of a non-negative integer.
    /// Public memoized version.
    /// </summary>
    /// <returns>The factorial of the specified non-negative integer.</returns>
    /// <exception cref="ArgumentOutOfRangeException">When the argument is negative.</exception>
    public static readonly Func<BigInteger, BigInteger> Factorial =
        Memoization.Memoize<BigInteger, BigInteger>(_Factorial);

    #endregion Factorial

    #region Methods relating to division

    /// <summary>
    /// Calculates the floored division of two BigInteger values.
    /// </summary>
    /// <param name="dividend">The dividend.</param>
    /// <param name="divisor">The divisor.</param>
    /// <returns>The result of the floored division.</returns>
    /// <exception cref="DivideByZeroException">Thrown when the divisor is zero.</exception>
    public static BigInteger FlooredDivision(BigInteger dividend, BigInteger divisor)
    {
        // Guard against division by zero.
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
        BigInteger r = NumberExtensions.Mod(dividend, divisor);
        return (q, r);
    }

    #endregion Methods relating to division
}
