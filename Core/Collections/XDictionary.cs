using Galaxon.Core.Exceptions;

namespace Galaxon.Core.Collections;

/// <summary>
/// Extension methods for Dictionary.
/// </summary>
public static class XDictionary
{
    /// <summary>
    /// Checks if a dictionary contains unique values.
    /// </summary>
    /// <typeparam name="TKey">The type of the keys in the dictionary.</typeparam>
    /// <typeparam name="TValue">The type of the values in the dictionary.</typeparam>
    /// <param name="dict">The dictionary to check.</param>
    /// <returns>True if the dictionary contains unique values; otherwise, false.</returns>
    /// <remarks>
    /// This method utilizes LINQ's All method to efficiently check if all values in the dictionary
    /// are unique. It uses a HashSet to track unique values.
    /// </remarks>
    public static bool HasUniqueValues<TKey, TValue>(this Dictionary<TKey, TValue> dict)
        where TKey : notnull
    {
        HashSet<TValue> uniqueValues = new HashSet<TValue>();
        return dict.Values.All(value => uniqueValues.Add(value));
    }

    /// <summary>
    /// Create a new dictionary from the instance with keys and values flipped.
    /// </summary>
    /// <typeparam name="TKey">The type of the keys in the dictionary.</typeparam>
    /// <typeparam name="TValue">The type of the values in the dictionary.</typeparam>
    /// <param name="dict">The dictionary to flip.</param>
    /// <returns>The flipped dictionary.</returns>
    /// <exception cref="ArgumentInvalidException">
    /// If the instance contains duplicate values.
    /// </exception>
    public static Dictionary<TValue, TKey> Flip<TKey, TValue>(this Dictionary<TKey, TValue> dict)
        where TKey : notnull
        where TValue : notnull
    {
        if (!dict.HasUniqueValues())
        {
            throw new ArgumentInvalidException(nameof(dict),
                "Cannot flip a dictionary with duplicate values.");
        }

        return dict.ToDictionary(kvp => kvp.Value, kvp => kvp.Key);
    }
}
