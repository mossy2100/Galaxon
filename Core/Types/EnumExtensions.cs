using System.Reflection;

namespace Galaxon.Core.Types;

/// <summary>
/// Extension methods for enum types.
/// </summary>
public static class EnumExtensions
{
    /// <summary>
    /// Similar to Enum.TryParse(), this method finds an enum value given a name or display name.
    /// If no values are found with a matching name, looks for a match on display name.
    /// Must match exactly (case-sensitive) one or the other.
    /// </summary>
    /// <param name="displayName">The enum value's name or display name.</param>
    /// <param name="value">The matching enum value, or default if not found.</param>
    /// <typeparam name="T">The enum type.</typeparam>
    /// <returns>If a matching enum value was found.</returns>
    /// <exception cref="ArgumentOutOfRangeException">
    /// If the type param is not an enum.
    /// </exception>
    public static bool TryParse<T>(string displayName, out T value) where T : struct, Enum
    {
        // Look for a matching name.
        if (Enum.TryParse(displayName, out T result))
        {
            value = result;
            return true;
        }

        // Look for a matching display name.
        foreach (FieldInfo field in typeof(T).GetFields()
            .Where(field => field.GetDisplayName() == displayName))
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
