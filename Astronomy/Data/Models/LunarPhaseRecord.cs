using Galaxon.Astronomy.Data.Enums;

namespace Galaxon.Astronomy.Data.Models;

public class LunarPhaseRecord : DatabaseRecord
{
    /// <summary>
    /// Meeus Lunation Number.
    /// </summary>
    public int LunationNumber { get; set; }

    /// <summary>
    /// The phase type.
    /// </summary>
    [Column(TypeName = "varchar(12)")]
    public ELunarPhaseType PhaseType { get; set; }

    /// <summary>
    /// The unique phase number, which is a multiple of 0.25. New moons will be integers.
    /// </summary>
    [NotMapped]
    public double PhaseNumber => LunationNumber + (int)PhaseType / 4.0;

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
