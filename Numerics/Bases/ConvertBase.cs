using System.Text;
using System.Text.RegularExpressions;
using Galaxon.Core.Exceptions;
using Galaxon.Core.Strings;

namespace Bases;

/// <summary>
/// This class supports conversion between unsigned long integers and strings of digits in a
/// specified base, which can be in the range 2..64.
///
/// As in hexadecimal literals, upper-case letters have the same value as lower-case letters.
/// Lower-case is the default for strings returned from methods.
/// In the ToBase(), ToHex(), and ToDuo() methods, the "upper" parameter can be used to specify if
/// the result should use upper-case letters instead.
/// Lower-case letters are the default because upper-case letters are more easily confused with
/// numerals than lower-case. For example:
/// - 'O' can look like '0'
/// - 'I' can look like '1'
/// - 'Z' can look like '2'
/// - 'S' can look like '5'
/// - 'G' can look like '6'
/// - 'B' can look like '8'
/// The only similar problem with lower-case letters is that 'l' can look like '1'. However, these
/// days, most fonts, especially those used by IDEs, are chosen so that it's easy to distinguish
/// between letters and numbers, so it's not the issue it once was.
/// Multiple coding standards for CSS require hex digits in color literals to be lower-case.
/// Other than that, I can't find any standards that mandate one over the other. It seems upper-case
/// is favoured in older languages, lower-case in newer.
///
/// The core methods are ToBase() and FromBase(). In addition, convenience methods are provided for
/// bases that are a power of 2.
/// |------------------|------------------------|--------|--------------------------|
/// |  Bits per digit  |  Numeral system        |  Base  |  Methods                 |
/// |------------------|------------------------|--------|--------------------------|
/// |        1         |  binary                |    2   |  ToBin()    FromBin()    |
/// |        2         |  quaternary            |    4   |  ToQuat()   FromQuat()   |
/// |        3         |  octal                 |    8   |  ToOct()    FromOct()    |
/// |        4         |  hexadecimal           |   16   |  ToHex()    FromHex()    |
/// |        5         |  duotrigesimal         |   32   |  ToDuo()    FromDuo()    |
/// |        6         |  tetrasexagesimal      |   64   |  ToTetra()  FromTetra()  |
/// |------------------|------------------------|--------|--------------------------|
///
/// The system used for base-32 encoding is known as base32hex or triacontakaidecimal. It is a
/// simple continuation of hexadecimal that uses the English letters from g-v (or G-V) to
/// represent the values 16-31. It is the same method as used by JavaScript.
///
/// The system used for base-64 encoding is new. It could perhaps be called "base64hex". It's also
/// a continuation of hexadecimal (and  base32hex) that uses the letters w-z (or W-Z) to represent
/// the values 32-35, and 28 ASCII symbol characters to represent the values 36-63. These characters
/// are ordered by ASCII value. As there are 32 symbol characters in the ASCII set, 4 are excluded.
///
/// The excluded characters are as follows:
///   period (.)        Indicates a decimal separator (and in some languages, digit grouping).
///   comma (,)         Commonly used for digit grouping (and in some languages, the decimal separator).
///   underscore (_)    Used for digit grouping in several programming languages (C#, Java, etc.)
///   dash (-)          Indicates a negative sign.
///
/// At this stage the class doesn't support negative values or values with a fractional part, but
/// it might do in the future.
///
/// Apostrophes (') are also valid for digit grouping in certain languages, and in C++, but they
/// aren't very common, and there aren't enough ASCII symbol characters to exclude more than 4.
///
/// Any whitespace characters in a string being parsed are ignored.
///
/// Digit characters that are invalid for the specified base of the value will trigger an exception,
/// as will any control characters.
/// </summary>
/// <see href="https://en.wikipedia.org/wiki/List_of_numeral_systems"/>
/// <see href="https://en.wikipedia.org/wiki/Binary_number"/>
/// <see href="https://en.wikipedia.org/wiki/Quaternary_numeral_system"/>
/// <see href="https://en.wikipedia.org/wiki/Octal"/>
/// <see href="https://en.wikipedia.org/wiki/Hexadecimal"/>
/// <see href="https://en.wikipedia.org/wiki/Base32"/>
/// <see href="https://en.wikipedia.org/wiki/Sexagesimal"/>
/// <see href="https://en.wikipedia.org/wiki/Base64"/>
public static class ConvertBase
{
    #region Constants

    /// <summary>
    /// The minimum supported base.
    /// </summary>
    public const int MIN_BASE = 2;

    /// <summary>
    /// The maximum supported base.
    /// </summary>
    public const int MAX_BASE = 64;

    /// <summary>
    /// Digit characters. The index of a character in the string equals its value.
    /// </summary>
    public const string DIGITS =
        "0123456789abcdefghijklmnopqrstuvwxyz!\"#$%&'()*+/:;<=>?@[\\]^`{|}~";

    #endregion Constants

    #region Extension methods

    /// <summary>
    /// Convert an integer to a string of digits in a given base.
    /// </summary>
    /// <param name="n">The instance value.</param>
    /// <param name="toBase">The base to convert to.</param>
    /// <param name="width">
    /// The minimum number of digits in the result. Any value less than 2 will have no effect.
    /// The result will be padded with '0' characters on the left as needed.
    /// This width does not include the '-' character if needed for a negative value. It only refers
    /// to the minimum number of digits.
    /// </param>
    /// <param name="upper">
    /// If letters should be lower or upper-case.
    ///   false = lower-case (default)
    ///   true  = upper-case
    /// </param>
    /// <returns>The string of digits.</returns>
    /// <exception cref="ArgumentOutOfRangeException">If the input value is negative.</exception>
    /// <exception cref="ArgumentOutOfRangeException">If the base is out of range.</exception>
    /// <exception cref="ArgumentOutOfRangeException">If the width is less than 1.</exception>
    public static string ToBase(this ulong n, byte toBase, int width = 1, bool upper = false)
    {
        // Guard. Check the base is valid.
        if (toBase is < MIN_BASE or > MAX_BASE)
        {
            throw new ArgumentOutOfRangeException(nameof(toBase),
                $"The base must be in the range {MIN_BASE}..{MAX_BASE}.");
        }

        // Guard. Make sure width is valid.
        if (width < 1)
        {
            throw new ArgumentOutOfRangeException(nameof(width), "Must be at least 1.");
        }

        // Get the digit values.
        List<byte> digitValues = CalcDigitValues(n, toBase);

        // Build the output string.
        StringBuilder sbDigits = new ();
        foreach (byte digitValue in digitValues)
        {
            // Get the character with this value.
            char c = DIGITS[digitValue];

            // Transform the case if necessary.
            if (upper && char.IsLetter(c))
            {
                c = char.ToUpper(c);
            }

            // Add the digit to the start of the string.
            sbDigits.Prepend(c);
        }
        string result = sbDigits.ToString();

        // Pad to the desired width.
        return result.ZeroPad(width);
    }

    /// <summary>Convert an integer to binary digits.</summary>
    /// <param name="n">The integer to convert.</param>
    /// <param name="width">The minimum number of digits in the result.</param>
    /// <returns>The value as a string of binary digits.</returns>
    public static string ToBin(this ulong n, int width = 1)
    {
        return ToBase(n, 2, width);
    }

    /// <summary>Convert integer to quaternary digits.</summary>
    /// <param name="n">The integer to convert.</param>
    /// <param name="width">The minimum number of digits in the result.</param>
    /// <returns>The value as a string of quaternary digits.</returns>
    public static string ToQuat(this ulong n, int width = 1)
    {
        return ToBase(n, 4, width);
    }

    /// <summary>Convert integer to octal digits.</summary>
    /// <param name="n">The integer to convert.</param>
    /// <param name="width">The minimum number of digits in the result.</param>
    /// <returns>The value as a string of octal digits.</returns>
    public static string ToOct(this ulong n, int width = 1)
    {
        return ToBase(n, 8, width);
    }

    /// <summary>Convert integer to hexadecimal digits.</summary>
    /// <param name="n">The integer to convert.</param>
    /// <param name="width">The minimum number of digits in the result.</param>
    /// <param name="upper">If letters should be upper-case (false = lower, true = upper).</param>
    /// <returns>The value as a string of hexadecimal digits.</returns>
    public static string ToHex(this ulong n, int width = 1, bool upper = false)

    {
        return ToBase(n, 16, width, upper);
    }

    /// <summary>Convert integer to duotrigesimal digits.</summary>
    /// <param name="n">The integer to convert.</param>
    /// <param name="width">The minimum number of digits in the result.</param>
    /// <param name="upper">If letters should be upper-case (false = lower, true = upper).</param>
    /// <returns>The value as a string of duotrigesimal digits.</returns>
    public static string ToDuo(this ulong n, int width = 1, bool upper = false)

    {
        return ToBase(n, 32, width, upper);
    }

    /// <summary>Convert integer to tetrasexagesimal digits.</summary>
    /// <param name="n">The integer to convert.</param>
    /// <param name="width">The minimum number of digits in the result.</param>
    /// <param name="upper">If letters should be upper-case (false = lower, true = upper).</param>
    /// <returns>The value as a string of tetrasexagesimal digits.</returns>
    public static string ToTetra(this ulong n, int width = 1, bool upper = false)

    {
        return ToBase(n, 64, width, upper);
    }

    #endregion Extension methods

    #region Static conversion methods

    /// <summary>
    /// Convert a string of digits in a given base into a ulong.
    /// Any whitespace characters in the input will be ignored.
    /// Commas and underscores are taken to indicate digit grouping and are also ignored.
    /// </summary>
    /// <param name="digits">A string of digits in the specified base.</param>
    /// <param name="fromBase">The base that the digits in the string are in.</param>
    /// <returns>The integer equivalent of the digits.</returns>
    /// <exception cref="ArgumentNullException">If string is null, empty, or whitespace.</exception>
    /// <exception cref="ArgumentInvalidException">
    /// If string contains invalid characters for the specified base.
    /// </exception>
    /// <exception cref="OverflowException">If the result is out of range for ulong.</exception>
    public static ulong FromBase(string digits, byte fromBase)
    {
        // Check the input string != null, empty, or whitespace.
        if (string.IsNullOrWhiteSpace(digits))
        {
            throw new ArgumentNullException(nameof(digits), "Cannot be empty or whitespace.");
        }

        // Check for a negative value.
        if (digits[0] == '-')
        {
            throw new ArgumentOutOfRangeException(nameof(digits),
                "Parsing of negative values is unsupported.");
        }

        // Check the base is valid. This could throw an ArgumentOutOfRangeException.
        if (fromBase is < MIN_BASE or > MAX_BASE)
        {
            throw new ArgumentOutOfRangeException(nameof(fromBase),
                $"The base must be in the range {MIN_BASE}..{MAX_BASE}.");
        }

        // Ignore whitespace and digit grouping characters.
        digits = Regex.Replace(digits, @$"[\s,_]", "");

        // Get a map of valid digits to their value, for this base.
        Dictionary<char, byte> digitValues = GetDigitValuesForBase(fromBase);

        // Do the conversion.
        ulong value = 0;
        foreach (char c in digits)
        {
            // Try to get the character value from the map.
            if (!digitValues.TryGetValue(c, out byte digitValue))
            {
                if (c == '.')
                {
                    throw new ArgumentInvalidException(nameof(digits),
                        "The period character, indicating a decimal point, is invalid, as parsing of values with a fractional part is unsupported.");
                }

                char[] digitChars = digitValues.Select(kvp => kvp.Key).ToArray();
                string digitList = string.Join(", ", digitChars[..^1]) + " and " + digitChars[^1];
                throw new ArgumentInvalidException(nameof(digits),
                    $"A string representing a number in base {fromBase} may only include the digits {digitList}. Whitespace characters, commas, and underscores are allowed but ignored.");
            }

            // Add it to the result. Check for overflow.
            try
            {
                value = value * fromBase + digitValue;
            }
            catch (OverflowException ex)
            {
                throw new OverflowException(
                    "The integer represented by the string is outside the valid range for ulong.",
                    ex);
            }
        }

        return value;
    }

    /// <summary>
    /// Convert a string of binary digits into an integer.
    /// </summary>
    /// <param name="digits">The string of digits to parse.</param>
    /// <returns>The integer equivalent of the digits.</returns>
    public static ulong FromBin(string digits)
    {
        return FromBase(digits, 2);
    }

    /// <summary>
    /// Convert a string of quaternary digits into an integer.
    /// </summary>
    /// <param name="digits">The string of digits to parse.</param>
    /// <returns>The integer equivalent of the digits.</returns>
    public static ulong FromQuat(string digits)
    {
        return FromBase(digits, 4);
    }

    /// <summary>
    /// Convert a string of octal digits into an integer.
    /// </summary>
    /// <param name="digits">The string of digits to parse.</param>
    /// <returns>The integer equivalent of the digits.</returns>
    public static ulong FromOct(string digits)
    {
        return FromBase(digits, 8);
    }

    /// <summary>
    /// Convert a string of hexadecimal digits into an integer.
    /// </summary>
    /// <param name="digits">The string of digits to parse.</param>
    /// <returns>The integer equivalent of the digits.</returns>
    public static ulong FromHex(string digits)
    {
        return FromBase(digits, 16);
    }

    /// <summary>
    /// Convert a string of duotrigesimal digits into an integer.
    /// </summary>
    /// <param name="digits">The string of digits to parse.</param>
    /// <returns>The integer equivalent of the digits.</returns>
    public static ulong FromDuo(string digits)
    {
        return FromBase(digits, 32);
    }

    /// <summary>
    /// Convert a string of tetrasexagesimal digits into an integer.
    /// </summary>
    /// <param name="digits">The string of digits to parse.</param>
    /// <returns>The integer equivalent of the digits.</returns>
    public static ulong FromTetra(string digits)
    {
        return FromBase(digits, 64);
    }

    #endregion Static conversion methods

    #region Private helper methods

    /// <summary>
    /// Get the values of valid digits for the specified base.
    /// </summary>
    /// <param name="radix">The base.</param>
    /// <returns>The map of digit characters to their values.</returns>
    private static Dictionary<char, byte> GetDigitValuesForBase(byte radix)
    {
        Dictionary<char, byte> digitValues = new ();

        for (byte i = 0; i < radix; i++)
        {
            char c = DIGITS[i];

            // The value equals the index within the digit string.
            digitValues[c] = i;

            // For letter characters, add the upper-case variant, too.
            if (char.IsLetter(c))
            {
                digitValues[char.ToUpper(c)] = i;
            }
        }

        return digitValues;
    }

    /// <summary>
    /// Calculate the digit values for a non-negative integer when converted to a given base.
    /// </summary>
    /// <param name="n">The integer.</param>
    /// <param name="toBase">The base.</param>
    /// <returns>
    /// The digit values as a list of bytes. An item's index in the list will equal the exponent
    /// (i.e. the first item will have an index of 0, corresponding to the exponent of 0 or units
    /// position). Thus the digit values will be in the reverse order to how they would appear if
    /// written using positional notation.</returns>
    /// <exception cref="ArgumentOutOfRangeException">If the argument is non-negative.</exception>
    internal static List<byte> CalcDigitValues(ulong n, byte toBase)
    {
        // Initialize the result.
        var result = new List<byte>();

        // Check for zero.
        if (n == 0)
        {
            result.Add(0);
            return result;
        }

        // Get the digit values.
        while (true)
        {
            // Get the next digit.
            ulong rem = n % toBase;
            result.Add((byte)rem);

            // Check if we're done.
            n -= rem;
            if (n == 0)
            {
                break;
            }

            // Prepare for next iteration.
            n /= toBase;
        }

        return result;
    }

    #endregion Private helper methods
}
