using System.Text;

namespace Galaxon.Core.Strings;

/// <summary>
/// Extension methods for StringBuilder.
/// </summary>
public static class XStringBuilder
{
    /// <summary>
    /// Prepends a StringBuilder with a string.
    /// Reflects Append().
    /// </summary>
    public static StringBuilder Prepend(this StringBuilder sb, object? value)
    {
        return value == null ? sb : sb.Insert(0, value.ToString());
    }
}
