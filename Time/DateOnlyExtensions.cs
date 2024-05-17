using System.Globalization;

namespace Galaxon.Time;

/// <summary>
/// Extension methods for the DateOnly class.
/// </summary>
public static class DateOnlyExtensions
{
    #region Properties

    /// <summary>
    /// Get today's date.
    /// </summary>
    public static DateOnly Today => DateOnly.FromDateTime(DateTime.Now);

    #endregion Properties

    #region Formatting

    /// <summary>
    /// Format the date using ISO 8601 format YYYY-MM-DD.
    /// <see href="https://en.wikipedia.org/wiki/ISO_8601#Calendar_dates"/>
    /// </summary>
    /// <param name="date">The DateOnly instance.</param>
    /// <returns>A string representing the date in ISO format.</returns>
    public static string ToIsoString(this DateOnly date)
    {
        return date.ToString("O");
    }

    #endregion Formatting

    #region Methods for converting to a DateTime

    /// <summary>
    /// Convert a DateOnly to a DateTime, with default time 00:00:00.
    /// </summary>
    /// <param name="date">The DateOnly instance.</param>
    /// <returns>The new DateTime object</returns>
    public static DateTime ToDateTime(this DateOnly date)
    {
        return date.ToDateTime(new TimeOnly(0));
    }

    /// <summary>
    /// Convert a DateOnly to a DateTime, with default time 00:00:00 and specified DateTimeKind.
    /// </summary>
    /// <param name="date">The DateOnly instance.</param>
    /// <param name="kind">The DateTimeKind.</param>
    /// <returns>The new DateTime object</returns>
    public static DateTime ToDateTime(this DateOnly date, DateTimeKind kind)
    {
        return date.ToDateTime(new TimeOnly(0), kind);
    }

    #endregion Methods for converting to a DateTime

    #region Methods for getting the instant as a count of time units

    // These methods treat the DateOnly as an instant (the start of the given date in UTC), which of
    // course it isn't. But they are still useful.

    /// <summary>
    /// Get the number of ticks between the start of the epoch (0001-01-01 00:00:00) and the start
    /// of the date.
    /// If extension properties are added to the language I may change this to a property "Ticks"
    /// later, for consistency with DateTime.
    /// </summary>
    /// <see href="https://learn.microsoft.com/en-us/dotnet/api/system.datetime.ticks?view=net-7.0"/>
    /// <param name="date">The DateOnly instance.</param>
    /// <returns>The number of ticks.</returns>
    public static long GetTicks(this DateOnly date)
    {
        return date.ToDateTime().Ticks;
    }

    /// <summary>
    /// Get the number of seconds between the start of the epoch and the start of the date.
    /// </summary>
    /// <param name="date">The DateOnly instance.</param>
    /// <returns>The number of seconds since the epoch start.</returns>
    public static long GetTotalSeconds(this DateOnly date)
    {
        return date.GetTicks() / TimeSpan.TicksPerSecond;
    }

    /// <summary>
    /// Get the number of days between the start of the epoch and the given date.
    /// </summary>
    /// <param name="date">The DateOnly instance.</param>
    /// <returns>The number of days since the epoch start.</returns>
    public static int GetTotalDays(this DateOnly date)
    {
        return (int)(date.GetTicks() / TimeSpan.TicksPerDay);
    }

    /// <summary>
    /// Get the number of years between the start of the epoch and the start of the date.
    /// The result will be greater than or equal to `date.Year - 1` and less than `date.Year`.
    /// </summary>
    /// <param name="date">The DateOnly instance.</param>
    /// <returns>The number of years since the epoch start.</returns>
    public static double GetTotalYears(this DateOnly date)
    {
        return (double)date.GetTicks() / TimeConstants.TICKS_PER_YEAR;
    }

    #endregion Methods for getting the instant as a count of time units

    #region Methods for addition and subtraction

    /// <summary>
    /// Add a period of time to a date to find a new DateTime.
    /// </summary>
    /// <see cref="DateTime.Add(TimeSpan)"/>
    /// <param name="date">The date.</param>
    /// <param name="period">The time period to add.</param>
    /// <returns>The resulting DateTime.</returns>
    public static DateTime Add(this DateOnly date, TimeSpan period)
    {
        return date.ToDateTime().Add(period);
    }

    /// <summary>
    /// Add a time of day to a date to find a new DateTime.
    /// </summary>
    /// <param name="date">The date.</param>
    /// <param name="time">The time of day to add.</param>
    /// <returns>The resulting DateTime.</returns>
    public static DateTime Add(this DateOnly date, TimeOnly time)
    {
        return date.ToDateTime(time);
    }

    /// <summary>
    /// Add a number of weeks to a date.
    /// </summary>
    /// <param name="date">The date.</param>
    /// <param name="weeks">The number of weeks to add.</param>
    /// <returns>The resulting date.</returns>
    public static DateOnly AddWeeks(this DateOnly date, int weeks)
    {
        return date.AddDays(weeks * TimeConstants.DAYS_PER_WEEK);
    }

    /// <summary>
    /// Returns the difference between two dates as a number of days.
    /// Emulates the <see cref="DateTime.Subtract(DateTime)"/> method.
    /// If the end date is later than the start date, the result will be positive.
    /// If they are equal, the result will be zero. Otherwise, the result will be negative.
    /// </summary>
    /// <param name="end">The end date.</param>
    /// <param name="start">The start date.</param>
    /// <returns>The number of days difference between the two dates.</returns>
    public static int Subtract(this DateOnly end, DateOnly start)
    {
        return end.GetTotalDays() - start.GetTotalDays();
    }

    #endregion Methods for addition and subtraction

    #region Create new object

    /// <summary>
    /// Find a date given the number of days from the start of the epoch.
    /// </summary>
    /// <see cref="DateTimeExtensions.FromTotalDays(double)"/>
    /// <param name="days">The number of days.</param>
    /// <returns>The resulting date.</returns>
    public static DateOnly FromTotalDays(long days)
    {
        return DateOnly.FromDateTime(DateTimeExtensions.FromTotalDays(days));
    }

    /// <summary>
    /// Find the date given the number of years since the start of the epoch.
    /// </summary>
    /// <param name="years">The number of years. May include a fractional part.</param>
    /// <returns>The resulting date.</returns>
    public static DateOnly FromTotalYears(double years)
    {
        return DateOnly.FromDateTime(DateTimeExtensions.FromTotalYears(years));
    }

    /// <summary>
    /// Find a date given a year and the day of the year.
    /// Formula from Meeus (Astronomical Algorithms 2 ed. p66).
    /// </summary>
    /// <param name="year">The year (1..9999).</param>
    /// <param name="dayOfYear">The day of the year (1..366).</param>
    /// <returns>The resulting date.</returns>
    public static DateOnly FromDayOfYear(int year, int dayOfYear)
    {
        // Check year is in the valid range.
        if (year is < 1 or > 9999)
        {
            throw new ArgumentOutOfRangeException(nameof(year), "Must be in the range 1..9999");
        }

        // Check day of year is in the valid range.
        GregorianCalendar gc = GregorianCalendarUtility.GetInstance();
        var daysInYear = gc.GetDaysInYear(year);
        if (dayOfYear < 1 || dayOfYear > daysInYear)
        {
            throw new ArgumentOutOfRangeException(nameof(dayOfYear),
                $"Must be in the range 1..{daysInYear}");
        }

        // Calculate.
        var k = gc.IsLeapYear(year) ? 1 : 2;
        var month = dayOfYear < 32 ? 1 : (int)(9 * (k + dayOfYear) / 275.0 + 0.98);
        var day = dayOfYear - (int)(275 * month / 9.0) + k * (int)((month + 9) / 12.0) + 30;

        return new DateOnly(year, month, day);
    }

    #endregion Create new object

    #region Month names

    /// <summary>
    /// Get the month name of a date in the specified language.
    /// </summary>
    /// <param name="d">The date.</param>
    /// <param name="languageCode">The language code, e.g. "fr", "es"</param>
    /// <returns>The month name.</returns>
    public static string GetMonthName(this DateOnly d, string languageCode = "en")
    {
        return GregorianCalendarUtility.MonthNumberToName(d.Month, languageCode);
    }

    #endregion Month names
}
