using System.Text;

namespace Galaxon.Core.Strings;

/// <summary>
/// Extension methods for StringBuilder.
/// </summary>
public static class StringBuilderExtensions
{
    /// <summary>
    /// Inserts the string representation of the specified object at the beginning of the current
    /// StringBuilder.
    /// </summary>
    /// <param name="sb">The StringBuilder instance.</param>
    /// <param name="value">The object to prepend. If null, no action is taken.</param>
    /// <returns>The current StringBuilder instance.</returns>
    public static StringBuilder Prepend(this StringBuilder sb, object? value)
    {
        return value == null ? sb : sb.Insert(0, value.ToString());
    }
}
