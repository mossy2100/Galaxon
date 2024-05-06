namespace Galaxon.Astronomy.Calendars.AstronomicalCalendar;

public class AstronomicalCalendar : Calendar
{
    /// <inheritdoc/>
    public override int[] Eras { get; } = [1];

    /// <inheritdoc/>
    public override DateTime AddMonths(DateTime time, int months)
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc/>
    public override DateTime AddYears(DateTime time, int years)
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc/>
    public override int GetDayOfMonth(DateTime time)
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc/>
    public override DayOfWeek GetDayOfWeek(DateTime time)
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc/>
    public override int GetDayOfYear(DateTime time)
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc/>
    public override int GetDaysInMonth(int year, int month, int era)
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc/>
    public override int GetDaysInYear(int year, int era)
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc/>
    public override int GetEra(DateTime time)
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc/>
    public override int GetMonth(DateTime time)
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc/>
    public override int GetMonthsInYear(int year, int era)
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc/>
    public override int GetYear(DateTime time)
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc/>
    public override bool IsLeapDay(int year, int month, int day, int era)
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc/>
    public override bool IsLeapMonth(int year, int month, int era)
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc/>
    public override bool IsLeapYear(int year, int era)
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc/>
    public override DateTime ToDateTime(int year, int month, int day, int hour, int minute,
        int second, int millisecond,
        int era)
    {
        throw new NotImplementedException();
    }
}
