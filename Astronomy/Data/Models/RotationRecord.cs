namespace Galaxon.Astronomy.Data.Models;

public class RotationRecord : DatabaseRecord
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
    /// Sidereal rotation period in days.
    /// </summary>
    public double? SiderealRotationPeriod_d { get; set; }

    /// <summary>
    /// Synodic rotation period in days.
    /// </summary>
    public double? SynodicRotationPeriod_d { get; set; }

    /// <summary>
    /// Equatorial rotational velocity in km/s.
    /// </summary>
    public double? EquatorialRotationalVelocity_m_s { get; set; }

    /// <summary>
    /// Axial tilt (obliquity) in degrees.
    /// </summary>
    public double? Obliquity_deg { get; set; }

    /// <summary>
    /// North pole right ascension in degrees.
    /// </summary>
    public double? NorthPoleRightAscension_deg { get; set; }

    /// <summary>
    /// North pole declination in degrees.
    /// </summary>
    public double? NorthPoleDeclination_deg { get; set; }
}
