using Galaxon.Astronomy.Data.Enums;

namespace Galaxon.Astronomy.Data.Models;

public class SeasonalMarkerRecord : DataObject
{
    /// <summary>
    /// This value is:
    ///   0 = March equinox
    ///   1 = June solstice
    ///   2 = September equinox
    ///   3 = December solstice
    /// </summary>
    [Column(TypeName = "tinyint")]
    public ESeasonalMarkerType Type { get; set; }

    /// <summary>
    /// The UTC datetime of the seasonal marker.
    /// </summary>
    [Column(TypeName = "datetime2")]
    public DateTime DateTimeUtcUsno { get; set; }
}
