namespace Galaxon.Core.Numbers.Extensions;

/// <summary>Extension methods for short.</summary>
public static class XShort
{
    /// <summary>
    /// Return the absolute value of a short as a ushort.
    /// This addresses an issue with short.Abs(), which is that Abs(short.MinValue) can't be expressed
    /// as a short, and so wrap-around occurs.
    /// Unsigned integer types don't have an Abs() method, so there's no collision.
    /// </summary>
    /// <param name="n">A short value.</param>
    /// <returns>The absolute value as a ushort.</returns>
    public static ushort Abs(short n)
    {
        return n switch
        {
            short.MinValue => short.MaxValue + 1,
            >= 0 => (ushort)n,
            _ => (ushort)-n
        };
    }
}
