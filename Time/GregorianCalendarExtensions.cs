using System.Globalization;

namespace Galaxon.Time;

/// <summary>
/// Extension methods for the GregorianCalendar class, and other useful methods relating to the
/// Gregorian Calendar.
/// </summary>
public static class GregorianCalendarExtensions
{
    #region Provide a singleton instance of GregorianCalendar

    private static readonly Lazy<GregorianCalendar> _GregorianCalendarInstance =
        new (() => new GregorianCalendar());

    /// <summary>
    /// Get the single GregorianCalendar object.
    /// </summary>
    public static GregorianCalendar GetInstance()
    {
        return _GregorianCalendarInstance.Value;
    }

    #endregion Provide a singleton instance of GregorianCalendar

    #region Guard clauses

    /// <summary>
    /// Check year is valid.
    /// </summary>
    /// <param name="year"></param>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    public static void CheckYearInRange(int year)
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
    public static void CheckMonthInRange(int month)
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
    /// <param name="year">The Gregorian year number.</param>
    /// <returns>The date of Easter Sunday for the given year.</returns>
    public static DateOnly GetEaster(int year)
    {
        CheckYearInRange(year);

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
    /// <param name="year">The year.</param>
    /// <returns>The date of Christmas in the given year.</returns>
    public static DateOnly GetChristmas(int year)
    {
        CheckYearInRange(year);

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
    /// <param name="year">The year.</param>
    /// <param name="month">The month.</param>
    /// <param name="n">Which occurence of the day of the week within the month.</param>
    /// <param name="dayOfWeek">The day of the week.</param>
    /// <returns>The requested date.</returns>
    /// <exception cref="ArgumentOutOfRangeException">If Abs(n) not in the range 1..5</exception>
    /// <exception cref="ArgumentOutOfRangeException">If a valid date could not be found.</exception>
    public static DateOnly GetNthWeekdayInMonth(int year, int month, int n, DayOfWeek dayOfWeek)
    {
        CheckYearInRange(year);

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
    /// <param name="year">The year.</param>
    /// <returns>The date of Thanksgiving.</returns>
    public static DateOnly GetThanksgiving(int year)
    {
        CheckYearInRange(year);

        // Get the 4th Thursday in November.
        return GetNthWeekdayInMonth(year, 11, 4, DayOfWeek.Thursday);
    }

    #endregion Find special dates

    #region Year and month start and end

    /// <summary>
    /// Get the DateTime for the start of a given Gregorian year.
    /// </summary>
    /// <param name="year">The year (1 through 9999).</param>
    /// <param name="kind">The DateTimeKind.</param>
    /// <returns>A DateTime representing the start of the year (UT).</returns>
    public static DateTime GetYearStart(int year, DateTimeKind kind = DateTimeKind.Unspecified)
    {
        CheckYearInRange(year);

        return new DateTime(year, 1, 1, 0, 0, 0, kind);
    }

    /// <summary>
    /// Get the DateTime for the end of a given Gregorian year.
    /// </summary>
    /// <param name="year">The year (1 through 9999).</param>
    /// <param name="kind">The DateTimeKind.</param>
    /// <returns>A DateTime representing the end of the year (UT).</returns>
    public static DateTime GetYearEnd(int year, DateTimeKind kind = DateTimeKind.Unspecified)
    {
        CheckYearInRange(year);

        // There isn't a DateTime constructor that allows us to specify the time of day with
        // resolution of 1 tick (the best is microsecond), so instead, we get the start point of the
        // following year and subtract 1 tick.
        return GetYearStart(year + 1, kind) - new TimeSpan(1);
    }

    /// <summary>
    /// Get the DateTime for the midpoint of a given Gregorian year.
    /// </summary>
    /// <param name="year">The year (1 through 9999).</param>
    /// <param name="kind">The DateTimeKind.</param>
    /// <returns>A DateTime representing the end of the year (UT).</returns>
    public static DateTime GetYearMidPoint(int year, DateTimeKind kind = DateTimeKind.Unspecified)
    {
        CheckYearInRange(year);

        return GetYearStart(year, kind) + new TimeSpan(GetTicksInYear(year) / 2);
    }

    /// <summary>
    /// Get the DateTime for the start of a given Gregorian month.
    /// </summary>
    /// <param name="year">The year (1 through 9999).</param>
    /// <param name="month">The month (1 through 12).</param>
    /// <param name="kind">The DateTimeKind.</param>
    /// <returns>A DateTime representing the start of the month (UT).</returns>
    public static DateTime GetMonthStart(int year, int month,
        DateTimeKind kind = DateTimeKind.Unspecified)
    {
        CheckYearInRange(year);
        CheckMonthInRange(month);

        return new DateTime(year, month, 1, 0, 0, 0, kind);
    }

    /// <summary>
    /// Get the DateTime for the end of a given Gregorian month.
    /// </summary>
    /// <param name="year">The year (1 through 9999).</param>
    /// <param name="month">The month (1 through 12).</param>
    /// <param name="kind">The DateTimeKind.</param>
    /// <returns>A DateTime representing the end of the month (UT).</returns>
    public static DateTime GetMonthEnd(int year, int month,
        DateTimeKind kind = DateTimeKind.Unspecified)
    {
        CheckYearInRange(year);
        CheckMonthInRange(month);

        // There isn't a DateTime constructor that allows us to specify the time of day with
        // resolution of 1 tick (the best is microsecond), so instead, we'll add the number of ticks
        // in the month less 1.
        return GetMonthStart(year, month, kind) + new TimeSpan(GetTicksInMonth(year, month) - 1);
    }

    /// <summary>
    /// Returns the date of the first day of the year for the specified year.
    /// </summary>
    /// <param name="year">The year (1 through 9999).</param>
    /// <returns>The first day of the specified year.</returns>
    public static DateOnly GetYearFirstDay(int year)
    {
        CheckYearInRange(year);

        return new DateOnly(year, 1, 1);
    }

    /// <summary>
    /// Returns the last day of the year for the specified year.
    /// </summary>
    /// <param name="year">The year (1 through 9999).</param>
    /// <returns>The last day of the specified year.</returns>
    public static DateOnly GetYearLastDay(int year)
    {
        CheckYearInRange(year);

        return new DateOnly(year, 12, 31);
    }

    /// <summary>
    /// Returns the date of the first day of the specified month.
    /// </summary>
    /// <param name="year">The year (1 through 9999).</param>
    /// <param name="month">The month (1 through 12).</param>
    /// <returns>The first day of the specified month.</returns>
    public static DateOnly GetMonthFirstDay(int year, int month)
    {
        CheckYearInRange(year);
        CheckMonthInRange(month);

        return new DateOnly(year, month, 1);
    }

    /// <summary>
    /// Returns the date of the last day of the specified month for the specified year.
    /// </summary>
    /// <param name="year">The year (1 through 9999).</param>
    /// <param name="month">The month (1 through 12).</param>
    /// <returns>The last day of the specified month and year.</returns>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when the month is not in the valid range (1-12).</exception>
    public static DateOnly GetMonthLastDay(int year, int month)
    {
        CheckYearInRange(year);
        CheckMonthInRange(month);

        GregorianCalendar gc = GetInstance();
        return new DateOnly(year, month, gc.GetDaysInMonth(year, month));
    }

    #endregion Year and month start and end

    #region Time units

    /// <summary>
    /// See if a given year is a leap year.
    /// </summary>
    /// <param name="year">The Gregorian Calendar year number (AD/CE).</param>
    /// <returns>If the year is a leap year.</returns>
    public static bool IsLeapYear(int year)
    {
        return year % 400 == 0 || (year % 4 == 0 && year % 100 != 0);
    }

    /// <summary>
    /// Get the number of days in a given Gregorian Calendar year.
    /// </summary>
    /// <param name="year">The year.</param>
    /// <returns>The number of days in the year.</returns>
    public static int GetDaysInYear(int year)
    {
        return IsLeapYear(year) ? 366 : 365;
    }

    /// <summary>
    /// Get the number of ticks in a given Gregorian Calendar year.
    /// Does not include leap seconds.
    /// </summary>
    /// <param name="year">The year.</param>
    /// <returns>The number of ticks in the year.</returns>
    public static long GetTicksInYear(int year)
    {
        return GetDaysInYear(year) * TimeConstants.TICKS_PER_DAY;
    }

    /// <summary>
    /// Get the number of days in a given Gregorian Calendar year.
    /// </summary>
    /// <param name="year">The year.</param>
    /// <param name="month">The month (1..12).</param>
    /// <returns>The number of days in the month.</returns>
    public static int GetDaysInMonth(int year, int month)
    {
        CheckMonthInRange(month);

        return (month == 2 && IsLeapYear(year)) ? 29 : GregorianMonth.Months[month].Length;
    }

    /// <summary>
    /// Get the number of ticks in a given Gregorian Calendar month.
    /// Does not include leap seconds.
    /// </summary>
    /// <param name="year">The year.</param>
    /// <param name="month">The month (1..12).</param>
    /// <returns>The number of ticks in the month.</returns>
    public static long GetTicksInMonth(int year, int month)
    {
        return GetDaysInMonth(year, month) * TimeConstants.TICKS_PER_DAY;
    }

    #endregion Time units

    /// <summary>
    /// Check if a date triplet could refer to a date in the Julian Calendar.
    /// </summary>
    /// <param name="year"></param>
    /// <param name="month"></param>
    /// <param name="day"></param>
    /// <returns></returns>
    public static bool IsJulianDate(int year, int month, int day)
    {
        CheckMonthInRange(month);

        if (year < 1582)
        {
            return true;
        }

        return year * 10000 + month * 100 + day <= 1582_10_04;
    }
}
