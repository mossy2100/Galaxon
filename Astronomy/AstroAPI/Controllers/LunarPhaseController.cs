using Galaxon.Astronomy.Algorithms.Records;
using Galaxon.Astronomy.Algorithms.Services;
using Galaxon.Time;
using Microsoft.AspNetCore.Mvc;

namespace Galaxon.Astronomy.AstroAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class LunarPhaseController(ILogger<LunarPhaseController> logger) : ControllerBase
{
    [HttpGet]
    public IActionResult GetLunarPhase(DateTime dateTime)
    {
        try
        {
            MoonPhase moonPhase = MoonService.GetPhaseNearDateTime(dateTime);

            // Construct the result.
            var result = new
            {
                PhaseType = moonPhase.Type.ToString(),
                DateTimeUtc = moonPhase.DateTimeUtc.ToIsoString()
            };

            logger.LogInformation("Lunar phases calculated: {Result}", result);

            // Return the lunar phase as HTTP response in JSON.
            return Ok(result);
        }
        catch (Exception ex)
        {
            // Return exception details in JSON format.
            string errorMessage = ex.Message;
            if (ex.InnerException != null)
            {
                errorMessage += $"; {ex.InnerException.Message}";
            }
            logger.LogError("Error calculating lunar phases: {ErrorMessage}", errorMessage);

            return StatusCode(500, new
            {
                Error =
                    "There was an error calculating the lunar phase. The error has been logged. Please email shaun@astromultimedia.com if you have questions."
            });
        }
    }
}
