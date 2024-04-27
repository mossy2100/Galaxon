using Galaxon.Time;
using Microsoft.OpenApi.Extensions;

namespace Galaxon.Astronomy.AstroAPI.DataTransferObjects;

public record struct YearInfoDto
{
    public int Year { get; init; }

    public bool IsLeapYear { get; init; }

    public int NumberOfDays { get; init; }

    public bool HasLeapSecond { get; init; }

    public string? LeapSecondDate { get; init; }

    public string FirstDayOfYearDayOfWeek { get; init; }

    public double StartOfYearJulianDate { get; init; }

    public int Century { get; init; }

    public int Millennium { get; init; }

    public int SolarCycle { get; init; }

    public YearInfoDto(int year, bool isLeapYear, int numberOfDays, bool hasLeapSecond,
        DateOnly? leapSecondDate, DayOfWeek firstDayOfYearDayOfWeek, double startOfYearJulianDate,
        int century, int millennium, int solarCycle)
    {
        Year = year;
        IsLeapYear = isLeapYear;
        NumberOfDays = numberOfDays;
        HasLeapSecond = hasLeapSecond;
        LeapSecondDate = leapSecondDate?.ToIsoString();
        FirstDayOfYearDayOfWeek = firstDayOfYearDayOfWeek.GetDisplayName();
        StartOfYearJulianDate = startOfYearJulianDate;
        Century = century;
        Millennium = millennium;
        SolarCycle = solarCycle;
    }
}
