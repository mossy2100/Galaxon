using Galaxon.Astronomy.Algorithms.Utilities;
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
    public static double CalculateShortestDistanceBetween(this AstroObjectRecord astroObj,
        GeoCoordinate location1, GeoCoordinate location2)
    {
        if (astroObj.Physical == null
            || astroObj.Physical.EquatorialRadius_km == null
            || astroObj.Physical.PolarRadius_km == null)
        {
            throw new InvalidOperationException(
                "To calculate the shortest distance between two points on a world, it is necessary to know both the equatorial and the polar radius.");
        }

        // Check if there are 2 different equatorial radii.
        if (astroObj.Physical.EquatorialRadius2_km != null
            && astroObj.Physical.EquatorialRadius2_km.Value
            != astroObj.Physical.EquatorialRadius_km.Value)
        {
            throw new InvalidOperationException(
                "This method only supports spheroidal worlds. However, this object has two values for equatorial radius, so it is not spheroidal.");
        }

        // Do the calculation.
        return DistanceUtility.CalculateShortestDistanceBetween(location1, location2,
            astroObj.Physical.EquatorialRadius_km.Value, astroObj.Physical.PolarRadius_km.Value);
    }
}
