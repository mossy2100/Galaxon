using Galaxon.Astronomy.Algorithms.Services;
using Galaxon.Astronomy.Data.Enums;
using Galaxon.Astronomy.Data.Models;
using Galaxon.Astronomy.Data.Repositories;
using Galaxon.Quantities.Kinds;
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
        AstroObject? planet = astroObjectRepository.LoadByName(planetName, "Planet");
        if (planet == null)
        {
            return Program.ReturnException(this, $"Invalid planet name '{planetName}'.");
        }

        // Convert the apside from a character to an enum value.
        EApside apside;
        switch (apsideCode)
        {
            case 'p' or 'P':
                apside = EApside.Periapsis;
                break;

            case 'a' or 'A':
                apside = EApside.Apoapsis;
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
        double jdtt1;
        double rMetres;
        double rAu;
        DateTime dt1;
        try
        {
            double jdtt = TimeScales.DateTimeToJulianDate(dt);
            (jdtt1, rMetres) = apsideService.GetClosestApside(planet, apside, jdtt);
            dt1 = TimeScales.JulianDateTerrestrialToDateTimeUniversal(jdtt1);
            rAu = rMetres / Length.METRES_PER_ASTRONOMICAL_UNIT;
        }
        catch (Exception ex)
        {
            return Program.ReturnException(this, "Error computing the apside event.", ex);
        }

        // Log it.
        string strApsideDateTime = dt1.ToIsoString();
        Log.Information(
            "{Apside} for {Planet} near {Date} computed to be {EventDateTime}, at a distance of {RadiusMetres} metres ({RadiusAU} AU) from the Sun.",
            apside.ToString(), planet.Name, dt.ToString("O"), strApsideDateTime, rMetres, rAu);

        // Construct the result.
        object result = new
        {
            Planet = planet.Name,
            Apside = apside == EApside.Periapsis ? "perihelion" : "aphelion",
            DateTime = strApsideDateTime,
            Radius = new Dictionary<string, double>
            {
                { "metres", Math.Round(rMetres) },
                { "AU", Math.Round(rAu, 9) }
            }
        };

        // Return the result as JSON.
        return Ok(result);
    }
}
