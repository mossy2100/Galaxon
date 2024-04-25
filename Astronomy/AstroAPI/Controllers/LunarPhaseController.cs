using Galaxon.Astronomy.Algorithms.Records;
using Galaxon.Astronomy.Algorithms.Services;
using Galaxon.Astronomy.AstroAPI.DataTransferObjects;
using Galaxon.Core.Types;
using Microsoft.AspNetCore.Mvc;

namespace Galaxon.Astronomy.AstroAPI.Controllers;

/// <summary>
/// Controller for API endpoints relating to lunar phases.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class LunarPhaseController : ControllerBase
{
    private readonly ILogger<LunarPhaseController> _logger;

    public LunarPhaseController(ILogger<LunarPhaseController> logger)
    {
        _logger = logger;
    }

    [HttpGet("NearDate")]
    public IActionResult GetLunarPhaseNearDate(string date)
    {
        try
        {
            DateOnly d = DateOnly.Parse(date);
            LunarPhase lunarPhase = MoonService.GetPhaseNearDate(d);
            LunarPhaseDto dto = new (lunarPhase);

            _logger.LogInformation("Lunar phases calculated: {Result}", dto);

            // Return the lunar phase as HTTP response in JSON.
            return Ok(dto);
        }
        catch (Exception ex)
        {
            return Program.ReturnException(this, ex, _logger);
        }
    }

    [HttpGet("InYear")]
    public IActionResult GetLunarPhasesInYear(int year)
    {
        try
        {
            List<LunarPhase> lunarPhases = MoonService.GetPhasesInYear(year);

            // Construct the result.
            List<LunarPhaseDto> results = [];
            foreach (LunarPhase lunarPhase in lunarPhases)
            {
                results.Add(new LunarPhaseDto(lunarPhase));
            }

            _logger.LogInformation("{Count} lunar phases found.", lunarPhases.Count);

            // Return the lunar phases as HTTP response in JSON.
            return Ok(results);
        }
        catch (Exception ex)
        {
            return Program.ReturnException(this, ex, _logger);
        }
    }

    [HttpGet("InMonth")]
    public IActionResult GetLunarPhasesInMonth(int year, int month)
    {
        try
        {
            List<LunarPhase> lunarPhases = MoonService.GetPhasesInMonth(year, month);

            // Construct the result.
            List<LunarPhaseDto> results = [];
            foreach (LunarPhase lunarPhase in lunarPhases)
            {
                results.Add(new LunarPhaseDto(lunarPhase));
            }

            _logger.LogInformation("{Count} lunar phases found.", lunarPhases.Count);

            // Return the lunar phases as HTTP response in JSON.
            return Ok(results);
        }
        catch (Exception ex)
        {
            return Program.ReturnException(this, ex, _logger);
        }
    }
}
