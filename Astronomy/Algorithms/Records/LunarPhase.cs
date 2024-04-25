using Galaxon.Astronomy.Data.Enums;

namespace Galaxon.Astronomy.Algorithms.Records;

/// <summary>
/// This type is similar to, but different from, Galaxon.Astronomy.Data.Models.LunarPhaseRecord,
/// which represents a database record.
/// This type represents a specific lunar phase, usually a method result.
/// </summary>
public record struct LunarPhase
{
    /// <summary>
    /// Meeus Lunation Number.
    /// </summary>
    public int LunationNumber { get; set; }

    /// <summary>
    /// Enum representing the lunar phase type.
    ///   0 = New Moon
    ///   1 = First Quarter
    ///   2 = Full Moon
    ///   3 = Third Quarter
    /// </summary>
    public ELunarPhaseType Type { get; init; }

    /// <summary>
    /// The UTC datetime of the lunar phase.
    /// </summary>
    public DateTime DateTimeUtc { get; init; }
}
