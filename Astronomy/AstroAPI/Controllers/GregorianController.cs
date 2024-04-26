using Galaxon.Time;
using Microsoft.AspNetCore.Mvc;

namespace Galaxon.Astronomy.AstroAPI.Controllers;

/// <summary>
/// Controller for API endpoints relating to lunar phases.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class GregorianController(ILogger<GregorianController> logger) : ControllerBase
{
    [HttpGet("YearLength")]
    public IActionResult GetYearLength(int year)
    {
        try
        {
            int days = GregorianCalendarExtensions.GetDaysInYear(year);

            logger.LogInformation("Gregorian calendar year {Year} determined to have {Days} days.", year, days);

            // Return the result as JSON.
            return Ok($"{days} days");
        }
        catch (Exception ex)
        {
            return Program.ReturnException(this, ex, logger);
        }
    }

    [HttpGet("IsLeapYear")]
    public IActionResult GetIsLeapYear(int year)
    {
        try
        {
            bool isLeapYear = GregorianCalendarExtensions.IsLeapYear(year);
            string yearType = isLeapYear ? "leap" : "common";

            logger.LogInformation("Gregorian calendar year {Year} determined to be a {YearType}.", year, yearType);

            // Return the result as JSON.
            return Ok($"{isLeapYear}".ToLower());
        }
        catch (Exception ex)
        {
            return Program.ReturnException(this, ex, logger);
        }
    }
}
