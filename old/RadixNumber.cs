using System.Text;
using Galaxon.Core.Exceptions;

namespace Galaxon.Numerics.Types;

/// <summary>
/// This class supports numbers with a specified radix, which can be in the range 2..36.
///
/// Additional support is provided in the form of "To" and "From" methods for:
///   - binary (base 2)
///   - quaternary (base 4)
///   - octal (base 8)
///   - decimal (base 10)
///   - duodecimal (base 12)
///   - hexadecimal (base 16)
///   - vigesimal (base 20)
///   - triacontakaidecimal (base 32)
/// </summary>
public class RadixNumber : IComparable<RadixNumber>, IEquatable<RadixNumber>, ICloneable
{
    #region Constants

    /// <summary>The minimum radix supported by the type.</summary>
    public const int MinRadix = 2;

    /// <summary>The maximum radix supported by the type.</summary>
    public const int MaxRadix = 36;

    #endregion Constants

    #region Fields

    /// <summary>
    /// Valid digits as a string, supporting up to radix 36. These are the same digits used by Java
    /// and JavaScript.
    /// Lower-case letters are also valid, but that's taken care of by changing the case of input
    /// and output strings as needed.
    /// </summary>
    public const string Digits = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ";

    /// <summary>Common radixes, which will allow us to use faster internal methods.</summary>
    public static readonly sbyte[] CommonRadixes = { 2, 8, 16 };

    /// <summary>The underlying radix field.</summary>
    private readonly sbyte _radix;

    #endregion Fields

    #region Properties

    /// <summary>
    /// The value of the number. Any ulong value is valid, so we don't need to change the range. The
    /// compiler will do that.
    /// </summary>
    public ulong Value { get; set; }

    /// <summary>The radix of the number system (e.g. 2 for binary, 16 for hexadecimal).</summary>
    /// <exception cref="ArgumentOutOfRangeException">If the radix is out of range.</exception>
    public sbyte Radix
    {
        get => _radix;

        init
        {
            CheckRadix(value);
            _radix = value;
        }
    }

    #endregion Properties

    #region Constructors

    /// <summary>Construct from a unsigned long (or smaller unsigned int).</summary>
    /// <exception cref="ArgumentOutOfRangeException">If the radix is out of range.</exception>
    public RadixNumber(ulong value, sbyte radix = 10)
    {
        Value = value;
        Radix = radix;
    }

    /// <summary>Construct from a string of digits in the given radix.</summary>
    /// <exception cref="ArgumentNullException">
    /// If the digits string == null, empty, or whitespace.
    /// </exception>
    /// <exception cref="ArgumentFormatException">
    /// If the digits string contains invalid characters.
    /// </exception>
    /// <exception cref="ArgumentOutOfRangeException">
    /// If the radix is out of range.
    /// </exception>
    /// <exception cref="OverflowException">
    /// If the value is out of range for a ulong.
    /// </exception>
    public RadixNumber(string digits, sbyte radix = 10) :
        this(DigitsToValue(digits, radix), radix)
    {
    }

    #endregion Constructors

    #region Instance methods

    /// <summary>Convert a RadixNumber to a string.</summary>
    public override string ToString() =>
        ValueToDigits(Value, Radix);

    /// <summary>Converts number to a given radix.</summary>
    /// <exception cref="ArgumentOutOfRangeException">If the radix is out of range.</exception>
    public RadixNumber ToRadix(sbyte radix2) =>
        new (Value, radix2);

    /// <summary>Converts to binary string representation.</summary>
    /// <param name="includePrefix">If to include the "0b" prefix.</param>
    /// <returns>The value as a string of binary digits.</returns>
    public string ToBinString(bool includePrefix = false)
    {
        string result = ToRadix(2).ToString();
        if (includePrefix)
        {
            result = "0b" + result;
        }
        return result;
    }

    /// <summary>Converts to quaternary string representation.</summary>
    /// <param name="includePrefix">If to include the "0q" prefix.</param>
    /// <returns>The value as a string of quaternary digits.</returns>
    public string ToQuatString(bool includePrefix = false)
    {
        string result = ToRadix(4).ToString();
        if (includePrefix)
        {
            result = "0q" + result;
        }
        return result;
    }

    /// <summary>Converts to octal string representation.</summary>
    /// <param name="includePrefix">If to include the "0o" prefix.</param>
    /// <returns>The value as a string of octal digits.</returns>
    public string ToOctString(bool includePrefix = false)
    {
        string result = ToRadix(8).ToString();
        if (includePrefix)
        {
            result = "0o" + result;
        }
        return result;
    }

    /// <summary>Converts to decimal string representation.</summary>
    /// <param name="includePrefix">If to include the "0d" prefix.</param>
    /// <returns>The value as a string of decimal digits.</returns>
    public string ToDecString(bool includePrefix = false)
    {
        string result = ToRadix(10).ToString();
        if (includePrefix)
        {
            result = "0d" + result;
        }
        return result;
    }

    /// <summary>Converts to duodecimal string representation.</summary>
    /// <param name="includePrefix">If to include the "0z" prefix.</param>
    /// <param name="upperCase">If the letter digits should be uppercase.</param>
    /// <returns>The value as a string of duodecimal digits.</returns>
    public string ToDuoString(bool includePrefix = false, bool upperCase = true)
    {
        string result = ToRadix(12).ToString();
        if (!upperCase)
        {
            result = result.ToLower();
        }
        if (includePrefix)
        {
            result = "0z" + result;
        }
        return result;
    }

    /// <summary>Converts to hexadecimal string representation.</summary>
    /// <param name="includePrefix">If to include the "0x" prefix.</param>
    /// <param name="upperCase">If the letter digits should be uppercase.</param>
    /// <returns>The value as a string of hexadecimal digits.</returns>
    public string ToHexString(bool includePrefix = false, bool upperCase = true)
    {
        string result = ToRadix(16).ToString();
        if (!upperCase)
        {
            result = result.ToLower();
        }
        if (includePrefix)
        {
            result = "0x" + result;
        }
        return result;
    }

    /// <summary>Converts to vigesimal string representation.</summary>
    /// <param name="includePrefix">If to include the "0v" prefix.</param>
    /// <param name="upperCase">If the letter digits should be uppercase.</param>
    /// <returns>The value as a string of vigesimal digits.</returns>
    public string ToVigString(bool includePrefix = false, bool upperCase = true)
    {
        string result = ToRadix(20).ToString();
        if (!upperCase)
        {
            result = result.ToLower();
        }
        if (includePrefix)
        {
            result = "0v" + result;
        }
        return result;
    }

    /// <summary>Converts to triacontakaidecimal string representation.</summary>
    /// <param name="includePrefix">If to include the "0t" prefix.</param>
    /// <param name="upperCase">If the letter digits should be uppercase.</param>
    /// <returns>The value as a string of triacontakaidecimal digits.</returns>
    public string ToTriaString(bool includePrefix = false, bool upperCase = true)
    {
        string result = ToRadix(32).ToString();
        if (!upperCase)
        {
            result = result.ToLower();
        }
        if (includePrefix)
        {
            result = "0t" + result;
        }
        return result;
    }

    /// <summary>Compare two values.</summary>
    /// <param name="radixNum2">Another RadixNum value.</param>
    /// <returns>
    /// An integer less than 0 if the first RadixNumber's value is less then the second's.
    /// Zero (0) if the first RadixNumber's value is equal to the second's.
    /// An integer greater than 0 if the first RadixNumber's value is greater the second's.
    /// </returns>
    public int CompareTo(RadixNumber? radixNum2)
    {
        if (radixNum2 is null)
        {
            throw new ArgumentNullException(nameof(radixNum2));
        }

        return Value.CompareTo(radixNum2.Value);
    }

    /// <summary>See if two RadixNum values are equal.</summary>
    /// <param name="radixNum2">Another RadixNum value.</param>
    /// <returns></returns>
    public bool Equals(RadixNumber? radixNum2) =>
        radixNum2 is not null && Value == radixNum2.Value;

    /// <summary>See if the RadixNum is equal to the given value.</summary>
    /// <param name="radixNum2">Some other value.</param>
    /// <returns></returns>
    public override bool Equals(object? radixNum2) =>
        radixNum2 is RadixNumber num2 && Equals(num2);

    // /// <summary>Return a hashcode for the object.</summary>
    // /// <returns>The hash code.</returns>
    public override int GetHashCode() =>
        ToString().GetHashCode();

    /// <summary>Clone a RadixNumber.</summary>
    /// <returns>The new RadixNumber.</returns>
    public object Clone() =>
        MemberwiseClone();

    #endregion Instance methods

    #region Static methods

    /// <summary>
    /// Checks if the provided radix is valid for use by this class.
    /// </summary>
    /// <param name="radix">The radix to check.</param>
    /// <exception cref="ArgumentOutOfRangeException">If the radix is out of range.</exception>
    public static void CheckRadix(sbyte radix)
    {
        if (radix is < MinRadix or > MaxRadix)
        {
            throw new ArgumentOutOfRangeException(
                $"Radix must be in the range {MinRadix}..{MaxRadix}.");
        }
    }

    /// <summary>
    /// Convert a string of digits in the specified radix to an unsigned long.
    /// </summary>
    /// <exception cref="ArgumentNullException">
    /// If the digits string == null, empty, or whitespace.
    /// </exception>
    /// <exception cref="ArgumentFormatException">
    /// If the digits string contains invalid characters.
    /// </exception>
    /// <exception cref="ArgumentOutOfRangeException">
    /// If the radix is out of range.
    /// </exception>
    /// <exception cref="OverflowException">
    /// If the value is out of range for a ulong.
    /// </exception>
    public static ulong DigitsToValue(string digits, sbyte radix = 10)
    {
        // Check the input string != null, empty, or whitespace.
        if (string.IsNullOrWhiteSpace(digits))
        {
            throw new ArgumentNullException(nameof(digits), "Cannot be empty or whitespace.");
        }

        // Check the radix is valid. This could throw an ArgumentOutOfRangeException.
        CheckRadix(radix);

        // Use the internal method for decimal; should be faster.
        if (radix == 10)
        {
            return ulong.Parse(digits);
        }

        // Use the internal method for other common radixes; should be faster.
        if (CommonRadixes.Contains(radix))
        {
            return Convert.ToUInt64(digits, radix);
        }

        // Do the conversion for uncommon radixes.
        ulong value = 0;

        foreach (char c in digits)
        {
            int digitValue;

            switch (c)
            {
                case >= '0' and <= '9':
                    digitValue = c - '0';
                    break;

                case >= 'a' and <= 'z':
                    digitValue = c - 'a' + 10;
                    break;

                case >= 'A' and <= 'Z':
                    digitValue = c - 'A' + 10;
                    break;

                default:
                    string ucMsg = radix > 10 ? " Letter digits may be lower or upper case." : "";
                    throw new ArgumentFormatException(nameof(digits),
                        $"A string representing a number in radix (or base) {radix} may only include these digits: {Digits[..radix]}.{ucMsg}");
            }

            value = value * (ulong)radix + (ulong)digitValue;
        }

        return value;
    }

    /// <summary>Convert an unsigned long to a string of digits in the specified radix.</summary>
    /// <exception cref="ArgumentOutOfRangeException">If the radix is invalid.</exception>
    public static string ValueToDigits(ulong value, sbyte radix = 10)
    {
        // Check the radix is valid.
        CheckRadix(radix);

        // Use the internal method for decimal; should be faster.
        if (radix == 10)
        {
            return value.ToString();
        }

        // Use the internal method for other common radixes, as this should be faster.
        if (CommonRadixes.Contains(radix))
        {
            return Convert.ToString((long)value, radix).ToUpper();
        }

        // Build the output string.
        StringBuilder digits = new ();

        while (true)
        {
            if (value < (ulong)radix)
            {
                if (value > 0 || digits.Length == 0)
                {
                    digits.Insert(0, Digits[(int)value]);
                }

                // We're done.
                break;
            }
            ulong rem = value % (ulong)radix;
            digits.Insert(0, Digits[(int)rem]);
            value = (value - rem) / (ulong)radix;
        }

        return digits.ToString();
    }

    /// <summary>Convert a string of binary digits into a RadixNum.</summary>
    public static RadixNumber FromBinString(string digits) =>
        new (digits, 2);

    /// <summary>Convert a string of quaternary digits into a RadixNum.</summary>
    public static RadixNumber FromQuatString(string digits) =>
        new (digits, 4);

    /// <summary>Convert a string of octal digits into a RadixNum.</summary>
    public static RadixNumber FromOctString(string digits) =>
        new (digits, 8);

    /// <summary>Convert a string of decimal digits into a RadixNum.</summary>
    public static RadixNumber FromDecString(string digits) =>
        new (digits);

    /// <summary>Convert a string of duodecimal digits into a RadixNum.</summary>
    public static RadixNumber FromDuoString(string digits) =>
        new (digits, 12);

    /// <summary>Convert a string of hexadecimal digits into a RadixNum.</summary>
    public static RadixNumber FromHexString(string digits) =>
        new (digits, 16);

    /// <summary>Convert a string of vigesimal digits into a RadixNum.</summary>
    public static RadixNumber FromVigString(string digits) =>
        new (digits, 20);

    /// <summary>Convert a string of triacontakaidecimal digits into a RadixNum.</summary>
    public static RadixNumber FromTriaString(string digits) =>
        new (digits, 32);

    #endregion Static methods

    #region Comparison operators

    /// <summary>Overload == operator.</summary>
    public static bool operator ==(RadixNumber radixNum, RadixNumber radixNum2) =>
        radixNum.Value == radixNum2.Value;

    /// <summary>Overload != operator.</summary>
    public static bool operator !=(RadixNumber radixNum, RadixNumber radixNum2) =>
        radixNum.Value != radixNum2.Value;

    /// <summary>Overload &lt; operator.</summary>
    public static bool operator <(RadixNumber radixNum, RadixNumber radixNum2) =>
        radixNum.Value < radixNum2.Value;

    /// <summary>Overload &gt; operator.</summary>
    public static bool operator >(RadixNumber radixNum, RadixNumber radixNum2) =>
        radixNum.Value > radixNum2.Value;

    /// <summary>Overload &lt;= operator.</summary>
    public static bool operator <=(RadixNumber radixNum, RadixNumber radixNum2) =>
        radixNum.Value <= radixNum2.Value;

    /// <summary>Overload &gt;= operator.</summary>
    public static bool operator >=(RadixNumber radixNum, RadixNumber radixNum2) =>
        radixNum.Value >= radixNum2.Value;

    #endregion Comparison operators

    #region Arithmetic operators

    /// <summary>Overload + operator.</summary>
    /// <exception cref="InvalidOperationException">
    /// If the numbers have different bases.
    /// </exception>
    /// <exception cref="OverflowException">
    /// If the result of the addition exceeds ulong.MaxValue.
    /// </exception>
    public static RadixNumber operator +(RadixNumber radixNum, RadixNumber radixNum2)
    {
        if (radixNum.Radix != radixNum2.Radix)
        {
            throw new InvalidOperationException("Numbers must have the same radix.");
        }

        return new RadixNumber(checked(radixNum.Value + radixNum2.Value), radixNum.Radix);
    }

    /// <summary>Overload - operator.</summary>
    /// <exception cref="InvalidOperationException">If the numbers have different bases.</exception>
    /// <exception cref="OverflowException">
    /// If the result of the subtraction produces a result less than 0.
    /// </exception>
    public static RadixNumber operator -(RadixNumber radixNum, RadixNumber radixNum2)
    {
        if (radixNum.Radix != radixNum2.Radix)
        {
            throw new InvalidOperationException("Numbers must have the same radix.");
        }

        return new RadixNumber(checked(radixNum.Value - radixNum2.Value), radixNum.Radix);
    }

    #endregion Arithmetic operators

    #region Bitwise operators

    /// <summary>Overload bitwise NOT (~) operator.</summary>
    public static RadixNumber operator ~(RadixNumber radixNum) =>
        new (~radixNum.Value, radixNum.Radix);

    /// <summary> Overload bitwise AND (&) operator. </summary>
    public static RadixNumber operator &(RadixNumber radixNum, RadixNumber radixNum2)
    {
        if (radixNum.Radix != radixNum2.Radix)
        {
            throw new InvalidOperationException("Numbers must have the same radix.");
        }

        return new RadixNumber(radixNum.Value & radixNum2.Value, radixNum.Radix);
    }

    /// <summary>Overload bitwise OR (|) operator.</summary>
    public static RadixNumber operator |(RadixNumber radixNum, RadixNumber radixNum2)
    {
        if (radixNum.Radix != radixNum2.Radix)
        {
            throw new InvalidOperationException("Numbers must have the same radix.");
        }

        return new RadixNumber(radixNum.Value | radixNum2.Value, radixNum.Radix);
    }

    #endregion Bitwise operators

    #region Conversion operators

    /// <summary>Implicit conversion to ulong.</summary>
    public static implicit operator ulong(RadixNumber radixNum) =>
        radixNum.Value;

    #endregion Conversion operators
}
