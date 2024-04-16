namespace Galaxon.Time;

public struct GregorianDate
{
    private long JulianDayNumber;

    public static GregorianDateTime operator +(GregorianDate date, TimeSpan time)
    {
        return new GregorianDateTime(date, time);
    }
}
