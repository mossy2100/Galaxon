using Galaxon.Astronomy.Algorithms.Services;
using Galaxon.Astronomy.AstroAPI.DataTransferObjects;
using Galaxon.Numerics.Extensions.FloatingPoint;
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
    [HttpGet("SolarDayLength")]
    public IActionResult GetSolarDayLength(int year)
    {
        try
        {
            // Get the approximate solar day length at the beginning of the given year.
            double solarDayLength = EarthService.GetSolarDayInSeconds(year);

            // Log it.
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

    [HttpGet("TropicalYearLength")]
    public IActionResult GetTropicalYearLength(int year)
    {
        try
        {
            // Get the year length in ephemeris and solar days.
            double ephemerisDays = EarthService.GetTropicalYearInEphemerisDaysForYear(year);
            double solarDays = EarthService.GetTropicalYearInSolarDaysForYear(year);

            // Construct result.
            TropicalYearDto dto = new (ephemerisDays, solarDays);

            // Log it.
            logger.LogInformation(
                "Length of tropical year {Year} computed to be {EphemerisDays} ephemeris days or approximately {SolarDays} solar days.",
                year, ephemerisDays, solarDays);

            // Return the result as JSON.
            return Ok(dto);
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
            double ephemerisDays = MoonService.GetLunationInEphemerisDaysForYear(year);
            double solarDays = MoonService.GetLunationInSolarDaysForYear(year);

            // Construct result.
            LunationDto dto = new (ephemerisDays, solarDays);

            // Log it.
            logger.LogInformation(
                "Length of lunation in year {Year} computed to be {EphemerisDays} ephemeris days or approximately {SolarDays} solar days.",
                year, ephemerisDays, solarDays);

            // Return the result as JSON.
            return Ok(dto);
        }
        catch (Exception ex)
        {
            return Program.ReturnException(this, ex, logger);
        }
    }
}
