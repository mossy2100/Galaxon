namespace Galaxon.Numerics.Extensions;

/// <summary>Extension methods for long.</summary>
public static class Int64Extensions
{
    /// <summary>
    /// Get the long value closest to x^y.
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <returns></returns>
    public static long Pow(long x, long y)
    {
        return (long)Math.Round(Math.Pow(x, y));
    }

    /// <summary>
    /// Get the long value closest to √x.
    /// </summary>
    /// <param name="x"></param>
    /// <returns></returns>
    public static long Sqrt(long x)
    {
        return (long)Math.Round(Math.Sqrt(x));
    }

    /// <summary>
    /// Return the absolute value of a long as a ulong.
    /// This addresses an issue with long.Abs(), which is that Abs(long.MinValue) can't be expressed
    /// as a long, and so wrap-around occurs.
    /// </summary>
    /// <param name="n">A long value.</param>
    /// <returns>The absolute value as a ulong.</returns>
    public static ulong Abs(long n)
    {
        return n switch
        {
            long.MinValue => long.MaxValue + 1ul,
            >= 0 => (ulong)n,
            _ => (ulong)(-n)
        };
    }

    /// <summary>
    /// Get the ordinal suffix of an integer.
    /// </summary>
    /// <param name="n">An integer.</param>
    /// <returns>The suffix.</returns>
    public static string GetOrdinalSuffix(long n)
    {
        ulong m = Abs(n);
        ulong a = m % 10;
        ulong b = m % 100;
        return a switch
        {
            1 when b != 11 => "st",
            2 when b != 12 => "nd",
            3 when b != 13 => "rd",
            _ => "th"
        };
    }
}
