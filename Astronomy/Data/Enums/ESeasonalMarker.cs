using System.ComponentModel;

namespace Galaxon.Astronomy.Data.Enums;

public enum ESeasonalMarker
{
    [Description("March/northern vernal/southern autumnal equinox")]
    NorthwardEquinox = 0,

    [Description("June/northern summer/southern winter solstice")]
    NorthernSolstice = 1,

    [Description("September/northern autumnal/southern vernal equinox")]
    SouthwardEquinox = 2,

    [Description("December/northern winter/southern summer solstice")]
    SouthernSolstice = 3
}
