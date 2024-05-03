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
    [HttpGet("api/closest-apside")]
    public IActionResult GetClosestApside(string planetName, char apsideCode, string isoDateString)
    {
        // Load the planet.
        AstroObject? planet = astroObjectRepository.LoadByName(planetName, "Planet");
        if (planet == null)
        {
            return Program.ReturnException(this, $"Invalid planet name '{planetName}'.");
        }

        // Convert the apside from an integer to an enum value.
        EApside apside;
        if (apsideCode is 'p' or 'P')
        {
            apside = EApside.Periapsis;
        }
        else if (apsideCode is 'a' or 'A')
        {
            apside = EApside.Apoapsis;
        }
        else
        {
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
        var result = new
        {
            Planet = planet.Name,
            Apside = apside == EApside.Periapsis ? "perihelion" : "aphelion",
            DateTime = strApsideDateTime,
            Radius = rMetres,
            RadiusInAU = rAu
        };

        // Return the result as JSON.
        return Ok(result);
    }
}
