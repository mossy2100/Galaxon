namespace Galaxon.Time.Idea;

public struct GregorianDateTime(GregorianDate date, TimeSpan time)
{
    /// <summary>
    /// Using a custom struct instead of a DateOnly, so we can support years before 1 CE and after
    /// 9999 CE.
    /// </summary>
    public GregorianDate Date { get; set; } = date;

    /// <summary>
    /// Using a TimeSpan for the time of day instead of a TimeOnly, so we can support leap seconds.
    /// </summary>
    public TimeSpan Time { get; set; } = time;
}
