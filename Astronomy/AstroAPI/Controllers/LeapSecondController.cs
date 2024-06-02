using Galaxon.Astronomy.AstroAPI.DataTransferObjects;
using Galaxon.Astronomy.Data;
using Galaxon.Astronomy.Data.Models;
using Microsoft.AspNetCore.Mvc;

namespace Galaxon.Astronomy.AstroAPI.Controllers;

/// <summary>
/// Controller for API endpoints relating to lunar phases.
/// </summary>
[ApiController]
public class LeapSecondController(AstroDbContext astroDbContext) : ControllerBase
{
    /// <summary>
    /// Get data on the leap seconds to date.
    /// </summary>
    /// <returns>
    /// All leap seconds so far, with the value (1 for an inserted second, -1 for a deleted second)
    /// and the date it was was inserted or deleted.
    /// </returns>
    [HttpGet("api/leap-seconds")]
    public IActionResult GetLeapSeconds()
    {
        try
        {
            // Get the leap second info.
            IOrderedQueryable<LeapSecondRecord> leapSeconds =
                astroDbContext.LeapSeconds.OrderBy(ls => ls.Date);
            List<LeapSecondDto> leapSecondDtos =
                leapSeconds.Select(ls => new LeapSecondDto(ls)).ToList();

            // Log it.
            Slog.Information("{Count} leap seconds reported.", leapSecondDtos.Count);

            // Return the lunar phase as HTTP response in JSON.
            return Ok(leapSecondDtos);
        }
        catch (Exception ex)
        {
            string error = "Error retrieving leap second data.";
            return Program.ReturnError(this, error, ex);
        }
    }
}
