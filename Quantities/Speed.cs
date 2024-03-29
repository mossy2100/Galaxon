﻿using Galaxon.Time;

namespace Galaxon.Quantities;

public static class Speed
{
    /// <summary>
    /// Convert a speed given in km/h to m/s.
    /// </summary>
    /// <param name="kmPerHour">Speed in km/h.</param>
    /// <returns>Speed in m/s.</returns>
    public static double KmPerHourToMetresPerSecond(double kmPerHour)
    {
        return kmPerHour * UnitPrefix.GetMultiplier("k") / TimeConstants.SECONDS_PER_HOUR;
    }
}
