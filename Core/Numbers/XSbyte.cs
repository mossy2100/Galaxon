namespace Galaxon.Core.Numbers;

/// <summary>Extension methods for sbyte.</summary>
public static class XSbyte
{
    /// <summary>
    /// Return the absolute value of an sbyte as a byte.
    /// This addresses an issue with sbyte.Abs(), which is that Abs(sbyte.MinValue) can't be
    /// expressed as a sbyte, and so wrap-around occurs.
    /// Unsigned integer types don't have an Abs() method, so there's no collision.
    /// </summary>
    /// <param name="n">A sbyte value.</param>
    /// <returns>The absolute value as a byte.</returns>
    public static byte Abs(sbyte n)
    {
        return n switch
        {
            sbyte.MinValue => sbyte.MaxValue + 1,
            >= 0 => (byte)n,
            _ => (byte)-n
        };
    }
}
