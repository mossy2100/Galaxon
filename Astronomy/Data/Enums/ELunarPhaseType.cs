using System.ComponentModel;

namespace Galaxon.Astronomy.Data.Enums;

public enum ELunarPhaseType
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
