using System.Reflection;

namespace Galaxon.Core.Collections;

/// <summary>
/// Extension methods for IEnumerable and IEnumerable{T}.
/// </summary>
public static class EnumerableExtensions
{
    /// <summary>
    /// Return values from one collection with values from a second collection removed.
    /// This method supports duplicates, so this is not the same as set difference.
    /// For example, if list1 has two instances of "cat" and list2 has one instance of "cat", the
    /// result will have one instance of "cat".
    /// </summary>
    public static List<T> Diff<T>(this IEnumerable<T> A, IEnumerable<T> B)
    {
        var result = new List<T>();
        var listB = B.ToList();

        foreach (T item in A)
        {
            if (listB.Contains(item))
            {
                listB.Remove(item);
            }
            else
            {
                result.Add(item);
            }
        }

        return result;
    }

    /// <summary>
    /// Converts an IEnumerable of objects with an Id property to a Dictionary{int, T},
    /// using the value of the Id property as the key.
    /// </summary>
    /// <typeparam name="T">The type of objects in the IEnumerable.</typeparam>
    /// <param name="items">The IEnumerable of objects.</param>
    /// <returns>
    /// A Dictionary{int, T} with the Id property values as keys and the objects as values.
    /// </returns>
    public static Dictionary<int, T> ToIndex<T>(this IEnumerable<T> items)
    {
        var dict = new Dictionary<int, T>();

        // Check for the integer Id property.
        Type t = typeof(T);
        PropertyInfo? idProperty = t.GetProperty("Id", typeof(int));
        if (idProperty == null || !idProperty.CanRead)
        {
            throw new InvalidOperationException(
                $"Type {t.Name} does not have a readable integer Id property.");
        }

        foreach (T item in items)
        {
            int id = (int)idProperty.GetValue(item)!;
            dict[id] = item;
        }

        return dict;
    }
}
