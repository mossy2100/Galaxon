using System.Numerics;
using Galaxon.Core.Functional;
using Galaxon.Numerics.Extensions;

namespace Galaxon.Numerics.Integers;

/// <summary>
/// Permutations and combinations.
/// </summary>
public static class Combinatorial
{
    #region Binomial coefficients

    /// <summary>Find a binomial coefficient.</summary>
    /// <param name="n">Set size/upper index.</param>
    /// <param name="k">Subset size/lower index.</param>
    /// <returns>The binomial coefficient.</returns>
    public static BigInteger BinomialCoefficient(int n, int k)
    {
        // Optimizations.
        if (k < 0 || k > n)
        {
            return 0;
        }
        if (k == 0 || k == n)
        {
            return 1;
        }

        // Take advantage of symmetry.
        k = int.Min(k, n - k);

        // Multiplicative formula.
        BigInteger c = 1;
        for (var i = 0; i < k; i++)
        {
            c = c * (n - i) / (i + 1);
        }
        return c;
    }

    #endregion Binomial coefficients

    #region Permutations

    /// <summary>
    /// Find the number of ways r items can be selected from a set of n items.
    /// The order of the items is important.
    /// i.e. If selecting 2 integers from a set {1, 2, 3, 4, 5}, the selection {1, 2} is different
    /// from the selection {2, 1}.
    /// </summary>
    /// <param name="n">Number of items to select from.</param>
    /// <param name="r">Number of items to select.</param>
    /// <returns></returns>
    public static BigInteger NumPermutations(long n, long r)
    {
        return BigIntegerExtensions.Factorial(n) / BigIntegerExtensions.Factorial(n - r);
    }

    /// <summary>
    /// Get all the different ways to select n items from a bag of items, considering order.
    /// </summary>
    public static List<List<T>> GetPermutations<T>(List<T> bag, int n)
    {
        List<List<T>> result = [];

        // There are 0 ways of selecting n items from a bag with fewer than n items.
        if (n > bag.Count)
        {
            return result;
        }

        // There is only 1 way of selecting 0 items from a bag, irrespective of how many items are
        // in the bag.
        if (n == 0)
        {
            result.Add([]);
            return result;
        }

        // There are n ways of selecting 1 item from a bag with n items.
        if (n == 1)
        {
            result.AddRange(bag.Select(t => new List<T> { t }));
            return result;
        }

        // Get all the ways of selecting n items from the bag.
        for (var i = 0; i < bag.Count; i++)
        {
            // Select an item from the bag.
            T item = bag[i];

            // Get the remainder of the bag.
            List<T> remainder = bag.GetRange(0, i);
            List<T> remainder2 = bag.GetRange(i + 1, bag.Count - i - 1);
            remainder.AddRange(remainder2);

            // Get all permutations of the remainder.
            List<List<T>> remPerms = GetPermutations(remainder, n - 1);
            foreach (List<T> remPerm in remPerms)
            {
                List<T> perm = [item];
                perm.AddRange(remPerm);
                result.Add(perm);
            }
        }

        return result;
    }

    /// <summary>
    /// Get all permutations of the given string of characters.
    /// </summary>
    /// <param name="chars">String of characters.</param>
    /// <returns>All possible permutations.</returns>
    private static List<string> _CharPermutations(string chars)
    {
        switch (chars.Length)
        {
            case 0:
                return [];

            case 1:
                return [chars];
        }

        HashSet<string> result = [];
        for (var i = 0; i < chars.Length; i++)
        {
            char ch = chars[i];
            string rem = chars[..i] + chars[(i + 1)..];
            List<string> permutations = CharPermutations(rem);
            foreach (string perm in permutations)
            {
                result.Add($"{ch}{perm}");
            }
        }
        return result.ToList();
    }

    /// <summary>
    /// Public memoized version of method.
    /// </summary>
    public static readonly Func<string, List<string>> CharPermutations =
        Memoization.Memoize<string, List<string>>(_CharPermutations);

    /// <summary>
    /// Sort the digits of the given number in ascending order.
    /// Returns a string rather than a new ulong to avoid loss of leading 0s.
    /// </summary>
    public static string SortDigits(ulong n)
    {
        char[] digits = n.ToString().ToCharArray();
        Array.Sort(digits);
        return new string(digits);
    }

    /// <summary>
    /// Checks to see if one number is a permutation of another.
    /// </summary>
    public static bool IsPermutationOf(ulong n, ulong m)
    {
        return SortDigits(n) == SortDigits(m);
    }

    #endregion Permutations

    #region Combinations

    /// <summary>
    /// Find the number of ways r items can be selected from a set of n items.
    /// The order of the items is unimportant.
    /// i.e. If selecting 2 integers from a set {1, 2, 3, 4, 5}, the selection {1, 2} is counted as
    /// equal to the selection {2, 1}.
    /// </summary>
    /// <param name="n">Number of items to select from.</param>
    /// <param name="r">Number of items to select.</param>
    /// <returns></returns>
    public static BigInteger NumCombinations(long n, long r)
    {
        return BigIntegerExtensions.Factorial(n) / (BigIntegerExtensions.Factorial(r) * BigIntegerExtensions.Factorial(n - r));
    }

    /// <summary>
    /// Get all the different ways to select n items from a bag of items, ignoring order.
    /// </summary>
    public static List<List<T>> GetCombinations<T>(List<T> bag, int n)
    {
        List<List<T>> result = [];

        // Combinations of 0 items is undefined.
        if (n == 0)
        {
            throw new ArgumentOutOfRangeException(nameof(n),
                "Number of items to select must be positive.");
        }

        // There is only one way of selecting n items from a bag with n items.
        if (n == bag.Count)
        {
            result.Add(bag.ToList());
            return result;
        }

        // There are zero ways of selecting n items from a bag with fewer than n items.
        if (n > bag.Count)
        {
            return result;
        }

        for (var i = 0; i < bag.Count; i++)
        {
            // If we only want 1 item, shortcut.
            if (n == 1)
            {
                List<T> newCombo = [bag[i]];
                result.Add(newCombo);
                continue;
            }

            // How many items left?
            int nRemainingItems = bag.Count - i;

            // If there are n items remaining, shortcut.
            if (nRemainingItems == n)
            {
                List<T> newCombo = bag.GetRange(i, n);
                result.Add(newCombo);
                continue;
            }

            // Get the bag with all items following the ith item.
            List<T> remainder = bag.GetRange(i + 1, nRemainingItems - 1);

            // Find all the ways of selecting n-1 items from the remainder.
            List<List<T>> remCombos = GetCombinations(remainder, n - 1);
            foreach (List<T> remCombo in remCombos)
            {
                List<T> newCombo = [bag[i]];
                newCombo.AddRange(remCombo);
                result.Add(newCombo);
            }
        }

        return result;
    }

    #endregion Combinations
}
