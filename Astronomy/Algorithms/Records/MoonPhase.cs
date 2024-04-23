using Galaxon.Astronomy.Data.Enums;

namespace Galaxon.Astronomy.Algorithms.Records;

/// <summary>
/// This type is similar to, but different from, Galaxon.Astronomy.Data.Models.LunarPhase.
/// That type represents a database record.
/// This type represents a specific lunar phase, usually a method result.
/// </summary>
public record struct MoonPhase
{
    /// <summary>
    /// This value is:
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
