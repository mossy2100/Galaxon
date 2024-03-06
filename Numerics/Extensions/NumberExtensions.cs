using System.Globalization;
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
    /// Corrected modulo operation, using floored division.
    /// The modulus will always have the same sign as the divisor.
    /// Unlike the truncated division and modulo provided by C#'s operators, floored division
    /// produces a regular cycling pattern through both negative and positive values of the divisor.
    /// It permits things like:
    /// bool isOdd = Modulo(num, 2) == 1;
    /// Trying to do this using the % operator will fail for negative divisors, however. e.g.
    /// bool isOdd = num % 2 == 1;
    /// In this case, if num is negative 0, num % 2 == -1
    /// </summary>
    /// <see href="https://en.wikipedia.org/wiki/Modulo_operation"/>
    public static T Mod<T>(T a, T b) where T : INumberBase<T>, IModulusOperators<T, T, T>,
        IComparisonOperators<T, T, bool>
    {
        T r = a % b;
        return r < T.Zero ? r + b : r;
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
