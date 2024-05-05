namespace Galaxon.Astronomy.Data.Models;

public class ApsideRecord : DataObject
{
    /// <summary>
    /// This value is:
    ///   0 = Periapsis (perihelion, perigee, etc.)
    ///   1 = Apoapsis (aphelion, apogee, etc.)
    /// </summary>
    [Column(TypeName = "TINYINT")]
    public int ApsideNumber { get; set; }

    /// <summary>
    /// The UTC datetime of the apside according to USNO.
    /// </summary>
    public DateTime DateTimeUtcUsno { get; set; }
}
