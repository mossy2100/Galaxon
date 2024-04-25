using Galaxon.Astronomy.Data.Enums;

namespace Galaxon.Astronomy.Algorithms.Records;

/// <summary>
/// This type is similar to, but different from, Galaxon.Astronomy.Data.Models.SeasonalMarker.
/// That type represents a database record.
/// This type represents a specific seasonal marker, usually a method result.
/// </summary>
public record struct SeasonalMarker
{
    /// <summary>
    /// This value is:
    ///   0 = Northward (March) Equinox
    ///   1 = Northern (June) Solstice
    ///   2 = Southward (September) Equinox
    ///   3 = Southern (December) Solstice
    /// </summary>
    public ESeasonalMarkerType Type { get; init; }

    /// <summary>
    /// The UTC datetime of the seasonal marker.
    /// </summary>
    public DateTime DateTimeUtc { get; init; }
}
