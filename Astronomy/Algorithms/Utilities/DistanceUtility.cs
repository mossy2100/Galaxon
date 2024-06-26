using GeoCoordinatePortable;

namespace Galaxon.Astronomy.Algorithms.Utilities;

/// <summary>
/// Provides methods for calculating distances between geographical coordinates
/// on the surface of an oblate spheroid world using Andoyer's method.
/// </summary>
public static class DistanceUtility
{
    /// <summary>
    /// Calculates the shortest distance between two points along the surface of an
    /// oblate spheroid world using Andoyer's method.
    /// </summary>
    /// <param name="location1">The geographical coordinates of location 1.</param>
    /// <param name="location2">The geographical coordinates of location 2.</param>
    /// <param name="equatorialRadius">The equatorial radius in kilometres.</param>
    /// <param name="polarRadius">The polar radius in kilometres.</param>
    /// <returns>The distance between the two locations in kilometres.</returns>
    /// <remarks>
    /// The formula used in this method is the higher-accuracy Andoyer's method as described on page 85
    /// of Astronomical Algorithms 2nd ed. by Jean Meeus.
    /// Unlike the Haversine formula, which assumes a spherical body, Andoyer's method takes
    /// flattening into account. This algorithm was designed for Earth and therefore assumes:
    /// (a) the object is an oblate spheroid;
    /// (b) coordinates are given in degrees;
    /// (c) the usual coordinate system is used, i.e., latitude is in the range -90..90,
    ///     and longitude is in the range -180..180. Altitude is ignored.
    /// </remarks>
    /// <exception cref="ArgumentOutOfRangeException">If either location is unknown.</exception>
    public static double CalculateShortestDistanceBetween(GeoCoordinate location1,
        GeoCoordinate location2, double equatorialRadius, double polarRadius)
    {
        // Validate inputs.
        if (location1.IsUnknown)
        {
            throw new ArgumentOutOfRangeException(nameof(location1), "Cannot be unknown.");
        }
        if (location2.IsUnknown)
        {
            throw new ArgumentOutOfRangeException(nameof(location2), "Cannot be unknown.");
        }

        // Calculate the flattening.
        double f = (equatorialRadius - polarRadius) / equatorialRadius;

        double F = DegreesToRadians((location1.Latitude + location2.Latitude) / 2);
        double sin2F = Sin2(F);
        double cos2F = Cos2(F);

        double G = DegreesToRadians((location1.Latitude - location2.Latitude) / 2);
        double sin2G = Sin2(G);
        double cos2G = Cos2(G);

        double lambda = DegreesToRadians((location1.Longitude - location2.Longitude) / 2);
        double sin2Lambda = Sin2(lambda);
        double cos2Lambda = Cos2(lambda);

        double S = sin2G * cos2Lambda + cos2F * sin2Lambda;
        double C = cos2G * cos2Lambda + sin2F * sin2Lambda;

        double omega = Atan(Sqrt(S / C));
        double R = Sqrt(S * C) / omega;
        double D = 2 * omega * equatorialRadius;
        double H1 = (3 * R - 1) / 2 / C;
        double H2 = (3 * R + 1) / 2 / S;
        return D * (1 + f * H1 * sin2F * cos2G - f * H2 * cos2F * sin2G);
    }
}
