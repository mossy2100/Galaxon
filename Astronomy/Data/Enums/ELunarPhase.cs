using Microsoft.OpenApi.Attributes;

namespace Galaxon.Astronomy.Data.Enums;

/// <summary>
/// Enum for the major phases in the lunar cycle.
/// I could possible add minor phases but as yet these aren't needed.
/// If I do, it will require renumbering the phases and updating some algorithms that rely on these
/// numbers.
/// </summary>
public enum ELunarPhase
{
    [Display("New Moon")]
    NewMoon = 0,

    [Display("First Quarter")]
    FirstQuarter = 1,

    [Display("Full Moon")]
    FullMoon = 2,

    [Display("Third Quarter")]
    ThirdQuarter = 3
}
