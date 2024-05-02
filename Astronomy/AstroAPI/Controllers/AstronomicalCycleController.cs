using Galaxon.Astronomy.Algorithms.Services;
using Galaxon.Astronomy.AstroAPI.DataTransferObjects;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace Galaxon.Astronomy.AstroAPI.Controllers;

/// <summary>
/// Controller for API endpoints relating to Delta-T.
/// </summary>
[ApiController]
public class AstronomicalCycleController : ControllerBase
{
    [HttpGet("api/solar-day-length")]
    public IActionResult GetSolarDayLength(double year)
    {
        try
        {
            // Get the approximate solar day length at specified time of the year.
            double solarDayLength = EarthService.GetSolarDayInSeconds(year);

            // Construct the result.
            Dictionary<string, double> result = new ()
            {
                ["seconds"] = double.Round(solarDayLength, 6)
            };

            // Log it.
            Log.Information(
                "Length of solar day at year {Year} computed to be {SolarDayLength} seconds.", year,
                solarDayLength);

            // Return the result as JSON.
            return Ok(result);
        }
        catch (Exception ex)
        {
            string error = $"Error computing solar day length for year {year}.";
            return Program.ReturnException(this, error, ex);
        }
    }

    [HttpGet("api/tropical-year-length")]
    public IActionResult GetTropicalYearLength(double year)
    {
        try
        {
            // Get the year length in ephemeris and solar days.
            double ephemerisDays = EarthService.GetTropicalYearInEphemerisDaysForYear(year);
            double solarDays = EarthService.GetTropicalYearInSolarDaysForYear(year);

            // Construct the result.
            TropicalYearDto result = new (ephemerisDays, solarDays);

            // Log it.
            Log.Information(
                "Length of tropical year {Year} computed to be {EphemerisDays} ephemeris days or approximately {SolarDays} solar days.",
                year, ephemerisDays, solarDays);

            // Return the result as JSON.
            return Ok(result);
        }
        catch (Exception ex)
        {
            string error = $"Error computing tropical year length for year {year}.";
            return Program.ReturnException(this, error, ex);
        }
    }

    [HttpGet("api/lunation-length")]
    public IActionResult GetLunationLength(double year)
    {
        try
        {
            double ephemerisDays = MoonService.GetLunationInEphemerisDaysForYear(year);
            double solarDays = MoonService.GetLunationInSolarDaysForYear(year);

            // Construct the result.
            LunationDto result = new (ephemerisDays, solarDays);

            // Log it.
            Log.Information(
                "Length of lunation in year {Year} computed to be {EphemerisDays} ephemeris days or approximately {SolarDays} solar days.",
                year, ephemerisDays, solarDays);

            // Return the result as JSON.
            return Ok(result);
        }
        catch (Exception ex)
        {
            string error = $"Error computing lunation length for year {year}.";
            return Program.ReturnException(this, error, ex);
        }
    }
}
