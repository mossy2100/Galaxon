using Galaxon.Astronomy.Data.Models;
using Galaxon.Astronomy.Data.Repositories;
using Galaxon.Time;

namespace Galaxon.Astronomy.Algorithms.Services;

public class LeapSecondService(LeapSecondRepository leapSecondRepository)
{
    /// <summary>
    /// Total the value of all leap seconds (positive and negative) up to and including a certain
    /// date.
    /// </summary>
    /// <param name="d">The date.</param>
    /// <returns>The total value of leap seconds inserted.</returns>
    public int TotalLeapSeconds(DateOnly d)
    {
        var total = 0;
        foreach (LeapSecond ls in leapSecondRepository.List)
        {
            // If the leap second date is earlier than or equal to the date argument, add the value
            // of the leap second (-1, 0, or 1). We're assuming that we want the total leap seconds
            // up to the very end of the given date.
            if (ls.LeapSecondDate <= d)
            {
                total += ls.Value;
            }
        }
        return total;
    }

    /// <summary>
    /// Total the value of all leap seconds (positive and negative) up to and including a certain
    /// datetime.
    /// This is slightly different to the DateOnly version, because any DateTime will be earlier
    /// than a leap second introduced that day, because the actual datetime of the leap second,
    /// which will have a time of day of 23:59:60, isn't representable as a DateTime.
    /// </summary>
    /// <param name="dt">The datetime. Defaults to now.</param>
    /// <returns>The total value of leap seconds inserted.</returns>
    public int TotalLeapSeconds(DateTime? dt = null)
    {
        // Default to now.
        dt ??= DateTimeExtensions.NowUtc;

        // Count leap seconds.
        var total = 0;
        foreach (LeapSecond ls in leapSecondRepository.List)
        {
            // Get the datetime of the leap second.
            // When creating the new DateTime we use the same DateTimeKind as the argument so the
            // comparison works correctly. The time of day will be set to 00:00:00.
            var dtLeapSecond = ls.LeapSecondDate.ToDateTime(dt.Value.Kind);
            // The actual time of day for the leap second is 23:59:60, which can't be represented
            // using DateTime. So we'll use the time 00:00:00 of the next day.
            dtLeapSecond = dtLeapSecond.AddDays(1);

            // If the datetime of the leap second is before or equal to the datetime argument, add
            // the value of the leap second (-1, 0, or 1) to the total.
            if (dtLeapSecond <= dt.Value)
            {
                total += ls.Value;
            }
        }

        return total;
    }

    /// <summary>
    /// Find the difference between TAI and UTC at a given point in time.
    /// </summary>
    /// <param name="dt">A point in time. Defaults to current DateTime.</param>
    /// <returns>The integer number of seconds difference.</returns>
    public byte CalcTAIMinusUTC(DateTime? dt = null)
    {
        // Default to now.
        dt ??= DateTimeExtensions.NowUtc;

        return (byte)(10 + TotalLeapSeconds(dt));
    }

    /// <summary>
    /// Calculate the difference between UT1 and UTC in seconds.
    /// DUT1 = UT1 - UTC
    /// In theory, this should always be between -0.9 and 0.9. However, because the CalcDeltaT()
    /// algorithm is only approximate, it isn't.
    ///
    /// DUT1 is normally measured in retrospect, not calculated. I have included this method to
    /// check the accuracy of the CalcDeltaT() method.
    ///
    /// This calculation of DUT1 is only valid within the nominal range from 1972..2010.
    /// Yet the *actual* DUT1 is within range up until 2022 (the time of writing) because leap
    /// seconds have been added to produce exactly this effect. The error must be in CalcDeltaT(),
    /// which leads me to believe the NASA formulae used in this library either aren't super
    /// accurate or (more likely) they were developed before 2010.
    /// <see href="https://en.wikipedia.org/wiki/DUT1"/>
    /// </summary>
    /// <param name="dt">A point in time. Defaults to current DateTime.</param>
    /// <returns>The difference between UT1 and UTC.</returns>
    /// <exception cref="ArgumentOutOfRangeException">If year less than 1972.</exception>
    public double CalcDUT1(DateTime? dt = null)
    {
        // Default to now.
        dt ??= DateTimeExtensions.NowUtc;

        return (double)TimeConstants.TT_MINUS_TAI_MILLISECONDS
            / TimeConstants.MILLISECONDS_PER_SECOND
            - TimeScales.CalcDeltaT(dt.Value)
            + CalcTAIMinusUTC(dt.Value);
    }

    public void TestCalcDUT1()
    {
        for (var y = 1972; y <= 2022; y++)
        {
            var dt = new DateTime(y, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            int LSC = TotalLeapSeconds(dt);
            double deltaT = TimeScales.CalcDeltaT(dt);
            double DUT1 = CalcDUT1(dt);
            Console.WriteLine($"Year={y}, LSC={LSC}, ∆T={deltaT}, DUT1={DUT1}");
            if (Abs(DUT1) > 0.9)
            {
                Console.WriteLine("Wrong.");
            }
        }
    }
}