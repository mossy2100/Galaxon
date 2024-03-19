namespace Galaxon.Astronomy.Data.Models;

public class RotationalRecord : DataObject
{
    // Link to owner.
    public virtual int AstroObjectId { get; set; }

    public virtual AstroObject? AstroObject { get; set; }

    // Sidereal rotation period in days.
    public double? SiderealRotationPeriod { get; set; }

    // Synodic rotation period in days.
    public double? SynodicRotationPeriod { get; set; }

    // Equatorial rotational velocity in km/s.
    public double? EquatRotationVelocity { get; set; }

    // Axial tilt (obliquity) in degrees.
    public double? Obliquity { get; set; }

    // North pole right ascension in degrees.
    public double? NorthPoleRightAscension { get; set; }

    // North pole declination in degrees.
    public double? NorthPoleDeclination { get; set; }
}
