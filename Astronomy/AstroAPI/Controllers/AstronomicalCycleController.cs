using Galaxon.Astronomy.Algorithms.Services;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace Galaxon.Astronomy.AstroAPI.Controllers;

/// <summary>
/// Controller for API endpoints relating to Delta-T.
/// </summary>
[ApiController]
public class AstronomicalCycleController : ControllerBase
{
    /// <summary>
    /// Calculate the approximate solar day length in SI seconds for a given year.
    /// </summary>
    /// <param name="year">
    /// The year. A decimal value is supported (e.g. 2025.0 for the start of the year, 2025.5 for
    /// the middle of the year, etc.).
    /// </param>
    /// <returns>The approximate solar day length in SI seconds at the specified time.</returns>
    [HttpGet("api/solar-day-length")]
    public IActionResult GetSolarDayLength(double year)
    {
        try
        {
            // Get the approximate solar day length at specified time of the year.
            double solarDayLength_s = EarthService.GetSolarDayInSeconds(year);

            // Construct the result.
            Dictionary<string, double> result = new ()
            {
                ["seconds"] = double.Round(solarDayLength_s, 6)
            };

            // Log it.
            Log.Information(
                "Length of solar day at year {Year} computed to be {SolarDayLength} seconds.", year,
                solarDayLength_s);

            // Return the result as JSON.
            return Ok(result);
        }
        catch (Exception ex)
        {
            string error = $"Error computing solar day length for year {year}.";
            return Program.ReturnException(this, error, ex);
        }
    }

    /// <summary>
    /// Calculate the approximate tropical year length in ephemeris days and solar days for a given
    /// year.
    /// </summary>
    /// <param name="year">
    /// The year. A decimal value is supported (e.g. 2025.0 for the start of the year, 2025.5 for
    /// the middle of the year, etc.).
    /// </param>
    /// <returns>
    /// The approximate tropical day length in ephemeris and solar days at the specified time.
    /// </returns>
    [HttpGet("api/tropical-year-length")]
    public IActionResult GetTropicalYearLength(double year)
    {
        try
        {
            // Get the year length in ephemeris and solar days.
            double ephemerisDays = EarthService.GetTropicalYearInEphemerisDaysForYear(year);
            double solarDays = EarthService.GetTropicalYearInSolarDaysForYear(year);

            // Construct the result.
            Dictionary<string, double> result = new ()
            {
                { "ephemerisDays", Math.Round(ephemerisDays, 6) },
                { "solarDays", Math.Round(solarDays, 6) }
            };

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

    /// <summary>
    /// Calculate the approximate lunation length in ephemeris days and solar days for a given year.
    /// </summary>
    /// <param name="year">
    /// The year. A decimal value is supported (e.g. 2025.0 for the start of the year, 2025.5 for
    /// the middle of the year, etc.).
    /// </param>
    /// <returns>
    /// The approximate lunation length in ephemeris and solar days at the specified time.
    /// </returns>
    [HttpGet("api/lunation-length")]
    public IActionResult GetLunationLength(double year)
    {
        try
        {
            double ephemerisDays = MoonService.GetLunationInEphemerisDaysForYear(year);
            double solarDays = MoonService.GetLunationInSolarDaysForYear(year);

            // Construct the result.
            Dictionary<string, double> result = new ()
            {
                { "ephemerisDays", Math.Round(ephemerisDays, 6) },
                { "solarDays", Math.Round(solarDays, 6) }
            };

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
