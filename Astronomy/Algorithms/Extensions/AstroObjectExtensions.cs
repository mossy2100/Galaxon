using Galaxon.Astronomy.Algorithms.Services;
using Galaxon.Astronomy.Data.Models;
using GeoCoordinatePortable;

namespace Galaxon.Astronomy.Algorithms.Extensions;

/// <summary>
/// Extension methods for AstroObject.
/// </summary>
public static class AstroObjectExtensions
{
    /// <summary>
    /// Calculates the shortest distance between two points along the surface of an oblate spheroid
    /// world using Andoyer's method.
    /// </summary>
    /// <param name="astroObj">The astronomical object representing the world's physical characteristics.</param>
    /// <param name="location1">The geographical coordinates of location 1.</param>
    /// <param name="location2">The geographical coordinates of location 2.</param>
    /// <returns>The distance between the two locations in kilometres.</returns>
    public static double CalculateShortestDistanceBetween(this AstroObject astroObj,
        GeoCoordinate location1, GeoCoordinate location2)
    {
        if (astroObj.Physical == null)
        {
            throw new InvalidOperationException(
                "Cannot calculate the shortest distance between two points on a world without known both the equatorial and the polar radii.");
        }

        return DistanceService.CalculateShortestDistanceBetween(location1, location2,
            astroObj.Physical.EquatorialRadius, astroObj.Physical.PolarRadius);
    }
}
