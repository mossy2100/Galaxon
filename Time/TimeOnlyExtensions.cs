namespace Galaxon.Time;

/// <summary>
/// Extension methods for TimeOnly.
/// </summary>
public static class TimeOnlyExtensions
{
    #region Methods for converting to a TimeSpan

    /// <summary>
    /// Convert a TimeOnly to a TimeSpan.
    /// </summary>
    /// <param name="time">The TimeOnly instance.</param>
    /// <returns>The new TimeSpan object</returns>
    public static TimeSpan ToTimeSpan(this TimeOnly time)
    {
        return new TimeSpan(time.Ticks);
    }

    #endregion Methods for converting to a TimeSpan

    #region Methods for addition and subtraction

    /// <summary>
    /// Subtract a start time from an end time to find the difference.
    /// </summary>
    /// <param name="end">The end time.</param>
    /// <param name="start">The start time.</param>
    /// <returns>The time elapsed.</returns>
    public static TimeSpan Subtract(this TimeOnly end, TimeOnly start)
    {
        return new TimeSpan(end.Ticks - start.Ticks);
    }

    #endregion Methods for addition and subtraction
}
