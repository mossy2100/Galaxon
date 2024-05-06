﻿namespace Galaxon.Astronomy.Data.Models;

public class SeasonalMarkerRecord : DataObject
{
    /// <summary>
    /// The seasonal marker as an integer:
    ///   0 = Northward (March) equinox
    ///   1 = Northern (June) solstice
    ///   2 = Southward (September) equinox
    ///   3 = Southern (December) solstice
    /// </summary>
    public byte MarkerNumber { get; set; }

    /// <summary>
    /// The UTC datetime of the seasonal marker.
    /// </summary>
    public DateTime? DateTimeUtcUsno { get; set; }
}
