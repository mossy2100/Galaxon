using System.Globalization;

namespace Galaxon.Time;

public static class JulianCalendarUtility
{
    /// <summary>
    /// Get the singleton instance of JulianCalendar.
    /// </summary>
    public static JulianCalendar JulianCalendarInstance { get; } = new ();

    /// <summary>
    /// Check if a date triplet could refer to a valid date in the Julian Calendar.
    /// The latest valid date possible is taken to be 1582-10-04, which ignores the fact that some
    /// cultures continued to use the Julian Calendar after the Gregorian reform.
    /// </summary>
    public static bool IsValidDate(int year, int month, int day)
    {
        // Check the month is valid.
        if (month is < 1 or > 12)
        {
            return false;
        }

        // Check the date is earlier than or equal to the last Julian Calendar date.
        // For 1582, check if the date is before October 5.
        if (year > 1582 || (year == 1582 && (month > 10 || (month == 10 && day > 4))))
        {
            return false;
        }

        // Check the day.
        // Note, I think this could throw if the year is less than or equal to 0, need to test.
        return day >= 1 && day <= JulianCalendarInstance.GetDaysInMonth(year, month);
    }
}
