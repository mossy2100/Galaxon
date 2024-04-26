using System.ComponentModel;

namespace Galaxon.Astronomy.Data.Enums;

/// <summary>
/// Enum for the major phases in the lunar cycle.
/// I could possible add minor phases but as yet these aren't needed.
/// If I do, it will require renumbering the phases and updating some algorithms that rely on these
/// numbers.
/// </summary>
public enum ELunarPhase
{
    [Description("New Moon")]
    NewMoon = 0,

    [Description("First Quarter")]
    FirstQuarter = 1,

    [Description("Full Moon")]
    FullMoon = 2,

    [Description("Third Quarter")]
    ThirdQuarter = 3
}
