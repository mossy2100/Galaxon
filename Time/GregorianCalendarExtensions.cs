using System.Globalization;

namespace Galaxon.Time;

/// <summary>
/// Extension methods for the GregorianCalendar class, and other useful methods relating to the
/// Gregorian Calendar.
/// </summary>
public static class GregorianCalendarExtensions
{
    #region Providing a singleton instance of GregorianCalendar

    private static readonly Lazy<GregorianCalendar> _gc = new (() => new GregorianCalendar());

    public static GregorianCalendar GetInstance() => _gc.Value;

    #endregion Providing a singleton instance of GregorianCalendar

    #region Guard clauses

    /// <summary>
    /// Check year is valid.
    /// </summary>
    /// <param name="year"></param>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    private static void CheckYear(int year)
    {
        if (year is < 1 or > 9999)
        {
            throw new ArgumentOutOfRangeException(nameof(year),
                "Year must be in the range 1..9999");
        }
    }

    /// <summary>
    /// Check month is valid.
    /// </summary>
    /// <param name="month"></param>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    private static void CheckMonth(int month)
    {
        if (month is < 1 or > 12)
        {
            throw new ArgumentOutOfRangeException(nameof(month),
                "Month must be in the range 1..12");
        }
    }

    #endregion Guard clauses

    #region Find special dates

    /// <summary>
    /// Get the date of Easter Sunday in the given year.
    /// Formula is from Wikipedia.
    /// This method uses the "Meeus/Jones/Butcher" algorithm from 1876, with the New Scientist
    /// modifications from 1961.
    /// Tested for years 1600..2299.
    /// </summary>
    /// <see href="https://en.wikipedia.org/wiki/Date_of_Easter#Anonymous_Gregorian_algorithm"/>
    /// <see href="https://www.census.gov/data/software/x13as/genhol/easter-dates.html"/>
    /// <see href="https://www.assa.org.au/edm"/>
    /// <param name="gc">The GregorianCalendar object.</param>
    /// <param name="year">The Gregorian year number.</param>
    /// <returns>The date of Easter Sunday for the given year.</returns>
    public static DateOnly Easter(this GregorianCalendar gc, int year)
    {
        CheckYear(year);

        int a = year % 19;
        int b = year / 100;
        int c = year % 100;
        int d = b / 4;
        int e = b % 4;
        int g = (8 * b + 13) / 25;
        int h = (19 * a + b - d - g + 15) % 30;
        int i = c / 4;
        int k = c % 4;
        int l = (32 + 2 * e + 2 * i - h - k) % 7;
        int m = (a + 11 * h + 19 * l) / 433;
        int q = h + l - 7 * m;
        int month = (q + 90) / 25;
        int day = (q + 33 * month + 19) % 32;
        return new DateOnly(year, month, day);
    }

    /// <summary>
    /// Get the date of Christmas Day in the given year.
    /// </summary>
    /// <param name="gc">The GregorianCalendar object.</param>
    /// <param name="year">The year.</param>
    /// <returns>The date of Christmas in the given year.</returns>
    public static DateOnly Christmas(this GregorianCalendar gc, int year)
    {
        CheckYear(year);

        return new DateOnly(year, 12, 31);
    }

    /// <summary>
    /// Find the nth weekday in a given month.
    ///
    /// Example: Get the 4th Thursday in January, 2023.
    /// <code>
    /// DateOnly meetup = DateOnlyExtensions.GetNthWeekdayInMonth(2023, 1, 4, DayOfWeek.Thursday);
    /// </code>
    ///
    /// A negative value for n means count from the end of the month.
    /// n = -1 means the last one in the month. n = -2 means the second-last, etc.
    /// Example: Get the last Monday in November, 2025.
    /// <code>
    /// DateOnly meetup = DateOnlyExtensions.GetNthWeekdayInMonth(2025, 11, -1, DayOfWeek.Monday);
    /// </code>
    /// </summary>
    /// <param name="gc">The GregorianCalendar object.</param>
    /// <param name="year">The year.</param>
    /// <param name="month">The month.</param>
    /// <param name="n">Which occurence of the day of the week within the month.</param>
    /// <param name="dayOfWeek">The day of the week.</param>
    /// <returns>The requested date.</returns>
    /// <exception cref="ArgumentOutOfRangeException">If Abs(n) not in the range 1..5</exception>
    /// <exception cref="ArgumentOutOfRangeException">If a valid date could not be found.</exception>
    public static DateOnly GetNthWeekdayInMonth(this GregorianCalendar gc, int year, int month,
        int n, DayOfWeek dayOfWeek)
    {
        CheckYear(year);

        // Guard.
        if (Math.Abs(n) is < 1 or > 5)
        {
            throw new ArgumentOutOfRangeException(nameof(n),
                "The absolute value must be in the range 1..5.");
        }

        int daysInMonth = DateTime.DaysInMonth(year, month);
        int daysPerWeek = 7;

        // Get the first or last day of the month.
        DateOnly firstOrLastOfMonth = new DateOnly(year, month, n > 0 ? 1 : daysInMonth);

        // Calculate the offset to the next or previous day of the week.
        int diffDays = ((int)dayOfWeek - (int)firstOrLastOfMonth.DayOfWeek + daysPerWeek)
            % daysPerWeek;

        // Calculate the day of the month.
        int day = firstOrLastOfMonth.Day + (n - (n > 0 ? 1 : 0)) * daysPerWeek + diffDays;

        // Check if the calculated day is within the month.
        if (day < 1 || day > daysInMonth)
        {
            throw new ArgumentOutOfRangeException(nameof(n), "Could not find a valid date.");
        }

        return new DateOnly(year, month, day);
    }

    /// <summary>
    /// Find the date of Thanksgiving (US and some other countries).
    /// </summary>
    /// <see href="https://en.wikipedia.org/wiki/Thanksgiving#Observance"/>
    /// <param name="gc">The GregorianCalendar object.</param>
    /// <param name="year">The year.</param>
    /// <returns>The date of Thanksgiving.</returns>
    public static DateOnly Thanksgiving(this GregorianCalendar gc, int year)
    {
        CheckYear(year);

        // Get the 4th Thursday in November.
        return gc.GetNthWeekdayInMonth(year, 11, 4, DayOfWeek.Thursday);
    }

    #endregion Find special dates

    #region Year and month start and end

    /// <summary>
    /// Get the DateTime for the start of a given Gregorian year.
    /// </summary>
    /// <param name="gc">The GregorianCalendar object.</param>
    /// <param name="year">The year (1 through 9999).</param>
    /// <param name="kind">The DateTimeKind.</param>
    /// <returns>A DateTime representing the start of the year (UT).</returns>
    public static DateTime GetYearStart(this GregorianCalendar gc, int year,
        DateTimeKind kind = DateTimeKind.Unspecified)
    {
        CheckYear(year);

        return new DateTime(year, 1, 1, 0, 0, 0, kind);
    }

    /// <summary>
    /// Get the DateTime for the end of a given Gregorian year.
    /// </summary>
    /// <param name="gc">The GregorianCalendar object.</param>
    /// <param name="year">The year (1 through 9999).</param>
    /// <param name="kind">The DateTimeKind.</param>
    /// <returns>A DateTime representing the end of the year (UT).</returns>
    public static DateTime GetYearEnd(this GregorianCalendar gc, int year,
        DateTimeKind kind = DateTimeKind.Unspecified)
    {
        CheckYear(year);

        // There isn't a DateTime constructor that allows us to specify the time of day with
        // resolution of 1 tick (the best is microsecond), so instead, we get the start point of the
        // following year and subtract 1 tick.
        return gc.GetYearStart(year + 1, kind).Subtract(new TimeSpan(1));
    }

    /// <summary>
    /// Get the DateTime for the midpoint of a given Gregorian year.
    /// </summary>
    /// <param name="gc">The GregorianCalendar object.</param>
    /// <param name="year">The year (1 through 9999).</param>
    /// <param name="kind">The DateTimeKind.</param>
    /// <returns>A DateTime representing the end of the year (UT).</returns>
    public static DateTime GetYearMidPoint(this GregorianCalendar gc, int year,
        DateTimeKind kind = DateTimeKind.Unspecified)
    {
        CheckYear(year);

        return gc.GetYearStart(year, kind).AddTicks(gc.GetTicksInYear(year) / 2);
    }

    /// <summary>
    /// Get the DateTime for the start of a given Gregorian month.
    /// </summary>
    /// <param name="gc">The GregorianCalendar object.</param>
    /// <param name="year">The year (1 through 9999).</param>
    /// <param name="month">The month (1 through 12).</param>
    /// <param name="kind">The DateTimeKind.</param>
    /// <returns>A DateTime representing the start of the month (UT).</returns>
    public static DateTime GetMonthStart(this GregorianCalendar gc, int year, int month,
        DateTimeKind kind = DateTimeKind.Unspecified)
    {
        CheckYear(year);
        CheckMonth(month);

        return new DateTime(year, month, 1, 0, 0, 0, kind);
    }

    /// <summary>
    /// Get the DateTime for the end of a given Gregorian month.
    /// </summary>
    /// <param name="gc">The GregorianCalendar object.</param>
    /// <param name="year">The year (1 through 9999).</param>
    /// <param name="month">The month (1 through 12).</param>
    /// <param name="kind">The DateTimeKind.</param>
    /// <returns>A DateTime representing the end of the month (UT).</returns>
    public static DateTime GetMonthEnd(this GregorianCalendar gc, int year, int month,
        DateTimeKind kind = DateTimeKind.Unspecified)
    {
        CheckYear(year);
        CheckMonth(month);

        // There isn't a DateTime constructor that allows us to specify the time of day with
        // resolution of 1 tick (the best is microsecond), so instead, we get the start point of the
        // following month and subtract 1 tick.
        if (month == 12)
        {
            month = 1;
            year++;
        }
        return gc.GetMonthStart(year, month, kind).Subtract(new TimeSpan(1));
    }

    /// <summary>
    /// Returns the date of the first day of the year for the specified year.
    /// </summary>
    /// <param name="gc">The GregorianCalendar object.</param>
    /// <param name="year">The year (1 through 9999).</param>
    /// <returns>The first day of the specified year.</returns>
    public static DateOnly GetYearFirstDay(this GregorianCalendar gc, int year)
    {
        CheckYear(year);

        return new DateOnly(year, 1, 1);
    }

    /// <summary>
    /// Returns the last day of the year for the specified year.
    /// </summary>
    /// <param name="gc">The GregorianCalendar object.</param>
    /// <param name="year">The year (1 through 9999).</param>
    /// <returns>The last day of the specified year.</returns>
    public static DateOnly GetYearLastDay(this GregorianCalendar gc, int year)
    {
        CheckYear(year);

        return new DateOnly(year, 12, 31);
    }

    /// <summary>
    /// Returns the date of the first day of the specified month.
    /// </summary>
    /// <param name="gc">The GregorianCalendar object.</param>
    /// <param name="year">The year (1 through 9999).</param>
    /// <param name="month">The month (1 through 12).</param>
    /// <returns>The first day of the specified month.</returns>
    public static DateOnly GetMonthFirstDay(this GregorianCalendar gc, int year, int month)
    {
        CheckYear(year);
        CheckMonth(month);

        return new DateOnly(year, month, 1);
    }

    /// <summary>
    /// Returns the date of the last day of the specified month for the specified year.
    /// </summary>
    /// <param name="gc">The GregorianCalendar object.</param>
    /// <param name="year">The year (1 through 9999).</param>
    /// <param name="month">The month (1 through 12).</param>
    /// <returns>The last day of the specified month and year.</returns>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when the month is not in the valid range (1-12).</exception>
    public static DateOnly GetMonthLastDay(this GregorianCalendar gc, int year, int month)
    {
        CheckYear(year);
        CheckMonth(month);

        return new DateOnly(year, month, gc.GetDaysInMonth(year, month));
    }

    #endregion Year and month start and end

    #region Time units

    /// <summary>
    /// Get the number of ticks in a given Gregorian Calendar year.
    /// </summary>
    /// <param name="gc">The GregorianCalendar object.</param>
    /// <param name="year">The year.</param>
    /// <returns>The number of ticks in the year.</returns>
    public static long GetTicksInYear(this GregorianCalendar gc, int year)
    {
        CheckYear(year);

        int days = gc.GetDaysInYear(year);
        return days * TimeConstants.TICKS_PER_DAY;
    }

    #endregion Time units

    #region Month names

    /// <summary>
    /// Dictionary mapping month numbers (1-12) to month names.
    /// </summary>
    private static readonly Dictionary<int, string> _MonthNames = new ()
    {
        { 1, "January" },
        { 2, "February" },
        { 3, "March" },
        { 4, "April" },
        { 5, "May" },
        { 6, "June" },
        { 7, "July" },
        { 8, "August" },
        { 9, "September" },
        { 10, "October" },
        { 11, "November" },
        { 12, "December" }
    };

    /// <summary>
    /// Get the month names in the Gregorian Calendar.
    /// </summary>
    /// <param name="gc">The GregorianCalendar object.</param>
    /// <returns></returns>
    public static Dictionary<int, string> GetMonthNames(this GregorianCalendar gc)
    {
        return _MonthNames;
    }

    /// <summary>
    /// Converts a month name or abbreviations to its corresponding number (1-12).
    /// Fails if 0 or more than 1 match is found.
    /// </summary>
    /// <param name="gc">The GregorianCalendar object.</param>
    /// <param name="monthName">The month name or abbreviation (case-insensitive).</param>
    /// <returns>The month number.</returns>
    /// <exception cref="ArgumentException">
    /// Thrown when the provided month name or abbreviation doesn't produce a unique result.
    /// </exception>
    public static int MonthNameToNumber(this GregorianCalendar gc, string monthName)
    {
        // Look for matches in the dictionary.
        List<KeyValuePair<int, string>> matches = _MonthNames
            .Where(pair =>
                pair.Value.StartsWith(monthName, StringComparison.CurrentCultureIgnoreCase))
            .ToList();

        // Handle failure modes.
        if (matches.Count == 0)
        {
            throw new ArgumentException("Invalid month name or abbreviation.", nameof(monthName));
        }
        if (matches.Count > 1)
        {
            throw new ArgumentException("More than one match found.", nameof(monthName));
        }

        // Return the result.
        return matches[0].Key;
    }

    /// <summary>
    /// Converts a month number (1-12) to its corresponding name.
    /// </summary>
    /// <param name="gc">The GregorianCalendar object.</param>
    /// <param name="month">The month number (1-12).</param>
    /// <returns>The month name.</returns>
    /// <exception cref="ArgumentException">Thrown when the provided month number is invalid.</exception>
    public static string MonthNumberToName(this GregorianCalendar gc, int month)
    {
        CheckMonth(month);

        if (_MonthNames.TryGetValue(month, out string? name))
        {
            return name;
        }

        throw new ArgumentOutOfRangeException(nameof(month), "Invalid month number.");
    }

    #endregion Month names
}
