using System.Numerics;
using Galaxon.Core.Types;

namespace Galaxon.Core.Numbers;

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
        return ReflectionExtensions.ImplementsSelfReferencingGenericInterface(type, typeof(INumberBase<>));
    }

    /// <summary>
    /// Check if a type is a signed number type.
    /// </summary>
    /// <param name="type">Some type.</param>
    /// <returns>If the type implements ISignedNumber{TSelf}.</returns>
    public static bool IsSignedNumberType(Type type)
    {
        return ReflectionExtensions.ImplementsSelfReferencingGenericInterface(type, typeof(ISignedNumber<>));
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

    #region Methods related to static properties

    /// <summary>
    /// Get the value of the static field or property for a specified number type.
    /// </summary>
    /// <typeparam name="T">The number type.</typeparam>
    /// <param name="name">The name of the static field or property.</param>
    /// <returns>The value of the specified field or property.</returns>
    /// <exception cref="MissingMemberException">
    /// If the class doesn't have a static field or property with the given name.
    /// </exception>
    public static T GetStaticValue<T>(string name) where T : INumberBase<T>
    {
        try
        {
            return ReflectionExtensions.GetStaticFieldOrPropertyValue<T, T>(name);
        }
        catch
        {
            throw new MissingMemberException(typeof(T).Name, name);
        }
    }

    /// <summary>
    /// Get the min value for a specified number type, if specified.
    /// </summary>
    /// <typeparam name="T">The number type.</typeparam>
    /// <returns>The value of the MinValue property.</returns>
    /// <exception cref="MissingMemberException">
    /// If the class doesn't have a static field or property names "MinValue".
    /// </exception>
    public static T GetMinValue<T>() where T : INumberBase<T>
    {
        return GetStaticValue<T>("MinValue");
    }

    /// <summary>
    /// Get the maximum value for a specified number type, if specified.
    /// </summary>
    /// <typeparam name="T">The number type.</typeparam>
    /// <returns>The value of the MaxValue property.</returns>
    /// <exception cref="MissingMemberException">
    /// If the class doesn't have a static field or property names "MaxValue".
    /// </exception>
    public static T GetMaxValue<T>() where T : INumberBase<T>
    {
        return GetStaticValue<T>("MaxValue");
    }

    /// <summary>
    /// Get the minimum and maximum values for a specified number type.
    /// </summary>
    /// <typeparam name="T">The number type.</typeparam>
    /// <returns>The value of the MinValue and MaxValue fields ot properties.</returns>
    /// <exception cref="MissingMemberException">
    /// If the class doesn't have static fields or properties called "MinValue" and "MaxValue".
    /// </exception>
    public static (T min, T max) GetRange<T>() where T : INumberBase<T>
    {
        return (GetMinValue<T>(), GetMaxValue<T>());
    }

    #endregion Methods related to static properties
}
