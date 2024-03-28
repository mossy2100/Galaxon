using Galaxon.Astronomy.Data.Enums;

namespace Galaxon.Astronomy.Algorithms.Models;

public class MoonPhase
{
    /// <summary>
    /// This value is:
    ///   0 = New Moon
    ///   1 = First Quarter
    ///   2 = Full Moon
    ///   3 = Third Quarter
    /// </summary>
    public ELunarPhaseType Type { get; set; }

    /// <summary>
    /// The UTC datetime of the lunar phase.
    /// </summary>
    public DateTime DateTimeUtc { get; set; }
}
