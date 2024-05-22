using System.Text.RegularExpressions;
using Galaxon.Astronomy.Algorithms.Records;
using Galaxon.Astronomy.Algorithms.Services;
using Galaxon.Astronomy.Algorithms.Utilities;
using Galaxon.Astronomy.Data.Enums;
using Galaxon.Astronomy.Data.Models;
using Galaxon.Astronomy.Data.Repositories;
using Galaxon.Time.Extensions;
using Microsoft.AspNetCore.Mvc;

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
    /// <param name="dateString">The date in ISO format (YYYY-MM-DD). Defaults to today.</param>
    /// <param name="planetName">The planet name. Defaults to "Earth".</param>
    /// <param name="apsideCode">Either 'P' for perihelion, 'A' for aphelion, or omit to detect whichever is closest.</param>
    /// <returns></returns>
    [HttpGet("api/closest-apside")]
    public IActionResult GetClosestApside(string? dateString, string? planetName, char? apsideCode)
    {
        // Get the date to search near.
        DateOnly date;
        if (string.IsNullOrEmpty(dateString))
        {
            // Default to today.
            date = DateOnlyExtensions.Today;
        }
        else
        {
            // Check the date is in the correct format.
            if (!Regex.IsMatch(dateString, @"^\d{4}-\d{2}-\d{2}$"))
            {
                return Program.ReturnException(this,
                    "The date was in an invalid format. Try formatting it as YYYY-MM-DD.");
            }

            // Convert the date string to a DateTime.
            try
            {
                date = DateOnly.Parse(dateString);
            }
            catch (Exception ex)
            {
                return Program.ReturnException(this, "The date is invalid.", ex);
            }
        }

        // Load the planet object. Default to Earth.
        if (string.IsNullOrEmpty(planetName))
        {
            planetName = "Earth";
        }
        AstroObjectRecord planet;
        try
        {
            planet = astroObjectRepository.LoadByName(planetName, "Planet");
        }
        catch (Exception ex)
        {
            return Program.ReturnException(this,
                $"The planet '{planetName}' is not supported. Try: Mercury, Venus, Earth, Mars, Jupiter, Saturn, Uranus, or Neptune.",
                ex);
        }

        // Convert the apside from a character to an enum value.
        EApsideType? apsideType;
        switch (apsideCode)
        {
            case null:
                apsideType = null;
                break;

            case 'p':
            case 'P':
                apsideType = EApsideType.Periapsis;
                break;

            case 'a':
            case 'A':
                apsideType = EApsideType.Apoapsis;
                break;

            default:
                return Program.ReturnException(this, $"Invalid apside code '{apsideCode}'.");
        }

        // Calculate the apside.
        ApsideEvent apsideEvent;
        try
        {
            double jdtt = JulianDateUtility.DateOnlyToJulianDate(date);
            apsideEvent = apsideService.GetClosestApside(planet, jdtt, apsideType);
        }
        catch (Exception ex)
        {
            return Program.ReturnException(this, "Error computing the apside event.", ex);
        }

        // Log it.
        string strApsideDateTime = apsideEvent.DateTimeUtc.ToIsoString();
        string strApsideType =
            apsideEvent.ApsideType == EApsideType.Periapsis ? "perihelion" : "aphelion";
        double radius_AU = Round(apsideEvent.Radius_AU!.Value, 9);
        Slog.Information(
            "Closest {ApsideType} of {Planet} to {Date} computed to occur at {EventDateTime}, at a distance of {RadiusAU} AU.",
            strApsideType, planet.Name, date.ToIsoString(), strApsideDateTime, radius_AU);

        // Construct the result.
        object result = new
        {
            Planet = planet.Name,
            ApsideType = strApsideType,
            DateTime = strApsideDateTime,
            Radius_AU = radius_AU
        };

        // Return the result as JSON.
        return Ok(result);
    }
}
