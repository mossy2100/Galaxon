using System.Globalization;
using System.Numerics;
using System.Text.RegularExpressions;
using Galaxon.Core.Exceptions;
using Galaxon.Numerics.Extensions;

namespace Galaxon.Numerics.BigNumbers;

public partial struct BigDecimal
{
    #region Parse methods

    /// <inheritdoc/>
    /// <remarks>Ignoring style parameter for now.</remarks>
    public static BigDecimal Parse(string s, NumberStyles style, IFormatProvider? provider)
    {
        return Parse(s, provider);
    }

    /// <inheritdoc/>
    public static BigDecimal Parse(string s, IFormatProvider? provider)
    {
        // Remove ignored characters from the string.
        s = NumberExtensions.RemoveWhitespaceAndDigitGroupSeparators(s);

        // See if there are any characters left.
        if (string.IsNullOrWhiteSpace(s))
        {
            throw new ArgumentFormatException(nameof(s), "Invalid BigDecimal format.");
        }

        // Check the string format and extract salient info.
        var strRxSign = @"[\-+]?";
        var strRxInt = $@"(?<int>{strRxSign}\d+)";
        var strRxFrac = $@"(\.(?<frac>\d+))?";
        var strRxExp = $@"(e(?<exp>{strRxSign}\d+))?";
        var strRx = $"^{strRxInt}{strRxFrac}{strRxExp}$";
        var match = Regex.Match(s, strRx, RegexOptions.IgnoreCase);

        if (!match.Success)
        {
            throw new ArgumentFormatException(nameof(s), "Invalid BigDecimal format.");
        }

        // Get the digits.
        var strInt = match.Groups["int"].Value;
        var strFrac = match.Groups["frac"].Value;
        var strExp = match.Groups["exp"].Value;

        // Construct the result.
        var sig = BigInteger.Parse(strInt + strFrac, provider);
        var exp = strExp == "" ? 0 : int.Parse(strExp, provider);
        exp -= strFrac.Length;
        return new BigDecimal(sig, exp);
    }

    /// <summary>Simplest version of Parse().</summary>
    /// <param name="s">The string to parse.</param>
    /// <returns>The BigDecimal value represented by the string.</returns>
    public static BigDecimal Parse(string s)
    {
        return Parse(s, NumberFormatInfo.InvariantInfo);
    }

    /// <inheritdoc/>
    /// <remarks>Ignoring style parameter for now.</remarks>
    public static BigDecimal Parse(ReadOnlySpan<char> s, NumberStyles style,
        IFormatProvider? provider)
    {
        return Parse(new string(s), provider);
    }

    /// <inheritdoc/>
    public static BigDecimal Parse(ReadOnlySpan<char> s, IFormatProvider? provider)
    {
        return Parse(new string(s), provider);
    }

    /// <inheritdoc/>
    /// <remarks>Ignoring style parameter for now.</remarks>
    public static bool TryParse(string? s, NumberStyles style, IFormatProvider? provider,
        out BigDecimal result)
    {
        return TryParse(s, provider, out result);
    }

    /// <inheritdoc/>
    public static bool TryParse(string? s, IFormatProvider? provider, out BigDecimal result)
    {
        // Check a value was provided.
        if (string.IsNullOrWhiteSpace(s))
        {
            result = 0;
            return false;
        }

        // Try to parse the provided string.
        try
        {
            result = Parse(s, provider);
            return true;
        }
        catch (Exception)
        {
            result = 0;
            return false;
        }
    }

    /// <summary>Simplest version of TryParse().</summary>
    /// <param name="s">The string to parse.</param>
    /// <param name="result">The BigDecimal value represented by the string.</param>
    /// <returns>If the attempt to parse the value succeeded.</returns>
    public static bool TryParse(string? s, out BigDecimal result)
    {
        return TryParse(s, NumberFormatInfo.InvariantInfo, out result);
    }

    /// <inheritdoc/>
    /// <remarks>Ignoring style parameter for now.</remarks>
    public static bool TryParse(ReadOnlySpan<char> span, NumberStyles style,
        IFormatProvider? provider, out BigDecimal result)
    {
        return TryParse(new string(span), provider, out result);
    }

    /// <inheritdoc/>
    public static bool TryParse(ReadOnlySpan<char> span, IFormatProvider? provider,
        out BigDecimal result)
    {
        return TryParse(new string(span), provider, out result);
    }

    #endregion Parse methods

    #region Format methods

    /// <summary>
    /// Format the BigDecimal as a string.
    /// Supported formats are the usual: D, E, F, G, N, P, and R.
    /// <see href="https://learn.microsoft.com/en-us/dotnet/standard/base-types/standard-numeric-format-strings"/>
    /// Although "D" is normally only used by integral types, in this case both the significand and
    /// exponent will be formatted as integers.
    /// An secondary code "U" is provided, which follows the precision (if given).
    /// - If omitted, the exponent (if present) will be formatted with the usual E[-+]999 format.
    /// - If present, the exponent is formatted with "×10" instead of "E" and the exponent digits
    /// will be rendered as superscript. Also, a "+" sign is not used for positive exponents,
    /// and the exponent digits are not zero-padded.
    /// Example: "E7U" will format as per usual (E with 7 decimal digits), except using Unicode
    /// characters for the exponent part.
    /// Codes "R" and "D" will produce the same output. However, the Unicode flag is undefined with
    /// "R", because Parse() doesn't support superscript exponents.
    /// </summary>
    /// <param name="specifier">The format specifier (default "G").</param>
    /// <param name="provider">The format provider (default null).</param>
    /// <returns>The formatted string.</returns>
    /// <exception cref="ArgumentInvalidException">If the format specifier is invalid.</exception>
    public string ToString(string? specifier, IFormatProvider? provider)
    {
        // Set defaults.
        var format = "G";
        var ucFormat = format;
        int? precision = null;
        var unicode = false;

        // Parse the format specifier.
        if (!string.IsNullOrEmpty(specifier))
        {
            var match = FormatRegex().Match(specifier);

            // Check format is valid.
            if (!match.Success)
            {
                throw new ArgumentOutOfRangeException(nameof(specifier),
                    $"Invalid format specifier \"{specifier}\".");
            }

            // Extract parts.
            format = match.Groups["format"].Value;
            ucFormat = format.ToUpper();
            var strPrecision = match.Groups["precision"].Value;
            precision = strPrecision == "" ? null : int.Parse(strPrecision);
            unicode = match.Groups["unicode"].Value.ToUpper() == "U";

            // Check if Unicode flag is present when it shouldn't be.
            if (unicode && ucFormat is "F" or "N" or "P" or "R")
            {
                throw new ArgumentInvalidException(nameof(specifier),
                    $"The Unicode flag is invalid with format \"{format}\".");
            }
        }

        // Format the significand.
        var exp = Exponent;
        switch (ucFormat)
        {
            case "D" or "R":
                var strSig = Significand.ToString($"D{precision}", provider);
                if (exp == 0)
                {
                    return strSig;
                }
                return strSig + FormatExponent(format, exp, unicode, 1, provider);

            case "E":
                return FormatScientific(format, precision, unicode, 3, provider);

            case "F" or "N":
                return FormatFixed(format, precision, provider);

            case "G":
            {
                // Default precision is unlimited, same as for BigInteger.
                // See: https://learn.microsoft.com/en-us/dotnet/standard/base-types/standard-numeric-format-strings#GFormatString

                // Get the format using E with 2-digit exponent.
                var strFormatE = FormatScientific(format, precision - 1, unicode, 2, provider);

                // Get the fixed point format, specifying the precision as the maximum number of
                // significant figures.
                var strFormatF = FormatFixedSigFigs(precision, provider);

                // Return the shorter, preferring F.
                return strFormatF.Length <= strFormatE.Length ? strFormatF : strFormatE;
            }

            case "P":
                return (this * 100).FormatFixed("F", precision ?? 2, provider) + "%";

            default:
                return "";
        }
    }

    /// <summary>
    /// Format the BigDecimal as a string.
    /// </summary>
    public string ToString(string format)
    {
        return ToString(format, CultureInfo.InvariantCulture);
    }

    /// <inheritdoc/>
    public override string ToString()
    {
        return ToString("G");
    }

    /// <inheritdoc/>
    public bool TryFormat(Span<char> destination, out int charsWritten,
        ReadOnlySpan<char> format, IFormatProvider? provider)
    {
        var formattedValue = ToString(new string(format), provider);
        try
        {
            formattedValue.CopyTo(destination);
            charsWritten = formattedValue.Length;
            return true;
        }
        catch
        {
            charsWritten = 0;
            return false;
        }
    }

    #endregion Format methods

    #region Helper methods

    /// <summary>
    /// From a BigDecimal, extract two strings of digits that would appear if the number was written
    /// in fixed-point format (i.e. without an exponent).
    /// Sign is ignored.
    /// </summary>
    private readonly (string strInt, string strFrac) PreformatFixed()
    {
        var strAbsSig = BigInteger.Abs(Significand).ToString();

        // Integer.
        if (Exponent == 0)
        {
            return (strAbsSig, "");
        }

        // Fraction between 0.1 and 1.
        if (-Exponent == strAbsSig.Length)
        {
            return ("0", strAbsSig);
        }

        // Integer multiple of 10.
        if (Exponent > 0)
        {
            return (strAbsSig.PadRight(strAbsSig.Length + Exponent, '0'), "");
        }

        // Fraction less than 0.1.
        if (-Exponent > strAbsSig.Length)
        {
            return ("0", strAbsSig.PadLeft(-Exponent, '0'));
        }

        // Fraction with non-zeros on both sides of the decimal point.
        return (strAbsSig[..^-Exponent], strAbsSig[^-Exponent..]);
    }

    /// <summary>
    /// Format the BigDecimal as a fixed-point number with a given precision.
    /// </summary>
    /// <param name="format">The format to use.</param>
    /// <param name="precision">The number of decimal places.</param>
    /// <param name="provider">The format provider.</param>
    /// <returns></returns>
    private readonly string FormatFixed(string format, int? precision, IFormatProvider? provider)
    {
        // Get the parts of the string.
        var bd = precision.HasValue ? Round(this, precision.Value) : this;
        var strSign = bd.Significand < 0 ? "-" : "";
        var (strInt, strFrac) = bd.PreformatFixed();

        // Add group separators to the integer part if necessary.
        if (format == "N")
        {
            strInt = BigInteger.Parse(strInt).ToString("N0", provider);
        }

        // Format the fractional part.
        if (strFrac != "" || precision is > 0)
        {
            // Add trailing 0s if the precision was specified and they're needed.
            if (precision.HasValue && precision.Value > strFrac.Length)
            {
                strFrac = strFrac.PadRight(precision.Value, '0');
            }
            strFrac = '.' + strFrac;
        }

        // If zero, omit sign. We don't want to render -0.0000...
        var strAbs = strInt + strFrac;
        // if (decimal.Parse(strAbs) == 0m)
        // {
        //     return strAbs;
        // }

        return strSign + strAbs;
    }

    /// <summary>
    /// Format as fixed point, except in this case the precision is the number of significant
    /// figures, not the number of decimal places.
    /// Note, this is not technically formatting as significant figures, since trailing 0s following
    /// the decimal point are not retained, as per the usual format for "G".
    /// </summary>
    private string FormatFixedSigFigs(int? nSigFigs, IFormatProvider? provider)
    {
        // If we don't have to remove any digits, use default fixed-point format.
        var nDigitsToCut = nSigFigs is null or 0 ? 0 : NumSigFigs - nSigFigs.Value;
        if (nDigitsToCut <= 0)
        {
            return FormatFixed("F", null, provider);
        }

        // Round the value.
        var rounded = Round(new BigDecimal(Significand, -nDigitsToCut));
        rounded.Exponent += Exponent + nDigitsToCut;

        // Format as fixed-point without trailing zeros.
        return rounded.FormatFixed("F", null, provider);
    }

    /// <summary>
    /// Format the value using scientific notation.
    /// </summary>
    private string FormatScientific(string format, int? precision, bool unicode, int expWidth,
        IFormatProvider? provider)
    {
        // Format the significand.
        var nDecimalPlacesToShift = NumSigFigs - 1;
        BigDecimal sig = new (Significand, -nDecimalPlacesToShift);
        var strSig = sig.FormatFixed("F", precision, provider);

        // Format the exponent.
        var exp = Exponent + nDecimalPlacesToShift;
        var strExp = FormatExponent(format, exp, unicode, expWidth, provider);

        return strSig + strExp;
    }

    /// <summary>
    /// Format the exponent part of scientific notation.
    /// </summary>
    /// <param name="format">
    /// The original format code (e.g. E, e, G, or g). We need to know this to determine whether to
    /// use an upper- or lower-case 'E'.
    /// </param>
    /// <param name="exp">The exponent value.</param>
    /// <param name="unicode">Whether to use Unicode or standard format.</param>
    /// <param name="expWidth">
    /// The minimum number of digits in the exponent (typically 3 for E and 2 for G).
    /// Relevant for standard (non-Unicode) format only.
    /// </param>
    /// <param name="provider">The format provider.</param>
    /// <returns>The formatted exponent.</returns>
    private static string FormatExponent(string format, BigInteger exp, bool unicode, int expWidth,
        IFormatProvider? provider)
    {
        // Use Unicode format if requested.
        if (unicode)
        {
            // Prepend the x10 part and superscript the exponent.
            return "×10" + exp.ToSuperscript();
        }

        // Standard format.
        return (char.IsLower(format[0]) ? 'e' : 'E')
            + (exp < 0 ? "-" : "+")
            + BigInteger.Abs(exp).ToString("D", provider).PadLeft(expWidth, '0');
    }

    [GeneratedRegex("^(?<format>[DEFGNPR])(?<precision>\\d*)(?<unicode>U?)$",
        RegexOptions.IgnoreCase, "en-AU")]
    private static partial Regex FormatRegex();

    #endregion Helper methods
}
