using Galaxon.Astronomy.Algorithms.Records;
using Galaxon.Astronomy.Algorithms.Services;
using Galaxon.Time;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Extensions;
using Serilog;

namespace Galaxon.Astronomy.AstroAPI.Controllers;

/// <summary>
/// Controller for API endpoints relating to lunar phases.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class SeasonalMarkerController(SeasonalMarkerService seasonalMarkerService) : ControllerBase
{
    [HttpGet("InYear")]
    public IActionResult GetSeasonalMarkersInYear(int year)
    {
        try
        {
            // Get the seasonal markers for the specified year.
            List<SeasonalMarkerEvent> seasonalMarkerEvents =
                seasonalMarkerService.GetSeasonalMarkersInYear(year);

            // Construct the result.
            Dictionary<string, string> results = new ();
            foreach (SeasonalMarkerEvent seasonalMarkerEvent in seasonalMarkerEvents)
            {
                results[seasonalMarkerEvent.SeasonalMarker.GetDisplayName()] = DateTimeExtensions
                    .RoundToNearestMinute(seasonalMarkerEvent.DateTimeUtc).ToString("u");
            }

            // Log it.
            Log.Information("{Count} lunar phases found.", results.Count);

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
