using Galaxon.Astronomy.Algorithms.Records;
using Galaxon.Astronomy.Algorithms.Services;
using Galaxon.Astronomy.Data.Enums;
using Galaxon.Astronomy.Data.Models;
using Galaxon.Astronomy.Data.Repositories;
using Galaxon.Time;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace Galaxon.Astronomy.AstroAPI.Controllers;

/// <summary>
/// Controller for API endpoints relating to Delta-T.
/// </summary>
[ApiController]
public class ApsideController(
    AstroObjectRepository astroObjectRepository,
    ApsideService apsideService) : ControllerBase
{
    /// <summary>
    /// Calculate the apside of a planet that occurs closest to a given date.
    /// </summary>
    /// <param name="planetName">The planet name. Defaults to "Earth".</param>
    /// <param name="apsideCode">Either 'p' for perihelion or 'a' for aphelion.</param>
    /// <param name="isoDateString">The date in the format YYYY-MM-DD.</param>
    /// <returns></returns>
    [HttpGet("api/closest-apside")]
    public IActionResult GetClosestApside(string? planetName, char apsideCode, string isoDateString)
    {
        // Load the planet.
        // Default to Earth.
        if (string.IsNullOrEmpty(planetName))
        {
            planetName = "Earth";
        }
        AstroObjectRecord planet = astroObjectRepository.LoadByName(planetName, "Planet");

        // Convert the apside from a character to an enum value.
        EApsideType apsideType;
        switch (apsideCode)
        {
            case 'p' or 'P':
                apsideType = EApsideType.Periapsis;
                break;

            case 'a' or 'A':
                apsideType = EApsideType.Apoapsis;
                break;

            default:
                return Program.ReturnException(this,
                    "The apside code should be 'p' or 'P' for perihelion, or 'a' or 'A' for aphelion.");
        }

        // Convert the date string to a DateTime.
        DateTime dt;
        try
        {
            dt = DateTime.Parse(isoDateString);
        }
        catch (Exception ex)
        {
            return Program.ReturnException(this,
                "The date was in an invalid format. Try formatting it as YYYY-MM-DD.", ex);
        }

        // Calculate the apside.
        double rAu;
        DateTime dt1;
        ApsideEvent apsideEvent;
        try
        {
            double jdtt = TimeScales.DateTimeToJulianDate(dt);
            apsideEvent = apsideService.GetClosestApside(planet, jdtt);
            dt1 = apsideEvent.DateTimeUtc;
            rAu = apsideEvent.Radius_AU!.Value;
        }
        catch (Exception ex)
        {
            return Program.ReturnException(this, "Error computing the apside event.", ex);
        }

        // Log it.
        string strApsideDateTime = dt1.ToIsoString();
        Log.Information(
            "{Apside} for {Planet} near {Date} computed to be {EventDateTime}, at a distance of {RadiusAU} AU from the Sun.",
            apsideType.ToString(), planet.Name, dt.ToString("O"), strApsideDateTime, rAu);

        // Construct the result.
        object result = new
        {
            Planet = planet.Name,
            Apside = apsideType == EApsideType.Periapsis ? "perihelion" : "aphelion",
            DateTime = strApsideDateTime,
            Radius_AU = Math.Round(rAu, 9)
        };

        // Return the result as JSON.
        return Ok(result);
    }
}
