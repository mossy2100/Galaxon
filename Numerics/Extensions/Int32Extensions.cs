namespace Galaxon.Numerics.Extensions;

/// <summary>Extension methods for int.</summary>
public static class Int32Extensions
{
    /// <summary>
    /// Get the int closest to the square root of the given int.
    /// </summary>
    /// <param name="n"></param>
    /// <returns></returns>
    public static int Sqrt(int n)
    {
        return (int)Math.Round(Math.Sqrt(n));
    }

    /// <summary>
    /// Return the absolute value of an int as a uint.
    /// This addresses an issue with int.Abs(), which is that Abs(int.MinValue) can't be expressed
    /// as a int, and so wrap-around occurs.
    /// Unsigned integer types don't have an Abs() method, so there's no collision.
    /// </summary>
    /// <param name="n">A int value.</param>
    /// <returns>The absolute value as a uint.</returns>
    public static uint Abs(int n)
    {
        return n switch
        {
            int.MinValue => int.MaxValue + 1u,
            >= 0 => (uint)n,
            _ => (uint)-n
        };
    }
}
