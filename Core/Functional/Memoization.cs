namespace Galaxon.Core.Functional;

/// <summary>
/// Enables memoization of pure functions by remembering previous results.
/// </summary>
public static class Memoization
{
    /// <summary>
    /// Set to true if you want to check if cache is being used to get a result.
    /// </summary>
    public static bool DebugMode { get; set; } = false;

    /// <summary>
    /// Enables caching of the results of unary pure functions.
    /// </summary>
    /// <param name="f">The pure function.</param>
    /// <typeparam name="T">The input type.</typeparam>
    /// <typeparam name="TResult">The result type.</typeparam>
    /// <returns>The memoized version of the pure function.</returns>
    public static Func<T, TResult> Memoize<T, TResult>(Func<T, TResult> f) where T : notnull
    {
        Dictionary<T, TResult> cache = new ();
        return x =>
        {
            if (cache.TryGetValue(x, out TResult? result))
            {
                if (DebugMode)
                {
                    Console.WriteLine("Result obtained from cache.");
                }

                return result;
            }

            // Compute the result.
            result = f(x);

            if (DebugMode)
            {
                Console.WriteLine("Result not obtained from cache.");
            }

            // Add it to the cache.
            cache.Add(x, result);

            // Return the result.
            return result;
        };
    }

    /// <summary>
    /// Enables caching of the results of binary pure functions.
    /// </summary>
    /// <param name="f">The pure function.</param>
    /// <typeparam name="T1">First argument type.</typeparam>
    /// <typeparam name="T2">Second argument type.</typeparam>
    /// <typeparam name="TResult">Result type.</typeparam>
    /// <returns>The memoized version of the pure function.</returns>
    public static Func<T1, T2, TResult> Memoize<T1, T2, TResult>(Func<T1, T2, TResult> f)
    {
        Dictionary<(T1, T2), TResult> cache = new ();
        return (x, y) =>
        {
            // Check the cache.
            if (cache.TryGetValue((x, y), out TResult? result))
            {
                if (DebugMode)
                {
                    Console.WriteLine("Result obtained from cache.");
                }

                return result;
            }

            // Compute the result.
            result = f(x, y);
            if (DebugMode)
            {
                Console.WriteLine("Result not obtained from cache.");
            }

            // Add it to the cache.
            cache.Add((x, y), result);

            // Return the result.
            return result;
        };
    }
}
