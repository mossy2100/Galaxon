using Microsoft.OpenApi.Attributes;

namespace Galaxon.Astronomy.Data.Enums;

public enum ESeasonalMarkerType
{
    [Display("Northward Equinox")]
    NorthwardEquinox = 0,

    [Display("Northern Solstice")]
    NorthernSolstice = 1,

    [Display("Southward Equinox")]
    SouthwardEquinox = 2,

    [Display("Southern Solstice")]
    SouthernSolstice = 3
}
