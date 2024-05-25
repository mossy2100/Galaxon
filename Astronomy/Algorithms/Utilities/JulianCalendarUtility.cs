using System.Globalization;
using Galaxon.Time;
using static Galaxon.Numerics.Extensions.NumberExtensions;

namespace Galaxon.Astronomy.Algorithms.Utilities;

public static class JulianCalendarUtility
{
    /// <summary>
    /// Get the singleton instance of JulianCalendar.
    /// </summary>
    public static JulianCalendar JulianCalendarInstance { get; } = new ();

    /// <summary>
    /// Check if a given year is a leap year.
    /// </summary>
    /// <param name="year">The Julian Calendar year number.</param>
    /// <returns>If the year is a leap year.</returns>
    /// <remarks>
    /// This method ignores the errors in intercalation that occurred during the initial
    /// half-century of the calendar's use, partly because catering for these errors would introduce
    /// unnecessary complexity, and also because there is no consensus on which years between 44 BC
    /// and 4 AD were actually leap years.
    /// See: <see href="https://en.wikipedia.org/wiki/Julian_calendar#Leap_years"/>
    /// </remarks>
    public static bool IsLeapYear(int year)
    {
        return Mod(year, 4) == 0;
    }

    /// <summary>
    /// Get the number of days in a given Julian Calendar year.
    /// </summary>
    /// <param name="year">The year.</param>
    /// <param name="month">The month (1..12).</param>
    /// <returns>The number of days in the month.</returns>
    /// <remarks>
    /// Although this method looks identical to
    /// <see cref="GregorianCalendarUtility.GetDaysInMonth"/>, it can produce different results for
    /// February because of the different leap year rule.
    /// </remarks>
    public static int GetDaysInMonth(int year, int month)
    {
        GregorianCalendarUtility.CheckMonthInRange(month);

        return month switch
        {
            2 => IsLeapYear(year) ? 29 : 28,
            4 or 6 or 9 or 11 => 30,
            _ => 31
        };
    }

    /// <summary>
    /// Check if a date triplet could refer to a valid date in the Julian Calendar.
    ///
    /// The latest valid date possible is taken to be 1582-10-04, which ignores the fact that some
    /// cultures continued to use the Julian Calendar after the Gregorian reform.
    ///
    /// This method currently has a limitation, which is that the .NET JulianCalendar class is used
    /// to check the day of the month is valid; however, it won't support any date earlier than
    /// Gregorian 0001-01-01, whereas the Julian Calendar started 1 January, 45 BCE (-44).
    /// </summary>
    public static bool IsValidDate(int year, int month, int day)
    {
        // Check the year is within the valid range.
        if (year is < -44 or > 1582)
        {
            return false;
        }

        // Check the month is within the valid range.
        if (month is < 1 or > 12)
        {
            return false;
        }

        // For 1582, check if the date is before October 5.
        if (year == 1582 && (month > 10 || (month == 10 && day > 4)))
        {
            return false;
        }

        // Check the day.
        // Note, I think this could throw if the year is less than or equal to 0, need to test.
        return day >= 1 && day <= GetDaysInMonth(year, month);
    }

    /// <summary>
    /// Convert a Julian Calendar date to a Gregorian Calendar date.
    /// </summary>
    /// <param name="year">The year (-44+)</param>
    /// <param name="month">The month (1-12)</param>
    /// <param name="day">The day (1-31)</param>
    /// <returns>The equivalent Gregorian date.</returns>
    public static DateOnly JulianCalendarDateToGregorianDate(int year, int month, int day)
    {
        DateTime dt = JulianCalendarInstance.ToDateTime(year, month, day, 0, 0, 0, 0);
        return DateOnly.FromDateTime(dt);
    }
}
