using Galaxon.Time;
using Microsoft.OpenApi.Extensions;

namespace Galaxon.Astronomy.AstroAPI.DataTransferObjects;

/// <summary>
/// Encapsulate information about a year for encoding as JSON.
/// </summary>
public record struct YearInfoDto
{
    /// <summary>
    /// Gets the year.
    /// </summary>
    public int Year { get; init; }

    /// <summary>
    /// Gets a value indicating whether the year is a leap year.
    /// </summary>
    public bool IsLeapYear { get; init; }

    /// <summary>
    /// Gets the number of days in the year.
    /// </summary>
    public int NumberOfDays { get; init; }

    /// <summary>
    /// Gets a value indicating whether the year has a leap second.
    /// </summary>
    public bool HasLeapSecond { get; init; }

    /// <summary>
    /// Gets the date of the leap second if it exists.
    /// </summary>
    public string? LeapSecondDate { get; init; }

    /// <summary>
    /// Gets the day of the week of the first day of the year.
    /// </summary>
    public string FirstDayOfYearDayOfWeek { get; init; }

    /// <summary>
    /// Gets the Julian date at the start of the year.
    /// </summary>
    public double StartOfYearJulianDate { get; init; }

    /// <summary>
    /// Gets the century of the year.
    /// </summary>
    public int Century { get; init; }

    /// <summary>
    /// Gets the millennium of the year.
    /// </summary>
    public int Millennium { get; init; }

    /// <summary>
    /// Gets the solar cycle of the year.
    /// </summary>
    public int SolarCycle { get; init; }

    /// <summary>
    /// Gets the main astronomical events occurring this year.
    /// </summary>
    public SortedDictionary<string, string> Events { get; init; }

    /// <summary>
    /// Initializes a new instance of the YearInfoDto struct with specified parameters.
    /// </summary>
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
        Events = new SortedDictionary<string, string>();
    }
}
