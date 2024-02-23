using System.Reflection;

namespace Galaxon.Core.Types;

/// <summary>
/// Extension methods for enum types.
/// </summary>
public static class EnumExtensions
{
    /// <summary>
    /// Get the value of the Description attribute for the enum value.
    /// If there is no Description attribute, returns an empty string.
    /// </summary>
    /// <param name="value">An enum value.</param>
    /// <returns>The value's description.</returns>
    /// <exception cref="ArgumentOutOfRangeException">
    /// If the value is not valid for the Enum type.
    /// </exception>
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

        // Get the field description.
        return field.GetDescription();
    }

    /// <summary>
    /// Get the value of the Description attribute for the enum value, or, if not provided,
    /// the name of the value (which is the same as ToString()).
    /// </summary>
    /// <param name="value">An enum value.</param>
    /// <returns>The value's description or name.</returns>
    /// <exception cref="ArgumentOutOfRangeException">
    /// If the value is not valid for the Enum type.
    /// </exception>
    public static string GetDescriptionOrName(this Enum value)
    {
        string description = value.GetDescription();
        return (description != "") ? description : value.ToString();
    }

    /// <summary>
    /// Similar to Enum.TryParse(), this method finds an enum value given a name or description.
    /// If no values are found with a matching name, looks for a match on description.
    /// Must match exactly (case-sensitive) the value's name or description.
    /// </summary>
    /// <param name="nameOrDescription">The enum value's name or description.</param>
    /// <param name="value">The matching enum value, or default if not found.</param>
    /// <typeparam name="T">The enum type.</typeparam>
    /// <returns>If a matching enum value was found.</returns>
    /// <exception cref="ArgumentOutOfRangeException">
    /// If the type param is not an enum.
    /// </exception>
    public static bool TryParse<T>(string nameOrDescription, out T value) where T : struct, Enum
    {
        // Look for a matching name.
        if (Enum.TryParse(nameOrDescription, out T result))
        {
            value = result;
            return true;
        }

        // Look for a matching description.
        foreach (FieldInfo field in typeof(T).GetFields()
            .Where(field => field.GetDescription() == nameOrDescription))
        {
            value = (T)field.GetValue()!;
            return true;
        }

        // No match, but we have to set the out parameter to something, so let's use the default.
        // We could make the out parameter nullable, and set the value to null, but I think that
        // would make the method less usable (especially since we won't even look at the out
        // parameter if the method returns false).
        value = default(T);
        return false;
    }
}
