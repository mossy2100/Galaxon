using Galaxon.Astronomy.Algorithms.Records;
using Galaxon.Astronomy.Algorithms.Services;
using Galaxon.Astronomy.AstroAPI.DataTransferObjects;
using Microsoft.AspNetCore.Mvc;

namespace Galaxon.Astronomy.AstroAPI.Controllers;

/// <summary>
/// Controller for API endpoints relating to lunar phases.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class SeasonalMarkerController(
    ILogger<SeasonalMarkerController> logger,
    SeasonalMarkerService seasonalMarkerService) : ControllerBase
{
    [HttpGet("InYear")]
    public IActionResult GetSeasonalMarkersInYear(int year)
    {
        try
        {
            // Get the seasonal markers for the specified year.
            List<SeasonalMarker> seasonalMarkers =
                seasonalMarkerService.GetSeasonalMarkersInYear(year);

            // Construct the result.
            List<SeasonalMarkerDto> results = [];
            foreach (SeasonalMarker seasonalMarker in seasonalMarkers)
            {
                results.Add(new SeasonalMarkerDto(seasonalMarker));
            }

            logger.LogInformation("{Count} lunar phases found.", results.Count);

            // Return the seasonal markers as HTTP response in JSON.
            return Ok(results);
        }
        catch (Exception ex)
        {
            return Program.ReturnException(this, ex, logger);
        }
    }
}
