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
    /// This will be a multiple of 0.5 and uniquely identify an apside for the given object.
    /// This value is called 'k' in Chapter 38, Astronomical Algorithms (2nd ed.) by Jeen Meeus.
    /// It will be 0 or positive for dates after the beginning of 2000, negative for earlier.
    /// Perihelion events have no fractional part; aphelion events have a fraction of 0.5.
    /// </summary>
    public double ApsideNumber { get; set; }

    /// <summary>
    /// The type of apside. This is stored in the database as a single char 'P' or 'A' (not null).
    /// </summary>
    [Column(TypeName = "varchar(9)")]
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
    public double? RadiusAstroPixels_AU { get; set; }
}
