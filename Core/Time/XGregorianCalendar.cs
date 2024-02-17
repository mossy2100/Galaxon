using System.Globalization;
using Galaxon.Core.Numbers;
using Galaxon.Core.Numbers.Extensions;

namespace Galaxon.Core.Time;

/// <summary>
/// Extension methods for the GregorianCalendar class, and other useful methods relating to the
/// Gregorian Calendar.
/// </summary>
public static class XGregorianCalendar
{
    #region Ticks per unit

    /// <summary>
    /// The average number of ticks in a (ephemeris) week.
    /// </summary>
    public const long TICKS_PER_WEEK = 6_048_000_000_000L;

    /// <summary>
    /// The average number of ticks in a Gregorian month.
    /// </summary>
    public const long TICKS_PER_MONTH = 26_297_460_000_000L;

    /// <summary>
    /// The average number of ticks in a Gregorian year.
    /// </summary>
    public const long TICKS_PER_YEAR = 315_569_520_000_000L;

    #endregion Ticks per unit

    #region Milliseconds per unit

    /// <summary>
    /// The number of milliseconds in a (ephemeris) week.
    /// </summary>
    public const long MILLISECONDS_PER_WEEK = 604_800_000L;

    /// <summary>
    /// The average number of milliseconds in a Gregorian month.
    /// </summary>
    public const long MILLISECONDS_PER_MONTH = 2_629_746_000L;

    /// <summary>
    /// The average number of milliseconds in a Gregorian year.
    /// </summary>
    public const long MILLISECONDS_PER_YEAR = 31_556_952_000L;

    #endregion Milliseconds per unit

    #region Seconds per unit

    /// <summary>
    /// The number of seconds in a (ephemeris) week.
    /// </summary>
    public const long SECONDS_PER_WEEK = 604_800L;

    /// <summary>
    /// The average number of seconds in a Gregorian month.
    /// </summary>
    public const long SECONDS_PER_MONTH = 2_629_746L;

    /// <summary>
    /// The average number of seconds in a Gregorian year.
    /// </summary>
    public const long SECONDS_PER_YEAR = 31_556_952L;

    #endregion Seconds per unit

    #region Days per unit

    /// <summary>
    /// The number of days in a Gregorian week.
    /// </summary>
    public const long DAYS_PER_WEEK = 7L;

    /// <summary>
    /// The average number of days in a Gregorian month.
    /// </summary>
    public const decimal DAYS_PER_MONTH = 30.436_875M;

    /// <summary>
    /// The average number of days in a Gregorian year.
    /// </summary>
    public const decimal DAYS_PER_YEAR = 365.2425M;

    #endregion Days per unit

    #region Weeks per unit

    /// <summary>
    /// The average number of weeks in a Gregorian month.
    /// </summary>
    public const decimal WEEKS_PER_MONTH = 4.348_125M;

    /// <summary>
    /// The average number of weeks in a Gregorian year.
    /// </summary>
    public const decimal WEEKS_PER_YEAR = 52.1775M;

    #endregion Weeks per unit

    #region Months per unit

    /// <summary>
    /// The number of months in a Gregorian year.
    /// </summary>
    public const long MONTHS_PER_YEAR = 12L;

    #endregion Months per unit

    #region Constants relating to Gregorian solar cycles

    /// <summary>
    /// The Gregorian Calendar repeats on a 400-year cycle called the "Gregorian solar cycle".
    /// (Not to be confused with the 11-year solar cycle.)
    /// There are 97 leap years in that period, giving an average calendar year length of
    /// 365 + (97/400) = 365.2425 days/year.
    /// 1 Gregorian solar cycle = 400 y = 4800 mon = 20,871 w = 146,097 d
    /// 5 Gregorian solar cycles = 2000 y = 2 ky
    /// Gregorian solar cycles are not ordinarily numbered, nor given a specific start date.
    /// However, within the proleptic Gregorian epoch (the one used by .NET), which began on Monday,
    /// 1 Jan, 1 CE, we are currently in the 6th solar cycle. It began on Monday, 1 Jan, 2001, which
    /// was also the first day of the 3rd millennium AD. It will end on Sunday, 31 Dec, 2400.
    /// See:
    /// - <see href="https://en.wikipedia.org/wiki/Solar_cycle_(calendar)"/>
    /// - <see href="https://en.wikipedia.org/wiki/Proleptic_Gregorian_calendar"/>
    /// </summary>
    public const long YEARS_PER_GREGORIAN_SOLAR_CYCLE = 400L;

    /// <summary>
    /// Number of olympiads in a Gregorian solar cycle.
    /// </summary>
    public const long OLYMPIADS_PER_GREGORIAN_SOLAR_CYCLE = 100L;

    /// <summary>
    /// Number of centuries in a Gregorian solar cycle.
    /// </summary>
    public const long CENTURIES_PER_GREGORIAN_SOLAR_CYCLE = 4L;

    /// <summary>
    /// The number of leap years in a Gregorian solar cycle.
    /// </summary>
    public const long LEAP_YEARS_PER_GREGORIAN_SOLAR_CYCLE = 97L;

    /// <summary>
    /// The number of weeks in a Gregorian solar cycle.
    /// </summary>
    public const long WEEKS_PER_GREGORIAN_SOLAR_CYCLE = 20_871L;

    /// <summary>
    /// The number of days in a Gregorian solar cycle.
    /// </summary>
    public const long DAYS_PER_GREGORIAN_SOLAR_CYCLE = 146_097L;

    /// <summary>
    /// The number of seconds in a Gregorian solar cycle.
    /// </summary>
    public const long SECONDS_PER_GREGORIAN_SOLAR_CYCLE = 12_622_780_800L;

    /// <summary>
    /// The number of ticks in a Gregorian solar cycle.
    /// </summary>
    public const long TICKS_PER_GREGORIAN_SOLAR_CYCLE = 126_227_808_000_000_000L;

    #endregion Constants relating to Gregorian solar cycles

    #region Replacment methods that support negative years

    /// <summary>
    /// Check if a year is a leap year.
    /// As we're using the floored division version of the modulo operator, years can be negative.
    /// The .NET GregorianCalendar and DateTime classes only supports positive years.
    /// To convert from BCE (BC) to a negative (proleptic) Gregorian Calendar year number, subtract
    /// the year from 1.
    ///   e.g. 45 BCE = 1 - 45 = -44
    /// </summary>
    /// <param name="y">The year number in the proleptic Gregorian Calendar.</param>
    /// <returns>If the year is a leap year.</returns>
    public static bool IsLeapYear(int y)
    {
        return XNumber.Mod(y, 400) == 0
            || (XNumber.Mod(y, 4) == 0 && XNumber.Mod(y, 100) != 0);
    }

    /// <summary>
    /// Get the number of days in a year.
    /// Supports negative years, unlike the .NET GregorianCalendar or DateTime classes.
    /// </summary>
    /// <param name="y">The year number in the proleptic Gregorian Calendar.</param>
    /// <returns>The number of days in the year.</returns>
    public static int DaysInYear(int y)
    {
        return IsLeapYear(y) ? 366 : 365;
    }

    /// <summary>
    /// Get the number of days in a month.
    /// Supports negative years, unlike the .NET GregorianCalendar or DateTime classes.
    /// </summary>
    /// <param name="y">The year number in the proleptic Gregorian Calendar.</param>
    /// <param name="m">The month number.</param>
    /// <returns>The number of days in the month.</returns>
    public static int DaysInMonth(int y, int m)
    {
        return m switch
        {
            1 or 3 or 5 or 7 or 8 or 10 or 12 => 31,
            4 or 6 or 9 or 11 => 30,
            2 => IsLeapYear(y) ? 29 : 28,
            _ => throw new ArgumentOutOfRangeException(nameof(m), "Must be in range 1-12.")
        };
    }

    /// <summary>
    /// Get the number of seconds in a year.
    /// Supports negative years, unlike the .NET GregorianCalendar or DateTime classes.
    /// Currently does not support leap seconds. May add this capability later.
    /// </summary>
    /// <param name="y">The year number in the proleptic Gregorian Calendar.</param>
    /// <returns>The number of seconds in the year.</returns>
    public static long SecondsInYear(int y)
    {
        return DaysInYear(y) * XTimeSpan.SECONDS_PER_DAY;
    }

    /// <summary>
    /// Which day of the year is a given date.
    /// Supports negative years, unlike the .NET GregorianCalendar or DateTime classes.
    /// </summary>
    /// <param name="y">The year number in the proleptic Gregorian Calendar.</param>
    /// <param name="m">The month number.</param>
    /// <param name="d">The day of the month.</param>
    /// <returns>The day of the year.</returns>
    public static int DayOfYear(int y, int m, int d)
    {
        // Check month.
        if (m is < 1 or > 12)
        {
            throw new ArgumentOutOfRangeException(nameof(m), "Must be in range 1-12.");
        }

        // Check day.
        int dim = DaysInMonth(y, m);
        if (d < 1 || d > dim)
        {
            throw new ArgumentOutOfRangeException(nameof(d), $"Must be in range 1-{dim}.");
        }

        // Calculate day of year.
        int doy = d;
        for (var n = 1; n < m; n++)
        {
            doy += DaysInMonth(y, n);
        }
        return doy;
    }

    #endregion Replacment methods that support negative years

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
    public static DateOnly Easter(int year)
    {
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
    public static DateOnly Christmas(int year)
    {
        return new DateOnly(year, 12, 31);
    }

    /// <summary>
    /// Find the nth weekday in a given month.
    ///
    /// Example: Get the 4th Thursday in January, 2023.
    /// <code>
    /// DateOnly meetup = XDateOnly.GetNthWeekdayInMonth(2023, 1, 4, DayOfWeek.Thursday);
    /// </code>
    ///
    /// A negative value for n means count from the end of the month.
    /// n = -1 means the last one in the month. n = -2 means the second-last, etc.
    /// Example: Get the last Monday in November, 2025.
    /// <code>
    /// DateOnly meetup = XDateOnly.GetNthWeekdayInMonth(2025, 11, -1, DayOfWeek.Monday);
    /// </code>
    /// </summary>
    /// <param name="year">The year.</param>
    /// <param name="month">The month.</param>
    /// <param name="n">Which occurence of the day of the week within the month.</param>
    /// <param name="dayOfWeek">The day of the week.</param>
    /// <returns>The requested date.</returns>
    /// <exception cref="ArgumentOutOfRangeException">If Abs(n) not in the range 1..5</exception>
    /// <exception cref="ArgumentOutOfRangeException">If a valid date could not be found.</exception>
    public static DateOnly NthWeekdayInMonth(int year, int month, int n, DayOfWeek dayOfWeek)
    {
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
    public static DateOnly Thanksgiving(int year)
    {
        // Get the 4th Thursday in November.
        return NthWeekdayInMonth(year, 11, 4, DayOfWeek.Thursday);
    }

    #endregion Find special dates

    #region Year and month start and end

    /// <summary>
    /// Get the DateTime for the start of a given Gregorian year.
    /// </summary>
    /// <param name="year">The year (1 through 9999).</param>
    /// <param name="kind">The DateTimeKind.</param>
    /// <returns>A DateTime representing the start of the year (UT).</returns>
    public static DateTime YearStart(int year, DateTimeKind kind = DateTimeKind.Unspecified)
    {
        // Check year is valid.
        if (year is < 1 or > 9999)
        {
            throw new ArgumentOutOfRangeException(nameof(year),
                "Year must be in the range 1..9999");
        }

        return new DateTime(year, 1, 1, 0, 0, 0, kind);
    }

    /// <summary>
    /// Get the DateTime for the end of a given Gregorian year.
    /// </summary>
    /// <param name="year">The year (1 through 9999).</param>
    /// <param name="kind">The DateTimeKind.</param>
    /// <returns>A DateTime representing the end of the year (UT).</returns>
    public static DateTime YearEnd(int year, DateTimeKind kind = DateTimeKind.Unspecified)
    {
        // Check year is valid.
        if (year is < 1 or > 9999)
        {
            throw new ArgumentOutOfRangeException(nameof(year),
                "Year must be in the range 1..9999");
        }

        // There isn't a DateTime constructor that allows us to specify the time of day with
        // resolution of 1 tick (the best is microsecond), so instead, we get the start point of the
        // following year and subtract 1 tick.
        return YearStart(year + 1, kind).Subtract(new TimeSpan(1));
    }

    /// <summary>
    /// Get the DateTime for the start of a given Gregorian month.
    /// </summary>
    /// <param name="year">The year (1 through 9999).</param>
    /// <param name="month">The month (1 through 12).</param>
    /// <param name="kind">The DateTimeKind.</param>
    /// <returns>A DateTime representing the start of the month (UT).</returns>
    public static DateTime MonthStart(int year, int month,
        DateTimeKind kind = DateTimeKind.Unspecified)
    {
        // Check year is valid.
        if (year is < 1 or > 9999)
        {
            throw new ArgumentOutOfRangeException(nameof(year),
                "Year must be in the range 1..9999");
        }

        // Check month is valid.
        if (month is < 1 or > 12)
        {
            throw new ArgumentOutOfRangeException(nameof(month),
                "Month must be in the range 1..12");
        }

        return new DateTime(year, month, 1, 0, 0, 0, kind);
    }

    /// <summary>
    /// Get the DateTime for the end of a given Gregorian month.
    /// </summary>
    /// <param name="year">The year (1 through 9999).</param>
    /// <param name="month">The month (1 through 12).</param>
    /// <param name="kind">The DateTimeKind.</param>
    /// <returns>A DateTime representing the end of the month (UT).</returns>
    public static DateTime MonthEnd(int year, int month,
        DateTimeKind kind = DateTimeKind.Unspecified)
    {
        // Check year is valid.
        if (year is < 1 or > 9999)
        {
            throw new ArgumentOutOfRangeException(nameof(year),
                "Year must be in the range 1..9999");
        }

        // Check month is valid.
        if (month is < 1 or > 12)
        {
            throw new ArgumentOutOfRangeException(nameof(month),
                "Month must be in the range 1..12");
        }

        // There isn't a DateTime constructor that allows us to specify the time of day with
        // resolution of 1 tick (the best is microsecond), so instead, we get the start point of the
        // following month and subtract 1 tick.
        if (month == 12)
        {
            month = 1;
            year++;
        }
        return MonthStart(year, month, kind).Subtract(new TimeSpan(1));
    }

    /// <summary>
    /// Returns the date of the first day of the year for the specified year.
    /// </summary>
    /// <param name="year">The year (1 through 9999).</param>
    /// <returns>The first day of the specified year.</returns>
    public static DateOnly YearFirstDay(int year)
    {
        // Check year is valid.
        if (year is < 1 or > 9999)
        {
            throw new ArgumentOutOfRangeException(nameof(year),
                "Year must be in the range 1..9999");
        }

        return new DateOnly(year, 1, 1);
    }

    /// <summary>
    /// Returns the last day of the year for the specified year.
    /// </summary>
    /// <param name="year">The year (1 through 9999).</param>
    /// <returns>The last day of the specified year.</returns>
    public static DateOnly YearLastDay(int year)
    {
        // Check year is valid.
        if (year is < 1 or > 9999)
        {
            throw new ArgumentOutOfRangeException(nameof(year),
                "Year must be in the range 1..9999");
        }

        return new DateOnly(year, 12, 31);
    }

    /// <summary>
    /// Returns the date of the first day of the specified month.
    /// </summary>
    /// <param name="year">The year (1 through 9999).</param>
    /// <param name="month">The month (1 through 12).</param>
    /// <returns>The first day of the specified month.</returns>
    public static DateOnly MonthFirstDay(int year, int month)
    {
        // Check year is valid.
        if (year is < 1 or > 9999)
        {
            throw new ArgumentOutOfRangeException(nameof(year),
                "Year must be in the range 1..9999");
        }

        // Check month is valid.
        if (month is < 1 or > 12)
        {
            throw new ArgumentOutOfRangeException(nameof(month),
                "Month must be in the range 1..12");
        }

        return new DateOnly(year, month, 1);
    }

    /// <summary>
    /// Returns the date of the last day of the specified month for the specified year.
    /// </summary>
    /// <param name="year">The year (1 through 9999).</param>
    /// <param name="month">The month (1 through 12).</param>
    /// <returns>The last day of the specified month and year.</returns>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when the month is not in the valid range (1-12).</exception>
    public static DateOnly MonthLastDay(int year, int month)
    {
        // Check year is valid.
        if (year is < 1 or > 9999)
        {
            throw new ArgumentOutOfRangeException(nameof(year),
                "Year must be in the range 1..9999");
        }

        // Check month is valid.
        if (month is < 1 or > 12)
        {
            throw new ArgumentOutOfRangeException(nameof(month),
                "Month must be in the range 1..12");
        }

        return new DateOnly(year, month, DaysInMonth(year, month));
    }

    #endregion Year and month start and end

    #region Month names

    /// <summary>
    /// Dictionary mapping month numbers (1-12) to month names.
    /// </summary>
    public static readonly Dictionary<int, string> MonthNames = new ()
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
    /// Converts a month name to its corresponding number (1-12).
    /// </summary>
    /// <param name="monthName">The month name (case-insensitive).</param>
    /// <returns>The month number.</returns>
    /// <exception cref="ArgumentException">Thrown when the provided month name is invalid.</exception>
    public static int MonthNameToNumber(string monthName)
    {
        monthName = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(monthName.ToLowerInvariant());

        if (MonthNames.ContainsValue(monthName))
        {
            return MonthNames.FirstOrDefault(x => x.Value == monthName).Key;
        }

        throw new ArgumentException("Invalid month name.", nameof(monthName));
    }

    /// <summary>
    /// Converts a month number (1-12) to its corresponding name.
    /// </summary>
    /// <param name="monthNumber">The month number (1-12).</param>
    /// <returns>The month name.</returns>
    /// <exception cref="ArgumentException">Thrown when the provided month number is invalid.</exception>
    public static string MonthNumberToName(int monthNumber)
    {
        if (MonthNames.ContainsKey(monthNumber))
        {
            return MonthNames[monthNumber];
        }

        throw new ArgumentException("Invalid month number.", nameof(monthNumber));
    }

    #endregion Month names
}
