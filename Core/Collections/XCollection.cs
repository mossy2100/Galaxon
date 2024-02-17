using System.Collections;

namespace Galaxon.Core.Collections;

/// <summary>
/// Extension methods for ICollection and ICollection{T}.
/// </summary>
public static class XCollection
{
    /// <summary>
    /// Check if an ICollection (array, list, etc.) is null or empty.
    /// </summary>
    public static bool IsEmpty(this ICollection? collection)
    {
        return collection is null || collection.Count == 0;
    }
}
