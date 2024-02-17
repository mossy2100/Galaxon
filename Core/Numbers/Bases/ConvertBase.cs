using System.Numerics;
using System.Text;
using System.Text.RegularExpressions;
using Galaxon.Core.Exceptions;
using Galaxon.Core.Strings;
using Galaxon.Core.Types;

namespace Galaxon.Core.Numbers.Bases;

/// <summary>
/// This class supports conversion between integers and strings of digits in a specified base,
/// which can be in the range 2..64.
///
/// All built-in integer types are supported, including Int128, UInt128, and BigInteger.
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
/// represent the values 16-31. It is the same system used by JavaScript.
///
/// The system used for base-64 encoding is new. It could perhaps be called "base64hex". It's also
/// a continuation of hexadecimal (and the above base32hex) that uses the letters w-z (or W-Z) to
/// represent the values 32-35, and printable non-alphanumeric ASCII characters to represent the
/// values 36-63. These characters are ordered by ASCII value. As there are 33 such characters, 5
/// are excluded.
/// The characters selected for exclusion include the 4 characters used for digit grouping:
///   space ( )
///   period (.)
///   comma (,)
///   apostrophe (')
/// The dash (-) is also excluded, as this is taken to indicate a negative value. A dash is only
/// valid at the start of a string.
///
/// Certain programming languages support underscores (_) for digit grouping. However, because there
/// are only 95 printable ASCII characters, the underscore is not excluded from the base64hex
/// character set and has a value of 58.
///
/// At this stage there is no support for decimal or floating point values, although it's
/// possible this could be added in the future.
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
        "0123456789abcdefghijklmnopqrstuvwxyz!\"#$%&()*+/:;<=>?@[\\]^_`{|}~";

    /// <summary>
    /// Digit grouping characters.
    /// </summary>
    public const string DIGIT_GROUPING_CHARACTERS = " ,.'";

    #endregion Constants

    #region Extension methods

    /// <summary>
    /// Convert an integer to a string of digits in a given base.
    /// Note, a negative value will be converted to a non-negative value with the same underlying
    /// bits. This reflects the behaviour of other base-conversion methods in .NET.
    /// </summary>
    /// <typeparam name="T">The integer type.</typeparam>
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
    /// <exception cref="ArgumentOutOfRangeException">If the base is out of range.</exception>
    /// <exception cref="ArgumentInvalidException"> If T is an unsupported type.</exception>
    public static string ToBase<T>(this T n, byte toBase, int width = 1, bool upper = false)
        where T : IBinaryInteger<T>
    {
        // Guard. Check the base is valid.
        CheckBase(toBase);

        // Guard. Make sure width is valid.
        if (width < 1)
        {
            throw new ArgumentOutOfRangeException(nameof(width), "Must be at least 1.");
        }

        // Convert value to BigInteger. It's much easier to work with BigInteger than T, and any
        // integer type can be converted to BigInteger.
        if (n is not BigInteger bi)
        {
            bi = XReflection.Cast<T, BigInteger>(n);
        }

        // Check for 0.
        if (bi == 0)
        {
            return "0";
        }

        // Check for negative value.
        if (bi < 0)
        {
            return "-" + (-bi).ToBase(toBase, width, upper);
        }

        // Get the digit values.
        List<byte> digitValues = bi.CalcDigitValues(toBase);

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
        var result = sbDigits.ToString();

        // Pad to the desired width.
        return result.PadLeft(width, '0');
    }

    /// <summary>Convert an integer to binary digits.</summary>
    /// <typeparam name="T">The integer type.</typeparam>
    /// <param name="n">The integer to convert.</param>
    /// <param name="width">The minimum number of digits in the result.</param>
    /// <returns>The value as a string of binary digits.</returns>
    public static string ToBin<T>(this T n, int width = 1) where T : IBinaryInteger<T>
    {
        return ToBase(n, 2, width);
    }

    /// <summary>Convert integer to quaternary digits.</summary>
    /// <typeparam name="T">The integer type.</typeparam>
    /// <param name="n">The integer to convert.</param>
    /// <param name="width">The minimum number of digits in the result.</param>
    /// <returns>The value as a string of quaternary digits.</returns>
    public static string ToQuat<T>(this T n, int width = 1) where T : IBinaryInteger<T>
    {
        return ToBase(n, 4, width);
    }

    /// <summary>Convert integer to octal digits.</summary>
    /// <typeparam name="T">The integer type.</typeparam>
    /// <param name="n">The integer to convert.</param>
    /// <param name="width">The minimum number of digits in the result.</param>
    /// <returns>The value as a string of octal digits.</returns>
    public static string ToOct<T>(this T n, int width = 1) where T : IBinaryInteger<T>
    {
        return ToBase(n, 8, width);
    }

    /// <summary>Convert integer to hexadecimal digits.</summary>
    /// <typeparam name="T">The integer type.</typeparam>
    /// <param name="n">The integer to convert.</param>
    /// <param name="width">The minimum number of digits in the result.</param>
    /// <param name="upper">If letters should be upper-case (false = lower, true = upper).</param>
    /// <returns>The value as a string of hexadecimal digits.</returns>
    public static string ToHex<T>(this T n, int width = 1, bool upper = false)
        where T : IBinaryInteger<T>
    {
        return ToBase(n, 16, width, upper);
    }

    /// <summary>Convert integer to duotrigesimal digits.</summary>
    /// <typeparam name="T">The integer type.</typeparam>
    /// <param name="n">The integer to convert.</param>
    /// <param name="width">The minimum number of digits in the result.</param>
    /// <param name="upper">If letters should be upper-case (false = lower, true = upper).</param>
    /// <returns>The value as a string of duotrigesimal digits.</returns>
    public static string ToDuo<T>(this T n, int width = 1, bool upper = false)
        where T : IBinaryInteger<T>
    {
        return ToBase(n, 32, width, upper);
    }

    /// <summary>Convert integer to tetrasexagesimal digits.</summary>
    /// <typeparam name="T">The integer type.</typeparam>
    /// <param name="n">The integer to convert.</param>
    /// <param name="width">The minimum number of digits in the result.</param>
    /// <param name="upper">If letters should be upper-case (false = lower, true = upper).</param>
    /// <returns>The value as a string of tetrasexagesimal digits.</returns>
    public static string ToTetra<T>(this T n, int width = 1, bool upper = false)
        where T : IBinaryInteger<T>
    {
        return ToBase(n, 64, width, upper);
    }

    #endregion Extension methods

    #region Static conversion methods

    /// <summary>
    /// Convert a string of digits in a given base into a int.
    /// Group separator characters, including spaces, newlines, thin spaces, periods, commas, and
    /// underscores, will be ignored.
    /// </summary>
    /// <typeparam name="T">The integer type to create.</typeparam>
    /// <param name="digits">A string of digits in the specified base.</param>
    /// <param name="fromBase">The base that the digits in the string are in.</param>
    /// <returns>The integer equivalent of the digits.</returns>
    /// <exception cref="ArgumentNullException">If string is null, empty, or whitespace.</exception>
    /// <exception cref="ArgumentFormatException">
    /// If string contains invalid characters for the specified base.
    /// </exception>
    /// <exception cref="InvalidCastException">If a cast to the target type failed.</exception>
    /// <exception cref="OverflowException">
    /// If the resulting value is out of range for the target type.
    /// </exception>
    public static T FromBase<T>(string digits, byte fromBase) where T : IBinaryInteger<T>
    {
        // Check the input string != null, empty, or whitespace.
        if (string.IsNullOrWhiteSpace(digits))
        {
            throw new ArgumentNullException(nameof(digits), "Cannot be empty or whitespace.");
        }

        // Check the base is valid. This could throw an ArgumentOutOfRangeException.
        CheckBase(fromBase);

        // Remove digit grouping characters. We'll allow them to be present anywhere in the input
        // string without throwing an exception, even if they aren't in the correct positions.
        // Any other characters that are invalid for the specified base will throw an exception.
        digits = Regex.Replace(digits, $"[{DIGIT_GROUPING_CHARACTERS}]", "");

        // See if the value is negative.
        var sign = 1;
        if (digits[0] == '-')
        {
            sign = -1;
            // Remove the minus sign.
            digits = digits[1..];
        }

        // Get a map of valid digits to their value, for this base.
        Dictionary<char, byte> digitValues = GetDigitValuesForBase(fromBase);

        // Do the conversion.
        BigInteger value = 0;
        foreach (char c in digits)
        {
            // Try to get the character value from the map.
            if (!digitValues.TryGetValue(c, out byte digitValue))
            {
                char[] digitChars = digitValues.Select(kvp => kvp.Key).ToArray();
                string digitList = string.Join(", ", digitChars[..^1]) + " and " + digitChars[^1];
                throw new ArgumentFormatException(nameof(digits),
                    $"A string representing a number in base {fromBase} may only include the digits {digitList}.");
            }

            // Add it to the result.
            value = value * fromBase + digitValue;
        }

        // Make the result negative or positive as needed.
        value *= sign;

        // Try to convert the value to the target type.
        try
        {
            return XReflection.Cast<BigInteger, T>(value);
        }
        catch (OverflowException ex)
        {
            throw new OverflowException(
                $"The integer represented by the string is outside the valid range for {typeof(T).Name}.",
                ex);
        }
    }

    /// <summary>
    /// Convert a string of binary digits into an integer.
    /// </summary>
    /// <typeparam name="T">The integer type to create.</typeparam>
    /// <param name="digits">The string of digits to parse.</param>
    /// <returns>The integer equivalent of the digits.</returns>
    public static T FromBin<T>(string digits) where T : IBinaryInteger<T>
    {
        return FromBase<T>(digits, 2);
    }

    /// <summary>
    /// Convert a string of quaternary digits into an integer.
    /// </summary>
    /// <typeparam name="T">The integer type to create.</typeparam>
    /// <param name="digits">The string of digits to parse.</param>
    /// <returns>The integer equivalent of the digits.</returns>
    public static T FromQuat<T>(string digits) where T : IBinaryInteger<T>
    {
        return FromBase<T>(digits, 4);
    }

    /// <summary>
    /// Convert a string of octal digits into an integer.
    /// </summary>
    /// <typeparam name="T">The integer type to create.</typeparam>
    /// <param name="digits">The string of digits to parse.</param>
    /// <returns>The integer equivalent of the digits.</returns>
    public static T FromOct<T>(string digits) where T : IBinaryInteger<T>
    {
        return FromBase<T>(digits, 8);
    }

    /// <summary>
    /// Convert a string of hexadecimal digits into an integer.
    /// </summary>
    /// <typeparam name="T">The integer type to create.</typeparam>
    /// <param name="digits">The string of digits to parse.</param>
    /// <returns>The integer equivalent of the digits.</returns>
    public static T FromHex<T>(string digits) where T : IBinaryInteger<T>
    {
        return FromBase<T>(digits, 16);
    }

    /// <summary>
    /// Convert a string of duotrigesimal digits into an integer.
    /// </summary>
    /// <typeparam name="T">The integer type to create.</typeparam>
    /// <param name="digits">The string of digits to parse.</param>
    /// <returns>The integer equivalent of the digits.</returns>
    public static T FromDuo<T>(string digits) where T : IBinaryInteger<T>
    {
        return FromBase<T>(digits, 32);
    }

    /// <summary>
    /// Convert a string of tetrasexagesimal digits into an integer.
    /// </summary>
    /// <typeparam name="T">The integer type to create.</typeparam>
    /// <param name="digits">The string of digits to parse.</param>
    /// <returns>The integer equivalent of the digits.</returns>
    public static T FromTetra<T>(string digits) where T : IBinaryInteger<T>
    {
        return FromBase<T>(digits, 64);
    }

    #endregion Static conversion methods

    #region Private helper methods

    /// <summary>
    /// Checks if the provided base is valid for use by this class.
    /// </summary>
    /// <param name="radix">The base to check.</param>
    /// <exception cref="ArgumentOutOfRangeException">If the base is out of range.</exception>
    private static void CheckBase(byte radix)
    {
        if (radix is < MIN_BASE or > MAX_BASE)
        {
            throw new ArgumentOutOfRangeException(nameof(radix),
                $"The base must be in the range {MIN_BASE}..{MAX_BASE}.");
        }
    }

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
    /// <typeparam name="T">The integer type.</typeparam>
    /// <returns>
    /// The digit values as a list of bytes. An item's index in the list will equal the exponent
    /// (i.e. the first item will have an index of 0, corresponding to the exponent of 0 or units
    /// position). Thus the digit values will be in the reverse order to how they would appear if
    /// written using positional notation.</returns>
    /// <exception cref="ArgumentOutOfRangeException">If the argument is non-negative.</exception>
    internal static List<byte> CalcDigitValues<T>(this T n, byte toBase) where T : IBinaryInteger<T>
    {
        // Guard against negative argument.
        if (T.IsNegative(n))
        {
            throw new ArgumentOutOfRangeException(nameof(n), "Must be non-negative.");
        }

        // Initialize the result.
        var result = new List<byte>();

        // Check for zero.
        if (T.IsZero(n))
        {
            result.Add(0);
            return result;
        }

        // Convert value to BigInteger. It's much easier to work with BigInteger than T, and any
        // integer type can be converted to BigInteger.
        if (n is not BigInteger bi)
        {
            bi = XReflection.Cast<T, BigInteger>(n);
        }

        // Get the digit values.
        while (true)
        {
            // Get the next digit.
            BigInteger rem = bi % toBase;
            result.Add((byte)rem);

            // Check if we're done.
            bi -= rem;
            if (bi == 0)
            {
                break;
            }

            // Prepare for next iteration.
            bi /= toBase;
        }

        return result;
    }

    #endregion Private helper methods
}
