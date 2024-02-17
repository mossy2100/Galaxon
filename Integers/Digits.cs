using System.Numerics;
using Galaxon.Core.Numbers;
using Galaxon.Core.Numbers.Extensions;

namespace Galaxon.Numerics.Integers;

public static class Digits
{
    public static List<byte> GetDigits(long n)
    {
        return Abs(n).ToString().Select(c => (byte)(c - '0')).ToList();
    }

    public static List<byte> GetDistinctDigits(long n)
    {
        return GetDigits(n).Distinct().ToList();
    }

    public static bool IsPandigital(string digits)
    {
        // Sort the digits.
        char[] sortedDigits = digits.ToCharArray();
        Array.Sort(sortedDigits);
        var strSortedDigits = string.Join("", sortedDigits);

        // Check they look like 1, 2, 3...
        return !strSortedDigits.Where((t, i) => t != i + '1').Any();
    }

    public static bool IsPandigital(ulong digits)
    {
        return IsPandigital(digits.ToString());
    }

    public static List<ulong> GetRotations(long n)
    {
        List<ulong> result = new ();
        var nString = n.ToString();
        int nDigits = nString.Length;
        for (var i = 1; i < nDigits; i++)
        {
            string newString = nString[i..] + nString[..i];
            result.Add(ulong.Parse(newString));
        }
        return result;
    }

    public static BigInteger SumFactorialDigits(BigInteger n)
    {
        return n.ToString().Select(c => XBigInteger.Factorial(c - '0')).Sum();
    }

    /// <summary>
    /// Get the number of digits in the BigInteger.
    /// </summary>
    public static int NumDigits(BigInteger n)
    {
        // Avoid logarithm of 0.
        if (n == 0)
        {
            return 1;
        }

        // Avoid logarithm of a negative number.
        n = BigInteger.Abs(n);

        // Get the logarithm, which will be within 1 of the answer.
        double log = BigInteger.Log10(n);

        // Account for fuzziness in the double representation of the logarithm.
        double floor = Floor(log);
        double round = Round(log);
        double nDigits = floor + (round > floor && log.FuzzyEquals(round) ? 2 : 1);

        return (int)nDigits;
    }
}
