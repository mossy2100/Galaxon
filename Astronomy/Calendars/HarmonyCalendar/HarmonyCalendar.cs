﻿using Galaxon.Astronomy.Data;
using Galaxon.Astronomy.Data.Enums;
using Galaxon.Astronomy.Data.Models;

namespace Galaxon.Astronomy.Calendars;

public class HarmonyCalendar : Calendar
{
    #region Properties

    public override int[] Eras => [1];

    #endregion Properties

    #region Constants

    public const int MinSupportedYear = 2002;

    public const int MaxSupportedYear = 2099;

    public const int MonthsPerYear = 13;

    public const int WeeksInYear = 61;

    public const int WeeksInRegularMonth = 5;

    public const int DaysInCommonYear = 365;

    public const int DaysInLeapYear = 366;

    public const int DaysInRegularMonth = 30;

    public const int DaysInRegularWeek = 6;

    #endregion Constants

    #region Methods

    #region OverriddenMethods

    public override CalendarAlgorithmType AlgorithmType => CalendarAlgorithmType.SolarCalendar;

    public override DateTime AddMonths(DateTime dt, int months)
    {
        return dt.AddDays(DaysInRegularMonth);
    }

    public override DateTime AddYears(DateTime dt, int years)
    {
        (int year, int dayOfYear) = HarmonyDate.CalcYearAndDayOfYear(dt);
        year += years;

        // If the given date is the last day of a leap year, but the updated year is not a leap
        // year, then change the day of the year to the last day of the updated year.
        int daysInYear = GetDaysInYear(year);
        if (dayOfYear > daysInYear)
        {
            dayOfYear = daysInYear;
        }
        return new HarmonyDate(year, dayOfYear).ToDateTime();
    }

    public override DateTime AddDays(DateTime dt, int days)
    {
        return dt.AddDays(days);
    }

    public override int GetDayOfMonth(DateTime dt)
    {
        return new HarmonyDate(dt).DayOfMonth;
    }

    public override DayOfWeek GetDayOfWeek(DateTime dt)
    {
        // This isn't quite right because the day names are different, but it kind of works. It's
        // like we used the normal days of the week, except for Saturday.
        return (DayOfWeek)(new HarmonyDate(dt).DayOfWeek - 1);
    }

    public override int GetDayOfYear(DateTime dt)
    {
        return new HarmonyDate(dt).DayOfYear;
    }

    public override int GetDaysInMonth(int year, int month)
    {
        if (!IsValidYear(year))
        {
            throw new ArgumentOutOfRangeException(nameof(year),
                $"Year outside valid range: {MinSupportedYear}-{MaxSupportedYear}");
        }
        if (!IsValidMonth(month))
        {
            throw new ArgumentOutOfRangeException(nameof(month),
                $"Month outside valid range: 1-{MonthsPerYear}");
        }
        return month < MonthsPerYear
            ? DaysInRegularMonth
            : DaysInRegularWeek - (IsLeapYear(year) ? 0 : 1);
    }

    public override int GetDaysInMonth(int year, int month, int era)
    {
        return GetDaysInMonth(year, month);
    }

    public override int GetDaysInYear(int year)
    {
        if (!IsValidYear(year))
        {
            throw new ArgumentOutOfRangeException(nameof(year),
                $"Year outside valid range: {MinSupportedYear}-{MaxSupportedYear}");
        }

        // Get first day of year.
        HarmonyDate hd1 = new (year, 1);
        DateTime dt1 = hd1.ToDateTime();

        // Get first day of next year.
        HarmonyDate hd2 = new (year + 1, 1);
        DateTime dt2 = hd2.ToDateTime();

        // Get difference in days.
        return dt2.Subtract(dt1).Days;
    }

    public override int GetDaysInYear(int year, int era)
    {
        return GetDaysInYear(year);
    }

    public override int GetEra(DateTime dt)
    {
        return 1;
    }

    public override int GetMonth(DateTime dt)
    {
        return new HarmonyDate(dt).MonthOfYear;
    }

    public override int GetMonthsInYear(int year)
    {
        return MonthsPerYear;
    }

    public override int GetMonthsInYear(int year, int era)
    {
        return GetMonthsInYear(year);
    }

    public override int GetYear(DateTime dt)
    {
        return new HarmonyDate(dt).Year;
    }

    public override bool IsLeapDay(int year, int month, int dayOfMonth)
    {
        if (!IsValidDate(year, month, dayOfMonth))
        {
            throw new ArgumentException(
                "Cannot create a valid Harmony Date from the supplied arguments.");
        }

        return month == MonthsPerYear && dayOfMonth == DaysInRegularWeek;
    }

    public override bool IsLeapDay(int year, int month, int dayOfMonth, int era)
    {
        return IsLeapDay(year, month, dayOfMonth);
    }

    public override bool IsLeapMonth(int year, int month)
    {
        return false;
    }

    public override bool IsLeapMonth(int year, int month, int era)
    {
        return IsLeapMonth(year, month);
    }

    public override bool IsLeapYear(int year)
    {
        if (!IsValidYear(year))
        {
            throw new ArgumentOutOfRangeException(nameof(year),
                $"Year outside valid range: {MinSupportedYear}-{MaxSupportedYear}");
        }

        return GetDaysInYear(year) == DaysInLeapYear;
    }

    public override bool IsLeapYear(int year, int era)
    {
        return IsLeapYear(year);
    }

    public override DateTime ToDateTime(int year, int month, int dayOfMonth, int hour, int minute,
        int second, int millisecond)
    {
        if (!IsValidDate(year, month, dayOfMonth))
        {
            throw new ArgumentException(
                "Cannot create a valid Harmony Date from the supplied arguments.");
        }

        DateTime dt = new HarmonyDate(year, month, dayOfMonth).ToDateTime();
        return new DateTime(dt.Year, dt.Month, dt.Day, hour, minute, second, millisecond);
    }

    public override DateTime ToDateTime(int year, int month, int dayOfMonth, int hour, int minute,
        int second, int millisecond, int era)
    {
        return ToDateTime(year, month, dayOfMonth, hour, minute, second, millisecond);
    }

    public static bool IsValidYear(int year)
    {
        return year is >= MinSupportedYear and <= MaxSupportedYear;
    }

    public static bool IsValidMonth(int month)
    {
        return month is >= 1 and <= MonthsPerYear;
    }

    public bool IsValidDate(int year, int month, int dayOfMonth)
    {
        return IsValidYear(year)
            && IsValidMonth(month)
            && dayOfMonth >= 1
            && dayOfMonth <= GetDaysInMonth(year, month);
    }

    public bool IsValidDate(int year, int dayOfYear)
    {
        return IsValidYear(year)
            && dayOfYear >= 1
            && dayOfYear <= GetDaysInYear(year);
    }

    public static bool IsValidDayOfWeek(int dayOfWeek)
    {
        return dayOfWeek is >= 1 and <= DaysInRegularWeek;
    }

    #endregion OverriddenMethods

    #region StaticMethods

    public DateTime[] GetSeasonalMarkers(int year)
    {
        AstroDbContext astroDbContext = new ();
        List<SeasonalMarkerRecord> seasonalMarkers = astroDbContext.SeasonalMarkers
            .Where(sm => sm.DateTimeUtcUsno != null && sm.DateTimeUtcUsno.Value.Year == year)
            .OrderBy(sm => sm.DateTimeUtcUsno)
            .ToList();

        if (seasonalMarkers.Count == 0)
        {
            // I'm not using IsValidYear() here because internal methods need access to the full
            // range of years within the data, which is 2001-2100. This will be updated when I
            // implement the actual astronomical calculations.
            throw new ArgumentOutOfRangeException(nameof(year), "Year outside valid range.");
        }

        return seasonalMarkers.Select(sm => sm.DateTimeUtcUsno!.Value).ToArray();
    }

    public DateTime GetSeasonalMarker(int year, ESeasonalMarkerType seasonalMarkerTypeNumber)
    {
        return GetSeasonalMarkers(year)[(int)seasonalMarkerTypeNumber];
    }

    public static int GetWeeksInYear(int year, int month)
    {
        return WeeksInYear;
    }

    public static int GetWeeksInMonth(int year, int month)
    {
        return month == MonthsPerYear ? 1 : WeeksInRegularMonth;
    }

    /// <summary>
    /// Get the date of the first day of the given Harmony Year.
    /// </summary>
    /// <param name="year">The date.</param>
    /// <returns></returns>
    public DateOnly GetFirstDayOfYear(int year)
    {
        // Find the March equinox date for the given Gregorian year.
        DateTime nve = GetSeasonalMarker(year, 0);
        DateOnly date = new (year, nve.Month, nve.Day);
        // If the vernal equinox is in the afternoon, it's New Year's Eve. Start the new year the
        // next day.
        if (nve.Hour >= 12)
        {
            date = date.AddDays(1);
        }
        return date;
    }

    #endregion StaticMethods

    #endregion Methods
}
