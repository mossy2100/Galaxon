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
public class LunarPhaseController(ILogger<LunarPhaseController> logger) : ControllerBase
{
    [HttpGet("NearDate")]
    public IActionResult GetLunarPhaseNearDate(string isoDateString)
    {
        try
        {
            DateOnly date = DateOnly.Parse(isoDateString);
            LunarPhaseEvent lunarPhase = MoonService.GetPhaseNearDate(date);
            LunarPhaseDto dto = new (lunarPhase);

            logger.LogInformation("Lunar phases calculated: {Result}", dto);

            // Return the lunar phase as HTTP response in JSON.
            return Ok(dto);
        }
        catch (Exception ex)
        {
            return Program.ReturnException(this, ex, logger);
        }
    }

    [HttpGet("InYear")]
    public IActionResult GetLunarPhasesInYear(int year)
    {
        try
        {
            List<LunarPhaseEvent> phaseEvents = MoonService.GetPhasesInYear(year);

            // Construct the result.
            List<LunarPhaseDto> phaseEventDtos = phaseEvents
                .Select(phaseEvent => new LunarPhaseDto(phaseEvent))
                .ToList();

            logger.LogInformation("{Count} lunar phases found.", phaseEvents.Count);

            // Return the lunar phases as HTTP response in JSON.
            return Ok(phaseEventDtos);
        }
        catch (Exception ex)
        {
            return Program.ReturnException(this, ex, logger);
        }
    }

    [HttpGet("InMonth")]
    public IActionResult GetLunarPhasesInMonth(int year, int month)
    {
        try
        {
            List<LunarPhaseEvent> phaseEvents = MoonService.GetPhasesInMonth(year, month);

            // Construct the result.
            List<LunarPhaseDto> phaseEventDtos = phaseEvents
                .Select(phaseEvent => new LunarPhaseDto(phaseEvent))
                .ToList();

            logger.LogInformation("{Count} lunar phases found.", phaseEvents.Count);

            // Return the lunar phases as HTTP response in JSON.
            return Ok(phaseEventDtos);
        }
        catch (Exception ex)
        {
            return Program.ReturnException(this, ex, logger);
        }
    }
}
