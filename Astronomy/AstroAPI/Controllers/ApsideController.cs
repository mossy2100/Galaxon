using System.Linq.Expressions;
using System.Net.WebSockets;
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
    /// <param name="date">The date in ISO format (YYYY-MM-DD). Defaults to today.</param>
    /// <param name="planet">
    /// The planet name. Supports major planets only; therefore, the acceptable values are
    /// "Mercury", "Venus", "Earth", "Mars", "Jupiter", "Saturn", "Uranus", and "Neptune".
    /// Case-insensitive. Defaults to "Earth".
    /// </param>
    /// <param name="apside">
    /// Either 'P' to find the perihelion event, 'A' for aphelion, or omit to find whichever is
    /// closer.
    /// </param>
    /// <param name="comparison">
    /// The following case-insensitive values are accepted:
    ///     'closest' (default) means find the event closest to the specified date.
    ///     'before' means find the event before or on the specified date.
    ///     'after' means find the event after or on the specified date.
    /// Dates are taken to be UTC.
    /// </param>
    /// <returns></returns>
    [HttpGet("apside")]
    public IActionResult GetClosestApside(string? date, string planet = "Earth",
        char? apside = null, string comparison = "closest")
    {
        // Get the date to search near.
        DateTime dt;
        if (string.IsNullOrEmpty(date))
        {
            // Default to today.
            dt = DateTime.UtcNow;
        }
        else
        {
            // Check the date is in the correct format.
            if (!Regex.IsMatch(date, @"^\d{4}-\d{2}-\d{2}$"))
            {
                return Program.ReturnError(this,
                    $"The date was in an invalid format. Please format it as 'YYYY-MM-DD' (without the quote marks). For example '{DateOnlyExtensions.Today.ToIsoString()}'.");
            }

            // Convert the date string to a DateTime.
            try
            {
                dt = DateTime.Parse(date);
            }
            catch (Exception ex)
            {
                return Program.ReturnError(this, "The date is invalid.", ex);
            }
        }

        // Load the planet object. Default to Earth.
        if (string.IsNullOrEmpty(planet))
        {
            planet = "Earth";
        }
        AstroObjectRecord planetObject;
        try
        {
            planetObject = astroObjectRepository.LoadByName(planet, "Planet");
        }
        catch (Exception ex)
        {
            return Program.ReturnError(this,
                $"The planet '{planet}' is not supported. Try: Mercury, Venus, Earth, Mars, Jupiter, Saturn, Uranus, or Neptune.",
                ex);
        }

        // Convert the apside from a character to an enum value.
        EApsideType? apsideType;
        if (apside == null)
        {
            apsideType = null;
        }
        else
        {
            switch (char.ToUpper(apside.Value))
            {
                case 'P':
                    apsideType = EApsideType.Periapsis;
                    break;

                case 'A':
                    apsideType = EApsideType.Apoapsis;
                    break;

                default:
                    return Program.ReturnError(this,
                        "Invalid apside code. Use 'P' for perihelion, 'A' for aphelion, or omit for closest.");
            }
        }

        // Convert the comparison argument into an expression type.
        ExpressionType expressionType;
        comparison = comparison.ToLower();
        if (comparison == "closest")
        {
            expressionType = ExpressionType.Default;
        }
        else if (comparison == "before")
        {
            expressionType = ExpressionType.LessThanOrEqual;
        }
        else if (comparison == "after")
        {
            expressionType = ExpressionType.GreaterThanOrEqual;
        }
        else
        {
            return Program.ReturnError(this,
                "Invalid comparison type. Use 'closest', 'before', or 'after'.");
        }

        // Calculate the apside.
        ApsideEvent apsideEvent;
        try
        {
            double jdtt = JulianDateUtility.DateTimeUniversalToJulianDateTerrestrial(dt);
            apsideEvent = apsideService.GetApside(planetObject, jdtt, apsideType, expressionType);
        }
        catch (Exception ex)
        {
            return Program.ReturnError(this, "Error computing the apside event.", ex);
        }

        // Log it.
        string strApsideDateTime = apsideEvent.DateTimeUtc.ToIsoString();
        string strApsideType =
            apsideEvent.ApsideType == EApsideType.Periapsis ? "perihelion" : "aphelion";
        double radius_AU = Round(apsideEvent.Radius_AU!.Value, 9);
        Slog.Information(
            "Closest {ApsideType} of {Planet} to {Date} computed to occur at {EventDateTime}, at a distance of {RadiusAU} AU.",
            strApsideType, planetObject.Name, dt.ToIsoString(), strApsideDateTime, radius_AU);

        // Construct the result.
        object result = new
        {
            Planet = planetObject.Name,
            ApsideType = strApsideType,
            DateTime = strApsideDateTime,
            Radius_AU = radius_AU
        };

        // Return the result as JSON.
        return Ok(result);
    }
}
