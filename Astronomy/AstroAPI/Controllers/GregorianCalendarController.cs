using System.Globalization;
using Galaxon.Astronomy.Algorithms.Records;
using Galaxon.Astronomy.Algorithms.Services;
using Galaxon.Astronomy.AstroAPI.DataTransferObjects;
using Galaxon.Astronomy.Data.Enums;
using Galaxon.Astronomy.Data.Models;
using Galaxon.Astronomy.Data.Repositories;
using Galaxon.Core.Exceptions;
using Galaxon.Time;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Extensions;
using Serilog;

namespace Galaxon.Astronomy.AstroAPI.Controllers;

/// <summary>
/// Controller for API endpoints relating to lunar phases.
/// </summary>
[ApiController]
public class GregorianCalendarController(
    AstroObjectRepository astroObjectRepository,
    LeapSecondService leapSecondService,
    MoonService moonService,
    SeasonalMarkerService seasonalMarkerService,
    ApsideService apsideService) : ControllerBase
{
    /// <summary>
    /// Get a bunch of interesting information about the specified year.
    /// </summary>
    /// <param name="year">The year. Must be in the range 1..9999.</param>
    /// <returns>A variety of information about the year.</returns>
    [HttpGet("api/year-info")]
    public IActionResult GetYearInfo(int year)
    {
        try
        {
            // Generate some info about the year.
            bool isLeapYear = GregorianCalendarExtensions.IsLeapYear(year);
            string yearType = isLeapYear ? "leap" : "common";
            int nDays = GregorianCalendarExtensions.GetDaysInYear(year);
            DateOnly? leapSecondDate = leapSecondService.LeapSecondDateForYear(year);
            bool hasLeapSecond = leapSecondDate.HasValue;
            DateOnly firstDayOfYear = new (year, 1, 1);
            double jdut = TimeScales.DateOnlyToJulianDate(firstDayOfYear);
            GregorianCalendar gc = GregorianCalendarExtensions.GetInstance();
            DayOfWeek dayOfWeek = gc.GetDayOfWeek(firstDayOfYear.ToDateTime());
            int century = (year - 1) / 100 + 1;
            int millennium = (year - 1) / 1000 + 1;
            int solarCycle = (year - 1) / 400 + 1;

            // Construct result.
            YearInfoDto dto = new (year, isLeapYear, nDays, hasLeapSecond, leapSecondDate,
                dayOfWeek, jdut, century, millennium, solarCycle);

            // Load planet Earth.
            AstroObjectRecord earth = astroObjectRepository.LoadByName("Earth", "Planet");

            // Add the events.

            // Apsides.
            // Perihelion.
            ApsideEvent perihelion =
                apsideService.GetClosestApside(earth, TimeScales.DecimalYearToJulianDate(year));
            dto.Events.Add(
                TimeScales
                    .JulianDateTerrestrialToDateTimeUniversal(perihelion.JulianDateTerrestrial)
                    .ToIsoString(), "Perihelion");

            // Aphelion.
            ApsideEvent aphelion = apsideService.GetClosestApside(earth,
                TimeScales.DecimalYearToJulianDate(year + 0.5));
            dto.Events.Add(
                TimeScales
                    .JulianDateTerrestrialToDateTimeUniversal(aphelion.JulianDateTerrestrial)
                    .ToIsoString(), "Aphelion");

            // Seasonal markers.
            List<SeasonalMarkerEvent> seasonalMarkerEvents =
                seasonalMarkerService.GetSeasonalMarkersInYear(year);
            foreach (SeasonalMarkerEvent seasonalMarkerEvent in seasonalMarkerEvents)
            {
                dto.Events.Add(seasonalMarkerEvent.DateTimeUtc.ToIsoString(),
                    seasonalMarkerEvent._SeasonalMarkerType.GetDisplayName());
            }

            // Lunar phases.
            List<LunarPhaseEvent> phases = moonService.GetPhasesInYear(year);
            foreach (LunarPhaseEvent phase in phases)
            {
                dto.Events.Add(phase.DateTimeUtc.ToIsoString(), phase.PhaseType.GetDisplayName());
            }

            // Log it.
            Log.Information(
                "Gregorian calendar year {Year} is a {YearType} year with {Days} days.",
                year, yearType, nDays);

            // Return the result as JSON.
            return Ok(dto);
        }
        catch (Exception ex)
        {
            string error = $"Error getting information for year {year}.";
            return Program.ReturnException(this, error, ex);
        }
    }
}
