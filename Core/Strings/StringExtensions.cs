using System.Text;
using System.Text.RegularExpressions;
using Galaxon.Core.Collections;

namespace Galaxon.Core.Strings;

/// <summary>
/// Extension methods for String.
/// </summary>
public static class StringExtensions
{
    #region Extension methods

    /// <summary>
    /// Determines whether two strings are equal, ignoring case sensitivity.
    /// </summary>
    /// <param name="str1">The first string to compare.</param>
    /// <param name="str2">The second string to compare.</param>
    /// <returns>True if the strings are equal, ignoring case; otherwise, false.</returns>
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
        // Optimization.
        if (charMap.IsEmpty())
        {
            return keepCharsNotInMap ? str : "";
        }

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
    /// Constructs a new string by repeating a specified string a specified number of times.
    /// </summary>
    /// <param name="s">The string to repeat.</param>
    /// <param name="n">The number of times to repeat the string.</param>
    /// <returns>A new string that consists of 'n' repetitions of the input string.</returns>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when 'n' is negative.</exception>
    public static string Repeat(this string s, int n)
    {
        // Guard.
        if (n < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(n), "Cannot be negative.");
        }

        // Using Enumerable.Repeat to create an IEnumerable<string> of length 'n'
        // Then, string.Concat joins all the strings in the IEnumerable into one string.
        return string.Concat(Enumerable.Repeat(s, n));
    }

    #endregion Extension methods

    #region Strip brackets

    /// <summary>
    /// Remove brackets (and whatever is between them) from a string.
    /// One use case is stripping HTML tags.
    /// TODO Unit tests, if I decide to keep this method.
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

    /// <summary>
    /// Strip HTML tags from a string.
    /// TODO Unit tests, if I decide to keep this method.
    /// </summary>
    /// <param name="str">The string to process.</param>
    /// <returns>The string with HTML tags removed.</returns>
    public static string StripTags(this string str)
    {
        return str.StripBrackets(false, false, false);
    }

    #endregion Strip brackets

    #region Inspect string

    /// <summary>
    /// Check if a string contains only ASCII characters.
    /// </summary>
    /// <param name="str">The string to check.</param>
    /// <returns>If the string is empty or fully ASCII.</returns>
    public static bool IsAscii(this string str)
    {
        return str.All(char.IsAscii);
    }

    /// <summary>
    /// Check if a string is a palindrome.
    /// </summary>
    public static bool IsPalindrome(this string str)
    {
        return str == string.Concat(str.Reverse());
    }

    #endregion Inspect string

    #region Small caps

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
        // lower-case 'x', which is the same as the original character.
        { 'x', "x" },
        { 'y', "ʏ" },
        { 'z', "ᴢ" }
    };

    /// <summary>
    /// Convert all lower-case letters in a string to their Unicode small caps variant.
    /// </summary>
    public static string ToSmallCaps(this string str)
    {
        return str.ReplaceChars(SmallCapsChars);
    }

    #endregion Small caps

    #region String case

    /// <summary>
    /// Return the string with the first letter converted to upper-case.
    /// The other letters aren't changed.
    /// </summary>
    /// <param name="str">The original string.</param>
    /// <returns>The string with the first letter upper-cased.</returns>
    public static string ToUpperFirstLetter(this string str)
    {
        if (string.IsNullOrWhiteSpace(str))
        {
            return str;
        }

        // Return upper-case first letter concatenated with remaining substring.
        return char.ToUpper(str[0]) + str[1..];
    }

    /// <summary>
    /// Return the string with the first letter of each word upper-case, and all the other letters
    /// in each word unchanged.
    /// Words are taken to be sequences of letters and apostrophes (two kinds are supported), and
    /// are thus separated by 1 or more non-word characters.
    /// </summary>
    /// <param name="str">The original string.</param>
    /// <returns>The string with the first letter of each word upper-cased.</returns>
    public static string ToProper(this string str)
    {
        if (string.IsNullOrWhiteSpace(str))
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
        // Test for empty string.
        if (string.IsNullOrWhiteSpace(str))
        {
            return EStringCase.None;
        }

        // Prepare.
        string upper = str.ToUpper();
        string lower = str.ToLower();

        // Test for no case.
        if (upper == lower)
        {
            return EStringCase.None;
        }

        // Test for lower case.
        if (str == lower)
        {
            return EStringCase.Lower;
        }

        // Test for upper case.
        if (str == upper)
        {
            return EStringCase.Upper;
        }

        // Test for proper case.
        if (str == str.ToProper())
        {
            return EStringCase.Proper;
        }

        // Test for upper case first letter.
        // This has to come after ToUpper() and ToProper() or they would never be detected.
        if (str == str.ToUpperFirstLetter())
        {
            return EStringCase.UpperFirstLetter;
        }

        // Something else. Could be title case, lower camel case, etc.
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
        // If the string's case is already None there's nothing to do.
        if (str.GetCase() == EStringCase.None)
        {
            return str;
        }

        // Can't set a string with letters (which means it has a case) to having no case.
        if (stringCase == EStringCase.None)
        {
            throw new InvalidOperationException(
                "You can't set the case of a string to None if it contains letters.");
        }

        // Change the case.
        return stringCase switch
        {
            EStringCase.Lower => str.ToLower(),
            EStringCase.Upper => str.ToUpper(),
            EStringCase.UpperFirstLetter => str.ToUpperFirstLetter(),
            EStringCase.Proper => str.ToProper(),
            // Default case, return the original string.
            _ => str
        };
    }

    #endregion String case

    #region Format numbers

    /// <summary>
    /// Pad a string on the left with zeroes to make it up to a certain width.
    /// </summary>
    /// <param name="str">The string to pad.</param>
    /// <param name="width">
    /// The minimum number of characters in the resulting string.
    /// Default is 2, which is useful for times and dates.
    /// </param>
    /// <returns>The zero-padded string.</returns>
    /// <exception cref="ArgumentNullException">
    /// Thrown when <paramref name="str"/> is null.
    /// </exception>
    /// <exception cref="ArgumentOutOfRangeException">
    /// Thrown when <paramref name="width"/> is not positive.
    /// </exception>
    public static string ZeroPad(this string str, int width = 2)
    {
        // Guards.
        if (str == null)
        {
            throw new ArgumentNullException(nameof(str), "Cannot be null.");
        }
        if (width <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(width), "Must be positive.");
        }

        return str.PadLeft(width, '0');
    }

    /// <summary>
    /// Formats a string of digits into groups using the specified group separator and size.
    /// </summary>
    /// <remarks>
    /// This method is designed primarily for formatting integers but can be used for other
    /// purposes, assuming the characters are digits. It allows formatting numbers in different
    /// bases like hexadecimal. However, it doesn't handle numbers with a fractional part.
    /// </remarks>
    /// <example>
    /// To format a decimal integer:
    /// <code>
    /// "12345678".GroupDigits() => "12,345,678"
    /// </code>
    /// You can chain methods:
    /// <code>
    /// "11111000000001010101".ZeroPad(24).GroupDigits('_', 8) => "00001111_10000000_01010101"
    /// 123456789.ToHex().ZeroPad(8).GroupDigits(' ') => "075b cd15"
    /// </code>
    /// </example>
    /// <param name="str">The string of digits to format.</param>
    /// <param name="separator">The group separator character. Default is ','.</param>
    /// <param name="size">The size of each group. Default is 3.</param>
    /// <returns>The formatted string.</returns>
    /// <exception cref="ArgumentNullException">
    /// Thrown when <paramref name="str"/> is null.
    /// </exception>
    public static string GroupDigits(this string str, char separator = ',', int size = 3)
    {
        // Guard clauses.
        if (str == null)
        {
            throw new ArgumentNullException(nameof(str), "Cannot be null.");
        }
        if (str.Length == 0)
        {
            return string.Empty;
        }

        // Iterate through the string to group digits.
        StringBuilder sb = new ();
        int groupStart = str.Length % size; // Start index of the first group.
        if (groupStart > 0)
        {
            sb.Append(str.Substring(0, groupStart));
        }
        for (int i = groupStart; i < str.Length; i += size)
        {
            if (sb.Length > 0)
            {
                sb.Append(separator);
            }
            sb.Append(str.Substring(i, Math.Min(size, str.Length - i)));
        }
        return sb.ToString();
    }

    #endregion Format numbers

    #region Convert strings to numbers

    /// <summary>
    /// Convert a nullable string to a nullable integer without throwing an exception.
    /// If the input string is null or cannot be parsed into an integer, returns null.
    /// </summary>
    /// <param name="str">The nullable string to convert to an integer.</param>
    /// <returns>
    /// A nullable integer representing the parsed value of the input string,
    /// or null if the input string is null or cannot be parsed.
    /// </returns>
    public static int? ToInt(this string? str)
    {
        return int.TryParse(str, out int result) ? result : null;
    }

    /// <summary>
    /// Convert a nullable string to a nullable double without throwing an exception.
    /// If the input string is null or cannot be parsed into an double, returns null.
    /// </summary>
    /// <param name="str">The nullable string to convert to an double.</param>
    /// <returns>
    /// A nullable double representing the parsed value of the input string,
    /// or null if the input string is null or cannot be parsed.
    /// </returns>
    public static double? ToDouble(this string? str)
    {
        return double.TryParse(str, out double d) ? d : null;
    }

    #endregion Convert strings to numbers
}
