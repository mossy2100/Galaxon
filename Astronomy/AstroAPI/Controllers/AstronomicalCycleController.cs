using Galaxon.Astronomy.Algorithms.Utilities;
using Microsoft.AspNetCore.Mvc;

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
            double solarDay_s = Round(DurationUtility.GetSolarDayInSeconds(year), 6);

            // Log it.
            Slog.Information(
                "Length of solar day at year {Year} computed to be {SolarDayLength} seconds.", year,
                solarDay_s);

            // Construct the result.
            object result = new { seconds = solarDay_s };

            // Return the result as JSON.
            return Ok(result);
        }
        catch (Exception ex)
        {
            return Program.ReturnError(this,
                $"Error computing solar day length for year {year}.", ex);
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
            double ephemerisDays =
                Round(DurationUtility.GetTropicalYearInEphemerisDaysForYear(year), 9);
            double solarDays = Round(DurationUtility.GetTropicalYearInSolarDaysForYear(year), 9);

            // Log it.
            Slog.Information(
                "Length of tropical year {Year} computed to be approximately {EphemerisDays} ephemeris days or approximately {SolarDays} solar days.",
                year, ephemerisDays, solarDays);

            // Construct the result.
            object result = new { ephemerisDays, solarDays };

            // Return the result as JSON.
            return Ok(result);
        }
        catch (Exception ex)
        {
            return Program.ReturnError(this,
                $"Error computing tropical year length for year {year}.", ex);
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
            double ephemerisDays =
                Round(DurationUtility.GetLunationInEphemerisDaysForYear(year), 9);
            double solarDays = Round(DurationUtility.GetLunationInSolarDaysForYear(year), 9);

            // Log it.
            Slog.Information(
                "Length of lunation in year {Year} computed to be approximately {EphemerisDays} ephemeris days or approximately {SolarDays} solar days.",
                year, ephemerisDays, solarDays);

            // Construct the result.
            object result = new { ephemerisDays, solarDays };

            // Return the result as JSON.
            return Ok(result);
        }
        catch (Exception ex)
        {
            return Program.ReturnError(this,
                $"Error computing lunation length for year {year}.", ex);
        }
    }
}
