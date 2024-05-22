namespace Galaxon.Time.Extensions;

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
    /// Format the date using ISO format, including time zone.
    /// Useful for JSON responses and user feedback.
    ///
    /// You can also use this method for database records, but since datetime column types in
    /// databases often don't support time zones, you may prefer to use the "s" standard format
    /// specifier (i.e. ToString("s")).
    ///
    /// As per the link below, the time zone string will depends on the DateTime.Kind:
    ///     * If the time zone is unspecified (DateTime.Kind = DateTimeKind.Unspecified), nothing.
    ///     * For a UTC time (DateTime.Kind = DateTimeKind.Utc), a "Z" character.
    ///     * For the local time zone (DateTime.Kind = DateTimeKind.Local), a string containing the
    ///       local offset from UTC; e.g. "-07:00"
    /// </summary>
    /// <see href="https://learn.microsoft.com/en-us/dotnet/api/system.datetime.tostring?view=net-7.0"/>
    /// <see href="https://learn.microsoft.com/en-us/dotnet/standard/base-types/standard-date-and-time-format-strings#the-sortable-s-format-specifier"/>
    /// <see href="https://learn.microsoft.com/en-us/dotnet/standard/base-types/custom-date-and-time-format-strings#KSpecifier"/>
    /// <param name="dt">The DateTime instance.</param>
    /// <returns>A string representing the datetime in ISO format.</returns>
    public static string ToIsoString(this DateTime dt)
    {
        return dt.ToString("yyyy-MM-ddThh:mm:ssK");
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

    #region Rounding off

    /// <summary>
    /// Round off a DateTime to the nearest value equal to a multiple of the TimeSpan argument.
    /// </summary>
    /// <param name="dt">The original DateTime</param>
    /// <param name="ts">The time unit duration.</param>
    /// <returns>The rounded-off DateTime.</returns>
    public static DateTime Round(this DateTime dt, TimeSpan ts)
    {
        long ticks = (long)Math.Round((double)dt.Ticks / ts.Ticks) * ts.Ticks;
        return new DateTime(ticks, dt.Kind);
    }

    /// <summary>
    /// Round off a datetime to the nearest second.
    /// </summary>
    /// <param name="dt"></param>
    /// <returns>A new DateTime equal to the parameter rounded off to the nearest second.</returns>
    public static DateTime RoundToNearestSecond(this DateTime dt)
    {
        return Round(dt, TimeSpan.FromSeconds(1));
    }

    /// <summary>
    /// Round off a datetime to the nearest minute.
    /// </summary>
    /// <param name="dt"></param>
    /// <returns>A new DateTime equal to the parameter rounded off to the nearest minute.</returns>
    public static DateTime RoundToNearestMinute(this DateTime dt)
    {
        return Round(dt, TimeSpan.FromMinutes(1));
    }

    /// <summary>
    /// Round off a datetime to the nearest hour.
    /// </summary>
    /// <param name="dt"></param>
    /// <returns>A new DateTime equal to the parameter rounded off to the nearest hour.</returns>
    public static DateTime RoundToNearestHour(this DateTime dt)
    {
        return Round(dt, TimeSpan.FromHours(1));
    }

    /// <summary>
    /// Round off a datetime to the nearest midnight.
    /// </summary>
    /// <param name="dt"></param>
    /// <returns>A new DateTime equal to the parameter rounded off to the nearest midnight.</returns>
    public static DateTime RoundToNearestDay(this DateTime dt)
    {
        return Round(dt, TimeSpan.FromDays(1));
    }

    #endregion Rounding off

    #region Month name

    /// <summary>
    /// Get the month name of a datetime in the specified language.
    /// </summary>
    /// <param name="dt">The datetime.</param>
    /// <param name="languageCode">The language code, e.g. "fr", "es"</param>
    /// <returns>The month name.</returns>
    public static string GetMonthName(this DateTime dt, string languageCode = "en")
    {
        return GregorianCalendarUtility.MonthNumberToName(dt.Month, languageCode);
    }

    #endregion Month name
}
