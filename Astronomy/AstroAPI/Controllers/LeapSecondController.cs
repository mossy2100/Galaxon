using Galaxon.Astronomy.AstroAPI.DataTransferObjects;
using Galaxon.Astronomy.Data;
using Galaxon.Astronomy.Data.Models;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace Galaxon.Astronomy.AstroAPI.Controllers;

/// <summary>
/// Controller for API endpoints relating to lunar phases.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class LeapSecondController(AstroDbContext astroDbContext) : ControllerBase
{
    [HttpGet("All")]
    public IActionResult GetLeapSeconds()
    {
        try
        {
            // Get the leap second info.
            IOrderedQueryable<LeapSecond> leapSeconds =
                astroDbContext.LeapSeconds.OrderBy(ls => ls.LeapSecondDate);
            List<LeapSecondDto> leapSecondDtos =
                leapSeconds.Select(ls => new LeapSecondDto(ls)).ToList();

            // Log it.
            Log.Information("{Count} leap seconds reported.", leapSecondDtos.Count);

            // Return the lunar phase as HTTP response in JSON.
            return Ok(leapSecondDtos);
        }
        catch (Exception ex)
        {
            string error = "Error retrieving leap second data.";
            return Program.ReturnException(this, error, ex);
        }
    }
}
