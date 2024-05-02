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
    [HttpGet("api/closest-apside")]
    public IActionResult GetClosestApside(string planetName, int apsideNumber, string dateString)
    {
        // Load the planet.
        AstroObject? planet = astroObjectRepository.LoadByName(planetName, "Planet");
        if (planet == null)
        {
            string error = $"Invalid planet name '{planetName}'.";
            return Program.ReturnException(this, error);
        }

        // Convert the apside from an integer to an enum value.
        EApside apside;
        try
        {
            apside = (EApside)apsideNumber;
        }
        catch (Exception ex)
        {
            string error = "The apside number should be 0 for perihelion and 1 for aphelion.";
            return Program.ReturnException(this, error, ex);
        }

        // Convert the date string to a DateTime.
        DateTime dt;
        try
        {
            dt = DateTime.Parse(dateString);
        }
        catch (Exception ex)
        {
            string error = "The date was in an invalid format. Try using the format YYYY-MM-DD.";
            return Program.ReturnException(this, error, ex);
        }

        // Calculate the apside.
        DateTime dtApside;
        try
        {
            dtApside = apsideService.GetClosestApsideApprox(planet, apside, dt);
        }
        catch (Exception ex)
        {
            string error = "Error computing the apside event.";
            return Program.ReturnException(this, error, ex);
        }

        // Log it.
        Log.Information("{Apside} for {Planet} nearest {Date} computed to be {EventDateTime}",
            apside.ToString(), planet.Name, dt.ToString("O"), dtApside.ToIsoString(true));

        // Construct the result.
        var result = new
        {
            Planet = planet.Name,
            Apside = apside == EApside.Periapsis ? "perihelion" : "aphelion",
            DateTime = dtApside.ToIsoString(true)
        };

        // Return the result as JSON.
        return Ok(result);
    }
}
