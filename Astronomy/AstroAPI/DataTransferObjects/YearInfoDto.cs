using Microsoft.OpenApi.Extensions;

namespace Galaxon.Astronomy.AstroAPI.DataTransferObjects;

public record struct YearInfoDto
{
    public int Year { get; init; }

    public bool IsLeapYear { get; init; }

    public string YearType { get; init; }

    public int NumberOfDays { get; init; }

    public bool HasLeapSecond { get; init; }

    public string? LeapSecondDate { get; init; }

    public string FirstDayOfYearDayOfWeek { get; init; }

    public double StartOfYearJulianDate { get; init; }

    public YearInfoDto(int year, bool isLeapYear, string yearType, int numberOfDays,
        bool hasLeapSecond, DateOnly? leapSecondDate, DayOfWeek firstDayOfYearDayOfWeek,
        double startOfYearJulianDate)
    {
        Year = year;
        IsLeapYear = isLeapYear;
        YearType = yearType;
        NumberOfDays = numberOfDays;
        HasLeapSecond = hasLeapSecond;
        LeapSecondDate = leapSecondDate?.ToString("O");
        FirstDayOfYearDayOfWeek = firstDayOfYearDayOfWeek.GetDisplayName();
        StartOfYearJulianDate = startOfYearJulianDate;
    }
}
