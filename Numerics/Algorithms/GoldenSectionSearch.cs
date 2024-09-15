using Galaxon.Core.Exceptions;
using Galaxon.Core.Functional;
using Galaxon.Numerics.Extensions.FloatingPoint;

namespace Galaxon.Numerics.Algorithms;

public static class GoldenSectionSearch
{
    /// <summary>
    /// Find the extremum (minimum or maximum) of a function between or equal to one of two
    /// boundary inputs.
    /// </summary>
    /// <param name="func">The function. It should be memoized for best performance.</param>
    /// <param name="a">The lower boundary input value.</param>
    /// <param name="b">The upper boundary input value.</param>
    /// <param name="findMax">
    /// Boolean value:
    ///     - false if looking for a minimum
    ///     - true if looking for a maximum
    /// </param>
    /// <param name="tolerance">Maximum acceptable error in the result.</param>
    /// <returns>The input and output of the function at the extremum.</returns>
    private static (double, double) _FindExtremum(Func<double, double> func, double a, double b,
        bool findMax, double tolerance = 1e-5)
    {
        // Handle if a == b.
        if (a == b)
        {
            throw new ArgumentInvalidException(nameof(b),
                "The two boundary values must be different.");
        }

        // Make sure a < b.
        if (a > b)
        {
            (a, b) = (b, a);
        }

        // Loop until we get a good enough result.
        while (true)
        {
            // Compute the difference.
            double diff = b - a;

            // We done?
            if (diff <= tolerance)
            {
                break;
            }

            // Find intermediate point and set new boundary.
            double delta = diff / DoubleExtensions.GOLDEN_RATIO;
            double c = b - delta;
            double d = a + delta;
            double fc = func(c);
            double fd = func(d);
            if ((!findMax && fc < fd) || (findMax && fc > fd))
            {
                b = d;
            }
            else
            {
                a = c;
            }
        }

        // Take the better of a and b as the result.
        double fa = func(a);
        double fb = func(b);
        if ((!findMax && fa < fb) || (findMax && fa > fb))
        {
            return (a, fa);
        }
        else
        {
            return (b, fb);
        }
    }

    /// <summary>
    /// Find the extremum (minimum or maximum) of a function between or equal to one of two
    /// boundary inputs.
    /// </summary>
    /// <param name="func">The function.</param>
    /// <param name="a">The lower boundary input value.</param>
    /// <param name="b">The upper boundary input value.</param>
    /// <param name="findMax">
    /// Boolean value:
    ///     - false if looking for a minimum
    ///     - true if looking for a maximum
    /// </param>
    /// <param name="tolerance">Maximum acceptable error in the result.</param>
    /// <returns>The input and output of the function at the extremum.</returns>
    public static (double, double) FindExtremum(Func<double, double> func, double a, double b,
        bool findMax, double tolerance = 1e-5)
    {
        // Memoize the function so we can reuse evaluations and improve performance.
        Func<double, double> memoizedFunc = Memoization.Memoize(func);

        // Call the recursive function.
        return _FindExtremum(memoizedFunc, a, b, findMax, tolerance);
    }

    /// <summary>
    /// Find the minimum of a function between or equal to one of two boundary inputs.
    /// </summary>
    /// <param name="func">The function.</param>
    /// <param name="a">The lower boundary input value.</param>
    /// <param name="b">The upper boundary input value.</param>
    /// <param name="tolerance">Maximum acceptable error in the result.</param>
    /// <returns>The input and output of the function at the minimum.</returns>
    public static (double, double) FindMinimum(Func<double, double> func, double a, double b,
        double tolerance = 1e-5)
    {
        return FindExtremum(func, a, b, false, tolerance);
    }

    /// <summary>
    /// Find the maximum of a function between or equal to one of two boundary inputs.
    /// </summary>
    /// <param name="func">The function.</param>
    /// <param name="a">The lower boundary input value.</param>
    /// <param name="b">The upper boundary input value.</param>
    /// <param name="tolerance">Maximum acceptable error in the result.</param>
    /// <returns>The input and output of the function at the maximum.</returns>
    public static (double, double) FindMaximum(Func<double, double> func, double a, double b,
        double tolerance = 1e-5)
    {
        return FindExtremum(func, a, b, true, tolerance);
    }
}
