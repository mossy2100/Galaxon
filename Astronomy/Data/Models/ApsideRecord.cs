using Galaxon.Astronomy.Data.Enums;

namespace Galaxon.Astronomy.Data.Models;

public class ApsideRecord : DatabaseRecord
{
    /// <summary>
    /// The id of the astronomical object (planet, minor planet, or moon) for which the apside has
    /// been computed.
    /// </summary>
    public int AstroObjectId { get; set; }

    /// <summary>
    /// The orbit number. Negative values are before J2000, non-negative values are after J2000.
    /// </summary>
    public int OrbitNumber { get; set; }

    /// <summary>
    /// The type of apside. This is stored as 0 for a periapsis or 1 for an apoapsis.
    /// </summary>
    [Column(TypeName = "tinyint unsigned")]
    public EApsideType ApsideType { get; set; }

    /// <summary>
    /// The UTC datetime of the apside according to my calculations.
    /// </summary>
    public DateTime? DateTimeUtcGalaxon { get; set; }

    /// <summary>
    /// The radius (distance to Sun) in AU of the apside according to my calculations.
    /// </summary>
    public double? RadiusGalaxon_AU { get; set; }

    /// <summary>
    /// The UTC datetime of the apside according to USNO.
    /// See: <see href="https://aa.usno.navy.mil/data/Earth_Seasons"/>
    /// </summary>
    public DateTime? DateTimeUtcUsno { get; set; }

    /// <summary>
    /// The UTC datetime of the apside according to AstroPixels.
    /// See: <see href="https://www.astropixels.com/ephemeris/ephemeris.html"/>
    /// </summary>
    public DateTime? DateTimeUtcAstroPixels { get; set; }

    /// <summary>
    /// The radius (distance to Sun) in AU of the apside according to AstroPixels.
    /// </summary>
    [Column(TypeName = "decimal(8,7)")]
    public decimal? RadiusAstroPixels_AU { get; set; }
}
