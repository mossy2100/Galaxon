using Galaxon.Astronomy.Algorithms.Records;
using Galaxon.Astronomy.Algorithms.Services;
using Galaxon.Astronomy.AstroAPI.DataTransferObjects;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace Galaxon.Astronomy.AstroAPI.Controllers;

/// <summary>
/// Controller for API endpoints relating to lunar phases.
/// </summary>
[ApiController]
public class SeasonalMarkerController(SeasonalMarkerService seasonalMarkerService) : ControllerBase
{
    /// <summary>
    /// Retrieves the seasonal markers for the specified year.
    /// </summary>
    /// <param name="year">The year for which to retrieve the seasonal markers.</param>
    /// <returns>
    /// Data on the seasonal markers occurring in the specified year.
    /// </returns>
    [HttpGet("api/seasonal-markers-in-year")]
    public IActionResult GetSeasonalMarkersInYear(int year)
    {
        try
        {
            // Get the seasonal markers for the specified year.
            List<SeasonalMarkerEvent> seasonalMarkerEvents =
                seasonalMarkerService.GetSeasonalMarkersInYear(year);

            // Construct the result.
            SeasonalMarkersDto results = new ();
            foreach (SeasonalMarkerEvent seasonalMarkerEvent in seasonalMarkerEvents)
            {
                results.Add(seasonalMarkerEvent);
            }

            // Log it.
            Log.Information("Seasonal markers found for year {Year}: {Results}", year, results);

            // Return the seasonal markers as HTTP response in JSON.
            return Ok(results);
        }
        catch (Exception ex)
        {
            string error = $"Error computing seasonal markers for year {year}.";
            return Program.ReturnException(this, error, ex);
        }
    }
}
