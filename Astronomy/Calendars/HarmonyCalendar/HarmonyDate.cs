using Galaxon.Time;

namespace Galaxon.Astronomy.Calendars;

public class HarmonyDate : IEarthDate
{
    #region Properties

    public int Year { get; set; }

    public int MonthOfYear { get; set; }

    public int WeekOfYear { get; set; }

    public int WeekOfMonth { get; set; }

    public int DayOfYear { get; set; }

    public int DayOfMonth { get; set; }

    public int DayOfWeek { get; set; }

    #endregion Properties

    #region Constructors

    public HarmonyDate(long ticks)
    {
        DateTime dt = new (ticks);
        (int year, int dayOfYear) = CalcYearAndDayOfYear(dt);
        CalcFields(year, dayOfYear);
    }

    public HarmonyDate(int year, int dayOfYear)
    {
        CalcFields(year, dayOfYear);
    }

    public HarmonyDate(int year, int month, int dayOfMonth)
    {
        int dayOfYear = (month - 1) * HarmonyCalendar.DaysInRegularMonth + dayOfMonth;
        CalcFields(year, dayOfYear);
    }

    public HarmonyDate(DateTime dt)
    {
        (int year, int dayOfYear) = CalcYearAndDayOfYear(dt);
        CalcFields(year, dayOfYear);
    }

    public HarmonyDate(DateOnly date)
    {
        DateTime dt = new (date.Year, date.Month, date.Day, 0, 0, 0, DateTimeKind.Utc);
        (int year, int dayOfYear) = CalcYearAndDayOfYear(dt);
        CalcFields(year, dayOfYear);
    }

    #endregion Constructors

    #region Methods

    /// <summary>
    /// Given a DateTime, calculate the year and the day of the year in the Harmony Calendar. The
    /// time part of the given DateTime is ignored.
    /// </summary>
    /// <param name="dt">The DateTime</param>
    /// <returns>A tuple containing the year and the day of the year.</returns>
    public static (int year, int dayOfYear) CalcYearAndDayOfYear(DateTime dt)
    {
        HarmonyCalendar cal = new ();
        DateOnly date = DateOnly.FromDateTime(dt.ToUniversalTime());
        int year = date.Year;
        DateOnly firstDayOfYear = cal.GetFirstDayOfYear(year);
        if (date < firstDayOfYear)
        {
            year--;
            firstDayOfYear = cal.GetFirstDayOfYear(year);
        }

        // Get the day of the year.
        int dayOfYear = date.Subtract(firstDayOfYear) + 1;

        return (year, dayOfYear);
    }

    /// <summary>
    /// Given the year and the day of the year in the Harmony Calendar, calculate the other date
    /// fields.
    /// </summary>
    /// <param name="year">The year (2001-2099)</param>
    /// <param name="dayOfYear">The day of the year.</param>
    public void CalcFields(int year, int dayOfYear)
    {
        Year = year;
        // Note, these next 2 lines use integer division.
        MonthOfYear = (dayOfYear - 1) / HarmonyCalendar.DaysInRegularMonth + 1;
        WeekOfYear = (dayOfYear - 1) / HarmonyCalendar.DaysInRegularWeek + 1;
        WeekOfMonth = WeekOfYear - (MonthOfYear - 1) * HarmonyCalendar.WeeksInRegularMonth;
        DayOfYear = dayOfYear;
        DayOfMonth = dayOfYear - (MonthOfYear - 1) * HarmonyCalendar.DaysInRegularMonth;
        DayOfWeek = dayOfYear - (WeekOfYear - 1) * HarmonyCalendar.DaysInRegularWeek;
    }

    public DateTime ToDateTime()
    {
        HarmonyCalendar hcal = new ();
        DateOnly date = hcal.GetFirstDayOfYear(Year);
        date.AddDays(DayOfYear - 1);
        return new DateTime(date.Year, date.Month, date.Day, 0, 0, 0, DateTimeKind.Utc);
    }

    public DateOnly ToDateOnly()
    {
        return DateOnly.FromDateTime(ToDateTime());
    }

    public TimeSpan Subtract(HarmonyDate hd)
    {
        return ToDateTime().Subtract(hd.ToDateTime());
    }

    public static string[] GetDayOfWeekNames()
    {
        return
        [
            "",
            "SolDay",
            "MercuryDay",
            "VenusDay",
            "EarthDay",
            "LunaDay",
            "MarsDay"
        ];
    }

    public static string GetDayOfWeekName(int dayOfWeek)
    {
        if (!HarmonyCalendar.IsValidDayOfWeek(dayOfWeek))
        {
            throw new ArgumentOutOfRangeException(nameof(dayOfWeek),
                $"Day of week outside valid range: 1-{HarmonyCalendar.DaysInRegularWeek}");
        }

        return GetDayOfWeekNames()[dayOfWeek];
    }

    public IEarthDate FromJulianDay(double jd)
    {
        throw new NotImplementedException();
    }

    public double ToJulianDay()
    {
        throw new NotImplementedException();
    }

    #endregion Methods
}
