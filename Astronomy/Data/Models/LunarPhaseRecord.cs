namespace Galaxon.Astronomy.Data.Models;

public class LunarPhaseRecord : DataObject
{
    /// <summary>
    /// Meeus Lunation Number.
    /// </summary>
    public int LunationNumber { get; set; }

    /// <summary>
    /// The phase as an integer:
    ///   0 = New Moon
    ///   1 = First Quarter
    ///   2 = Full Moon
    ///   3 = Third Quarter
    /// </summary>
    public byte PhaseNumber { get; set; }

    /// <summary>
    /// The UTC datetime of the lunar phase event according to AstroPixels.
    /// </summary>
    public DateTime? DateTimeUtcAstroPixels { get; set; }

    /// <summary>
    /// The UTC datetime of the lunar phase event according to USNO.
    /// </summary>
    public DateTime? DateTimeUtcUsno { get; set; }
}
