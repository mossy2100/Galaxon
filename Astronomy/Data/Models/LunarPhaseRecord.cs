using Galaxon.Astronomy.Data.Enums;

namespace Galaxon.Astronomy.Data.Models;

public class LunarPhaseRecord : DatabaseRecord
{
    /// <summary>
    /// Meeus Lunation Number.
    /// </summary>
    public int LunationNumber { get; set; }

    /// <summary>
    /// The lunar phase type.
    /// </summary>
    [Column(TypeName = "tinyint unsigned")]
    public ELunarPhaseType PhaseType { get; set; }

    /// <summary>
    /// The UTC datetime of the lunar phase according to my calculations.
    /// </summary>
    public DateTime? DateTimeUtcGalaxon { get; set; }

    /// <summary>
    /// The UTC datetime of the lunar phase according to AstroPixels.
    /// See: <see href="https://www.astropixels.com/ephemeris/phasescat/phasescat.html"/>
    /// </summary>
    public DateTime? DateTimeUtcAstroPixels { get; set; }

    /// <summary>
    /// The UTC datetime of the lunar phase according to USNO.
    /// See: <see href="https://aa.usno.navy.mil/data/MoonPhases"/>
    /// </summary>
    public DateTime? DateTimeUtcUsno { get; set; }
}
