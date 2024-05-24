using System.Collections;
using System.Text;

namespace Galaxon.Core.Types;

/// <summary>
/// Extension methods for ICollection and ICollection{T}.
/// </summary>
public static class ObjectExtensions
{
    /// <summary>
    /// Determines whether the specified object is considered empty.
    /// </summary>
    /// <param name="obj">The object to test for emptiness.</param>
    /// <returns>true if the object is considered empty; otherwise, false.</returns>
    /// <remarks>
    /// An object is considered empty if it is null, an empty string, an empty StringBuilder,
    /// an empty array, an empty collection, or an empty IEnumerable.
    /// </remarks>
    public static bool IsEmpty(this object? obj)
    {
        return obj == null
            || obj is string { Length: 0 }
            || obj is StringBuilder { Length: 0 }
            || obj is Array { Length: 0 }
            || obj is ICollection { Count: 0 }
            || (obj is IEnumerable enumerable && !enumerable.Cast<object>().Any());
    }
}
