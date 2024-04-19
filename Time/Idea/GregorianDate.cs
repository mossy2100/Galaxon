namespace Galaxon.Time.Idea;

public struct GregorianDate
{
    /// <summary>
    /// The date is stored internally as a Julian Day Number (integer) not a Julian Date (float).
    /// A Julian Day Number, if converted to a float and used as a Julian Date, would refer to noon
    /// on that day.
    /// </summary>
    private long JulianDayNumber;

    #region Operators

    /// <summary>
    /// Add a date and time to get a datetime.
    /// </summary>
    /// <param name="date"></param>
    /// <param name="time"></param>
    /// <returns></returns>
    public static GregorianDateTime operator +(GregorianDate date, TimeSpan time)
    {
        return new GregorianDateTime(date, time);
    }

    /// <summary>
    /// Add a date and time to get a datetime.
    /// </summary>
    /// <param name="date"></param>
    /// <param name="time"></param>
    /// <returns></returns>
    public static GregorianDateTime operator +(GregorianDate date, TimeOnly time)
    {
        return new GregorianDateTime(date, time.ToTimeSpan());
    }

    #endregion Operators
}
