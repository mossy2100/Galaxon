using System.Numerics;
using System.Text.RegularExpressions;
using Galaxon.Core.Types;

namespace Galaxon.Numerics.Extensions;

/// <summary>Extension methods for numbers (INumber{T} and INumberBase{T}).</summary>
/// <remarks>
/// TODO: Sort out methods to check for implementation of generic interfaces.
/// </remarks>
public static class NumberExtensions
{
    #region Methods for inspecting the number type

    /// <summary>
    /// Check if a type is a standard numerical type in .NET.
    /// Excludes char, vector, and matrix types.
    /// </summary>
    /// <param name="type">The type to check.</param>
    /// <returns>True if the type is a standard numerical type.</returns>
    public static bool IsStandardNumberType(Type type)
    {
        return type == typeof(sbyte)
            || type == typeof(byte)
            || type == typeof(short)
            || type == typeof(ushort)
            || type == typeof(int)
            || type == typeof(uint)
            || type == typeof(long)
            || type == typeof(ulong)
            || type == typeof(Int128)
            || type == typeof(UInt128)
            || type == typeof(Half)
            || type == typeof(float)
            || type == typeof(double)
            || type == typeof(decimal)
            || type == typeof(BigInteger)
            || type == typeof(Complex);
    }

    /// <summary>
    /// Check if a type is a number type.
    /// </summary>
    /// <param name="type">Some type.</param>
    /// <returns>If the type implements INumberBase{TSelf}.</returns>
    public static bool IsNumberType(Type type)
    {
        return ReflectionExtensions.ImplementsSelfReferencingGenericInterface(type,
            typeof(INumberBase<>));
    }

    /// <summary>
    /// Check if a type is a signed number type.
    /// </summary>
    /// <param name="type">Some type.</param>
    /// <returns>If the type implements ISignedNumber{TSelf}.</returns>
    public static bool IsSignedNumberType(Type type)
    {
        return ReflectionExtensions.ImplementsSelfReferencingGenericInterface(type,
            typeof(ISignedNumber<>));
    }

    /// <summary>
    /// Check if a type is an unsigned number type.
    /// </summary>
    /// <param name="type">Some type.</param>
    /// <returns>If the type implements IUnsignedNumber{TSelf}.</returns>
    public static bool IsUnsignedNumberType(Type type)
    {
        return ReflectionExtensions.ImplementsSelfReferencingGenericInterface(type,
            typeof(IUnsignedNumber<>));
    }

    /// <summary>
    /// Check if a type is an integer type.
    /// </summary>
    /// <param name="type">Some type.</param>
    /// <returns>If the type implements IBinaryInteger{TSelf}.</returns>
    public static bool IsIntegerType(Type type)
    {
        return ReflectionExtensions.ImplementsSelfReferencingGenericInterface(type,
            typeof(IBinaryInteger<>));
    }

    /// <summary>
    /// Check if a type is a floating point type.
    /// </summary>
    /// <param name="type">Some type.</param>
    /// <returns>If the type implements IFloatingPoint{TSelf}.</returns>
    public static bool IsFloatingPointType(Type type)
    {
        return ReflectionExtensions.ImplementsSelfReferencingGenericInterface(type,
            typeof(IFloatingPoint<>));
    }

    /// <summary>
    /// Check if a type is a signed integer type.
    /// </summary>
    /// <param name="type">Some type.</param>
    /// <returns></returns>
    public static bool IsSignedIntegerType(Type type)
    {
        return IsSignedNumberType(type) && IsIntegerType(type);
    }

    /// <summary>
    /// Check if a type is an unsigned integer type.
    /// </summary>
    /// <param name="type">Some type.</param>
    /// <returns></returns>
    public static bool IsUnsignedIntegerType(Type type)
    {
        return IsUnsignedNumberType(type) && IsIntegerType(type);
    }

    /// <summary>
    /// Check if a type is a real (non-complex) number type.
    /// </summary>
    /// <param name="type">Some type.</param>
    /// <returns></returns>
    public static bool IsRealNumberType(Type type)
    {
        return IsIntegerType(type) || IsFloatingPointType(type);
    }

    /// <summary>
    /// Check if a type is a complex number type.
    /// TODO Make it support BigComplex without actually referencing it.
    /// </summary>
    /// <param name="type">Some type.</param>
    /// <returns></returns>
    public static bool IsComplexNumberType(Type type)
    {
        return type == typeof(Complex);
    }

    #endregion Inspection methods

    #region Division-related methods

    /// <summary>
    /// Performs a modulo operation using floored division, ensuring that the result always has the
    /// same sign as the divisor.
    /// This method differs from the standard '%' operator used in C# and other C-style languages,
    /// which performs truncated division.
    /// Floored division modulo produces a regular cycling pattern through both negative and
    /// positive dividends, making it suitable for scenarios like checking if a number is odd across
    /// both positive and negative domains.
    /// </summary>
    /// <param name="a">The dividend in the modulo operation.</param>
    /// <param name="b">
    /// The divisor in the modulo operation. Must not be zero; otherwise, a
    /// <see cref="DivideByZeroException"/> is thrown.</param>
    /// <returns>
    /// The result of the modulo operation, adjusted so that it always has the same sign as the
    /// divisor <paramref name="b"/>.</returns>
    /// <exception cref="DivideByZeroException">
    /// Thrown when the divisor <paramref name="b"/> is zero.
    /// </exception>
    /// <example>
    /// This example shows how to call the <see cref="Mod{T}(T, T)"/> method.
    /// <code>
    /// int result = NumberExtensions.Mod(-3, 2); // result is 1
    /// </code>
    /// </example>
    /// <remarks>
    /// Using this modulo operation is particularly useful for applications where a consistent
    /// positive remainder is necessary, such as in mathematical computations following modular
    /// arithmetic rules that differ from typical programming language implementations.
    /// </remarks>
    public static T Mod<T>(T a, T b) where T : INumberBase<T>, IModulusOperators<T, T, T>,
        IComparisonOperators<T, T, bool>
    {
        // Perform default modulo operation, which will work fine if both operands are positive.
        T r = a % b;

        // Adjust the result to ensure it has the same sign as the divisor.
        return (r < T.Zero && b > T.Zero) || (r > T.Zero && b < T.Zero) ? r + b : r;
    }

    #endregion Division-related methods

    #region String methods

    /// <summary>
    /// Removed whitespace and digit group separator characters from a string.
    /// This is useful for parsing numeric strings.
    ///
    /// This includes:
    ///   * whitespace (all ASCII and non-ASCII whitespace characters)
    ///   * commas
    ///   * underscores
    ///   * ASCII apostrophes
    ///
    /// Culture isn't considered. A period is expected to mean a decimal point rather than a digit
    /// grouping character.
    /// </summary>
    /// <param name="str">The string.</param>
    /// <returns>The string with the ignorable characters removed.</returns>
    public static string RemoveWhitespaceAndDigitGroupSeparators(string str)
    {
        return Regex.Replace(str, @"[\p{Z},_']", "");
    }

    #endregion String methods

    #region Min and Max

    public static T Min<T>(params T[] values)
        where T : INumberBase<T>, IComparisonOperators<T, T, bool>
    {
        // Guard.
        if (values.Length == 0)
        {
            throw new ArgumentOutOfRangeException(nameof(values),
                "There must be at least one value.");
        }

        // Find the minimum value.
        T min = values[0];
        for (var i = 1; i < values.Length; i++)
        {
            if (values[i] < min)
            {
                min = values[i];
            }
        }
        return min;
    }

    public static T Max<T>(params T[] values)
        where T : INumberBase<T>, IComparisonOperators<T, T, bool>
    {
        // Guard.
        if (values.Length == 0)
        {
            throw new ArgumentOutOfRangeException(nameof(values),
                "There must be at least one value.");
        }

        // Find the minimum value.
        T max = values[0];
        for (var i = 1; i < values.Length; i++)
        {
            if (values[i] > max)
            {
                max = values[i];
            }
        }
        return max;
    }

    #endregion Min and Max
}
