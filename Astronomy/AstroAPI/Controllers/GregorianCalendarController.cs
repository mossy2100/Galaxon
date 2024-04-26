using System.Globalization;
using Galaxon.Astronomy.AstroAPI.DataTransferObjects;
using Galaxon.Time;
using Microsoft.AspNetCore.Mvc;

namespace Galaxon.Astronomy.AstroAPI.Controllers;

/// <summary>
/// Controller for API endpoints relating to lunar phases.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class GregorianCalendarController(ILogger<GregorianCalendarController> logger)
    : ControllerBase
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
            DateOnly firstDayOfYear = new DateOnly(year, 1, 1);
            double jdut = TimeScales.DateOnlyToJulianDate(firstDayOfYear);
            GregorianCalendar gc = GregorianCalendarExtensions.GetInstance();
            DayOfWeek dayOfWeek = gc.GetDayOfWeek(firstDayOfYear.ToDateTime());
            YearInfoDto dto = new (year, isLeapYear, yearType, nDays, dayOfWeek, jdut);

            // Log it.
            logger.LogInformation(
                "Gregorian calendar year {Year} is a {YearType} year with {Days} days.",
                year, yearType, nDays);

            // Return the result as JSON.
            return Ok(dto);
        }
        catch (Exception ex)
        {
            return Program.ReturnException(this, ex, logger);
        }
    }
}
