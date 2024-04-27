using System.Globalization;
using Galaxon.Astronomy.Algorithms.Services;
using Galaxon.Astronomy.AstroAPI.DataTransferObjects;
using Galaxon.Time;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace Galaxon.Astronomy.AstroAPI.Controllers;

/// <summary>
/// Controller for API endpoints relating to lunar phases.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class GregorianCalendarController(LeapSecondService leapSecondService) : ControllerBase
{
    [HttpGet("YearInfo")]
    public IActionResult GetYearInfo(int year)
    {
        try
        {
            // Generate some info about the year.
            bool isLeapYear = GregorianCalendarExtensions.IsLeapYear(year);
            string yearType = isLeapYear ? "leap" : "common";
            int nDays = GregorianCalendarExtensions.GetDaysInYear(year);
            DateOnly? leapSecondDate = leapSecondService.LeapSecondDateForYear(year);
            bool hasLeapSecond = leapSecondDate.HasValue;
            DateOnly firstDayOfYear = new (year, 1, 1);
            double jdut = TimeScales.DateOnlyToJulianDate(firstDayOfYear);
            GregorianCalendar gc = GregorianCalendarExtensions.GetInstance();
            DayOfWeek dayOfWeek = gc.GetDayOfWeek(firstDayOfYear.ToDateTime());
            int century = (year - 1) / 100 + 1;
            int millennium = (year - 1) / 1000 + 1;
            int solarCycle = (year - 1) / 400 + 1;

            // Construct result.
            YearInfoDto dto = new (year, isLeapYear, nDays, hasLeapSecond, leapSecondDate,
                dayOfWeek, jdut, century, millennium, solarCycle);

            // Log it.
            Log.Information(
                "Gregorian calendar year {Year} is a {YearType} year with {Days} days.",
                year, yearType, nDays);

            // Return the result as JSON.
            return Ok(dto);
        }
        catch (Exception ex)
        {
            string error = $"Error getting information for year {year}.";
            return Program.ReturnException(this, error, ex);
        }
    }
}
