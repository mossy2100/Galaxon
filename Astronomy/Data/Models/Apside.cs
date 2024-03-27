using Galaxon.Astronomy.Data.Enums;

namespace Galaxon.Astronomy.Data.Models;

public class Apside : DataObject
{
    /// <summary>
    /// This value is:
    ///     0 = Periapsis (perihelion, perigee, etc.)
    ///     1 = Apoapsis (aphelion, apogee, etc.)
    /// </summary>
    [Column(TypeName = "tinyint")]
    public EApsideType Type { get; set; }

    /// <summary>
    /// The UTC datetime of the apside.
    /// </summary>
    [Column(TypeName = "datetime2")]
    public DateTime DateTimeUTC { get; set; }
}
