using Microsoft.OpenApi.Attributes;

namespace Galaxon.Astronomy.Data.Enums;

public enum ESeasonalMarker
{
    [Display("Northward equinox, also known as the March, northern vernal, or southern autumnal equinox")]
    NorthwardEquinox = 0,

    [Display("Northern solstice, also known as the June, northern summer, or southern winter solstice")]
    NorthernSolstice = 1,

    [Display("Southward equinox, also known as the September, northern autumnal, or southern vernal equinox")]
    SouthwardEquinox = 2,

    [Display("Southern solstice, also known as the December, northern winter, or southern summer solstice")]
    SouthernSolstice = 3
}
