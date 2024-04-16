namespace Galaxon.Time;

public struct GregorianDateTime(GregorianDate date, TimeSpan time)
{
    public GregorianDate Date { get; set; } = date;

    public TimeSpan Time { get; set; } = time;
}
