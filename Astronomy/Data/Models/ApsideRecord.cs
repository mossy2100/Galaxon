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
    /// As per Chapter 38, Astronomical Algorithms (2nd ed.) by Jeen Meeus.
    /// This value is 0 or positive for dates after the beginning of 2000, negative for earlier.
    /// </summary>
    public int Orbit { get; set; }

    /// <summary>
    /// The apside type; periapsis or apoapsis.
    /// </summary>
    [Column(TypeName = "varchar(20)")]
    public EApside Type { get; set; }

    /// <summary>
    /// This will be a multiple of 0.5 and uniquely identify an apside for the given object.
    /// This value is called 'k' in the book (Astronomical Algorithms, 2nd Ed. by Jean Meeus) and is
    /// a multiple of 0.5 It will be 0 or positive for events after the beginning of 2000, negative
    /// for before.
    /// </summary>
    [NotMapped]
    public double Value => Orbit + (int)Type / 2.0;

    /// <summary>
    /// The UTC datetime of the apside according to the Galaxon algorithms.
    /// </summary>
    public DateTime? DateTimeUtc { get; set; }

    /// <summary>
    /// The UTC datetime of the apside according to the USNO web service.
    /// <see href="https://aa.usno.navy.mil/data/Earth_Seasons"/>
    /// </summary>
    public DateTime? DateTimeUtcUsno { get; set; }
}
