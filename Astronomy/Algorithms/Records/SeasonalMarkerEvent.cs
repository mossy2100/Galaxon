using Galaxon.Astronomy.Data.Enums;

namespace Galaxon.Astronomy.Algorithms.Records;

/// <summary>
/// This type is similar to, but different from, Galaxon.Astronomy.Data.Models.SeasonalMarker.
/// That type represents a database record.
/// This type represents a specific seasonal marker, usually a method result.
/// </summary>
public record struct SeasonalMarkerEvent
{
    /// <summary>
    /// The year of the seasonal marker event.
    /// </summary>
    public int Year { get; init; }

    /// <summary>
    /// The type of the seasonal marker.
    /// </summary>
    public ESeasonalMarkerType MarkerType { get; init; }

    /// <summary>
    /// The Julian Date (Terrestrial Time) of the seasonal marker event.
    /// </summary>
    public double JulianDateTerrestrial { get; init; }

    /// <summary>
    /// The UTC date and time of the seasonal marker event.
    /// </summary>
    public DateTime DateTimeUtc { get; init; }

    /// <summary>
    /// Initializes a new instance of the SeasonalMarkerEvent record struct.
    /// </summary>
    /// <param name="year">The year of the seasonal marker event.</param>
    /// <param name="markerType">The type of the seasonal marker.</param>
    /// <param name="julianDateTerrestrial">The Julian date for the terrestrial time of the seasonal marker
    /// event.</param>
    /// <param name="dateTimeUtc">The UTC date and time of the seasonal marker event.</param>
    public SeasonalMarkerEvent(int year, ESeasonalMarkerType markerType,
        double julianDateTerrestrial, DateTime dateTimeUtc)
    {
        Year = year;
        MarkerType = markerType;
        JulianDateTerrestrial = julianDateTerrestrial;
        DateTimeUtc = dateTimeUtc;
    }
}
