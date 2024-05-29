namespace Galaxon.Astronomy.Data.Models;

/// <summary>
/// Represents an orbital record associated with an astronomical object.
/// </summary>
public class OrbitRecord : DatabaseRecord
{
    /// <summary>
    /// Primary key of the astronomical object this component relates to.
    /// </summary>
    public int AstroObjectId { get; set; }

    /// <summary>
    /// Astronomical object this component relates to.
    /// </summary>
    public virtual AstroObjectRecord? AstroObject { get; set; }

    /// <summary>
    /// Date/time of reference for the current orbital parameters.
    /// </summary>
    public DateTime? Epoch { get; set; }

    /// <summary>
    /// Apoapsis of the orbit in kilometres.
    /// </summary>
    public double? Apoapsis_km { get; set; }

    /// <summary>
    /// Periapsis of the orbit in kilometres.
    /// </summary>
    public double? Periapsis_km { get; set; }

    /// <summary>
    /// Semi-major axis of the orbit in kilometres.
    /// </summary>
    public double? SemiMajorAxis_km { get; set; }

    /// <summary>
    /// Eccentricity of the orbit.
    /// </summary>
    public double? Eccentricity { get; set; }

    /// <summary>
    /// Sidereal orbit period in days.
    /// </summary>
    public double? SiderealOrbitPeriod_d { get; set; }

    /// <summary>
    /// Synodic orbit period in days.
    /// </summary>
    public double? SynodicOrbitPeriod_d { get; set; }

    /// <summary>
    /// Average orbital speed in kilometres per second (km/s).
    /// </summary>
    public double? AverageOrbitalSpeed_km_s { get; set; }

    /// <summary>
    /// Mean anomaly in degrees.
    /// </summary>
    public double? MeanAnomaly_deg { get; set; }

    /// <summary>
    /// Inclination to the ecliptic in degrees.
    /// </summary>
    public double? Inclination_deg { get; set; }

    /// <summary>
    /// Longitude of the ascending node in degrees.
    /// </summary>
    public double? LongitudeAscendingNode_deg { get; set; }

    /// <summary>
    /// Argument of periapsis in degrees.
    /// </summary>
    public double? ArgumentPeriapsis_deg { get; set; }

    /// <summary>
    /// Mean motion in degrees per day.
    /// </summary>
    public double? MeanMotion_deg_d => Angles.DEGREES_PER_CIRCLE / SiderealOrbitPeriod_d;

    /// <summary>
    /// Gets the longitude of the periapsis in degrees.
    /// See: <see href="https://en.wikipedia.org/wiki/Longitude_of_the_periapsis"/>
    /// </summary>
    public double? LongitudePeriapsis_deg => LongitudeAscendingNode_deg + ArgumentPeriapsis_deg;

    /// <summary>
    /// Calculates the approximate true anomaly in degrees.
    /// See: <see href="https://en.wikipedia.org/wiki/True_anomaly#From_the_mean_anomaly"/>
    ///
    /// Note that for reasons of accuracy, this approximation is usually limited to orbits
    /// where the eccentricity (e) is small.
    /// </summary>
    public double? ApproxTrueAnomaly
    {
        get
        {
            if (MeanAnomaly_deg == null || Eccentricity == null)
            {
                return null;
            }
            double M = MeanAnomaly_deg.Value;
            double e = Eccentricity.Value;
            double e3 = Pow(e, 3);
            return M + (2 * e - e3 / 4) * Sin(M) + 5 * e * e * Sin(2 * M) / 4
                + 13 * e3 * Sin(3 * M) / 12;
        }
    }
}
