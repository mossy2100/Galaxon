using Galaxon.Astronomy.Algorithms.Records;
using Galaxon.Astronomy.Algorithms.Services;
using Galaxon.Astronomy.AstroAPI.DataTransferObjects;
using Galaxon.Core.Types;
using Microsoft.AspNetCore.Mvc;

namespace Galaxon.Astronomy.AstroAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class LunarPhaseController : ControllerBase
{
    private readonly ILogger<LunarPhaseController> _logger;

    public LunarPhaseController(ILogger<LunarPhaseController> logger)
    {
        _logger = logger;
    }

    [HttpGet]
    [Route("/LunarPhaseNearDate")]
    public IActionResult GetLunarPhaseNear(DateTime dateTime)
    {
        try
        {
            MoonPhase moonPhase = MoonService.GetPhaseNearDateTime(dateTime);

            // Construct the result.
            LunarPhaseDto result = new ()
            {
                PhaseType = moonPhase.Type.GetDescription(),
                DateTimeUTC = $"{moonPhase.DateTimeUtc:s}"
            };

            _logger.LogInformation("Lunar phases calculated: {Result}", result);

            // Return the lunar phase as HTTP response in JSON.
            return Ok(result);
        }
        catch (Exception ex)
        {
            return Program.ReturnException(this, ex, _logger);
        }
    }

    [HttpGet]
    [Route("/LunarPhasesInYear")]
    public IActionResult GetLunarPhasesInYear(int year)
    {
        try
        {
            List<MoonPhase> moonPhases = MoonService.GetPhasesInYear(year);

            // Construct the result.
            List<LunarPhaseDto> results = [];
            foreach (MoonPhase moonPhase in moonPhases)
            {
                results.Add(new LunarPhaseDto
                {
                    PhaseType = moonPhase.Type.GetDescription(),
                    DateTimeUTC = $"{moonPhase.DateTimeUtc:s}"
                });
            }

            _logger.LogInformation("{Count} lunar phases found.", moonPhases.Count);

            // Return the lunar phases as HTTP response in JSON.
            return Ok(results);
        }
        catch (Exception ex)
        {
            return Program.ReturnException(this, ex, _logger);
        }
    }
}
