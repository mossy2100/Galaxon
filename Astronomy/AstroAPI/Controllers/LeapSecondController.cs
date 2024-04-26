using Galaxon.Astronomy.AstroAPI.DataTransferObjects;
using Galaxon.Astronomy.Data;
using Galaxon.Astronomy.Data.Models;
using Microsoft.AspNetCore.Mvc;

namespace Galaxon.Astronomy.AstroAPI.Controllers;

/// <summary>
/// Controller for API endpoints relating to lunar phases.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class LeapSecondController(
    ILogger<LeapSecondController> logger,
    AstroDbContext astroDbContext) : ControllerBase
{
    [HttpGet("All")]
    public IActionResult GetLeapSeconds()
    {
        try
        {
            IOrderedQueryable<LeapSecond> leapSeconds =
                astroDbContext.LeapSeconds.OrderBy(ls => ls.LeapSecondDate);
            List<LeapSecondDto> leapSecondDtos =
                leapSeconds.Select(ls => new LeapSecondDto(ls)).ToList();

            logger.LogInformation("{Count} leap seconds reported.", leapSecondDtos.Count);

            // Return the lunar phase as HTTP response in JSON.
            return Ok(leapSecondDtos);
        }
        catch (Exception ex)
        {
            return Program.ReturnException(this, ex, logger);
        }
    }
}
