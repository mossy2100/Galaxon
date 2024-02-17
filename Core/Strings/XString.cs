using System.Text;
using System.Text.RegularExpressions;
using Galaxon.Core.Numbers;
using Galaxon.Core.Numbers.Extensions;

namespace Galaxon.Core.Strings;

/// <summary>
/// Extension methods for String.
/// </summary>
public static class XString
{
    /// <summary>
    /// Map from lower-case letters to their Unicode small caps equivalents.
    /// </summary>
    public static readonly Dictionary<char, string> SmallCapsChars = new ()
    {
        { 'a', "ᴀ" },
        { 'b', "ʙ" },
        { 'c', "ᴄ" },
        { 'd', "ᴅ" },
        { 'e', "ᴇ" },
        { 'f', "ꜰ" },
        { 'g', "ɢ" },
        { 'h', "ʜ" },
        { 'i', "ɪ" },
        { 'j', "ᴊ" },
        { 'k', "ᴋ" },
        { 'l', "ʟ" },
        { 'm', "ᴍ" },
        { 'n', "ɴ" },
        { 'o', "ᴏ" },
        { 'p', "ᴘ" },
        { 'q', "ꞯ" },
        { 'r', "ʀ" },
        { 's', "ꜱ" },
        { 't', "ᴛ" },
        { 'u', "ᴜ" },
        { 'v', "ᴠ" },
        { 'w', "ᴡ" },
        // Note: there is no Unicode small caps variant for 'x'. The character used here is the
        // lower-case 'x' (i.e. same as the original character).
        { 'x', "x" },
        { 'y', "ʏ" },
        { 'z', "ᴢ" }
    };

    /// <summary>
    /// See if 2 strings are equal, ignoring case.
    /// </summary>
    public static bool EqualsIgnoreCase(this string str1, string? str2)
    {
        return str1.Equals(str2, StringComparison.OrdinalIgnoreCase);
    }

    /// <summary>
    /// Replace characters in a string with other characters by using a character map.
    /// Example use cases:
    /// * making a string upper- or lower-case
    /// * converting lowercase characters to small caps
    /// * making a string superscript or subscript
    /// * transliteration/removal of diacritics
    /// </summary>
    /// <param name="str">The original string.</param>
    /// <param name="charMap">The character map.</param>
    /// <param name="keepCharsNotInMap">
    /// If a character is encountered that is not in the character
    /// map, either keep it (true) or skip it (false).
    /// </param>
    /// <returns>The transformed string.</returns>
    public static string ReplaceChars(this string str, Dictionary<char, string> charMap,
        bool keepCharsNotInMap = true)
    {
        StringBuilder sb = new ();

        foreach (char original in str)
        {
            // Get the replacement character if it's there.
            if (charMap.TryGetValue(original, out string? replacement))
            {
                // Append the replacement character.
                sb.Append(replacement);
            }
            else if (keepCharsNotInMap)
            {
                // Append the original character.
                sb.Append(original);
            }
        }

        return sb.ToString();
    }

    /// <summary>
    /// Construct a new string by repeating a string multiple times.
    /// </summary>
    public static string Repeat(string s, int n)
    {
        // Guard.
        if (n < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(n), "Cannot be negative.");
        }

        StringBuilder sb = new ();
        for (var i = 0; i < n; i++)
        {
            sb.Append(s);
        }
        return sb.ToString();
    }

    /// <summary>
    /// Reverse a string.
    /// e.g. "You are awesome." becomes ".emosewa era uoY".
    /// </summary>
    public static string Reverse(this string str)
    {
        char[] chars = str.ToArray();
        Array.Reverse(chars);
        return new string(chars);
    }

    /// <summary>
    /// Check if a string is a palindrome.
    /// </summary>
    public static bool IsPalindrome(this string str)
    {
        return str == str.Reverse();
    }

    /// <summary>
    /// Remove whitespace from a string.
    /// </summary>
    /// <see href="https://www.compart.com/en/unicode/category/Zs"/>
    /// <param name="str">The string to process.</param>
    /// <returns>The string with whitespace characters removed.</returns>
    public static string StripWhitespace(this string str)
    {
        return Regex.Replace(str,
            @"[\s\u00A0\u1680\u2000\u2001\u2002\u2003\u2004\u2005\u2006\u2007\u2008"
            + @"\u2009\u200A\u202F\u205F\u3000]", "");
    }

    /// <summary>
    /// Remove brackets (and whatever's between them) from a string.
    /// </summary>
    /// <param name="str">The string to process.</param>
    /// <param name="round">If round brackets should be removed.</param>
    /// <param name="square">If square brackets should be removed</param>
    /// <param name="curly">If curly brackets should be removed</param>
    /// <param name="angle">If angle brackets should be removed</param>
    /// <returns>The string with brackets removed.</returns>
    public static string StripBrackets(this string str, bool round = true, bool square = true,
        bool curly = true, bool angle = true)
    {
        if (round)
        {
            str = Regex.Replace(str, @"\([^\)]*\)", "");
        }

        if (square)
        {
            str = Regex.Replace(str, @"\[[^\]]*\]", "");
        }

        if (curly)
        {
            str = Regex.Replace(str, @"{[^}]*}", "");
        }

        if (angle)
        {
            str = Regex.Replace(str, @"<[^>]*>", "");
        }

        return str;
    }

    /// <summary>Strip HTML tags from a string.</summary>
    /// <param name="str">The string to process.</param>
    /// <returns>The string with HTML tags removed.</returns>
    public static string StripTags(this string str)
    {
        return str.StripBrackets(false, false, false);
    }

    /// <summary>
    /// Check if a string contains only ASCII characters.
    /// </summary>
    /// <param name="str">The string to check.</param>
    /// <returns></returns>
    public static bool IsAscii(this string str)
    {
        return str.All(char.IsAscii);
    }

    /// <summary>
    /// Convert all lower-case letters in a string to their Unicode small caps variant.
    /// Only works for English letters, so, if necessary (e.g. if the string is in a different
    /// language), you may wish to call AnyAscii.Transliterate() on the string first.
    /// </summary>
    /// <exception cref="NotImplementedException"></exception>
    public static string ToSmallCaps(this string str)
    {
        return str.ReplaceChars(SmallCapsChars);
    }

    /// <summary>
    /// Return the string with the first letter of each word upper-case, and all the other letters
    /// in each word unchanged.
    /// Words are taken to be sequences of letters and apostrophes (two kinds are supported), and
    /// are thus separated by 1 or more non-word characters.
    /// </summary>
    /// <param name="str">The original string.</param>
    /// <returns>The string with the first letter of each word upper-cased.</returns>
    /// <remarks>
    /// A method ToTitle() could be developed based on this one, but it would require extracting the
    /// words first, then seeing which ones should be all lower-case (a, an, for, to, etc.) and
    /// which should have the first letter upper-cased.
    /// </remarks>
    public static string ToProper(this string str)
    {
        // Guard.
        if (string.IsNullOrEmpty(str))
        {
            return str;
        }

        // Iterate through the source string, making letters upper and lower case as needed.
        StringBuilder result = new ();
        const string APOSTROPHES = "'’";
        bool firstLetterOfWordFound = false;

        foreach (char c in str)
        {
            bool isLetter = char.IsLetter(c);
            bool isApostrophe = APOSTROPHES.Contains(c);
            bool isWordChar = isLetter || isApostrophe;
            bool toUpper = false;

            if (isLetter && !firstLetterOfWordFound)
            {
                toUpper = true;
                firstLetterOfWordFound = true;
            }
            else if (!isWordChar)
            {
                firstLetterOfWordFound = false;
            }

            // Add the character, upper-cased only if it's the first one in the word.
            result.Append(toUpper ? char.ToUpper(c) : c);
        }

        // Return result.
        return result.ToString();
    }

    /// <summary>
    /// Get the string's case.
    /// If it could not be detected, defaults to Mixed.
    /// </summary>
    /// <param name="str">Source string.</param>
    /// <returns>The string's case.</returns>
    public static EStringCase GetCase(this string str)
    {
        if (str == str.ToLower())
        {
            return EStringCase.Lower;
        }

        if (str == str.ToUpper())
        {
            return EStringCase.Upper;
        }

        if (str == str.ToProper())
        {
            return EStringCase.Proper;
        }

        // This has to come after ToUpper() and ToProper() otherwise they wouldn't ever be detected.
        if (str == str.ToUpperFirstLetter())
        {
            return EStringCase.UpperFirstLetter;
        }

        return EStringCase.Mixed;
    }

    /// <summary>
    /// Generates a new string from a source string and a desired string case.
    /// </summary>
    /// <param name="str">Source string.</param>
    /// <param name="stringCase">The string case to convert to.</param>
    /// <returns>The new string.</returns>
    public static string SetCase(this string str, EStringCase stringCase)
    {
        if (stringCase == EStringCase.Lower)
        {
            return str.ToLower();
        }

        if (stringCase == EStringCase.Upper)
        {
            return str.ToUpper();
        }

        if (stringCase == EStringCase.UpperFirstLetter)
        {
            return str.ToUpperFirstLetter();
        }

        if (stringCase == EStringCase.Proper)
        {
            return str.ToProper();
        }

        return str;
    }

    /// <summary>
    /// Return the string with the first letter converted to upper-case.
    /// The other letters aren't changed.
    /// </summary>
    /// <param name="str">The original string.</param>
    /// <returns>The string with the first letter upper-cased.</returns>
    public static string ToUpperFirstLetter(this string str)
    {
        // Check for empty string.
        if (str == "")
        {
            return "";
        }

        // Return char and concat substring.
        return char.ToUpper(str[0]) + str[1..];
    }

    #region Methods for formatting numbers

    /// <summary>
    /// Render a string with valid integer characters (i.e. minus sign or digits) converted to their
    /// Unicode superscript versions.
    /// </summary>
    /// <param name="str">The string.</param>
    /// <returns>The string of superscript characters.</returns>
    public static string ToSuperscript(this string str)
    {
        return str.ReplaceChars(XBinaryInteger.SuperscriptChars);
    }

    /// <summary>
    /// Render a string with valid integer characters (i.e. minus sign or digits) converted to their
    /// Unicode subscript versions.
    /// </summary>
    /// <param name="str">The string.</param>
    /// <returns>The string of subscript characters.</returns>
    public static string ToSubscript(this string str)
    {
        return str.ReplaceChars(XBinaryInteger.SubscriptChars);
    }

    /// <summary>
    /// Pad a string on the left with 0s to make it up to a certain width.
    /// </summary>
    /// <param name="str">The string.</param>
    /// <param name="width">The minimum number of characters in the the result.</param>
    /// <returns>The zero-padded string.</returns>
    public static string ZeroPad(this string str, int width)
    {
        return str.PadLeft(width, '0');
    }

    /// <summary>
    /// Given a string of digits, format in groups using the specified group separator and group
    /// size.
    /// This method is designed for formatting numbers but it could be used for other purposes,
    /// since the method doesn't check if the characters are actually digits. It just assumes they
    /// are. Apart from saving time, it allows the method to be used for hexadecimal or other bases.
    /// Grouping starts from the right. Here's how you would format an integer:
    /// "12345678".GroupDigits(',', 3) => "12,345,678"
    /// You can chain methods if you need to, e.g.
    /// "11111000000001010101".GroupDigits('_', 8) => "1111_10000000_01010101"
    /// "11111000000001010101".ZeroPad(24).GroupDigits('_', 8) => "00001111_10000000_01010101"
    /// 123456789.ToHex().ZeroPad(8).GroupDigits(' ') => "075b cd15"
    /// </summary>
    /// <param name="str">The string, nominally of digits, but can be whatever.</param>
    /// <param name="separator">The group separator character.</param>
    /// <param name="size">The group size.</param>
    /// <returns>The formatted string.</returns>
    public static string GroupDigits(this string str, char separator = '_', byte size = 4)
    {
        StringBuilder sb = new ();
        while (true)
        {
            if (str == "")
            {
                break;
            }
            string group = str.Length > size ? str[^size..] : str;
            if (sb.Length != 0)
            {
                sb.Prepend(separator);
            }
            sb.Prepend(group);
            if (str.Length <= size)
            {
                break;
            }
            str = str[..^size];
        }
        return sb.ToString();
    }

    #endregion Methods for formatting numbers

    #region Methods for converting strings to numbers

    /// <summary>
    /// Convert nullable string to nullable int without throwing.
    /// If the string cannot be parsed into an int, return null.
    /// Of course, TryParse() can be used, but this method is a bit more concise, and saves time
    /// thinking about nulls.
    /// </summary>
    public static int? ToInt(this string? str)
    {
        return int.TryParse(str, out int i) ? i : null;
    }

    /// <summary>
    /// Convert nullable string to nullable double without throwing.
    /// If the string cannot be parsed into a double, return null.
    /// </summary>
    public static double? ToDouble(this string? str)
    {
        return double.TryParse(str, out double d) ? d : null;
    }

    /// <summary>
    /// Convert nullable string to nullable decimal without throwing.
    /// If the string cannot be parsed into a decimal, return null.
    /// </summary>
    public static decimal? ToDecimal(this string? str)
    {
        return decimal.TryParse(str, out decimal m) ? m : null;
    }

    #endregion Methods for converting strings to numbers
}
