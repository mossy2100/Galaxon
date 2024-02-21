using Galaxon.Core.Exceptions;

namespace Galaxon.Numerics.Integers;

public static class Compare
{
    /// <summary>
    /// Get the maximum value from a series of values.
    /// Normally this method would be used with numbers, but it can be used for any IComparable.
    /// In C# 11 we might be able to use INumber, but then it couldn't be used with strings or other
    /// IComparable.
    /// </summary>
    /// <param name="values">A series of values.</param>
    /// <returns>The maximum value.</returns>
    /// <exception cref="ArgumentInvalidException">If no values were provided.</exception>
    public static T Max<T>(params T[] values) where T : IComparable
    {
        switch (values.Length)
        {
            case 0:
                throw new ArgumentInvalidException(nameof(values),
                    "At least one value must be provided.");

            case 1:
                return values[0];

            default:
                T max = values[0];
                for (var i = 1; i < values.Length; i++)
                {
                    if (values[i].CompareTo(max) == 1)
                    {
                        max = values[i];
                    }
                }
                return max;
        }
    }

    /// <summary>
    /// Get the minimum value from a series of values.
    /// Normally this method would be used with numbers, but it can be used for
    /// any IComparable.
    /// </summary>
    /// <param name="values">A series of values.</param>
    /// <returns>The minimum value.</returns>
    /// <exception cref="ArgumentInvalidException">If no values were provided.</exception>
    public static T Min<T>(params T[] values) where T : IComparable
    {
        switch (values.Length)
        {
            case 0:
                throw new ArgumentInvalidException(nameof(values),
                    "At least one value must be provided.");

            case 1:
                return values[0];

            default:
                T min = values[0];
                for (var i = 1; i < values.Length; i++)
                {
                    if (values[i].CompareTo(min) == -1)
                    {
                        min = values[i];
                    }
                }
                return min;
        }
    }
}
