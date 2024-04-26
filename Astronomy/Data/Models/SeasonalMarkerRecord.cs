namespace Galaxon.Astronomy.Data.Models;

public class SeasonalMarkerRecord : DataObject
{
    /// <summary>
    /// This value is:
    ///   0 = Northward (March) equinox
    ///   1 = Northern (June) solstice
    ///   2 = Southward (September) equinox
    ///   3 = Southern (December) solstice
    /// </summary>
    [Column(TypeName = "tinyint")]
    public int MarkerNumber { get; set; }

    /// <summary>
    /// The UTC datetime of the seasonal marker.
    /// </summary>
    [Column(TypeName = "datetime2")]
    public DateTime DateTimeUtcUsno { get; set; }
}
