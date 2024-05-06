namespace Galaxon.Astronomy.Calendars;

public class SerenityDate : IEarthDate
{
    /// <summary>
    /// Empty constructor.
    /// Sets a default date of day 1 in month 0.
    /// </summary>
    public SerenityDate() : this(0, 1) { }

    /// <summary>
    /// Creates a new SerenityDate from a number of ticks.
    /// </summary>
    /// <param name="ticks"></param>
    public SerenityDate(long ticks)
    {
        Ticks = ticks;
    }

    /// <summary>
    /// Construct a new SerenityDate from the month (i.e. the Serenity Lunation
    /// Number) and day of the month (1..30).
    /// </summary>
    /// <param name="month">The month (i.e. the Serenity Lunation Number)</param>
    /// <param name="dayOfMonth">The day of the month (1..30)</param>
    public SerenityDate(int month, int dayOfMonth)
    {
        // todo argument checking

        // Ticks = CalculateTicks(month, dayOfMonth);
    }

    // The number of 100-nanosecond intervals that have elapsed since January 1,
    // 0001 at 00:00:00.000 in the Gregorian calendar.
    // This is the core property represents a point in time, and is useful for
    // interoperability with the .NET DateTime class and my other calendars.
    public long Ticks { get; set; }

    // The year number in the Serenity Calendar will usually be the same as the
    // Earthian year number, but it can be different for dates close to the
    // beginning or ending of a year, because, although they run in parallel,
    // Serenity years begin and end at month boundaries.
    //
    // The idea is to start the calendar at a point in time
    // where the new moon coincides with the northern vernal equinox;
    // and thus, Y0-001 (year 0, day 1) is equal to M0-01 (month 0, day 1).
    public int Year { get; set; }

    // Range from 1..384.
    public int DayOfYear { get; set; }

    // The month number in the Serenity Calendar is equal to Meeus' Lunation
    // Number (LN).
    public int Month { get; set; }

    // Range from 1..30.
    public int DayOfMonth { get; set; }

    // The single instance of the calendar.
    protected static SerenityCalendar TheCalendar { get; } = new ();

    //==========================================================================

    #region Conversions

    /// <summary>
    /// Convert a Gregorian datetime to a Serenity date.
    /// </summary>
    /// <param name="dt">A Gregorian date time as a .NET DateTime object.</param>
    /// <returns>The equivalent Serenity date.</returns>
    public static SerenityDate FromDateTime(DateTime dt)
    {
        return new SerenityDate(dt.Ticks);
    }

    /// <summary>
    /// Convert a SerenityDate to a DateTime.
    /// </summary>
    /// <returns>The equivalent Gregorian date time as a .NET DateTime object.</returns>
    public DateTime ToDateTime()
    {
        return new DateTime(Ticks);
    }

    /// <summary>
    /// Convert a Gregorian date to a Serenity date.
    /// </summary>
    /// <param name="date">A Gregorian date as a .NET DateOnly object.</param>
    /// <returns>The equivalent Serenity date.</returns>
    public static SerenityDate FromDateOnly(DateOnly date)
    {
        return FromDateTime(date.ToDateTime(new TimeOnly(0)));
    }

    /// <summary>
    /// Convert a Serenity date to a Gregorian date.
    /// </summary>
    /// <returns>The equivalent Gregorian date as a .NET DateOnly object.</returns>
    public DateOnly ToDateOnly()
    {
        return DateOnly.FromDateTime(ToDateTime());
    }

    public IEarthDate FromJulianDay(double jd)
    {
        throw new NotImplementedException();
    }

    public double ToJulianDay()
    {
        throw new NotImplementedException();
    }

    //public SerenityDate FromEarthian(EarthianDate earthianDate)
    //{
    //    return new(earthianDate.Ticks);
    //}

    //public EarthianDate ToEarthian()
    //{
    //    return new EarthianDate(Ticks);
    //}

    #endregion Conversions

    //==========================================================================
}
