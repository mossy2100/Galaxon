using System.Numerics;
using Galaxon.Core.Types;

namespace Galaxon.Numerics.Extensions.FloatingPoint;

/// <summary>Extension methods for IFloatingPoint{T}.</summary>
public static class FloatingPointExtensions
{
    #region Methods for getting information about a standard floating point type.

    /// <summary>
    /// Get the total number of bits in values of this type.
    /// </summary>
    public static byte GetTotalNumBits<T>() where T : IFloatingPointIeee754<T>
    {
        return T.Zero switch
        {
            Half => HalfExtensions.TOTAL_NUM_BITS,
            Single => SingleExtensions.TOTAL_NUM_BITS,
            Double => DoubleExtensions.TOTAL_NUM_BITS,
            _ => throw new InvalidOperationException($"Unsupported type: {typeof(T).Name}.")
        };
    }

    /// <summary>
    /// Get the number of bits in the fraction part of values of this type.
    /// </summary>
    public static byte GetNumFracBits<T>() where T : IFloatingPointIeee754<T>
    {
        return T.Zero switch
        {
            Half => HalfExtensions.NUM_FRAC_BITS,
            Single => SingleExtensions.NUM_FRAC_BITS,
            Double => DoubleExtensions.NUM_FRAC_BITS,
            _ => throw new InvalidOperationException($"Unsupported type: {typeof(T).Name}.")
        };
    }

    /// <summary>
    /// Get the number of bits in the exponent part of values of this type.
    /// </summary>
    public static byte GetNumExpBits<T>() where T : IFloatingPointIeee754<T>
    {
        return (byte)(GetTotalNumBits<T>() - GetNumFracBits<T>() - 1);
    }

    /// <summary>
    /// Get the exponent bias for this type.
    /// </summary>
    public static short GetExpBias<T>() where T : IFloatingPointIeee754<T>
    {
        return (short)((1 << (GetNumExpBits<T>() - 1)) - 1);
    }

    /// <summary>
    /// Get the minimum binary exponent supported by the type.
    /// </summary>
    public static short GetMinExp<T>() where T : IFloatingPointIeee754<T>
    {
        return (short)(1 - GetExpBias<T>());
    }

    /// <summary>
    /// Get the maximum binary exponent supported by the type.
    /// </summary>
    public static short GetMaxExp<T>() where T : IFloatingPointIeee754<T>
    {
        return GetExpBias<T>();
    }

    /// <summary>
    /// Get the sign bit mask for this type.
    /// </summary>
    public static ulong GetSignBitMask<T>() where T : IFloatingPointIeee754<T>
    {
        return 1ul << (GetTotalNumBits<T>() - 1);
    }

    /// <summary>
    /// Get the exponent bit mask for this type.
    /// </summary>
    public static ulong GetExpBitMask<T>() where T : IFloatingPointIeee754<T>
    {
        return ((1ul << GetNumExpBits<T>()) - 1) << GetNumFracBits<T>();
    }

    /// <summary>
    /// Get the fraction bit mask for this type.
    /// </summary>
    public static ulong GetFracBitMask<T>() where T : IFloatingPointIeee754<T>
    {
        return (1ul << GetNumFracBits<T>()) - 1;
    }

    /// <summary>
    /// Get the minimum positive subnormal value for this type.
    /// </summary>
    public static T GetMinPosSubnormalValue<T>() where T : IFloatingPointIeee754<T>
    {
        try
        {
            return ReflectionExtensions.GetStaticFieldOrPropertyValue<T, T>("Epsilon");
        }
        catch
        {
            throw new InvalidOperationException(
                $"Could not find the minimum positive subnormal value for {typeof(T).Name}.");
        }
    }

    /// <summary>
    /// Get the maximum positive subnormal value for this type.
    /// </summary>
    public static T GetMaxPosSubnormalValue<T>() where T : IFloatingPointIeee754<T>
    {
        return Assemble<T>(0, 0, GetFracBitMask<T>());
    }

    /// <summary>
    /// Get the minimum positive normal value for this type.
    /// </summary>
    public static T GetMinPosNormalValue<T>() where T : IFloatingPointIeee754<T>
    {
        return Assemble<T>(0, 1, 0);
    }

    /// <summary>
    /// Get the minimum positive subnormal value for this type.
    /// </summary>
    public static T GetMaxPosNormalValue<T>() where T : IFloatingPointIeee754<T>
    {
        try
        {
            return ReflectionExtensions.GetStaticFieldOrPropertyValue<T, T>("MaxValue");
        }
        catch
        {
            throw new InvalidOperationException(
                $"Could not find the maximum positive normal value for {typeof(T).Name}.");
        }
    }

    /// <summary>
    /// Get the negative infinity value for a standard binary floating point type.
    /// </summary>
    /// <typeparam name="T">The standard binary floating point type.</typeparam>
    /// <returns>The value of the NegativeInfinity property.</returns>
    /// <exception cref="MissingMemberException">
    /// If the class doesn't have a static field or property names "NegativeInfinity".
    /// </exception>
    public static T GetNegativeInfinity<T>() where T : IFloatingPointIeee754<T>
    {
        try
        {
            return ReflectionExtensions.GetStaticFieldOrPropertyValue<T, T>("NegativeInfinity");
        }
        catch
        {
            throw new MissingMemberException(typeof(T).Name, "NegativeInfinity");
        }
    }

    /// <summary>
    /// Get the positive infinity value for a standard binary floating point type.
    /// </summary>
    /// <typeparam name="T">The standard binary floating point type.</typeparam>
    /// <returns>The value of the PositiveInfinity property.</returns>
    /// <exception cref="MissingMemberException">
    /// If the class doesn't have a static field or property names "PositiveInfinity".
    /// </exception>
    public static T GetPositiveInfinity<T>() where T : IFloatingPointIeee754<T>
    {
        try
        {
            return ReflectionExtensions.GetStaticFieldOrPropertyValue<T, T>("PositiveInfinity");
        }
        catch
        {
            throw new MissingMemberException(typeof(T).Name, "PositiveInfinity");
        }
    }

    /// <summary>
    /// Get the positive and negative infinity values for a standard binary floating point type.
    /// </summary>
    /// <typeparam name="T">The standard binary floating point type.</typeparam>
    /// <returns>
    /// The value of the NegativeInfinity and PositiveInfinity fields or properties.
    /// </returns>
    /// <exception cref="MissingMemberException">
    /// If the class doesn't have static fields or properties called "NegativeInfinity" and
    /// "PositiveInfinity".
    /// </exception>
    public static (T min, T max) GetInfinities<T>() where T : IFloatingPointIeee754<T>
    {
        return (GetNegativeInfinity<T>(), GetPositiveInfinity<T>());
    }

    #endregion Methods for getting information about a standard floating point type.

    #region Methods for assembling and disassembling floating point values.

    /// <summary>
    /// Disassemble the floating point value into its bitwise components.
    /// </summary>
    public static (byte signBit, ushort expBits, ulong fracBits) Disassemble<T>(this T x)
        where T : IFloatingPointIeee754<T>
    {
        // Get the bits.
        var bits = x switch
        {
            Half h => BitConverter.HalfToUInt16Bits(h),
            float f => BitConverter.SingleToUInt32Bits(f),
            double d => BitConverter.DoubleToUInt64Bits(d),
            _ => throw new InvalidOperationException("Unsupported type.")
        };

        // Get some info about the type.
        var nTotalBits = GetTotalNumBits<T>();
        var nFracBits = GetNumFracBits<T>();
        var signBitMask = GetSignBitMask<T>();
        var expBitMask = GetExpBitMask<T>();
        var fracBitMask = GetFracBitMask<T>();

        // Extract the parts.
        var signBit = (byte)((bits & signBitMask) >>> (nTotalBits - 1));
        var expBits = (ushort)((bits & expBitMask) >>> nFracBits);
        var fracBits = bits & fracBitMask;

        return (signBit, expBits, fracBits);
    }

    /// <summary>Get the exponent bits from a floating point number.</summary>
    public static ushort GetExpBits<T>(this T x) where T : IFloatingPointIeee754<T>
    {
        var (_, expBits, _) = x.Disassemble();
        return expBits;
    }

    /// <summary>
    /// Assemble a new floating point value from parts.
    /// </summary>
    /// <param name="signBit">The sign bit (1 or 0).</param>
    /// <param name="expBits">The exponent bits.</param>
    /// <param name="fracBits">The fraction bits.</param>
    /// <typeparam name="T">A floating point type.</typeparam>
    /// <returns>The new floating point value.</returns>
    /// <exception cref="InvalidOperationException">If the type is unsupported.</exception>
    public static T Assemble<T>(byte signBit, ushort expBits, ulong fracBits)
        where T : IFloatingPointIeee754<T>
    {
        var nFracBits = GetNumFracBits<T>();
        var nExpBits = GetNumExpBits<T>();
        var nSignBitShift = (byte)(nFracBits + nExpBits);

        // Check values are within valid ranges for the type.
        if (signBit > 1)
        {
            throw new ArgumentOutOfRangeException(nameof(signBit), "Must be 0 or 1.");
        }
        var expBitsMax = (ushort)((1 << nExpBits) - 1);
        if (expBits > expBitsMax)
        {
            throw new ArgumentOutOfRangeException(nameof(expBits),
                $"Must be less than or equal to {expBitsMax}.");
        }
        var fracBitsMax = GetFracBitMask<T>();
        if (fracBits > fracBitsMax)
        {
            throw new ArgumentOutOfRangeException(nameof(fracBits),
                $"Must be less than or equal to {fracBitsMax}.");
        }

        // Construct the sequence of bits.
        var bits = ((ulong)signBit << nSignBitShift) | ((ulong)expBits << nFracBits) | fracBits;

        // Return a value of the correct type.
        var t = typeof(T);
        if (t == typeof(Half))
        {
            return (T)(object)BitConverter.UInt16BitsToHalf((ushort)bits);
        }
        if (t == typeof(float))
        {
            return (T)(object)BitConverter.UInt32BitsToSingle((uint)bits);
        }
        if (t == typeof(double))
        {
            return (T)(object)BitConverter.UInt64BitsToDouble(bits);
        }

        throw new InvalidOperationException("Unsupported type.");
    }

    #endregion Methods for assembling and disassembling floating point values.
}
