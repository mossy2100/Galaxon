﻿using Galaxon.Astronomy.Data.Enums;

namespace Galaxon.Astronomy.Data.Models;

public class LunarPhase : DataObject
{
    /// <summary>
    /// This value is:
    ///     0 = New Moon
    ///     1 = First Quarter
    ///     2 = Full Moon
    ///     3 = Third Quarter
    /// </summary>
    [Column(TypeName = "tinyint")]
    public ELunarPhaseType Type { get; set; }

    /// <summary>
    /// The UTC datetime of the lunar phase.
    /// </summary>
    [Column(TypeName = "datetime2")]
    public DateTime DateTimeUTC { get; set; }
}
