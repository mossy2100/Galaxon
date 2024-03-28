using Galaxon.Astronomy.Data.Enums;

namespace Galaxon.Astronomy.Data.Models;

public class Apside : DataObject
{
    /// <summary>
    /// This value is:
    ///   0 = Periapsis (perihelion, perigee, etc.)
    ///   1 = Apoapsis (aphelion, apogee, etc.)
    /// </summary>
    [Column(TypeName = "tinyint")]
    public EApsideType Type { get; set; }

    /// <summary>
    /// The UTC datetime of the apside according to USNO.
    /// </summary>
    [Column(TypeName = "datetime2")]
    public DateTime DateTimeUtcUsno { get; set; }
}
