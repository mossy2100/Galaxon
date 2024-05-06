namespace Galaxon.Astronomy.Data.Models;

public class ApsideRecord : DataObject
{
    /// <summary>
    /// The id of the astronomical object (planet, minor planet, or moon) for which the apside has
    /// been computed.
    /// </summary>
    public int AstroObjectId { get; set; }

    /// <summary>
    /// As per Chapter 38, Astronomical Algorithms (2nd ed.) by Jeen Meeus, this value (called 'k'
    /// in the book) is 0 or positive for dates after the beginning of 2000, negative for earlier.
    /// </summary>
    public int OrbitNumber { get; set; }

    /// <summary>
    /// This value is:
    ///   0 = Periapsis (perihelion, perigee, etc.)
    ///   1 = Apoapsis (aphelion, apogee, etc.)
    /// </summary>
    public byte ApsideNumber { get; set; }

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
