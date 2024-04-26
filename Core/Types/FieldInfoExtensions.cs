using System.Reflection;
using Microsoft.OpenApi.Attributes;

namespace Galaxon.Core.Types;

/// <summary>
/// Extension methods for FieldInfo.
/// </summary>
public static class FieldInfoExtensions
{
    /// <summary>
    /// Get the value of a static field.
    /// </summary>
    /// <param name="field">A FieldInfo object.</param>
    /// <returns>The value of the field, or null.</returns>
    /// <exception cref="TargetException">Thrown if field is an instance field.</exception>
    public static object? GetValue(this FieldInfo field)
    {
        // Call GetValue() without specifying an instance.
        return field.GetValue(null);
    }

    /// <summary>
    /// Get the value of the Display attribute for a field.
    /// If there is no Display attribute, return an empty string.
    /// </summary>
    /// <param name="field">A FieldInfo object.</param>
    /// <returns>The field's display name.</returns>
    public static string GetDisplayName(this FieldInfo field)
    {
        return (field.GetCustomAttribute(typeof(DisplayAttribute), true) is DisplayAttribute attr)
            ? attr.Name
            : "";
    }
}
