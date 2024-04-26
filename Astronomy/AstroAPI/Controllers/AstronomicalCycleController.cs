using Galaxon.Astronomy.Algorithms.Services;
using Microsoft.AspNetCore.Mvc;

namespace Galaxon.Astronomy.AstroAPI.Controllers;

/// <summary>
/// Controller for API endpoints relating to Delta-T.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class AstronomicalCycleController(ILogger<AstronomicalCycleController> logger)
    : ControllerBase
{
    [HttpGet("TropicalYearLength")]
    public IActionResult GetTropicalYearLength(int year)
    {
        try
        {
            double tropicalYearLength =
                EarthService.GetTropicalYearInEphemerisDaysForYear(year);

            logger.LogInformation(
                "Length of tropical year {Year} computed to be {TropicalYearLength} days.",
                year, tropicalYearLength);

            // Return the tropical year as JSON.
            return Ok($"{tropicalYearLength:F9} ephemeris days");
        }
        catch (Exception ex)
        {
            return Program.ReturnException(this, ex, logger);
        }
    }

    [HttpGet("SolarDayLength")]
    public IActionResult GetSolarDayLength(int year)
    {
        try
        {
            double solarDayLength = EarthService.GetSolarDayInSeconds(year);

            logger.LogInformation(
                "Length of solar day {Year} computed to be {SolarDayLength} seconds.", year,
                solarDayLength);

            // Return the result as JSON.
            return Ok($"{solarDayLength:F4} seconds");
        }
        catch (Exception ex)
        {
            return Program.ReturnException(this, ex, logger);
        }
    }

    [HttpGet("LunationLength")]
    public IActionResult GetLunationLength(int year)
    {
        try
        {
            double lunationLength = MoonService.GetLunationInEphemerisDaysForYear(year);

            logger.LogInformation(
                "Length of tropical year {Year} computed to be {LunationLength} days.",
                year, lunationLength);

            // Return the result as JSON.
            return Ok($"{lunationLength:F9} ephemeris days");
        }
        catch (Exception ex)
        {
            return Program.ReturnException(this, ex, logger);
        }
    }
}
