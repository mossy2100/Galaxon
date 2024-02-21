using System.Numerics;

namespace Galaxon.Core.Numbers;

/// <summary>
/// LINQ methods for IEnumerable{INumberBase}.
/// </summary>
public static class EnumerableNumberExtensions
{
    /// <summary>
    /// Get the sum of all values in the collection, transformed by a function, if provided.
    /// </summary>
    public static T Sum<T>(this IEnumerable<T> source, Func<T, T>? f = null)
        where T : INumberBase<T>
    {
        return source.Aggregate(T.AdditiveIdentity, (sum, num) => sum + (f == null ? num : f(num)));
    }

    /// <summary>
    /// Get the product of all values in the collection, transformed by a function, if provided.
    /// </summary>
    public static T Product<T>(this IEnumerable<T> source, Func<T, T>? f = null)
        where T : INumberBase<T>
    {
        return source.Aggregate(T.MultiplicativeIdentity,
            (prod, num) => prod * (f == null ? num : f(num)));
    }

    /// <summary>
    /// Given a collection of T values, get the average (i.e. the arithmetic mean).
    /// </summary>
    /// <see href="https://en.wikipedia.org/wiki/Arithmetic_mean"/>
    public static double Average<T>(this IEnumerable<T> source) where T : INumberBase<T>
    {
        List<T> nums = source.ToList();

        // Guard.
        if (nums.Count == 0)
        {
            throw new ArithmeticException("At least one value must be provided.");
        }

        // Get the sum as a double.
        double sum = (double)Convert.ChangeType(nums.Sum(), typeof(double));

        // Calculate the average.
        return sum / nums.Count;
    }

    /// <summary>
    /// Given a collection of T numbers, get the geometric mean.
    /// </summary>
    /// <see href="https://en.wikipedia.org/wiki/Geometric_mean"/>
    public static double GeometricMean<T>(this IEnumerable<T> source) where T : INumberBase<T>
    {
        List<T> nums = source.ToList();

        // Make sure there's at least one value.
        if (nums.Count == 0)
        {
            throw new ArithmeticException("At least one value must be provided.");
        }

        // Ensure all values are non-negative.
        if (nums.Any(T.IsNegative))
        {
            throw new ArithmeticException("All values must be non-negative.");
        }

        // Get the product as a double.
        double product = (double)Convert.ChangeType(nums.Product(), typeof(double));

        // Calculate the geometric mean.
        return double.RootN(product, nums.Count);
    }
}
