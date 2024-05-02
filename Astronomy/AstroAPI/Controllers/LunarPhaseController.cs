using Galaxon.Astronomy.Algorithms.Records;
using Galaxon.Astronomy.Algorithms.Services;
using Galaxon.Astronomy.AstroAPI.DataTransferObjects;
using Galaxon.Time;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace Galaxon.Astronomy.AstroAPI.Controllers;

/// <summary>
/// Controller for API endpoints relating to lunar phases.
/// </summary>
[ApiController]
public class LunarPhaseController(MoonService moonService) : ControllerBase
{
    [HttpGet("api/lunar-phase-near-date")]
    public IActionResult GetLunarPhaseNearDate(string isoDateString)
    {
        try
        {
            // Get the lunar phase.
            DateOnly date = DateOnly.Parse(isoDateString);
            LunarPhaseEvent lunarPhase = moonService.GetPhaseNearDate(date);

            // Construct the result.
            LunarPhaseDto dto = new (lunarPhase);

            // Log it.
            Log.Information("Lunar phases calculated: {Result}", dto);

            // Return the lunar phase as JSON.
            return Ok(dto);
        }
        catch (Exception ex)
        {
            string error = $"Error computing lunar phase near {isoDateString}.";
            return Program.ReturnException(this, error, ex);
        }
    }

    [HttpGet("api/lunar-phases-in-year")]
    public IActionResult GetLunarPhasesInYear(int year)
    {
        try
        {
            // Get the lunar phase data.
            List<LunarPhaseEvent> phaseEvents = moonService.GetPhasesInYear(year);

            // Construct the result.
            List<LunarPhaseDto> phaseEventDtos = phaseEvents
                .Select(phaseEvent => new LunarPhaseDto(phaseEvent))
                .ToList();

            // Log it.
            Log.Information("{Count} lunar phases found.", phaseEvents.Count);

            // Return the lunar phases as HTTP response in JSON.
            return Ok(phaseEventDtos);
        }
        catch (Exception ex)
        {
            string error = "Error computing lunar phases for year {year}.";
            return Program.ReturnException(this, error, ex);
        }
    }

    [HttpGet("api/lunar-phases-in-month")]
    public IActionResult GetLunarPhasesInMonth(int year, int month)
    {
        try
        {
            // Get the lunar phases.
            List<LunarPhaseEvent> phaseEvents = moonService.GetPhasesInMonth(year, month);

            // Construct the result.
            List<LunarPhaseDto> phaseEventDtos = phaseEvents
                .Select(phaseEvent => new LunarPhaseDto(phaseEvent))
                .ToList();

            // Log it.
            Log.Information("{Count} lunar phases found.", phaseEvents.Count);

            // Return the lunar phases as HTTP response in JSON.
            return Ok(phaseEventDtos);
        }
        catch (Exception ex)
        {
            string monthName = GregorianCalendarExtensions.MonthNumberToName(month);
            string error = $"Error computing lunar phases for {monthName}, {year}.";
            return Program.ReturnException(this, error, ex);
        }
    }
}
