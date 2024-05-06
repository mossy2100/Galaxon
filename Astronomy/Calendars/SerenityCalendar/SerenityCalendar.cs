namespace Galaxon.Astronomy.Calendars;

public class SerenityCalendar : Calendar
{
    public override int[] Eras => throw new NotImplementedException();

    public int NumDaysInYear(int year)
    {
        return IsLeapYear(year) ? 366 : 365;
    }

    public static bool IsLongMonth(int month)
    {
        return month % 2 == 0 || month % 850 % 32 == 25;
    }

    public static int NumDaysInMonth(int month)
    {
        return IsLongMonth(month) ? 30 : 29;
    }

    public override DateTime AddMonths(DateTime time, int months)
    {
        throw new NotImplementedException();
    }

    public override DateTime AddYears(DateTime time, int years)
    {
        throw new NotImplementedException();
    }

    public override int GetDayOfMonth(DateTime time)
    {
        throw new NotImplementedException();
    }

    public override DayOfWeek GetDayOfWeek(DateTime time)
    {
        throw new NotImplementedException();
    }

    public override int GetDayOfYear(DateTime time)
    {
        throw new NotImplementedException();
    }

    public override int GetDaysInMonth(int year, int month, int era)
    {
        throw new NotImplementedException();
    }

    public override int GetDaysInYear(int year, int era)
    {
        throw new NotImplementedException();
    }

    public override int GetEra(DateTime time)
    {
        throw new NotImplementedException();
    }

    public override int GetMonth(DateTime time)
    {
        throw new NotImplementedException();
    }

    public override int GetMonthsInYear(int year, int era)
    {
        throw new NotImplementedException();
    }

    public override int GetYear(DateTime time)
    {
        throw new NotImplementedException();
    }

    public override bool IsLeapDay(int year, int month, int day, int era)
    {
        throw new NotImplementedException();
    }

    public override bool IsLeapMonth(int year, int month, int era)
    {
        throw new NotImplementedException();
    }

    public override bool IsLeapYear(int year)
    {
        return year % 4 == 0 && year % 128 != 0;
    }

    public override bool IsLeapYear(int year, int era)
    {
        return IsLeapYear(year);
    }

    public override DateTime ToDateTime(int year, int month, int day, int hour, int minute,
        int second, int millisecond, int era)
    {
        throw new NotImplementedException();
    }
}
