using System.Reflection;

namespace Galaxon.Core.Types;

/// <summary>
/// Handy reflection-related methods.
/// </summary>
public static class ReflectionExtensions
{
    #region Methods for accessing static members of a type

    /// <summary>
    /// Get the value of a static field.
    /// </summary>
    /// <param name="classType">The class type.</param>
    /// <param name="name">The name of the field.</param>
    /// <typeparam name="TField">The field type.</typeparam>
    /// <returns>The value of the static field.</returns>
    /// <exception cref="MissingMemberException">
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
    /// <exception cref="MissingMemberException">
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
        catch (MissingMemberException)
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

    #endregion Methods for accessing static members of a type

    #region Check for cast operators

    /// <summary>
    /// See if a cast operator exists from one type to another.
    /// </summary>
    /// <param name="sourceType">The source type.</param>
    /// <param name="targetType">The target type.</param>
    /// <returns>If a cast operator exists.</returns>
    public static bool CanCast(Type sourceType, Type targetType)
    {
        // Search for explicit and implicit cast operators.
        return targetType.GetMethods(BindingFlags.Public | BindingFlags.Static)
            .Any(m =>
            {
                var parameters = m.GetParameters();
                return m.Name is "op_Implicit" or "op_Explicit"
                    && m.ReturnType == targetType
                    && parameters.Length == 1
                    && parameters[0].ParameterType == sourceType;
            });
    }

    /// <summary>
    /// Get the info for a method that casts one type to another.
    /// </summary>
    /// <param name="sourceType">The source type.</param>
    /// <param name="targetType">The target type.</param>
    /// <returns>The method info.</returns>
    public static MethodInfo? GetCastMethod(Type sourceType, Type targetType)
    {
        // Check the target type.
        var targetTypeMethod = targetType.GetMethods(BindingFlags.Public | BindingFlags.Static)
            .FirstOrDefault(m =>
            {
                var parameters = m.GetParameters();
                return m.Name is "op_Implicit" or "op_Explicit"
                    && m.ReturnType == targetType
                    && parameters.Length == 1
                    && parameters[0].ParameterType == sourceType;
            });
        if (targetTypeMethod != null) return targetTypeMethod;

        // Check the source type.
        return sourceType.GetMethods(BindingFlags.Public | BindingFlags.Static)
            .FirstOrDefault(m =>
            {
                var parameters = m.GetParameters();
                return m.Name is "op_Implicit" or "op_Explicit"
                    && m.ReturnType == sourceType
                    && parameters.Length == 1
                    && parameters[0].ParameterType == targetType;
            });
    }

    /// <summary>
    /// See if a cast operator exists from one type to another.
    /// </summary>
    /// <typeparam name="TSource">The source type.</typeparam>
    /// <typeparam name="TTarget">The target type.</typeparam>
    /// <returns>If a cast operator exists.</returns>
    public static bool CanCast<TSource, TTarget>()
    {
        return CanCast(typeof(TSource), typeof(TTarget));
    }

    /// <summary>
    /// Case a value from a source type to a target type.
    /// </summary>
    /// <typeparam name="TSource">The source type.</typeparam>
    /// <typeparam name="TTarget">The target type.</typeparam>
    /// <param name="src">The source value.</param>
    /// <returns>The target value.</returns>
    /// <exception cref="InvalidCastException">If the cast failed.</exception>
    public static TTarget Cast<TSource, TTarget>(TSource src)
    {
        var typeSource = typeof(TSource);
        var typeTarget = typeof(TTarget);

        var methodInfo = GetCastMethod(typeSource, typeTarget);
        if (methodInfo == null)
        {
            throw new InvalidCastException(
                $"No operator exists for casting from {typeSource.Name} to {typeTarget.Name}.");
        }

        var tmp = methodInfo.Invoke(null, new object?[] { src });
        if (tmp == null)
        {
            throw new InvalidCastException(
                $"Cast from {typeSource.Name} to {typeTarget.Name} failed.");
        }

        return (TTarget)tmp;
    }

    #endregion Check for cast operators

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
    /// Check if a type implements an interface.
    /// </summary>
    /// <typeparam name="T">The type.</typeparam>
    /// <typeparam name="TInterface">The interface type.</typeparam>
    /// <returns>True if the specified type implements the specified interface.</returns>
    public static bool ImplementsInterface<T, TInterface>()
    {
        return ImplementsInterface(typeof(T), typeof(TInterface));
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
    /// Check if a type implements an generic interface.
    /// </summary>
    /// <typeparam name="T">The type.</typeparam>
    /// <typeparam name="TInterface">The generic interface type.</typeparam>
    /// <returns>True if the specified type implements the specified interface.</returns>
    public static bool ImplementsGenericInterface<T, TInterface>()
    {
        return ImplementsGenericInterface(typeof(T), typeof(TInterface));
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

    /// <summary>
    /// Check if a type implements an self-referencing generic interface.
    /// </summary>
    /// <typeparam name="T">The type.</typeparam>
    /// <typeparam name="TInterface">The self-referencing generic interface type.</typeparam>
    /// <returns>True if the specified type implements the specified interface.</returns>
    public static bool ImplementsSelfReferencingGenericInterface<T, TInterface>()
    {
        return ImplementsSelfReferencingGenericInterface(typeof(T), typeof(TInterface));
    }

    #endregion Check for interface implementation
}
