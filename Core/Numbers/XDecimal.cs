using DecimalMath;

namespace Galaxon.Core.Numbers;

/// <summary>Extension methods for decimal.</summary>
public static class XDecimal
{
    #region Constants

    /// <summary>The number of bits in the exponent.</summary>
    public const byte NUM_EXP_BITS = 8;

    /// <summary>The number of bits in the integer part.</summary>
    public const byte NUM_INT_BITS = 96;

    /// <summary>The minimum scale factor (inverse decimal exponent).</summary>
    public const short MAX_SCALE = 28;

    #endregion Constants

    #region Exponentiation methods

    /// <summary>
    /// Calculate the natural logarithm of a decimal.
    /// The algorithm is from:
    /// <see href="https://en.wikipedia.org/wiki/Natural_logarithm"/>
    /// DecimalEx.Log() hangs for very small values, so I made this version, which doesn't.
    /// It's tested, fast, and doesn't break with the largest or smallest decimal values.
    /// <see cref="Math.Log(double)"/>
    /// </summary>
    /// <param name="m">A decimal value.</param>
    /// <returns>The natural logarithm of the given value.</returns>
    /// <exception cref="ArgumentOutOfRangeException">
    /// If the argument is less than or equal to 0.
    /// </exception>
    public static decimal Log(decimal m)
    {
        switch (m)
        {
            // Guards.
            case 0:
                throw new ArgumentOutOfRangeException(nameof(m),
                    "The logarithm of 0 is undefined. Math.Log(0) returns -Infinity, but the decimal type doesn't provide a way to represent this.");

            case < 0:
                throw new ArgumentOutOfRangeException(nameof(m),
                    "The logarithm of a negative value is a complex number. Use DecimalComplex.Ln().");

            // Optimizations.
            case 1:
                return 0;

            case 2:
                return DecimalEx.Ln2;

            case 10:
                return DecimalEx.Ln10;

            case DecimalEx.E:
                return 1;
        }

        // Scale the value to the range (0..1) so the Taylor series converges quickly and to avoid
        // overflow.
        var scale = (int)Math.Floor(Math.Log10((double)m)) + 1;
        decimal x;

        // Some cleverness to avoid overflow if scale == 29.
        if (scale <= MAX_SCALE)
        {
            x = m / Exp10(scale);
        }
        else
        {
            x = m / 1e28m / Exp10(scale - MAX_SCALE);
        }

        // Use the Taylor series.
        x--;
        var xx = x;
        var n = 1;
        var s = 1;
        decimal oldValue = 0;
        decimal newValue = 0;
        while (true)
        {
            // Calculate the next term in the series.
            var term = s * xx / n;

            // Check if done.
            if (term == 0m)
            {
                break;
            }

            // Add the term.
            newValue = oldValue + term;

            // Prepare to calculate the next term.
            s = -s;
            xx *= x;
            n++;
            oldValue = newValue;
        }

        // Scale back.
        return newValue + scale * DecimalEx.Ln10;
    }

    /// <summary>
    /// Logarithm of a decimal in a specified base.
    /// <see cref="Math.Log(double, double)"/>
    /// <see cref="Log(decimal, decimal)"/>
    /// </summary>
    /// <param name="m">The decimal value.</param>
    /// <param name="b">The base.</param>
    /// <returns>The logarithm of z in base b.</returns>
    /// <exception cref="ArgumentOutOfRangeException">
    /// If the number is less
    /// than or equal to 0.
    /// </exception>
    /// <exception cref="ArgumentOutOfRangeException">
    /// If the base is less than
    /// or equal to 0, or equal to 1.
    /// </exception>
    public static decimal Log(decimal m, decimal b)
    {
        if (b == 1)
        {
            throw new ArgumentOutOfRangeException(nameof(b),
                "Logarithms are undefined for a base of 1.");
        }

        // 0^0 == 1. Mimics Math.Log().
        if (m == 1 && b == 0)
        {
            return 0;
        }

        // This will throw if m <= 0 || b <= 0.
        return Log(m) / Log(b);
    }

    /// <summary>
    /// Logarithm of a decimal in base 10.
    /// <see cref="Math.Log10"/>
    /// <see cref="Log10"/>
    /// </summary>
    /// <param name="m">The decimal value.</param>
    /// <returns>The logarithm of the number in base 10.</returns>
    /// <exception cref="ArgumentOutOfRangeException">
    /// If the number is less than or equal to 0.
    /// </exception>
    public static decimal Log10(decimal m)
    {
        return Log(m, 10);
    }

    /// <summary>
    /// Calculate 10 raised to a decimal power.
    /// </summary>
    /// <param name="m">A decimal value.</param>
    /// <returns>10^d</returns>
    public static decimal Exp10(decimal m)
    {
        return DecimalEx.Pow(10, m);
    }

    /// <summary>
    /// Logarithm of a decimal in base 2.
    /// </summary>
    /// <param name="m">The decimal value.</param>
    /// <returns>The logarithm of the number in base 2.</returns>
    /// <exception cref="ArgumentOutOfRangeException">
    /// If the number is less than or equal to 0.
    /// </exception>
    public static decimal Log2(decimal m)
    {
        return Log(m, 2);
    }

    /// <summary>
    /// Calculate 2 raised to a decimal power.
    /// </summary>
    /// <param name="m">A decimal value.</param>
    /// <returns>2^d</returns>
    public static decimal Exp2(decimal m)
    {
        return DecimalEx.Pow(2, m);
    }

    #endregion Exponentiation methods

    #region Hyperbolic trigonometric methods

    /// <summary>
    /// Hyperbolic sine.
    /// </summary>
    /// <param name="x">The hyperbolic angle.</param>
    /// <returns>The hyperbolic sine of the given angle.</returns>
    public static decimal Sinh(decimal x)
    {
        return (DecimalEx.Exp(x) - DecimalEx.Exp(-x)) / 2;
    }

    /// <summary>
    /// Hyperbolic cosine.
    /// </summary>
    /// <param name="x">The hyperbolic angle.</param>
    /// <returns>The hyperbolic cosine of the given angle.</returns>
    public static decimal Cosh(decimal x)
    {
        return (DecimalEx.Exp(x) + DecimalEx.Exp(-x)) / 2;
    }

    /// <summary>
    /// Hyperbolic tangent.
    /// </summary>
    /// <param name="x">The hyperbolic angle.</param>
    /// <returns>The hyperbolic tangent of the given angle.</returns>
    public static decimal Tanh(decimal x)
    {
        var f = DecimalEx.Exp(2 * x);
        return (f - 1) / (f + 1);
    }

    /// <summary>
    /// Inverse hyperbolic sine.
    /// </summary>
    /// <param name="x">The hyperbolic sine of an angle.</param>
    /// <returns>The angle.</returns>
    public static decimal Asinh(decimal x)
    {
        return Log(x + DecimalEx.Sqrt(x * x + 1));
    }

    /// <summary>
    /// Inverse hyperbolic cosine.
    /// </summary>
    /// <param name="x">The hyperbolic cosine of an angle.</param>
    /// <returns>The angle.</returns>
    public static decimal Acosh(decimal x)
    {
        return Log(x + DecimalEx.Sqrt(x * x - 1));
    }

    /// <summary>
    /// Inverse hyperbolic tangent.
    /// </summary>
    /// <param name="x">The hyperbolic tangent of an angle.</param>
    /// <returns>The angle.</returns>
    public static decimal Atanh(decimal x)
    {
        return Log((1 + x) / (1 - x)) / 2;
    }

    #endregion Hyperbolic trigonometric methods

    #region Miscellaneous methods

    /// <summary>
    /// Check if a decimal value is an integer.
    /// </summary>
    public static bool IsInteger(decimal m)
    {
        return m == decimal.Truncate(m);
    }

    /// <summary>
    /// Round off a value to a given number of significant figures.
    /// </summary>
    /// <param name="m">The number to round.</param>
    /// <param name="n">The number of significant figures.</param>
    /// <returns>The rounded number.</returns>
    /// TODO: Test. If digits is too high an exception will be thrown, so this needs to be checked.
    public static decimal RoundSigFigs(decimal m, int n)
    {
        if (m == 0)
        {
            return 0;
        }
        var scale = Exp10(Math.Floor(Log10(Math.Abs(m))) + 1);
        return scale * Math.Round(m / scale, n);
    }

    /// <summary>Disassemble the decimal into bitwise parts.</summary>
    public static (byte signBit, byte scaleBits, UInt128 intBits) Disassemble(this decimal x)
    {
        var parts = decimal.GetBits(x);
        var lo = (uint)parts[0];
        var mid = (uint)parts[1];
        var hi = (uint)parts[2];
        var flags = (uint)parts[3];
        var signBit = (byte)(flags >> 31);
        var scaleBits = (byte)((flags >> 16) & 0xff);
        var intBits = ((UInt128)hi << 64) | ((UInt128)mid << 32) | lo;
        return (signBit, scaleBits, intBits);
    }

    /// <summary>Get the scale bits from a decimal value.</summary>
    public static byte GetScaleBits(this decimal x)
    {
        var (_, scaleBits, _) = x.Disassemble();
        return scaleBits;
    }

    /// <summary>
    /// Assemble a new decimal value from bitwise parts.
    /// </summary>
    /// <param name="signBit">The sign bit (1 or 0).</param>
    /// <param name="scaleBits">The scale bits.</param>
    /// <param name="intBits">The integer bits.</param>
    /// <returns>The new decimal.</returns>
    public static decimal Assemble(byte signBit, byte scaleBits, UInt128 intBits)
    {
        // Check signBit has a valid value.
        if (signBit > 1)
        {
            throw new ArgumentOutOfRangeException(nameof(signBit), "Must be 0 or 1.");
        }

        // Check scaleBits is within the valid range.
        if (scaleBits > MAX_SCALE)
        {
            throw new ArgumentOutOfRangeException(nameof(scaleBits),
                $"Must be less than or equal to {MAX_SCALE}.");
        }

        // Check intBits is within the valid range.
        var intBitsMax = ((UInt128)1 << 96) - 1;
        if (intBits > intBitsMax)
        {
            throw new ArgumentOutOfRangeException(nameof(intBits),
                $"Must be less than or equal to {intBitsMax}.");
        }

        var lo = (int)(intBits & 0xffffffff);
        var mid = (int)((intBits >> 32) & 0xffffffff);
        var hi = (int)((intBits >> 64) & 0xffffffff);

        return new decimal(lo, mid, hi, signBit == 1, scaleBits);
    }

    #endregion Miscellaneous methods
}
