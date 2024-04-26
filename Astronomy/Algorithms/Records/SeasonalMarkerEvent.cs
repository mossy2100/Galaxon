using Galaxon.Astronomy.Data.Enums;

namespace Galaxon.Astronomy.Algorithms.Records;

/// <summary>
/// This type is similar to, but different from, Galaxon.Astronomy.Data.Models.SeasonalMarker.
/// That type represents a database record.
/// This type represents a specific seasonal marker, usually a method result.
/// </summary>
public record struct SeasonalMarkerEvent(ESeasonalMarker SeasonalMarker, DateTime DateTimeUtc);
