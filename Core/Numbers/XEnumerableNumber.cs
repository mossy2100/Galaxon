using System.Numerics;
using Galaxon.Core.Types;

namespace Galaxon.Core.Numbers;

/// <summary>
/// LINQ methods for IEnumerable{INumberBase}.
/// </summary>
public static class XEnumerableNumber
{
    /// <summary>
    /// Get the sum of all values in the collection, transformed by the supplied map function.
    /// If the map function is null or omitted, just get the sum of the values.
    /// </summary>
    public static T Sum<T>(this IEnumerable<T> source, Func<T, T>? map = null)
        where T : INumberBase<T>
    {
        return source.Aggregate(T.AdditiveIdentity,
            (sum, num) => sum + (map == null ? num : map(num)));
    }

    /// <summary>
    /// Get the product of all values in the collection, transformed by the supplied map function.
    /// If the map function is null or omitted, just get the product of the values.
    /// </summary>
    public static T Product<T>(this IEnumerable<T> source, Func<T, T>? map = null)
        where T : INumberBase<T>
    {
        return source.Aggregate(T.MultiplicativeIdentity,
            (prod, num) => prod * (map == null ? num : map(num)));
    }

    /// <summary>
    /// Given a collection of T values, get the average (i.e. the arithmetic mean).
    /// </summary>
    /// <see href="https://en.wikipedia.org/wiki/Arithmetic_mean"/>
    public static T Average<T>(this IEnumerable<T> source) where T : INumberBase<T>
    {
        var nums = source.ToList();

        // Guard.
        if (nums.Count == 0)
        {
            throw new ArithmeticException("At least one value must be provided.");
        }

        // Optimization.
        if (nums.Count == 1) return nums[0];

        // Get count as a T.
        var count = XReflection.Cast<int, T>(nums.Count);

        // Calculate the average.
        return nums.Sum() / count;
    }

    // /// <summary>
    // /// Given a collection of T values, get the geometric mean.
    // /// </summary>
    // /// <see href="https://en.wikipedia.org/wiki/Geometric_mean"/>
    // public static T GeometricMean<T>(this IEnumerable<T> source) where T : INumberBase<T>
    // {
    //     var nums = source.ToList();
    //
    //     // Make sure there's at least one value.
    //     if (nums.Count == 0)
    //     {
    //         throw new ArithmeticException("At least one value must be provided.");
    //     }
    //
    //     // Ensure all values are non-negative.
    //     if (nums.Any(x => x < 0))
    //     {
    //         throw new ArithmeticException("All values must be non-negative.");
    //     }
    //
    //     // Optimization.
    //     if (nums.Count == 1)
    //     {
    //         return nums[0];
    //     }
    //
    //     return T.RootN(nums.Product(), nums.Count);
    // }
}
