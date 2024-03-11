using Galaxon.Core.Exceptions;
using Galaxon.Numerics.Geometry;
using GeoCoordinatePortable;

namespace Galaxon.Astronomy.Algorithms.Services;

/// <summary>
/// Provides methods for calculating distances between geographical coordinates
/// on the surface of an oblate spheroid world using Andoyer's method.
/// </summary>
public class DistanceService
{
    /// <summary>
    /// Calculates the shortest distance between two points along the surface of an
    /// oblate spheroid world using Andoyer's method.
    /// </summary>
    /// <param name="location1">The geographical coordinates of location 1.</param>
    /// <param name="location2">The geographical coordinates of location 2.</param>
    /// <param name="radiusEquat">The equatorial radius in kilometres.</param>
    /// <param name="radiusPolar">The polar radius in kilometres.</param>
    /// <returns>The distance between the two locations in kilometres.</returns>
    /// <remarks>
    /// The formula used in this method is from Astronomical Algorithms 2nd ed. by
    /// Jean Meeus, page 85. Unlike the Haversine formula, which assumes a spherical
    /// body, Andoyer's method takes flattening into account. This algorithm was
    /// designed for Earth and therefore assumes:
    /// (a) the object is an oblate spheroid;
    /// (b) coordinates are given in degrees;
    /// (c) the usual coordinate system is used, i.e., latitude is in the range -90..90,
    ///     and longitude is in the range -180..180. Altitude is ignored.
    /// </remarks>
    public static double CalculateShortestDistanceBetween(GeoCoordinate location1,
        GeoCoordinate location2, double radiusEquat, double radiusPolar)
    {
        // Validate inputs.
        if (location1.IsUnknown)
        {
            throw new ArgumentInvalidException(nameof(location1), "Cannot be unknown.");
        }
        if (location2.IsUnknown)
        {
            throw new ArgumentInvalidException(nameof(location2), "Cannot be unknown.");
        }

        // Calculate the flattening.
        double f = (radiusEquat - radiusPolar) / radiusEquat;

        double F = Angles.DegreesToRadians((location1.Latitude + location2.Latitude) / 2);
        double sin2F = Angles.Sin2(F);
        double cos2F = Angles.Cos2(F);

        double G = Angles.DegreesToRadians((location1.Latitude - location2.Latitude) / 2);
        double sin2G = Angles.Sin2(G);
        double cos2G = Angles.Cos2(G);

        double lambda = Angles.DegreesToRadians((location1.Longitude - location2.Longitude) / 2);
        double sin2Lambda = Angles.Sin2(lambda);
        double cos2Lambda = Angles.Cos2(lambda);

        double S = sin2G * cos2Lambda + cos2F * sin2Lambda;
        double C = cos2G * cos2Lambda + sin2F * sin2Lambda;

        double omega = Atan(Sqrt(S / C));
        double R = Sqrt(S * C) / omega;
        double D = 2 * omega * radiusEquat;
        double H1 = (3 * R - 1) / 2 / C;
        double H2 = (3 * R + 1) / 2 / S;
        return D * (1 + f * H1 * sin2F * cos2G - f * H2 * cos2F * sin2G);
    }
}
