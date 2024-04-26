using Galaxon.Astronomy.Data.Enums;

namespace Galaxon.Astronomy.Algorithms.Records;

/// <summary>
/// This type is similar to, but different from, Galaxon.Astronomy.Data.Models.LunarPhaseRecord,
/// which represents a database record.
/// This type represents a specific lunar phase, usually a method result.
/// </summary>
public record struct LunarPhaseEvent(int LunationNumber, ELunarPhase Phase, DateTime DateTimeUtc);
