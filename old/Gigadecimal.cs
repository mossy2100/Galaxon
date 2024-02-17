using System.Text;
using System.Text.RegularExpressions;
using Galaxon.Core.Exceptions;
using Galaxon.Core.Numbers;
using DecimalMath;

namespace Galaxon.Numerics.Types;

/// <summary>
/// Software implementation of large decimal floating point value.
///
/// In this file:
/// - "B" means 1 billion.
/// - "gigadecimal" means base 1 billion (c.f. decimal, hexadecimal, sexagesimal).
/// - A "gigadigit" is a "gigadecimal digit".
///
/// The number has two parts: a significand and a exponent.
/// The maximum total size in memory is 60 bytes:
///   Significand: pointer (8 bytes) plus up to 12 ints (48 bytes)
///   Exponent: 1 int (4 bytes)
///
/// Significand
/// ===========
/// The maximum size of the significand is 12 ints or 48 bytes, which can encode up to 108 decimal
/// digits.
/// Each int encodes 1 gigadigit, equal to 9 digits, or a range of 0..999,999,999.
/// The int at index 0 in the array thus encodes the first 9 digits of the decimal significand, the
/// int at index 1 encodes the second 9 digits, etc.
/// Each int in the significand also includes the sign. The overall sign of the number will be the
/// same for each gigadigit in the significand.
///
/// Exponent
/// ========
/// 4 bytes (1 int). Note, this is not a power of 10 but a power of B.
/// The decimal exponent equals the value stored in the byte (the gigadecimal exponent) * 9.
/// The gigadecimal exponent range is -2,147,483,648 to 2,147,483,647, which corresponds to a decimal
/// exponent range of -19,327,352,832 to 19,327,352,823.
/// Each number conceptually represents a value in scientific notation, i.e. having the form
/// [-]9.999.. * 10^[-]999.. (or [-]9.999..e[-]9.999..) except the base is gigadecimal, i.e. the
/// value is found from
/// S[0] * B^M + S[1] * B^(M - 1) + S[2] * B^(M - 2)...
/// where S is the significand, B is 10^9, and M is the exponent.
/// </summary>
public class Gigadecimal : IEquatable<Gigadecimal>, IComparable<Gigadecimal>
{
    #region Constants

    private const int _B = 1_000_000_000;

    private const int _DigitsPerGigadigit = 9;

    private const byte _MaxLength = 12;

    #endregion Constants

    #region Properties

    public int[] Significand { get; init; }

    public int Exponent { get; init; }

    public int Sign => GetSign(Significand);

    public bool IsZero => Length == 0;

    public bool IsOne => Length == 1 && Significand[0] == 1 && Exponent == 0;

    public bool IsNegative => Sign == -1;

    public bool IsPositive => Sign == 1;

    public int Length => Significand.Length;

    #endregion Properties

    #region Constructors and creation methods

    // /// <summary>
    // /// Protected constructor.
    // /// Constructs a new object with a preset number of gigadigits.
    // /// Used for preparing result objects for calculations.
    // /// </summary>
    // /// <param name="length"></param>
    // /// <param name="exponent"></param>
    // protected Gigadecimal(int length, int exponent = 0)
    // {
    //     Significand = new int[length];
    //     Exponent = exponent;
    // }

    /// <summary>
    /// Construct from properties.
    /// </summary>
    public Gigadecimal(int[]? significand = null, int exponent = 0)
    {
        if (significand == null)
        {
            Significand = Array.Empty<int>();
        }
        else
        {
            significand = TrimLeadingZeros(significand, ref exponent);
            significand = Round(significand, ref exponent);
            Significand = TrimTrailingZeros(significand, ref exponent);
        }

        Exponent = exponent;
    }

    /// <summary>
    /// Construct from string.
    /// TODO Implement this functionality as Parse() and TryParse(), from the IParseable interface.
    /// </summary>
    public Gigadecimal(string s)
    {
        // Default to 0.
        if (string.IsNullOrWhiteSpace(s) || s == "0")
        {
            Significand = Array.Empty<int>();
            Exponent = 0;
            return;
        }

        Regex rx = new (@"^(-?)(\d+)(\.(\d+))?(e([-+]?\d+))?$", RegexOptions.IgnoreCase);
        MatchCollection matches = rx.Matches(s);

        // Check we got a match.
        if (matches.Count != 1)
        {
            throw new ArgumentFormatException(nameof(s),
                "Invalid string format for long decimal value.");
        }

        GroupCollection groups = matches[0].Groups;

        // Get the sign.
        string strSign = groups[1].Value;
        int sign = strSign == "-" ? -1 : 1;

        // Get the digits.
        string strDigitsBeforeDecimal = groups[2].Value;
        string strDigitsAfterDecimal = groups[4].Value;
        string strDigits = strDigitsBeforeDecimal + strDigitsAfterDecimal;

        // Get the decimal exponent.
        string strExponent = groups[6].Value;
        int decExponent = (strExponent == "" ? 0 : int.Parse(strExponent))
            + strDigitsBeforeDecimal.Length - 1;

        // Trim zeros and adjust exponent as needed.
        strDigits = TrimDigits(strDigits, ref decExponent);

        // Calculate number of decimal places to shift.
        int gigExponent = (int)Floor(decExponent / (decimal)_DigitsPerGigadigit);
        int nShift = decExponent - gigExponent * _DigitsPerGigadigit;
        int nLeadingZeros = _DigitsPerGigadigit - 1 - nShift;
        int nDigitsWithLeadingZeros = nLeadingZeros + strDigits.Length;
        int nGigadigits = (int)Ceiling(nDigitsWithLeadingZeros / (decimal)_DigitsPerGigadigit);
        int nTrailingZeros = nGigadigits * _DigitsPerGigadigit - nDigitsWithLeadingZeros;
        strDigits += new string('0', nTrailingZeros);

        // Create an array to store the significand.
        // int sigLen = strDigits.Length / DigitsPerGigadigit;
        int[] significand = new int[nGigadigits];
        for (int i = 0; i < nGigadigits; i++)
        {
            int start = i * _DigitsPerGigadigit - nLeadingZeros;
            int end = start + _DigitsPerGigadigit;
            if (start < 0)
            {
                start = 0;
            }
            significand[i] = sign * int.Parse(strDigits[start..end]);
        }

        Significand = Round(significand, ref gigExponent);
        Exponent = gigExponent;
    }

    public static Gigadecimal Clone(Gigadecimal gd) =>
        new (gd.Significand, gd.Exponent);

    public static Gigadecimal Zero() =>
        new ();

    public static Gigadecimal One() =>
        new (new[] { 1 });

    public static Gigadecimal MinValue() =>
        new (new[]
        {
            -999999999, -999999999, -999999999, -999999999, -999999999, -999999999,
            -999999999, -999999999, -999999999, -999999999, -999999999, -999999999
        }, int.MaxValue);

    /// <summary>
    /// Minimum (largest negative) value.
    /// </summary>
    /// <returns></returns>
    public static Gigadecimal MaxValue() =>
        new (new[]
        {
            999999999, 999999999, 999999999, 999999999, 999999999, 999999999,
            999999999, 999999999, 999999999, 999999999, 999999999, 999999999
        }, int.MaxValue);

    /// <summary>
    /// Smallest positive value. B^-(2^31)
    /// </summary>
    /// <returns></returns>
    public static Gigadecimal Epsilon() =>
        new (new[] { 1 }, int.MinValue);

    #endregion Constructors and creation methods

    #region Overridden methods

    public override bool Equals(object? obj) =>
        obj is Gigadecimal gd && Equals(gd);

    public override int GetHashCode() =>
        HashCode.Combine(Significand, Exponent);

    public override string ToString()
    {
        // Optimization.
        if (Length == 0)
        {
            return "0";
        }

        // Sign.
        string strSign = IsNegative ? "-" : "";

        // Decimal exponent.
        int decExponent = Exponent * _DigitsPerGigadigit + (_DigitsPerGigadigit - 1);

        // Decimal significand.
        StringBuilder sbSignificand = new ();
        foreach (int gigadigit in Significand)
        {
            sbSignificand.Append(Math.Abs(gigadigit).ToString().PadLeft(_DigitsPerGigadigit, '0'));
        }
        string strSignificand = TrimDigits(sbSignificand.ToString(), ref decExponent);
        if (strSignificand.Length > 1)
        {
            strSignificand = $"{strSignificand[0]}.{strSignificand[1..]}";
        }

        // Create a substring for the exponent.
        string strExponent = decExponent switch
        {
            < 0 => $"E{decExponent}",
            > 0 => $"E+{decExponent}",
            _ => ""
        };

        return $"{strSign}{strSignificand}{strExponent}";
    }

    #endregion Overridden methods

    #region Interface methods

    public bool Equals(Gigadecimal? other)
    {
        if (other is null)
        {
            return false;
        }

        // Compare flags and exponent.
        if (IsNegative != other.IsNegative || Exponent != other.Exponent)
        {
            return false;
        }

        // Compare number of gigadigits.
        if (Length != other.Length)
        {
            return false;
        }

        // Compare the gigadigits in the significand.
        for (int i = 0; i < Length; i++)
        {
            if (Significand[i] != other.Significand[i])
            {
                return false;
            }
        }

        return true;
    }

    /// <summary>
    /// </summary>
    /// <param name="other"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    public int CompareTo(Gigadecimal? other)
    {
        if (other is null)
        {
            throw new ArgumentNullException(nameof(other), "Cannot compare a Gigadecimal and null.");
        }

        // Compare signs.
        if (Sign < other.Sign)
        {
            return -1;
        }
        if (Sign > other.Sign)
        {
            return 1;
        }

        // Compare exponents.
        if (Exponent < other.Exponent)
        {
            return -1;
        }
        if (Exponent > other.Exponent)
        {
            return 1;
        }

        // Compare significands.
        int maxLen = Max(Length, other.Length);
        for (int i = 0; i < maxLen; i++)
        {
            int left = i < Length ? Significand[i] : 0;
            int right = i < other.Length ? other.Significand[i] : 0;
            if (left < right)
            {
                return -1;
            }
            if (left > right)
            {
                return 1;
            }
        }

        // The numbers are equal.
        return 0;
    }

    #endregion Interface methods

    #region Comparison operators

    public static bool operator ==(Gigadecimal left, Gigadecimal right) =>
        left.Equals(right);

    public static bool operator !=(Gigadecimal left, Gigadecimal right) =>
        !left.Equals(right);

    public static bool operator <(Gigadecimal left, Gigadecimal right) =>
        left.CompareTo(right) < 0;

    public static bool operator >(Gigadecimal left, Gigadecimal right) =>
        left.CompareTo(right) > 0;

    public static bool operator <=(Gigadecimal left, Gigadecimal right) =>
        left.CompareTo(right) is < 0 or 0;

    public static bool operator >=(Gigadecimal left, Gigadecimal right) =>
        left.CompareTo(right) is > 0 or 0;

    #endregion Comparison operators

    #region Arithmetic methods

    public static Gigadecimal Abs(Gigadecimal gd)
    {
        // Make every gigadigit in the significand positive.
        int[] significand = gd.Significand.Select(Math.Abs).ToArray();
        return new Gigadecimal(significand, gd.Exponent);
    }

    /// <summary>
    /// Negation method.
    /// </summary>
    /// <param name="gd"></param>
    /// <returns></returns>
    public static Gigadecimal Negate(Gigadecimal gd)
    {
        // Negate every gigadigit in the significand.
        int[] significand = gd.Significand.Select(i => -i).ToArray();
        return new Gigadecimal(significand, gd.Exponent);
    }

    /// <summary>
    /// Negation operator (unary minus).
    /// </summary>
    /// <param name="gd"></param>
    /// <returns></returns>
    public static Gigadecimal operator -(Gigadecimal gd) =>
        Negate(gd);

    public static Gigadecimal Add(Gigadecimal gd1, Gigadecimal gd2)
    {
        // Optimizations.
        if (gd1.IsZero)
        {
            return gd2;
        }
        if (gd2.IsZero)
        {
            return gd1;
        }
        if (gd1 == -gd2)
        {
            return Zero();
        }

        // Determine the min and max exponents of the operands.
        int maxExp1 = gd1.Exponent;
        int len1 = gd1.Length;
        int minExp1 = maxExp1 - len1 + 1;

        int maxExp2 = gd2.Exponent;
        int len2 = gd2.Length;
        int minExp2 = maxExp2 - len2 + 1;

        // Determine the min and max exponents of the result.
        // Add 1 extra to the max in case of overflow.
        int maxExp3 = Max(maxExp1, maxExp2) + 1;
        int minExp3 = Min(minExp1, minExp2);
        int len3 = maxExp3 - minExp3 + 1;

        // TODO Check for overflow/underflow here.

        // Cap at maximum length. Add 1 extra at the beginning to allow for
        // overflow and 1 at the end for rounding.
        const int maxLen2 = _MaxLength + 2;
        if (len3 > maxLen2)
        {
            len3 = maxLen2;
            minExp3 = maxExp3 - len3 + 1;
        }

        // If one number is much greater than the other, and no digits overlap,
        // nothing to add.
        if (minExp3 > maxExp2)
        {
            return gd1;
        }
        if (minExp3 > maxExp1)
        {
            return gd2;
        }

        // Create array for the result.
        int[] significand3 = new int[len3];

        // Loop through the result digits, adding.
        for (int index3 = 1; index3 < len3; index3++)
        {
            // What exponent are we looking at?
            int exp = maxExp3 - index3;

            // Get the gigadigit from gd1.
            int gigadigit1 = GetGigadigit(gd1.Significand, maxExp1 - exp);

            // Get the gigadigit from gd2.
            int gigadigit2 = GetGigadigit(gd2.Significand, maxExp2 - exp);

            // Add the two gigadigits and place in the result.
            SetGigadigit(significand3, index3, gigadigit1 + gigadigit2);
        }

        // Check for zero
        if (GetSign(significand3) == 0)
        {
            return Zero();
        }

        // Fix over/underflow.
        FixOverflow(ref significand3, ref maxExp3);

        // Construct the result object.
        return new Gigadecimal(significand3, maxExp3);
    }

    public static Gigadecimal operator +(Gigadecimal gd1, Gigadecimal gd2) =>
        Add(gd1, gd2);

    public static Gigadecimal Subtract(Gigadecimal gd1, Gigadecimal gd2) =>
        Add(gd1, -gd2);

    public static Gigadecimal operator -(Gigadecimal gd1, Gigadecimal gd2) =>
        Subtract(gd1, gd2);

    public static Gigadecimal Multiply(Gigadecimal gd1, Gigadecimal gd2)
    {
        // Optimizations.
        if (gd1.IsZero || gd2.IsZero)
        {
            return Zero();
        }
        if (gd1.IsOne)
        {
            return gd2;
        }
        if (gd2.IsOne)
        {
            return gd1;
        }

        // Determine the min and max exponents of the operands.
        int maxExp1 = gd1.Exponent;
        int len1 = gd1.Length;
        int minExp1 = maxExp1 - len1 + 1;

        int maxExp2 = gd2.Exponent;
        int len2 = gd2.Length;
        int minExp2 = maxExp2 - len2 + 1;

        // Determine the min and max exponents of the result.
        // Add 1 extra to the max in case of overflow.
        int maxExp3 = maxExp1 + maxExp2 + 1;
        int minExp3 = minExp1 + minExp2;
        int len3 = maxExp3 - minExp3 + 1;

        // Cap at maximum length. Add 1 extra at the beginning to allow for
        // overflow and 1 at the end for rounding.
        if (len3 > _MaxLength + 2)
        {
            len3 = _MaxLength + 2;
        }

        // Initialise result array.
        int[] significand3 = new int[len3];

        for (int index1 = 0; index1 < gd1.Length; index1++)
        {
            // Get the left-hand digit and exponent.
            int gigadigit1 = gd1.Significand[index1];
            int exp1 = maxExp1 - index1;
            for (int index2 = 0; index2 < gd2.Length; index2++)
            {
                // Get the right-hand digit and exponent.
                int gigadigit2 = gd2.Significand[index2];
                int exp2 = maxExp2 - index2;

                // Shortcut.
                if (gigadigit1 == 0 || gigadigit2 == 0)
                {
                    continue;
                }

                // Multiply.
                long product = (long)gigadigit1 * gigadigit2;
                int exp3 = exp1 + exp2;

                // Add to result.
                int index3 = maxExp3 - exp3;
                List<int> indices = Enumerable.Range(0, significand3.Length).ToList();
                bool index3InRange = indices.Contains(index3);
                bool index3Less1InRange = indices.Contains(index3 - 1);
                if (index3InRange)
                {
                    significand3[index3] += (int)(product % _B);
                }
                if (index3Less1InRange)
                {
                    significand3[index3 - 1] += (int)(product / _B);
                }

                // Check for overflow.
                if (index3InRange)
                {
                    FixOverflow(ref significand3, ref maxExp3, index3);
                }
                else if (index3Less1InRange)
                {
                    FixOverflow(ref significand3, ref maxExp3, index3 - 1);
                }
            } // for index2
        } // for index1

        return new Gigadecimal(significand3, maxExp3);
    }

    public static Gigadecimal operator *(Gigadecimal gd1, Gigadecimal gd2) =>
        Multiply(gd1, gd2);

    /// <summary>
    /// Computes the division using the Goldschmidt algorithm.
    /// <see href="https://en.wikipedia.org/wiki/Division_algorithm#Goldschmidt_division" />
    /// </summary>
    /// <param name="numerator"></param>
    /// <param name="denominator"></param>
    /// <returns></returns>
    /// <exception cref="DivideByZeroException"></exception>
    public static Gigadecimal Divide(Gigadecimal numerator, Gigadecimal denominator)
    {
        // Guard.
        if (denominator.IsZero)
        {
            throw new DivideByZeroException("Division by 0 is undefined.");
        }

        // Shortcuts.
        if (denominator.IsOne)
        {
            return numerator;
        }
        if (numerator == denominator)
        {
            return One();
        }

        // Get the approximate result exponent.
        int resultExponent = numerator.Exponent - denominator.Exponent;

        // Remove the exponents from the operands. This enables us to calculate
        // f using the decimal type.
        Gigadecimal n = new (numerator.Significand);
        Gigadecimal d = new (denominator.Significand);

        // Get a good initial estimate of the multiplication factor.
        Gigadecimal f = 1m / (decimal)d;

        // Get 2 as a Gigadecimal to avoid doing the conversion every time.
        Gigadecimal two = 2;

        while (true)
        {
            n = Multiply(n, f);
            d = Multiply(d, f);
            // If the denominator is 1, then n is the result.
            if (d.IsOne)
            {
                break;
            }

            f = Subtract(two, d);

            // If d is not 1, but is close to 1, then f can be 1 due to
            // rounding after the subtraction. If it is, there's no point
            // continuing.
            if (f.IsOne)
            {
                break;
            }
        }

        return new Gigadecimal(n.Significand, n.Exponent + resultExponent);
    }

    public static Gigadecimal operator /(Gigadecimal gd1, Gigadecimal gd2) =>
        Divide(gd1, gd2);

    /// <summary>
    /// Compute the reciprocal of the given value.
    /// </summary>
    /// <param name="gd"></param>
    /// <returns></returns>
    public static Gigadecimal Reciprocal(Gigadecimal gd) =>
        Divide(One(), gd);

    #endregion Arithmetic methods

    #region Conversion methods and cast operators

    /// <summary>
    /// Construct from ulong.
    /// </summary>
    /// <param name="l">An unsigned long value.</param>
    public static Gigadecimal FromUInt64(ulong l)
    {
        ulong rem = l;
        List<int> gigadigits = new ();
        while (rem > 0)
        {
            int gigadigit = (int)(rem % _B);
            gigadigits.Insert(0, gigadigit);
            rem /= _B;
        }

        return new Gigadecimal(gigadigits.ToArray(), gigadigits.Count - 1);
    }

    /// <summary>
    /// Construct from long.
    /// </summary>
    /// <param name="l">A signed long value.</param>
    public static Gigadecimal FromInt64(long l) =>
        l < 0 ? -FromUInt64(XUlong.Abs(l)) : FromUInt64((ulong)l);

    public static Gigadecimal FromDecimal(decimal d)
    {
        if (d == 0)
        {
            return Zero();
        }

        bool isNegative = d < 0;
        decimal abs = Math.Abs(d);
        int exponent = (int)decimal.Floor(XDecimal.Log(abs, _B));
        decimal rem;

        switch (exponent)
        {
            // Shift the decimal to something reasonable so we don't end up generating a bunch of
            // zeros.
            case < 0:
            {
                // Cap the exponent so Pow() doesn't overflow.
                if (exponent < -3)
                {
                    exponent = -3;
                }
                rem = abs * DecimalEx.Pow(_B, -exponent);
                break;
            }

            case > 0:
            {
                // Cap the exponent so Pow() doesn't overflow.
                if (exponent > 3)
                {
                    exponent = 3;
                }
                rem = abs / DecimalEx.Pow(_B, exponent);
                break;
            }

            default:
                // exponent = 0
                rem = abs;
                break;
        }

        int[] gigadigits = new int[_MaxLength];
        int cur = 0;

        while (rem > 0)
        {
            int gigadigit = (int)Floor(rem);
            gigadigits[cur++] = gigadigit;
            decimal frac = rem - gigadigit;
            rem = frac * _B;
        }

        Gigadecimal gd = new (gigadigits, exponent);
        return isNegative ? -gd : gd;
    }

    /// <summary>
    /// Convert a Gigadecimal to a decimal.
    /// NB: May involve loss of precision.
    /// </summary>
    /// <param name="gd"></param>
    /// <returns></returns>
    /// <exception cref="OverflowException">
    /// If the magnitude of the Gigadecimal
    /// is too large to be represented by a decimal.
    /// </exception>
    public static decimal ToDecimal(Gigadecimal gd)
    {
        decimal m = 0;
        decimal scale = 0;

        for (int i = 0; i < gd.Length; i++)
        {
            try
            {
                // Get the scale for the gigadigit.
                scale = i == 0
                    ? DecimalEx.Pow(_B, gd.Exponent)
                    : scale / _B;
                // Add the gigadigit value.
                m += gd.Significand[i] * scale;
            }
            catch (OverflowException ex)
            {
                throw new OverflowException("The number is too large to convert to a decimal.", ex);
            }
        }

        return m;
    }

    public static Gigadecimal FromDouble(double d)
    {
        if (d == 0)
        {
            return Zero();
        }

        bool isNegative = d < 0;
        double abs = Math.Abs(d);
        int exponent = (int)Floor(Log(abs, _B));
        double rem;

        // Shift the decimal to something reasonable so we don't waste time// generating a bunch of
        // 0s.
        switch (exponent)
        {
            case < 0:
            {
                // Cap the exponent so Pow() doesn't overflow.
                if (exponent < -34)
                {
                    exponent = -34;
                }
                rem = abs * Pow(_B, -exponent);
                break;
            }

            case > 0:
            {
                // Cap the exponent so Pow() doesn't overflow.
                if (exponent > 34)
                {
                    exponent = 34;
                }
                rem = abs / Pow(_B, exponent);
                break;
            }

            default:
                // exponent = 0
                rem = abs;
                break;
        }

        int[] gigadigits = new int[_MaxLength + 1];
        int cur = 0;

        while (rem > 0)
        {
            int gigadigit = (int)Floor(rem);
            gigadigits[cur++] = gigadigit;
            if (cur == gigadigits.Length)
            {
                break;
            }
            double frac = rem - gigadigit;
            rem = frac * _B;
        }

        Gigadecimal gd = new (gigadigits, exponent);
        return isNegative ? -gd : gd;
    }

    /// <summary>
    /// Convert a Gigadecimal to a double.
    /// NB: May involve loss of precision.
    /// </summary>
    /// <param name="gd"></param>
    /// <returns></returns>
    /// <exception cref="OverflowException">
    /// If the magnitude of the Gigadecimal
    /// is too large to be represented by a double.
    /// </exception>
    public static double ToDouble(Gigadecimal gd) =>
        double.Parse(gd.ToString());

    public static implicit operator Gigadecimal(ulong l) =>
        FromUInt64(l);

    public static implicit operator Gigadecimal(long l) =>
        FromInt64(l);

    public static implicit operator Gigadecimal(uint i) =>
        FromUInt64(i);

    public static implicit operator Gigadecimal(int i) =>
        FromInt64(i);

    public static implicit operator Gigadecimal(decimal m) =>
        FromDecimal(m);

    public static explicit operator decimal(Gigadecimal gd) =>
        ToDecimal(gd);

    public static implicit operator Gigadecimal(double d) =>
        FromDouble(d);

    public static explicit operator double(Gigadecimal gd) =>
        ToDouble(gd);

    #endregion Conversion methods and cast operators

    #region Helper methods

    /// <summary>
    /// Round off an array of gigadigits to MaxGigadigits gigadigits.
    /// The rounding strategy employed is MidpointRounding.ToEven, the same as
    /// for Math.Round().
    /// </summary>
    /// <param name="significand"></param>
    /// <param name="exponent"></param>
    /// <returns>A new array containing maximum MaxGigadigits gigadigits.</returns>
    public static int[] Round(int[] significand, ref int exponent)
    {
        if (significand.Length <= _MaxLength)
        {
            // No rounding to do, so just return a copy of the array.
            return significand[..];
        }

        const int midpoint = 500000000;

        // Trim excess digits.
        int[] result = significand[.._MaxLength];

        // See if we need to round up the last digit.
        if (significand[_MaxLength] > midpoint ||
            significand[_MaxLength] == midpoint && significand[_MaxLength - 1] % 2 == 1)
        {
            result[^1]++;
            FixOverflow(ref result, ref exponent);

            // If an extra digit was prepended due to overflow, the last one
            // must be 0. Lop it off to get the right number of gigadigits.
            if (result.Length > _MaxLength)
            {
                result = result[.._MaxLength];
            }
        }

        return result;
    }

    public static Gigadecimal Round(Gigadecimal gd)
    {
        int[] significand = gd.Significand;
        int exponent = gd.Exponent;
        significand = Round(significand, ref exponent);
        return new Gigadecimal(significand, exponent);
    }

    /// <summary>
    /// Trim leading zeros, adjusting the exponent if needed.
    /// </summary>
    /// <param name="significand">The array of gigadigits.</param>
    /// <param name="exponent">The exponent of the first gigadigit.</param>
    /// <returns>A new array of gigadigits with leading zeros removed.</returns>
    public static int[] TrimLeadingZeros(int[] significand, ref int exponent)
    {
        // Find the first non-zero int in the array.
        int? first = null;
        for (int i = 0; i < significand.Length; i++)
        {
            if (significand[i] != 0)
            {
                first = i;
                break;
            }
        }

        // Check for zero.
        if (first == null)
        {
            exponent = 0;
            return Array.Empty<int>();
        }

        // Adjust the exponent.
        exponent -= first.Value;

        // Copy the gigadigits into the result without the leading zeros.
        return significand[first.Value..];
    }

    /// <summary>
    /// Trim trailing zeros.
    /// </summary>
    /// <param name="significand">The array of gigadigits.</param>
    /// <param name="exponent">The exponent of the first gigadigit.</param>
    /// <returns>A new array of gigadigits with trailing zeros removed.</returns>
    public static int[] TrimTrailingZeros(int[] significand, ref int exponent)
    {
        // Find the last non-zero int in the array.
        int? last = null;
        for (int i = significand.Length - 1; i >= 0; i--)
        {
            if (significand[i] != 0)
            {
                last = i;
                break;
            }
        }

        // Check for zero.
        if (last == null)
        {
            exponent = 0;
            return Array.Empty<int>();
        }

        // Copy the bytes into the result without the trailing zeros.
        return significand[..(last.Value + 1)];
    }

    /// <summary>
    /// Trim any leading or trailing zeros, adjusting the exponent if needed.
    /// Same as above but for strings of decimal digits.
    /// </summary>
    /// <param name="digits">The string of decimal digits.</param>
    /// <param name="exponent">The max power of 10.</param>
    /// <returns></returns>
    private static string TrimDigits(string digits, ref int exponent)
    {
        // Find the first and last non-zero chars in the array.
        int? first = null;
        int? last = null;
        for (int i = 0; i < digits.Length; i++)
        {
            if (digits[i] != '0')
            {
                first ??= i;
                last = i;
            }
        }

        // Check for zero.
        if (first == null)
        {
            exponent = 0;
            return "";
        }

        // Adjust the exponent.
        exponent -= first.Value;

        // Copy the chars into the result without the leading or trailing zeros.
        return digits[first.Value..(last!.Value + 1)];
    }

    public static int GetSign(int[] significand)
    {
        // Look for the first non-zero digit.
        foreach (int gigadigit in significand)
        {
            switch (gigadigit)
            {
                case > 0:
                    return 1;

                case < 0:
                    return -1;
            }
        }

        return 0;
    }

    public static void FixOverflow(ref int[] significand, ref int exponent, int? startIndex = null)
    {
        // Get the min and max valid values for the gigadigits.
        int sign = GetSign(significand);
        int min = sign == 1 ? 0 : -_B + 1;
        int max = sign == 1 ? _B - 1 : 0;

        // Spare gigadigit if needed for overflow.
        int? extraGigadigit = null;

        // Set start index at the right-most gigadigit if not provided.
        startIndex ??= significand.Length - 1;

        // Check gigadigits for over/underflow. Go right to left.
        for (int i = startIndex.Value; i >= 0; i--)
        {
            if (significand[i] < min)
            {
                significand[i] += _B;
                if (i - 1 < 0)
                {
                    extraGigadigit = -1;
                }
                else
                {
                    significand[i - 1]--;
                }
            }
            else if (significand[i] > max)
            {
                significand[i] -= _B;
                if (i - 1 < 0)
                {
                    extraGigadigit = 1;
                }
                else
                {
                    significand[i - 1]++;
                }
            }
        }

        // Prepend the extra gigadigit if necessary.
        if (extraGigadigit.HasValue)
        {
            int[] temp = significand;
            significand = new int[temp.Length + 1];
            significand[0] = extraGigadigit.Value;
            Array.Copy(temp, 0, significand, 1, temp.Length);
            exponent++;
        }
    }

    public static int GetGigadigit(int[] significand, int index) =>
        index >= 0 && index < significand.Length ? significand[index] : 0;

    public static void SetGigadigit(int[] significand, int index, int value)
    {
        if (index >= 0 && index < significand.Length)
        {
            significand[index] = value;
        }
    }

    #endregion Helper methods
}
