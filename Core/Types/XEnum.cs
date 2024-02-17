using System.ComponentModel;
using System.Reflection;

namespace Galaxon.Core.Types;

/// <summary>
/// Extension methods for enum types.
/// </summary>
public static class XEnum
{
    /// <summary>
    /// Get the value of the Description attribute for the enum value, or, if not provided,
    /// the name of the value (same as ToString()).
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    public static string GetDescription(this Enum value)
    {
        // Get the value name.
        var name = value.ToString();

        // Get the field info of the value.
        FieldInfo? field = value.GetType().GetField(name);

        // If we couldn't find it, the value is invalid.
        if (field == null)
        {
            throw new ArgumentOutOfRangeException(nameof(value), "Invalid enum value.");
        }

        // Get the attributes attached to the value.
        var attributes =
            (DescriptionAttribute[])field.GetCustomAttributes(typeof(DescriptionAttribute), false);

        // Return the Description if set, otherwise the name.
        return attributes.Length > 0 ? attributes[0].Description : name;
    }

    /// <summary>
    /// Similar to Enum.TryParse(), this method finds an enum value given a name or description.
    /// If no values are found with a matching name, looks for a match on description.
    /// Must match exactly (case-sensitive) the value name or the Description attribute.
    /// </summary>
    /// <param name="nameOrDescription">The enum value's name or description.</param>
    /// <param name="value">The matching enum value, or default if not found.</param>
    /// <typeparam name="T">The enum type.</typeparam>
    /// <returns>If a matching enum value was found.</returns>
    /// <exception cref="ArgumentOutOfRangeException">
    /// If the type param is not an enum.
    /// </exception>
    public static bool TryParse<T>(string nameOrDescription, out T value) where T : struct
    {
        // Make sure this is being used with an enum type.
        Type enumType = typeof(T);
        if (!enumType.IsEnum)
        {
            throw new ArgumentOutOfRangeException(nameof(T),
                "The provided type must be an enum type.");
        }

        // Look for a matching name.
        if (Enum.TryParse(nameOrDescription, out T result))
        {
            value = result;
            return true;
        }

        // Look for matching description.
        foreach (FieldInfo field in enumType.GetFields())
        {
            // Check the description attribute.
            if (Attribute.GetCustomAttribute(field, typeof(DescriptionAttribute)) is
                DescriptionAttribute attribute)
            {
                if (attribute.Description == nameOrDescription)
                {
                    value = (T)field.GetValue(null)!;
                    return true;
                }
            }
        }

        value = default(T);
        return false;
    }
}
