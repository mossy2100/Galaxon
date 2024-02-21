namespace Galaxon.Numerics.Extensions;

/// <summary>Extension methods and other bonus stuff for Half.</summary>
public static class HalfExtensions
{
    /// <summary>The total number of bits in the value.</summary>
    public const byte TOTAL_NUM_BITS = 16;

    /// <summary>The number of bits in the fraction.</summary>
    public const byte NUM_FRAC_BITS = 10;

    /// <summary>
    /// Disassemble the Half into its bitwise components.
    /// </summary>
    /// <see href="https://en.wikipedia.org/wiki/Half-precision_floating-point_format"/>
    public static (byte signBit, ushort expBits, ulong fracBits) Disassemble(this Half x)
    {
        return x.Disassemble<Half>();
    }

    /// <summary>
    /// Assemble a new Half from parts.
    /// </summary>
    /// <param name="signBit">The sign bit (1 or 0).</param>
    /// <param name="expBits">The exponent bits.</param>
    /// <param name="fracBits">The fraction bits.</param>
    /// <returns>The new Half.</returns>
    public static Half Assemble(byte signBit, ushort expBits, ulong fracBits)
    {
        return FloatingPointExtensions.Assemble<Half>(signBit, expBits, fracBits);
    }
}
