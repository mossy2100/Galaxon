using Galaxon.Astronomy.Data.Enums;

namespace Galaxon.Astronomy.Data.Models;

public class LunarPhaseRecord : DatabaseRecord
{
    /// <summary>
    /// Meeus Lunation Number.
    /// </summary>
    public int Lunation { get; set; }

    /// <summary>
    /// The phase type.
    /// </summary>
    [Column(TypeName = "varchar(20)")]
    public ELunarPhaseType Type { get; set; }

    /// <summary>
    /// The unique phase number, which is a multiple of 0.25. New moons will be integers.
    /// </summary>
    public double Value => Lunation + (int)Type / 4.0;

    /// <summary>
    /// The UTC datetime of the lunar phase according to my calculations.
    /// </summary>
    public DateTime? DateTimeUtcGalaxon { get; set; }

    /// <summary>
    /// The UTC datetime of the lunar phase according to AstroPixels.
    /// <see href="https://www.astropixels.com/ephemeris/phasescat/phasescat.html"/>
    /// </summary>
    public DateTime? DateTimeUtcAstroPixels { get; set; }

    /// <summary>
    /// The UTC datetime of the lunar phase according to USNO.
    /// <see href="https://aa.usno.navy.mil/data/MoonPhases"/>
    /// </summary>
    public DateTime? DateTimeUtcUsno { get; set; }
}
