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
                EarthService.GetTropicalYearLengthInEphemerisDaysForYear(year);

            logger.LogInformation(
                "Length of tropical year {Year} computed to be {TropicalYearLength} days.",
                year, tropicalYearLength);

            // Return the delta-T value as HTTP response in JSON.
            return Ok(tropicalYearLength);
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
            double solarDayLength = EarthService.GetSolarDayLength(year);

            logger.LogInformation(
                "Length of solar day {Year} computed to be {SolarDayLength} seconds.", year,
                solarDayLength);

            // Return the delta-T value as HTTP response in JSON.
            return Ok(solarDayLength);
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
            double lunationLength = MoonService.GetLunationLengthInEphemerisDaysForYear(year);

            logger.LogInformation(
                "Length of tropical year {Year} computed to be {LunationLength} days.",
                year, lunationLength);

            // Return the lunation length as HTTP response in JSON.
            return Ok(lunationLength);
        }
        catch (Exception ex)
        {
            return Program.ReturnException(this, ex, logger);
        }
    }
}
