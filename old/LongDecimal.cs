using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;
using Galaxon.Core.Exceptions;

namespace Galaxon.Numerics.Types;

public enum Flag
{
    Negative,
    Infinity,
    NaN,
    Signalling
}

/// <summary>
/// Software implementation of large decimal floating point value.
/// The maximum total size is 55 bytes.
/// Flags: 1 byte. Contains bits for the sign, Infinity, and NaN.
/// This is probably an unnecessary memory optimisation since it will cost speed
/// to extract bits from the flags byte. I might change them to booleans later.
/// * bit 0 is for the sign. 0 means positive, 1 means negative.
/// * bit 1 is for Infinity.
/// * bit 2 is for NaN.
/// * bit 3 indicates a quiet NaN (0) or a signalling NaN (1). Not sure if
/// I'll need this - got the idea from the IEEE decimal specification.
/// * bits 4..7 are unused. I could wedge another digit in here if needs be
/// but for now I'll keep it spare.
/// Bits 1 and 0:
/// 00 => finite and positive
/// 01 => finite and negative
/// 10 => infinite and positive = +∞
/// 11 => infinite and negative = -∞
/// Bits 3 and 2:
/// 00 => A number
/// 10 => Invalid
/// 01 => Quiet NaN
/// 11 => Signalling NaN
/// Significand: Up to 50 bytes. As each byte encodes 2 binary-coded decimal
/// digits, this will permit a maximum of 100 digits (1 before the decimal, 99
/// after). Conceptually, the number is written using scientific notation, e.g.
/// 9.999999... * 10^999...
/// Thus:
/// The byte at index 0 in the array encodes digits 0 and 1, which correspond to
/// 10^0 and 10^-1 (i.e. units and tenths (0.0..9.9).
/// The byte at index 1 in the array encodes digits 2 and 3, which correspond to
/// 10^-2 and 10^-3 (i.e. hundredths and thousands) (0.000..0.099).
/// etc.
/// In theory the maximum number of digits could be configurable, but this would
/// add another byte per object.
/// Exponent: 4 bytes. One ordinary signed int.
/// </summary>
public class LongDecimal : IEquatable<LongDecimal>, IComparable<LongDecimal>
{
    #region Constants

    private const byte _MaxSignificandLength = 50;

    private const byte _MaxDigits = 2 * _MaxSignificandLength;

    #endregion Constants

    #region Main properties

    private byte[] Significand { get; init; }

    private int Exponent { get; init; }

    private byte Flags { get; init; }

    #endregion Main properties

    #region Flag properties

    public bool IsPositive => !FlagIsSet(Flag.Negative);

    public bool IsNegative => FlagIsSet(Flag.Negative);

    public bool IsZero =>
        IsANumber && IsPositive && !FlagIsSet(Flag.Infinity)
        && AllZeros(Significand);

    public bool IsNegativeZero =>
        IsANumber && IsNegative && !FlagIsSet(Flag.Infinity)
        && AllZeros(Significand);

    public bool IsInfinity => IsANumber && IsPositive && FlagIsSet(Flag.Infinity);

    public bool IsNegativeInfinity => IsANumber && IsNegative && FlagIsSet(Flag.Infinity);

    public bool IsANumber => !FlagIsSet(Flag.NaN);

    public bool IsNaN => FlagIsSet(Flag.NaN);

    public bool IsQuietNaN => IsNaN && !FlagIsSet(Flag.Signalling);

    public bool IsSignallingNaN => IsNaN && FlagIsSet(Flag.Signalling);

    #endregion Flag properties

    #region Constructors and creation methods

    /// <summary>
    /// Construct a number from an array of digits.
    /// </summary>
    public LongDecimal(byte[] digits, int exponent = 0, bool isNegative = false)
    {
        Significand = PackDigits(Round(TrimZeros(digits, ref exponent)));
        Exponent = exponent;
        Flags = (byte)(isNegative ? 1 : 0);
    }

    /// <summary>
    /// Construct a special value, like -0, Infinity, -Infinity, or NaN.
    /// </summary>
    public LongDecimal(bool isNegative = false, bool isInfinity = false,
        bool isNaN = false, bool isSignalling = false)
    {
        Significand = Array.Empty<byte>();
        Exponent = 0;
        Flags = (byte)((isNegative ? 1 : 0) | (isInfinity ? 2 : 0)
            | (isNaN ? 4 : 0) | (isSignalling ? 8 : 0));
    }

    /// <summary>
    /// Create from string.
    /// </summary>
    /// <exception cref="ArgumentNullException"></exception>
    /// <exception cref="ArgumentFormatException"></exception>
    public static LongDecimal FromString(string s)
    {
        if (string.IsNullOrWhiteSpace(s))
        {
            throw new ArgumentNullException(nameof(s),
                "String should represent a long decimal value.");
        }

        Regex rx = new (@"^(-?)(\d+)(\.(\d+))?(e(-?\d+))?$", RegexOptions.IgnoreCase);
        MatchCollection matches = rx.Matches(s);

        // Check we got exactly one match.
        if (matches.Count != 1)
        {
            throw new ArgumentFormatException(nameof(s),
                "Invalid string format for long decimal value.");
        }

        GroupCollection groups = matches[0].Groups;

        // Get the sign as a bool.
        string strSign = groups[1].Value;
        bool isNegative = strSign == "-";

        // Add the significand digits.
        string strDigitsBeforeDecimal = groups[2].Value;
        string strDigitsAfterDecimal = groups[4].Value;
        byte[] digits = new byte[strDigitsBeforeDecimal.Length + strDigitsAfterDecimal.Length];
        int i = 0;
        foreach (char c in strDigitsBeforeDecimal)
        {
            digits[i++] = (byte)(c - '0');
        }
        foreach (char c in strDigitsAfterDecimal)
        {
            digits[i++] = (byte)(c - '0');
        }

        // Determine the exponent.
        string strExponent = groups[6].Value;
        int exponent = (strExponent == "" ? 0 : int.Parse(strExponent))
            + strDigitsBeforeDecimal.Length - 1;

        return new LongDecimal(digits, exponent, isNegative);
    }

    public static LongDecimal Clone(LongDecimal ld) =>
        new ()
        {
            Flags = ld.Flags,
            Significand = ld.Significand[..],
            Exponent = ld.Exponent
        };

    private static LongDecimal Zero() =>
        new ();

    private static LongDecimal NegativeZero() =>
        new (true);

    private static LongDecimal Infinity() =>
        new (false, true);

    private static LongDecimal NegativeInfinity() =>
        new (true, true);

    private static LongDecimal QuietNaN() =>
        new (false, false, true);

    private static LongDecimal SignallingNaN() =>
        new (false, false, true, true);

    #endregion Constructors and creation methods

    #region Overridden methods

    public override bool Equals(object? obj) =>
        obj is LongDecimal ld && Equals(ld);

    public override int GetHashCode() =>
        HashCode.Combine(Significand, Exponent, Flags);

    public override string ToString()
    {
        StringBuilder sb = new ();

        // Sign.
        if (IsNegative)
        {
            sb.Append('-');
        }

        // Significand.
        int exponent = Exponent;
        if (Significand.Length == 0)
        {
            sb.Append('0');
            exponent = 0;
        }
        else
        {
            byte[] digits = TrimZeros(UnpackDigits(Significand), ref exponent);
            if (digits.Length == 0)
            {
                sb.Append(0);
            }
            else
            {
                sb.Append(digits[0]);
                if (digits.Length > 1)
                {
                    sb.Append('.');
                    for (int i = 1; i < digits.Length; i++)
                    {
                        sb.Append(digits[i]);
                    }
                }
            }
        }

        // Add the exponent if needed.
        if (exponent != 0)
        {
            sb.Append($"e{exponent}");
        }

        return sb.ToString();
    }

    #endregion Overridden methods

    #region Interface methods

    public bool Equals(LongDecimal? other)
    {
        if (other is null)
        {
            return false;
        }

        // Compare flags and exponent.
        if (Flags != other.Flags || Exponent != other.Exponent)
        {
            return false;
        }

        // Compare significand length.
        if (Significand.Length != other.Significand.Length)
        {
            return false;
        }

        // Compare the bytes in the significand.
        return !Significand.Where((t, i) => t != other.Significand[i]).Any();
    }

    /// <summary>
    /// </summary>
    /// <param name="other"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    public int CompareTo(LongDecimal? other)
    {
        if (other is null)
        {
            throw new ArgumentNullException(nameof(other),
                "Cannot compare a LongDecimal and null.");
        }

        // Check if signs are different.
        if (IsNegative && other.IsPositive)
        {
            return -1;
        }
        if (IsPositive && other.IsNegative)
        {
            return 1;
        }

        // Check exponent.
        if (Exponent < other.Exponent)
        {
            return -1;
        }
        if (Exponent > other.Exponent)
        {
            return 1;
        }
        // Exponents are equal.

        // Check significand.
        int nBytes = Max(Significand.Length, other.Significand.Length);
        for (int i = 0; i < nBytes; i++)
        {
            int leftByte = i < Significand.Length ? Significand[i] : 0;
            int rightByte = i < other.Significand.Length ? other.Significand[i] : 0;
            if (leftByte < rightByte)
            {
                return -1;
            }
            if (leftByte > rightByte)
            {
                return 1;
            }
        }
        // Significands are equal.

        // The numbers are equal.
        return 0;
    }

    #endregion Interface methods

    #region Comparison operators

    public static bool operator ==(LongDecimal left, LongDecimal right) =>
        left.Equals(right);

    public static bool operator !=(LongDecimal left, LongDecimal right) =>
        !left.Equals(right);

    public static bool operator <(LongDecimal left, LongDecimal right) =>
        left.CompareTo(right) < 0;

    public static bool operator >(LongDecimal left, LongDecimal right) =>
        left.CompareTo(right) > 0;

    public static bool operator <=(LongDecimal left, LongDecimal right) =>
        left.CompareTo(right) is < 0 or 0;

    public static bool operator >=(LongDecimal left, LongDecimal right) =>
        left.CompareTo(right) is > 0 or 0;

    #endregion Comparison operators

    #region Arithmetic methods

    public static LongDecimal Abs(LongDecimal ld) =>
        new ()
        {
            Significand = ld.Significand[..],
            Exponent = ld.Exponent,
            Flags = (byte)(ld.Flags & ~1)
        };

    public static LongDecimal Negate(LongDecimal ld) =>
        new ()
        {
            Significand = ld.Significand[..],
            Exponent = ld.Exponent,
            Flags = (byte)(ld.IsPositive ? ld.Flags | 1 : ld.Flags & ~1)
        };

    public static LongDecimal operator -(LongDecimal ld) =>
        Negate(ld);

    public static LongDecimal Add(LongDecimal ld1, LongDecimal ld2)
    {
        if (ld1.IsZero)
        {
            return ld2;
        }
        if (ld2.IsZero)
        {
            return ld1;
        }
        if (ld1 == -ld2)
        {
            return Zero();
        }

        // The algorithm in this method is for adding 2 positive numbers, so
        // defer to Subtract() if needed.
        if (ld1.IsNegative && ld2.IsPositive)
        {
            return Subtract(ld2, -ld1);
        }
        if (ld1.IsPositive && ld2.IsNegative)
        {
            return Subtract(ld1, -ld2);
        }

        // Converts negatives to positives.
        if (ld1.IsNegative && ld2.IsNegative)
        {
            return -Add(-ld1, -ld2);
        }

        // Create an array for the result. Leave a spare slot at the beginning
        // and at the end.
        // TODO Calculate resultDigitsLength more accurately based on the number
        // TODO of digits in each operand, to make the number of loops as few as possible.
        const int resultDigitsLen = _MaxDigits + 2;
        byte[] resultDigits = new byte[resultDigitsLen];

        // Determine the largest possible exponent of the result.
        int resultExponent = Max(ld1.Exponent, ld2.Exponent) + 1;

        // Check if one number is much greater than the other.
        int smallestExponentChecked = resultExponent - resultDigitsLen + 1;
        if (smallestExponentChecked > ld2.Exponent)
        {
            return ld1;
        }
        if (smallestExponentChecked > ld1.Exponent)
        {
            return ld2;
        }

        // Get the digits.
        byte[] ld1Digits = UnpackDigits(ld1.Significand);
        byte[] ld2Digits = UnpackDigits(ld2.Significand);

        // Add
        for (int i = 1; i < resultDigitsLen; i++)
        {
            // What power of ten are we looking at?
            int currentExponent = resultExponent - i;

            // Get the indexes we need to check the digit arrays.
            int index1 = ld1.Exponent - currentExponent;
            int index2 = ld2.Exponent - currentExponent;

            // If both are beyond the limits of the digit arrays, we can stop
            // here.
            if (index1 >= ld1Digits.Length && index2 >= ld2Digits.Length)
            {
                break;
            }

            // Get the digit from ld1.
            byte digit1 = index1 >= 0 && index1 < ld1Digits.Length
                ? ld1Digits[index1]
                : (byte)0;

            // Get the digit from ld2.
            byte digit2 = index2 >= 0 && index2 < ld2Digits.Length
                ? ld2Digits[index2]
                : (byte)0;

            // Add the two digits.
            int sum = digit1 + digit2;

            // Check if we need to carry.
            if (sum >= 10)
            {
                resultDigits[i - 1]++;
                sum -= 10;
            }

            // Add the sum to the result.
            resultDigits[i] += (byte)sum;
        }

        // Construct the result object.
        return new LongDecimal(resultDigits, resultExponent);
    }

    public static LongDecimal operator +(LongDecimal ld1, LongDecimal ld2) =>
        Add(ld1, ld2);

    public static LongDecimal Subtract(LongDecimal ld1, LongDecimal ld2)
    {
        // Optimizations.
        if (ld2.IsZero)
        {
            return ld1;
        }
        if (ld1.IsZero)
        {
            return -ld2;
        }
        if (ld1 == ld2)
        {
            return Zero();
        }

        // Defer to Add() if possible.
        if (ld1.IsNegative && ld2.IsPositive)
        {
            return -Add(-ld1, ld2);
        }
        if (ld1.IsPositive && ld2.IsNegative)
        {
            return Add(ld1, -ld2);
        }

        // Units negatives to positives.
        if (ld1.IsNegative && ld2.IsNegative)
        {
            return Subtract(-ld2, -ld1);
        }

        // Subtract smaller from larger.
        if (ld2 > ld1)
        {
            return -Subtract(ld2, ld1);
        }

        // Do the subtraction. With the above checks, we know that we are
        // subtracting a smaller positive number (ld2) from a larger positive
        // number (ld1), which will give a positive number.

        // Create an array for the result. Leave a spare slot at the end.
        // TODO Calculate resultDigitsLength more accurately based on the number
        // TODO of digits in each operand, to make the number of loops as few as possible.
        const int resultDigitsLen = _MaxDigits + 1;
        byte[] resultDigits = new byte[resultDigitsLen];

        // Determine the exponent of the result.
        int resultExponent = Max(ld1.Exponent, ld2.Exponent);

        // Check if one number is much greater than the other.
        int smallestExponentChecked = resultExponent - resultDigitsLen + 1;
        if (smallestExponentChecked > ld2.Exponent)
        {
            return ld1;
        }
        if (smallestExponentChecked > ld1.Exponent)
        {
            return ld2;
        }

        // Get the digits.
        byte[] ld1Digits = UnpackDigits(ld1.Significand);
        byte[] ld2Digits = UnpackDigits(ld2.Significand);

        int carry = 0;

        // Subtract.
        for (int i = resultDigitsLen - 1; i >= 0; i--)
        {
            int currentExponent = resultExponent - i;

            // Get the digit from ld1.
            int index1 = ld1.Exponent - currentExponent;
            int digit1 = index1 >= 0 && index1 < ld1Digits.Length
                ? ld1Digits[index1]
                : 0;

            // Get the digit from ld2, adding the carry.
            int index2 = ld2.Exponent - currentExponent;
            int digit2 = (index2 >= 0 && index2 < ld2Digits.Length
                ? ld2Digits[index2]
                : 0) + carry;

            // Subtract the two digits. Borrow 10 if needed.
            bool takeTen = digit1 < digit2;
            if (takeTen)
            {
                digit1 += 10;
            }
            resultDigits[i] = (byte)(digit1 - digit2);

            // Carry one for the next loop if needed.
            carry = takeTen ? 1 : 0;
        }

        // Construct the result object.
        return new LongDecimal(resultDigits, resultExponent);
    }

    public static LongDecimal operator -(LongDecimal ld1, LongDecimal ld2) =>
        Subtract(ld1, ld2);

    public static LongDecimal Multiply(LongDecimal ld1, LongDecimal ld2)
    {
        // Optimizations.
        if (ld1 == 0 || ld2 == 0)
        {
            return 0;
        }
        if (ld1 == 1)
        {
            return ld2;
        }
        if (ld2 == 1)
        {
            return ld1;
        }

        // Get the digits.
        byte[] digits1 = UnpackDigits(ld1.Significand);
        byte[] digits2 = UnpackDigits(ld2.Significand);

        // Get the minimum exponent for each operand.
        int maxExp1 = ld1.Exponent;
        int maxExp2 = ld2.Exponent;

        // There could be a trailing zero in either of the digits array if there
        // was an odd number of digits in the significand, which is possible
        // because 2 digits are stored per byte. Trim them off.
        digits1 = TrimZeros(digits1, ref maxExp1);
        digits2 = TrimZeros(digits2, ref maxExp2);

        // Get the minimum exponent for each operand.
        int minExp1 = maxExp1 - digits1.Length + 1;
        int minExp2 = maxExp2 - digits2.Length + 1;

        // Get the min and max possible exponents for the result.
        // The max exponent will be the exponent.
        int maxExpResult = maxExp1 + maxExp2 + 1;
        int minExpResult = minExp1 + minExp2;

        // Create an array to hold the result digits.
        byte[] digitsResult = new byte[maxExpResult - minExpResult + 1];

        for (int i1 = 0; i1 < digits1.Length; i1++)
        {
            // Get the left-hand digit and exponent.
            byte digit1 = digits1[i1];
            int exp1 = maxExp1 - i1;
            for (int i2 = 0; i2 < digits2.Length; i2++)
            {
                // Get the right-hand digit and exponent.
                byte digit2 = digits2[i2];
                int exp2 = maxExp2 - i2;

                // Look for shortcut.
                if (digit1 == 0 || digit2 == 0)
                {
                    continue;
                }

                // Multiply.
                int product = digit1 * digit2;
                int expResult = exp1 + exp2;

                // Add to result.
                int indexResult = maxExpResult - expResult;
                digitsResult[indexResult] += (byte)(product % 10);
                digitsResult[indexResult - 1] += (byte)(product / 10);

                // Check for overflow. Start at the current digit and work back.
                for (int j = indexResult; j >= 1; j--)
                {
                    if (digitsResult[j] >= 10)
                    {
                        digitsResult[j] -= 10;
                        digitsResult[j - 1]++;
                    }
                    else if (j <= indexResult - 2)
                    {
                        break;
                    }
                }
            } // for i2
        } // for i1

        // Get the sign of the result.
        bool isNegative = ld1.IsPositive != ld2.IsPositive;

        return new LongDecimal(digitsResult, maxExpResult, isNegative);
    }

    public static LongDecimal operator *(LongDecimal ld1, LongDecimal ld2) =>
        Multiply(ld1, ld2);

    #endregion Arithmetic methods

    #region Cast methods

    public static implicit operator LongDecimal(int i) =>
        FromString(i.ToString());

    public static implicit operator LongDecimal(decimal m) =>
        FromString(m.ToString(CultureInfo.InvariantCulture));

    #endregion Cast methods

    #region Helper methods

    public static byte[] UnpackDigits(byte[] significand)
    {
        byte[] digits = new byte[significand.Length * 2];
        for (int i = 0; i < significand.Length; i++)
        {
            byte b = significand[i];
            int j = i * 2;
            digits[j] = (byte)(b >> 4);
            digits[j + 1] = (byte)(b & 0b00001111);
        }

        return digits;
    }

    public static byte[] PackDigits(byte[] digits)
    {
        int nBytes = digits.Length / 2 + digits.Length % 2;
        byte[] significand = new byte[nBytes];
        for (int i = 0; i < nBytes; i++)
        {
            int j = i * 2;
            int high = digits[j];
            int low = j + 1 < digits.Length ? digits[j + 1] : 0;
            significand[i] = (byte)(high * 16 + low);
        }

        return significand;
    }

    /// <summary>
    /// Round off an array of digits to MaxDigits digits.
    /// The rounding strategy employed is MidpointRounding.ToEven, the same as
    /// for Math.Round().
    /// </summary>
    /// <param name="digits"></param>
    /// <returns>A new byte array containing maximum MaxDigits digits.</returns>
    public static byte[] Round(byte[] digits)
    {
        if (digits.Length <= _MaxDigits)
        {
            // No rounding to do, so just return a copy of the array.
            return digits[..];
        }

        // Trim excess digits.
        byte[] result = digits[.._MaxDigits];
        // See if we need to round up the last digit.
        if (digits[_MaxDigits] > 5 || digits[_MaxDigits] == 5 && digits[_MaxDigits - 1] % 2 == 1)
        {
            result[^1]++;
        }
        return result;
    }

    public bool FlagIsSet(Flag flag) =>
        (Flags & (byte)(1 << (byte)flag)) != 0;

    /// <summary>
    /// Check if an array of bytes contains only zeros.
    /// Can be used for the significand or an array of digits.
    /// </summary>
    /// <returns></returns>
    private static bool AllZeros(IEnumerable<byte> bytes) =>
        bytes.All(b => b == 0);

    /// <summary>
    /// Trim any leading or trailing zeros, adjusting the exponent if needed.
    /// </summary>
    /// <param name="digits"></param>
    /// <param name="exponent"></param>
    /// <returns></returns>
    public static byte[] TrimZeros(byte[] digits, ref int exponent)
    {
        // Find the first and last non-zero bytes in the array.
        int? first = null;
        int? last = null;
        for (int i = 0; i < digits.Length; i++)
        {
            if (digits[i] != 0)
            {
                first ??= i;
                last = i;
            }
        }

        // Check for zero.
        if (first is null)
        {
            exponent = 0;
            return Array.Empty<byte>();
        }

        // Adjust the exponent.
        exponent -= first.Value;

        // Copy the bytes into the result without the leading or trailing zeros.
        return digits[first.Value..(last!.Value + 1)];
    }

    #endregion Helper methods
}
