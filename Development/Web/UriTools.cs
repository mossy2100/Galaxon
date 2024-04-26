using System.Text.RegularExpressions;
using AnyAscii;

namespace Galaxon.Web;

public static class UriTools
{
    /// <summary>
    /// Convert a Unicode string (for example, a page title) to a user-, browser-, and search
    /// engine-friendly URL slug containing only lower-case alphanumeric ASCII characters and
    /// hyphens.
    /// This method does not remove short words like "a", "the", "of", etc., like some algorithms
    /// do. I don't perceive this as necessary - please let me know if you disagree.
    /// </summary>
    /// <param name="str">The string to process.</param>
    /// <returns>The ASCII slug.</returns>
    public static string MakeSlug(string str)
    {
        // Convert to ASCII.
        string? result = str.Transliterate();

        // Remove apostrophes.
        result = result.Replace("'", "");

        // Replace non-alphanumeric characters with hyphens.
        result = Regex.Replace(result, "[^0-9a-z]+", "-", RegexOptions.IgnoreCase);

        // Trim hyphens from the start and end and lower-case the result.
        return result.Trim('-').ToLower();
    }
}
