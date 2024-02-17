namespace Galaxon.Core.Numbers.Extensions;

/// <summary>Extension methods and other bonus stuff for float.</summary>
public static class XFloat
{
    /// <summary>The total number of bits in the value.</summary>
    public const byte TOTAL_NUM_BITS = 32;

    /// <summary>The number of bits in the fraction.</summary>
    public const byte NUM_FRAC_BITS = 23;

    /// <summary>
    /// Disassemble the float into its bitwise components.
    /// </summary>
    /// <see href="https://en.wikipedia.org/wiki/Single-precision_floating-point_format"/>
    public static (byte signBit, ushort expBits, ulong fracBits) Disassemble(this float x)
    {
        return x.Disassemble<float>();
    }

    /// <summary>
    /// Assemble a new float from parts.
    /// </summary>
    /// <param name="signBit">The sign bit (1 or 0).</param>
    /// <param name="expBits">The exponent bits.</param>
    /// <param name="fracBits">The fraction bits.</param>
    /// <returns>The new float.</returns>
    public static float Assemble(byte signBit, ushort expBits, ulong fracBits)
    {
        return XFloatingPoint.Assemble<float>(signBit, expBits, fracBits);
    }
}
