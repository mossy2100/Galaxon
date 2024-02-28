using System.Reflection;

namespace Galaxon.Core.Types;

/// <summary>
/// Handy reflection-related methods.
/// </summary>
public static class ReflectionExtensions
{
    #region Access static field or property

    /// <summary>
    /// Get the value of a static field.
    /// </summary>
    /// <param name="classType">The class type.</param>
    /// <param name="name">The name of the field.</param>
    /// <typeparam name="TField">The field type.</typeparam>
    /// <returns>The value of the static field.</returns>
    /// <exception cref="MissingFieldException">
    /// If the static field doesn't exist on the specified type.
    /// </exception>
    public static TField GetStaticFieldValue<TField>(Type classType, string name)
    {
        FieldInfo? fieldInfo = classType.GetField(name);
        if (fieldInfo != null && fieldInfo.GetValue(null) is TField value)
        {
            return value;
        }

        // Exception.
        throw new MissingFieldException(classType.Name, name);
    }

    /// <summary>
    /// Get the value of a static field.
    /// </summary>
    /// <param name="name">The name of the field.</param>
    /// <typeparam name="TClass">The class type.</typeparam>
    /// <typeparam name="TField">The field type.</typeparam>
    /// <returns>The value of the static field.</returns>
    /// <exception cref="MissingFieldException">
    /// If the static field doesn't exist on the specified type.
    /// </exception>
    public static TField GetStaticFieldValue<TClass, TField>(string name)
    {
        Type classType = typeof(TClass);
        return GetStaticFieldValue<TField>(classType, name);
    }

    /// <summary>
    /// Get the value of a static property.
    /// </summary>
    /// <param name="name">The name of the property.</param>
    /// <param name="classType">The class type.</param>
    /// <typeparam name="TProperty">The property type.</typeparam>
    /// <returns>The value of the static property.</returns>
    /// <exception cref="MissingMemberException">
    /// If the static property doesn't exist on the specified type.
    /// </exception>
    public static TProperty GetStaticPropertyValue<TProperty>(Type classType, string name)
    {
        PropertyInfo? propertyInfo = classType.GetProperty(name);
        if (propertyInfo != null && propertyInfo.GetValue(null) is TProperty value)
        {
            return value;
        }

        // Exception.
        throw new MissingMemberException(classType.Name, name);
    }

    /// <summary>
    /// Get the value of a static property.
    /// </summary>
    /// <param name="name">The name of the property.</param>
    /// <typeparam name="TClass">The class type.</typeparam>
    /// <typeparam name="TProperty">The property type.</typeparam>
    /// <returns>The value of the static property.</returns>
    /// <exception cref="MissingMemberException">
    /// If the static property doesn't exist on the specified type.
    /// </exception>
    public static TProperty GetStaticPropertyValue<TClass, TProperty>(string name)
    {
        Type classType = typeof(TClass);
        return GetStaticPropertyValue<TProperty>(classType, name);
    }

    /// <summary>
    /// Get the value of a static field or property.
    /// </summary>
    /// <param name="name">The name of the property.</param>
    /// <param name="classType">The class type.</param>
    /// <typeparam name="TMember">The field or property type.</typeparam>
    /// <returns>The value of the static field or property.</returns>
    /// <exception cref="MissingMemberException">
    /// If the static field or property doesn't exist on the specified type.
    /// </exception>
    public static TMember GetStaticFieldOrPropertyValue<TMember>(Type classType, string name)
    {
        try
        {
            // Try to get the field.
            return GetStaticFieldValue<TMember>(classType, name);
        }
        catch (MissingFieldException)
        {
            // Field not found, let's try the property.
            try
            {
                return GetStaticPropertyValue<TMember>(classType, name);
            }
            catch (MissingMemberException)
            {
                // Neither was found, exception.
                throw new MissingMemberException(classType.Name, name);
            }
        }
    }

    /// <summary>
    /// Get the value of a static field or property.
    /// </summary>
    /// <param name="name">The name of the property.</param>
    /// <typeparam name="TClass">The class type.</typeparam>
    /// <typeparam name="TMember">The field or property type.</typeparam>
    /// <returns>The value of the static field or property.</returns>
    /// <exception cref="MissingMemberException">
    /// If the static field or property doesn't exist on the specified type.
    /// </exception>
    public static TMember GetStaticFieldOrPropertyValue<TClass, TMember>(string name)
    {
        Type classType = typeof(TClass);
        return GetStaticFieldOrPropertyValue<TMember>(classType, name);
    }

    #endregion Access static field or property

    #region Type conversion

    /// <summary>
    /// Get the method that converts one type to another, if it exists, otherwise null.
    /// </summary>
    /// <param name="sourceType">The source type.</param>
    /// <param name="targetType">The target type.</param>
    /// <returns>The method info.</returns>
    public static MethodInfo? GetConversionMethod(Type sourceType, Type targetType)
    {
        // Search methods on the System.Convert type, the source type, and the target type, for a
        // method with a name of the form "ToType" or "FromType", or a user-defined cast operator.
        Type[] types = [typeof(Convert), sourceType, targetType];
        foreach (Type type in types)
        {
            IEnumerable<MethodInfo> methods =
                type.GetMethods(BindingFlags.Public | BindingFlags.Static);
            foreach (MethodInfo method in methods)
            {
                if ((method.Name == "op_Implicit"
                        || method.Name == "op_Explicit"
                        || method.Name == "To" + targetType.Name
                        || method.Name == "From" + sourceType.Name
                        || (method.Name == "Parse" && sourceType == typeof(string)))
                    && method.ReturnType == targetType)
                {
                    ParameterInfo[] parameters = method.GetParameters();
                    if (parameters.Length == 1 && parameters[0].ParameterType == sourceType)
                    {
                        // Return the first matching method.
                        return method;
                    }
                }
            }
        }

        // No matching conversion method found, return null.
        return null;
    }

    /// <summary>
    /// Get the method that converts one type to another, if it exists, otherwise null.
    /// </summary>
    /// <typeparam name="TSource">The source type.</typeparam>
    /// <typeparam name="TTarget">The target type.</typeparam>
    /// <returns>The method info.</returns>
    public static MethodInfo? GetConversionMethod<TSource, TTarget>()
    {
        return GetConversionMethod(typeof(TSource), typeof(TTarget));
    }

    /// <summary>
    /// See if a conversion operator exists from one type to another.
    /// </summary>
    /// <param name="sourceType">The source type.</param>
    /// <param name="targetType">The target type.</param>
    /// <returns>If a conversion operator exists.</returns>
    private static bool CanConvert(Type sourceType, Type targetType)
    {
        return GetConversionMethod(sourceType, targetType) != null;
    }

    /// <summary>
    /// See if a conversion operator exists from one type to another.
    /// </summary>
    /// <typeparam name="TSource">The source type.</typeparam>
    /// <typeparam name="TTarget">The target type.</typeparam>
    /// <returns>If a conversion operator exists.</returns>
    public static bool CanConvert<TSource, TTarget>()
    {
        return CanConvert(typeof(TSource), typeof(TTarget));
    }

    /// <summary>
    /// Case a value from a source type to a target type.
    /// </summary>
    /// <typeparam name="TSource">The source type.</typeparam>
    /// <typeparam name="TTarget">The target type.</typeparam>
    /// <param name="src">The source value.</param>
    /// <returns>The target value.</returns>
    /// <exception cref="InvalidCastException">If the conversion failed.</exception>
    public static TTarget Convert<TSource, TTarget>(TSource src)
    {
        Type sourceType = typeof(TSource);
        Type targetType = typeof(TTarget);

        // Get the conversion method, if it exists.
        MethodInfo? methodInfo = GetConversionMethod(sourceType, targetType);
        if (methodInfo == null)
        {
            throw new InvalidCastException(
                $"No operator exists for converting from {sourceType.Name} to {targetType.Name}.");
        }

        // Try to do the conversion.
        bool ok;
        Exception? innerException = null;
        object? result = null;
        try
        {
            result = methodInfo.Invoke(null, [src]);
            // We'll assume a null result means the conversion failed.
            ok = result != null;
        }
        catch (Exception ex)
        {
            ok = false;
            innerException = ex;
        }

        // If it didn't work, throw an exception.
        if (!ok)
        {
            throw new InvalidCastException(
                $"Converting from {sourceType.Name} to {targetType.Name} failed.", innerException);
        }

        return (TTarget)result!;
    }

    #endregion Type conversion

    #region Check for interface implementation

    /// <summary>
    /// Check if a type implements an interface.
    /// </summary>
    /// <param name="type">The type.</param>
    /// <param name="interfaceType">The interface type.</param>
    /// <returns>True if the specified type implements the specified interface.</returns>
    public static bool ImplementsInterface(Type type, Type interfaceType)
    {
        return type.GetInterfaces().Any(i => i == interfaceType);
    }

    /// <summary>
    /// Check if a type implements a generic interface.
    /// </summary>
    /// <param name="type">The type.</param>
    /// <param name="interfaceType">The generic interface type.</param>
    /// <returns>True if the specified type implements the specified interface.</returns>
    public static bool ImplementsGenericInterface(Type type, Type interfaceType)
    {
        return type.GetInterfaces().Any(i => i.IsGenericType
            && i.GetGenericTypeDefinition() == interfaceType);
    }

    /// <summary>
    /// Check if a type implements a self-referencing generic interface
    /// (e.g. IBinaryInteger{TSelf}).
    /// Only works if the self-referenced type is the first type parameter.
    /// </summary>
    /// <param name="type">The type.</param>
    /// <param name="interfaceType">The self-referencing generic interface.</param>
    /// <returns>True if the specified type implements the specified interface.</returns>
    public static bool ImplementsSelfReferencingGenericInterface(Type type, Type interfaceType)
    {
        return type.GetInterfaces().Any(i => i.IsGenericType
            && i.GetGenericTypeDefinition() == interfaceType
            && i.GenericTypeArguments[0] == type);
    }

    #endregion Check for interface implementation
}
