using System.Numerics;
using Galaxon.Core.Functional;

namespace Galaxon.Numerics.Integers;

public static class Partitions
{
    /// <summary>
    /// Memoized version of P().
    /// </summary>
    public static readonly Func<ushort, BigInteger> P =
        Memoization.Memoize<ushort, BigInteger>(_P);

    /// <summary>
    /// P() function using a recurrence relation.
    /// The number of unique ways an integer can be partitioned into smaller integers.
    /// The argument is limited to ushort to avoid out-of-memory errors.
    /// </summary>
    /// <see href="https://en.wikipedia.org/wiki/Partition_function_(number_theory)#Recurrence_relations"/>
    /// <param name="n">The number to partition.</param>
    /// <returns>The number of partitions.</returns>
    public static BigInteger _P(ushort n)
    {
        // Terminating condition.
        if (n == 0)
        {
            return 1;
        }

        // Get the signs and arguments so we can calculate the smaller partitions first.
        // This will avoid a stack overflow.
        // The results of P() are cached due to memoization of the method.
        List<(int sign, ushort arg)> terms = new ();
        var k = 1;
        var sign = 1;
        while (true)
        {
            // Positive k term.
            int arg = n - k * (3 * k - 1) / 2;
            if (arg < 0)
            {
                break;
            }
            terms.Add((sign, (ushort)arg));

            // Negative k term.
            arg = n - k * (3 * k + 1) / 2;
            if (arg < 0)
            {
                break;
            }
            terms.Add((sign, (ushort)arg));

            // Go to next pair.
            k++;
            sign = -sign;
        }

        // Sum the terms from smallest to largest.
        BigInteger sum = 0;
        for (int i = terms.Count - 1; i >= 0; i--)
        {
            sum += terms[i].sign * P(terms[i].arg);
        }
        return sum;
    }
}
