using System.Text;

namespace Galaxon.Numerics.Integers;

public static class NumberStrings
{
    /// <summary>
    /// Convert a long value to words.
    /// </summary>
    public static string NumberToWords(long n)
    {
        long quotient, remainder;
        string quotientStr, remainderStr;

        switch (n)
        {
            // We can't get the negative of long.MinValue as a long (see case < 0 below), so let's
            // just hard code the result for that one.
            // long.MinValue == -9,223,372,036,854,775,808
            case long.MinValue:
                return "negative nine quintillion, "
                    + "two hundred and twenty-three quadrillion, "
                    + "three hundred and seventy-two trillion, "
                    + "thirty-six billion, "
                    + "eight hundred and fifty-four million, "
                    + "seven hundred and seventy-five thousand, "
                    + "eight hundred and eight";

            case < 0:
                return $"negative {NumberToWords(-n)}";

            case < 20:
                return n switch
                {
                    0 => "zero",
                    1 => "one",
                    2 => "two",
                    3 => "three",
                    4 => "four",
                    5 => "five",
                    6 => "six",
                    7 => "seven",
                    8 => "eight",
                    9 => "nine",
                    10 => "ten",
                    11 => "eleven",
                    12 => "twelve",
                    13 => "thirteen",
                    14 => "fourteen",
                    15 => "fifteen",
                    16 => "sixteen",
                    17 => "seventeen",
                    18 => "eighteen",
                    19 => "nineteen",
                    // ReSharper disable once UnreachableSwitchArmDueToIntegerAnalysis
                    _ => "?"
                };

            case < 100:
                quotient = n / 10;
                remainder = n % 10;
                quotientStr = quotient switch
                {
                    2 => "twenty",
                    3 => "thirty",
                    4 => "forty",
                    5 => "fifty",
                    6 => "sixty",
                    7 => "seventy",
                    8 => "eighty",
                    9 => "ninety",
                    // Silence the code analyser about the non-exhaustive switch.
                    _ => throw new Exception()
                };
                remainderStr = remainder > 0 ? $"-{NumberToWords(remainder)}" : "";
                return $"{quotientStr}{remainderStr}";

            case < 1000:
                quotient = n / 100;
                remainder = n % 100;
                quotientStr = $"{NumberToWords(quotient)} hundred";
                remainderStr = remainder > 0 ? $" and {NumberToWords(remainder)}" : "";
                return $"{quotientStr}{remainderStr}";

            default:
                Dictionary<long, string> multiplierWords = new ()
                {
                    { 1000, "thousand" },
                    { 1_000_000, "million" },
                    { 1_000_000_000, "billion" },
                    { 1_000_000_000_000, "trillion" },
                    { 1_000_000_000_000_000, "quadrillion" },
                    { 1_000_000_000_000_000_000, "quintillion" }
                };
                var multiplier = 1_000_000_000_000_000_000;
                while (n < multiplier)
                {
                    multiplier /= 1000;
                }
                quotient = n / multiplier;
                remainder = n % multiplier;
                quotientStr = $"{NumberToWords(quotient)} {multiplierWords[multiplier]}";
                remainderStr = remainder > 0 ? $", {NumberToWords(remainder)}" : "";
                return $"{quotientStr}{remainderStr}";
        }
    }

    /// <summary>
    /// Count all the letters in a string.
    /// </summary>
    /// <param name="s"></param>
    /// <returns></returns>
    public static int LetterCount(string s)
    {
        return s.Count(char.IsLetter);
    }

    public static int GetAlphabeticalValue(string name)
    {
        return name.Sum(c => c - (char.IsLower(c) ? 'a' : 'A') + 1);
    }

    /// <summary>
    /// Calculate the inverse of an integer, showing reptend in brackets.
    /// </summary>
    /// <param name="x">An integer greater than 1.</param>
    /// <param name="reptend">The reptend, if any.</param>
    /// <returns>The inverse as a string.</returns>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    public static string Inverse(long x, out string? reptend)
    {
        if (x < 2)
        {
            throw new ArgumentOutOfRangeException(nameof(x),
                "Method only supports values greater than 1.");
        }

        StringBuilder decimals = new ();
        long n = 1;
        long d = x;
        var current = 0;
        Dictionary<(long n, long d), int> seen = new ();
        reptend = null;

        while (true)
        {
            // Get the quotient.
            long q = n / d;

            // Check for a repeat of this quotient. If we've seen it before, the pattern of digits
            // will repeat, indicating a reptend.
            if (seen.TryGetValue((n, d), out int reptendStart))
            {
                // Found a repeat.
                int reptendLen = current - reptendStart;
                var decimalString = decimals.ToString();
                reptend = decimalString[^reptendLen..];
                return $"0.{decimalString[1..reptendStart]}({reptend})";
            }

            // Add the digit and record the numerator and denominator that produced it, and where.
            decimals.Append(q);
            seen[(n, d)] = current;

            // Get the remainder. If it's 0, we're done.
            long r = n % d;
            if (r == 0)
            {
                return $"0.{decimals.ToString()[1..]}";
            }

            // The remainder is the new numerator.
            n = r * 10;

            // Go to the next digit.
            current++;
        }
    }
}
