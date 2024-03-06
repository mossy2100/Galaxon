using System.Globalization;

namespace Galaxon.Time;

/// <summary>
/// Extension methods for the DateTime class.
/// </summary>
public static class DateTimeExtensions
{
    #region Properties

    /// <summary>
    /// Get the current DateTime in UTC.
    /// </summary>
    public static DateTime NowUtc => new (DateTime.Now.Ticks, DateTimeKind.Utc);

    #endregion Properties

    #region Formatting

    /// <summary>
    /// Format the date using ISO format YYYY-MM-DDThh:mm:ss.
    /// This format is useful for databases.
    /// The time zone is not shown. For that, call ToString() with the format specifier "U" or
    /// UniversalSortableDateTimePattern.
    /// See
    /// <see href="https://learn.microsoft.com/en-us/dotnet/api/system.datetime.tostring?view=net-7.0"/>
    /// </summary>
    /// <param name="date">The DateTime instance.</param>
    /// <returns>A string representing the datetime in ISO format.</returns>
    public static string ToIsoString(this DateTime date)
    {
        DateTimeFormatInfo dtfi = new ();
        return date.ToString(dtfi.SortableDateTimePattern);
    }

    #endregion Formatting

    #region Methods for addition and subtraction

    /// <summary>
    /// Add a number of weeks to a DateTime to get a new DateTime.
    /// </summary>
    /// <param name="dt">A DateTime.</param>
    /// <param name="weeks">The number of weeks to add.</param>
    /// <returns></returns>
    public static DateTime AddWeeks(this DateTime dt, double weeks)
    {
        return dt.AddDays(weeks * TimeConstants.DAYS_PER_WEEK);
    }

    #endregion Methods for addition and subtraction

    #region Extract date and time parts

    /// <summary>
    /// Get the date part of a DateTime as a DateOnly object.
    /// An alternative to the Date property, which returns a DateTime.
    /// </summary>
    /// <see cref="DateTime.Date"/>
    /// <param name="dt">The DateTime.</param>
    /// <returns>The date part of the DateTime.</returns>
    public static DateOnly GetDateOnly(this DateTime dt)
    {
        return DateOnly.FromDateTime(dt);
    }

    /// <summary>
    /// Get the time of day part of a DateTime as a TimeOnly object.
    /// An alternative to the TimeOfDay property, which returns a TimeSpan.
    /// </summary>
    /// <see cref="DateTime.TimeOfDay"/>
    /// <param name="dt">The DateTime.</param>
    /// <returns>The time of day part of the DateTime.</returns>
    public static TimeOnly GetTimeOnly(this DateTime dt)
    {
        return TimeOnly.FromTimeSpan(dt.TimeOfDay);
    }

    #endregion Extract date and time parts

    #region Methods for getting the instant as a count of time units

    /// <summary>
    /// Get the total number of seconds from the start of the epoch to the datetime.
    /// </summary>
    /// <param name="dt">The DateTime instance.</param>
    /// <returns>The number of seconds since the epoch start.</returns>
    public static double GetTotalSeconds(this DateTime dt)
    {
        return (double)dt.Ticks / TimeSpan.TicksPerSecond;
    }

    /// <summary>
    /// Get the total number of days from the start of the epoch to the datetime.
    /// </summary>
    /// <param name="dt">The DateTime instance.</param>
    /// <returns>The number of days since the epoch start.</returns>
    public static double GetTotalDays(this DateTime dt)
    {
        return (double)dt.Ticks / TimeSpan.TicksPerDay;
    }

    /// <summary>
    /// Get the number of years between the start of the epoch and the start of the date.
    /// </summary>
    /// <param name="dt">The DateTime instance.</param>
    /// <returns>The number of years since the epoch start.</returns>
    public static double GetTotalYears(this DateTime dt)
    {
        return (double)dt.Ticks / TimeConstants.TICKS_PER_YEAR;
    }

    #endregion Methods for getting the instant as a count of time units

    #region Create new object

    /// <summary>
    /// Create a new DateTime given the number of seconds since the start of the epoch.
    /// </summary>
    /// <param name="seconds">The number of seconds.</param>
    /// <returns>A new DateTime object.</returns>
    public static DateTime FromTotalSeconds(double seconds)
    {
        var ticks = (long)Math.Round(seconds * TimeSpan.TicksPerSecond);
        return new DateTime(ticks, DateTimeKind.Utc);
    }

    /// <summary>
    /// Create a new DateTime given the number of days since the start of the epoch.
    /// </summary>
    /// <param name="days">
    /// The number of days. May include a fractional part indicating the time of day.
    /// </param>
    /// <returns>A new DateTime object.</returns>
    public static DateTime FromTotalDays(double days)
    {
        var ticks = (long)Math.Round(days * TimeSpan.TicksPerDay);
        return new DateTime(ticks, DateTimeKind.Utc);
    }

    /// <summary>
    /// Create a new DateTime given the number of years since the start of the epoch.
    /// </summary>
    /// <param name="years">The number of years. May include a fractional part.</param>
    /// <returns>A new DateTime object.</returns>
    public static DateTime FromTotalYears(double years)
    {
        var ticks = (long)Math.Round(years * TimeConstants.TICKS_PER_YEAR);
        return new DateTime(ticks, DateTimeKind.Utc);
    }

    #endregion Create new object
}
