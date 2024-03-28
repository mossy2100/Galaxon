using System.Text.RegularExpressions;

namespace Galaxon.Astronomy.DataImport.Services;

public static class ParseUtility
{
    public static double ParseDouble(string str)
    {
        // Ignore some chars.
        // Approximately equal.
        str = str.Replace("\u2248", "");
        // Non-breaking space.
        str = str.Replace("&#160;", "");
        // Commas.
        str = str.Replace(",", "");
        // Less than sign.
        str = str.Replace("&lt;", "");
        // Greater than sign.
        str = str.Replace("&gt;", "");

        // Remove the error part, if present.
        int pos = str.IndexOf('±');
        if (pos != -1)
        {
            str = str[..pos];
        }
        pos = str.IndexOf('+');
        if (pos != -1)
        {
            str = str[..pos];
        }

        // Trim.
        str = str.Trim();

        // Parse what remains.
        return double.Parse(str);
    }

    public static uint? ExtractNumber(string input)
    {
        // Define a regular expression pattern to match the number in parentheses
        string pattern = @"\((\d+)\)";

        // Use Regex.Match to find the number in parentheses
        Match match = Regex.Match(input, pattern);

        // Check if a match was found
        if (match.Success)
        {
            // Extract the matched number and parse it as an integer
            string numberStr = match.Groups[1].Value;
            if (uint.TryParse(numberStr, out uint number))
            {
                return number;
            }
        }

        // If no match was found or parsing failed, return a default value (e.g., -1)
        return null;
    }
}
