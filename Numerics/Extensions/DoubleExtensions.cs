namespace Galaxon.Numerics.Extensions;

/// <summary>Extension methods and other bonus stuff for double.</summary>
public static class DoubleExtensions
{
    #region Constants

    /// <summary>
    /// The default maximum difference between 2 double values being compared for equality.
    /// </summary>
    public const double DELTA = 1e-9;

    /// <summary>The total number of bits in the value.</summary>
    public const byte TOTAL_NUM_BITS = 64;

    /// <summary>The number of bits in the fraction.</summary>
    public const byte NUM_FRAC_BITS = 52;

    #endregion Constants

    #region Miscellaneous methods

    /// <summary>
    /// Round off a value to a given number of significant figures.
    /// </summary>
    /// <param name="d">The number to round.</param>
    /// <param name="nSigFigs">The number of significant figures.</param>
    /// <returns>The rounded number.</returns>
    public static double RoundSigFigs(double d, int nSigFigs)
    {
        if (d <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(nSigFigs), "Must be positive.");
        }

        double scale = Math.Pow(10, Math.Floor(Math.Log10(d)) + 1);
        double result = scale * Math.Round(d / scale, nSigFigs);

        return result;
    }

    /// <summary>
    /// Disassemble the double into its bitwise components.
    /// </summary>
    /// <see href="https://en.wikipedia.org/wiki/Double-precision_floating-point_format"/>
    public static (byte signBit, ushort expBits, ulong fracBits) Disassemble(this double x)
    {
        return x.Disassemble<double>();
    }

    /// <summary>
    /// Assemble a new double from parts.
    /// </summary>
    /// <param name="signBit">The sign bit (1 or 0).</param>
    /// <param name="expBits">The exponent bits.</param>
    /// <param name="fracBits">The fraction bits.</param>
    /// <returns>The new double.</returns>
    public static double Assemble(byte signBit, ushort expBits, ulong fracBits)
    {
        return FloatingPointExtensions.Assemble<double>(signBit, expBits, fracBits);
    }

    /// <summary>
    /// Return the fractional part of a double.
    /// The result will have the same sign as the argument.
    /// </summary>
    /// <param name="x">A double value.</param>
    /// <returns>The fractional part of the value.</returns>
    public static double Frac(double x)
    {
        return x - double.Truncate(x);
    }

    #endregion Miscellaneous methods

    #region Methods for checking doubles as integers

    /// <summary>
    /// Check if a double is a positive integer.
    /// </summary>
    public static bool IsPositiveInteger(double d)
    {
        return d > 0 && double.IsInteger(d);
    }

    /// <summary>
    /// Check if a double is a negative integer.
    /// </summary>
    public static bool IsNegativeInteger(double d)
    {
        return d < 0 && double.IsInteger(d);
    }

    /// <summary>
    /// Check if a value is a perfect square.
    /// </summary>
    public static bool IsPerfectSquare(double d)
    {
        return double.IsPositive(d) && double.IsInteger(Math.Sqrt(d));
    }

    #endregion Methods for checking doubles as integers

    #region Methods for fuzzy equals

    /// <summary>
    /// Check if 2 double values are equal for practical purposes.
    /// If two double values differ only by the least significant bit, this is more likely
    /// due to inaccuracies in floating point representations than actual inequality.
    /// This code is copied/adapted from Google Guava DoubleMath.fuzzyEquals().
    /// <see href="https://github.com/google/guava/blob/master/guava/src/com/google/common/math/DoubleMath.java#L360"/>
    /// I initially tried the algorithm from the Microsoft documentation, it didn't work in all cases.
    /// <see href="https://learn.microsoft.com/en-us/dotnet/api/system.double.equals?view=net-7.0#system-double-equals(system-double)"/>
    /// </summary>
    /// <param name="a">First number.</param>
    /// <param name="b">Second number.</param>
    /// <param name="tolerance">The maximum allowable difference between them.</param>
    /// <returns>If close enough to equal.</returns>
    public static bool FuzzyEquals(this double a, double b, double tolerance = DELTA)
    {
        // Ensure tolerance is non-negative.
        if (tolerance < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(tolerance), "Cannot be negative.");
        }

        // ReSharper disable once CompareOfFloatsByEqualityOperator
        // Handle NaN separately so the method behaves the same as double.Equals().
        // The equality operator will return false when comparing 2 NaN values.
        // The equality operator will return true when comparing infinities.
        return (double.IsNaN(a) && double.IsNaN(b)) || a == b || Math.Abs(a - b) <= tolerance;
    }

    /// <summary>
    /// Compare two nullable doubles for fuzzy equality.
    /// </summary>
    /// <param name="a"></param>
    /// <param name="b"></param>
    /// <param name="tolerance"></param>
    /// <returns></returns>
    public static bool FuzzyEquals(this double? a, double? b, double tolerance = DELTA)
    {
        // If they're both null then they're equal.
        if (!a.HasValue && !b.HasValue)
        {
            return true;
        }

        // If only one of them is null they're unequal.
        if (!a.HasValue || !b.HasValue)
        {
            return false;
        }

        // Check for fuzzy equality.
        return a.Value.FuzzyEquals(b.Value, tolerance);
    }

    /// <summary>
    /// IsInteger() can be a bit strict. This method allows for some fuzziness.
    /// </summary>
    public static bool FuzzyIsInteger(double d, double tolerance = DELTA)
    {
        return d.FuzzyEquals(Math.Round(d), tolerance);
    }

    /// <summary>
    /// Check if a double is a positive integer, with some fuzziness.
    /// </summary>
    public static bool FuzzyIsPositiveInteger(double d, double tolerance = DELTA)
    {
        return d > 0 && FuzzyIsInteger(d, tolerance);
    }

    /// <summary>
    /// Check if a double is a negative integer, with some fuzziness.
    /// </summary>
    public static bool FuzzyIsNegativeInteger(double d, double tolerance = DELTA)
    {
        return d < 0 && FuzzyIsInteger(d, tolerance);
    }

    #endregion Methods for fuzzy equals
}
