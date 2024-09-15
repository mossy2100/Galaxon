using Galaxon.Astronomy.Algorithms.Utilities;
using Galaxon.Numerics.Extensions.FloatingPoint;
using Humanizer;

namespace Galaxon.ConsoleApp.Services;

public class ProgressiveLeapYearRuleService
{
    public void Version1()
    {
        int prevBestD = 0;

        for (int y = 2025; y <= 10000; y++)
        {
            double tropicalYearLength_d = DurationUtility.GetTropicalYearInSolarDaysForYear(y);
            double targetFrac = tropicalYearLength_d.Frac();

            // Find the closest fraction.
            int bestN = 0;
            int bestD = 0;
            double diff;
            double minDiff = double.MaxValue;
            double frac;

            for (int n = 32; n > 20; n--)
            {
                int d = n * 4 + 4;
                frac = (double)n / d;
                diff = Abs(frac - targetFrac);
                if (diff < minDiff)
                {
                    bestN = n;
                    bestD = d;
                    minDiff = diff;
                }
                if (diff < 1e-7)
                {
                    Console.WriteLine(
                        $"In year {y}, the average calendar length of {365 + frac:F6} days is virtually equal to the tropical year length of {tropicalYearLength_d:F6} solar days.");
                }
            }

            // Report.
            if (bestD != prevBestD)
            {
                Console.WriteLine($"From year {y}: {bestN}/{bestD}");
                prevBestD = bestD;
            }
        }
    }

    public void Version2()
    {
        double totalDrift = 0;
        int maxN = 35;
        Dictionary<int, (int, int, int, int)> results = new ();

        for (int millennium = 3; millennium <= 10; millennium++)
        {
            int maxYear = millennium * 1000;
            int minYear = maxYear - 999;

            Console.WriteLine(
                $"Results for the {millennium.Ordinalize()} millennium ({minYear}-{maxYear}).");

            // Get total solar days in this tropical millennium (e.g. 2001-3000).
            double tropicalMillennium_d = 0;
            for (int y = minYear; y <= maxYear; y++)
            {
                tropicalMillennium_d += DurationUtility.GetTropicalYearInSolarDaysForYear(y);
            }
            double avgTropicalYear_d = tropicalMillennium_d / 1000;
            double actualFrac = avgTropicalYear_d.Frac();

            // Consider each fraction and calculate the total drift by the end.
            int bestN = 0;
            int bestD = 0;
            int bestNumLeapYears = 0;
            double minDrift = double.MaxValue;
            double bestTotalDriftForMillennium = 0;
            double minFracDiff = double.MaxValue;
            double bestAvgCalendarYear_d = 0;

            for (int n = maxN; n >= 25; n--)
            {
                int d = n * 4 + 4;
                double ruleFrac = (double)n / d;
                double avgCalendarYear_d = 365 + ruleFrac;
                double fracDiff = Abs(ruleFrac - actualFrac);

                // Get the length of the calendar millennium in solar days.
                int calendarMillennium_d = 0;
                int nLeapYears = 0;
                for (int y = minYear; y <= maxYear; y++)
                {
                    bool isLeapYear = y % 4 == 0 && y % d != 0;

                    int calendarYear_d = isLeapYear ? 366 : 365;
                    calendarMillennium_d += calendarYear_d;

                    if (isLeapYear)
                    {
                        nLeapYears++;
                    }
                }

                // Compute the drift at the end of the millennium.
                double totalDriftForMillennium = calendarMillennium_d - tropicalMillennium_d;
                double testTotalDrift = Abs(totalDrift + totalDriftForMillennium);

                // Console.WriteLine($"    At end of millennium {millennium}, with rule {n} / {d}, total drift would be {testTotalDrift}");

                // Choose the result that produces the lower total drift from the tropical year;
                // or, if it produces the same, choose the result with closer alignment between the
                // average calendar year length and the tropical year, as this rule will last longer.
                // Console.WriteLine($"Rule {n}/{d} => drift {testTotalDrift:F6} days, frac diff {fracDiff:F6}");
                // if (testTotalDrift < minDrift || (testTotalDrift == minDrift && fracDiff < minFracDiff))
                if (testTotalDrift < minDrift
                    || (testTotalDrift == minDrift && fracDiff < minFracDiff))
                {
                    // Console.WriteLine("This is better");
                    minDrift = testTotalDrift;
                    minFracDiff = fracDiff;
                    bestN = n;
                    bestD = d;
                    bestNumLeapYears = nLeapYears;
                    bestTotalDriftForMillennium = totalDriftForMillennium;
                    bestAvgCalendarYear_d = avgCalendarYear_d;
                }
            }

            // Console.WriteLine($"Best total drift = {minDrift:F3} days ({minDrift * 24:F3} hours).");
            Console.WriteLine($"Best rule: {bestN} leap years per {bestD} years.");
            Console.WriteLine($"Actual: {bestNumLeapYears} leap years in the millennium.");
            Console.WriteLine($"Average tropical year length {avgTropicalYear_d:F6} solar days.");
            Console.WriteLine(
                $"Average calendar year length {bestAvgCalendarYear_d:F6} solar days.");
            Console.WriteLine(
                $"Difference between average tropical year and average calendar year = {minFracDiff * 86400:F3} seconds.");

            // Update the total drift.
            totalDrift += bestTotalDriftForMillennium;
            Console.WriteLine(
                $"Total drift by the end of the millennium is {Abs(totalDrift):F3} days ({Abs(totalDrift) * 24:F3} hours).");
            Console.WriteLine();

            results[millennium] = (minYear, maxYear, bestN, bestD);
        }

        foreach (KeyValuePair<int, (int, int, int, int)> result in results)
        {
            (int minYear, int maxYear, int n, int d) = result.Value;
            Console.WriteLine($"For years {minYear}-{maxYear}: {n} leap years in {d} years.");
        }
    }

    public void Version3()
    {
        // Compute the length of a century in calendar days given a century number and value for n.
        static int calendarCenturyInDays(int century, int n)
        {
            int maxYear = century * 100;
            int minYear = maxYear - 99;
            int d = 4 * (n + 1);
            int total_d = 0;
            for (int y = minYear; y <= maxYear; y++)
            {
                bool isLeapYear = y % 4 == 0 && y % d != 0;
                total_d += isLeapYear ? 366 : 365;
            }
            return total_d;
        }

        double maxDrift = 0;

        // Step 1. Find the best fit rule of the form n/(4(n+1)) for the 21st century.
        int century = 21;
        int maxYear = century * 100;
        int minYear = maxYear - 99;

        // Get the total length of the tropical century in solar days.
        double tropicalCentury_d = DurationUtility.GetTropicalCenturyInSolarDays(century);
        double targetFrac = (tropicalCentury_d / 100).Frac();
        int currentN = 0;

        // Find which value for n produces the best fit for the 21st century.
        double minDiff = double.MaxValue;
        int currentD;
        double frac;
        for (int n = 1; n <= 40; n++)
        {
            frac = (double)n / (4 * (n + 1));
            double diff = Abs(frac - targetFrac);
            if (diff < minDiff)
            {
                minDiff = diff;
                currentN = n;
            }
        }
        currentD = 4 * (currentN + 1);
        frac = (double)currentN / currentD;

        Console.WriteLine(targetFrac);

        Console.WriteLine(frac);

        // Compute the drift at the end of the 21st century.
        // Note, this presupposes that the century started at 0 drift.
        // In fact, we'll probably start halfway through the century.
        int calendarCentury_d = calendarCenturyInDays(century, currentN);
        double currentDrift = tropicalCentury_d - calendarCentury_d;

        // Report.
        Console.WriteLine(
            $"For century {century} ({minYear}-{maxYear}), n = {currentN}, d = {currentD}, frac = {frac:F6}, drift at end of century is {currentDrift:F3} days ({currentDrift * 24:F1} hours)");

        Dictionary<int, (int minYear, int maxYear, int nLeapYears)> result = new ();
        int nLeapYears = calendarCentury_d % 365;
        result[currentN] = (minYear, maxYear, nLeapYears);

        // Loop through remaining centuries and see when it makes sense to decrement n.
        for (century = 22; century <= 120; century++)
        {
            maxYear = century * 100;
            minYear = maxYear - 99;

            // Console.WriteLine(
            //     $"Results for the {century.Ordinalize()} century ({minYear}-{maxYear}).");

            // Get total solar days in this tropical century.
            tropicalCentury_d = DurationUtility.GetTropicalCenturyInSolarDays(century);

            // Get total calendar days in this tropical century for current value of n and next value of n.
            int calendarCentury1_d = calendarCenturyInDays(century, currentN);
            int nLeapYears1 = calendarCentury1_d % 365;
            int calendarCentury2_d = calendarCenturyInDays(century, currentN - 1);
            int nLeapYears2 = calendarCentury2_d % 365;

            // Calculate accumulated drift at the end of the century for the current value of n and next value of n.
            double newDrift1 = currentDrift + (tropicalCentury_d - calendarCentury1_d);
            double newDrift2 = currentDrift + (tropicalCentury_d - calendarCentury2_d);

            // if switching produces a better result, switch.
            if (Abs(newDrift2) < Abs(newDrift1))
            {
                Console.WriteLine();
                currentN--;
                currentDrift = newDrift2;
                result[currentN] = (minYear, maxYear, nLeapYears2);
            }
            else
            {
                currentDrift = newDrift1;
                result[currentN] = (result[currentN].minYear, maxYear,
                    result[currentN].nLeapYears + nLeapYears1);
            }

            currentD = 4 * (currentN + 1);
            frac = (double)currentN / currentD;

            Console.WriteLine(
                $"For century {century} ({minYear}-{maxYear}), n = {currentN}, d = {currentD}, frac = {frac:F6}, drift at end of century is {currentDrift:F3} days ({currentDrift * 24:F1} hours)");
            if (Abs(currentDrift) > maxDrift)
            {
                maxDrift = Abs(currentDrift);
            }
        }

        Console.WriteLine();
        foreach (var item in result)
        {
            int nYears = item.Value.maxYear - item.Value.minYear + 1;
            string label = $"For years {item.Value.minYear}-{item.Value.maxYear}:";
            Console.WriteLine(
                $"    {label,-22} {item.Key} leap years per {4 * (item.Key + 1)} years (actually {item.Value.nLeapYears} leap years in {nYears} years).");
        }
        Console.WriteLine();
        Console.WriteLine($"Max drift is {maxDrift:F3} days ({maxDrift * 24:F1} hours)");
    }

    public void Version4()
    {
        // Compute the length of a millennium in calendar days given a millennium number and value for n.
        static int calendarMillenniumInDays(int millennium, int n)
        {
            int maxYear = millennium * 1000;
            int minYear = maxYear - 999;
            int d = 4 * (n + 1);
            int total_d = 0;
            for (int y = minYear; y <= maxYear; y++)
            {
                bool isLeapYear = y % 4 == 0 && y % d != 0;
                total_d += isLeapYear ? 366 : 365;
            }
            return total_d;
        }

        double maxDrift = 0;

        // Step 1. Find the best fit rule of the form n/(4(n+1)) for the 3rd millennium.
        int millennium = 3;
        int maxYear = millennium * 1000;
        int minYear = maxYear - 999;

        // Get the total length of the tropical millennium in solar days.
        double tropicalMillennium_d = DurationUtility.GetTropicalMillenniumInSolarDays(millennium);
        double targetFrac = (tropicalMillennium_d / 1000).Frac();
        int currentN = 0;

        // Find which value for n produces the best fit for the 3rd millennium.
        double minDiff = double.MaxValue;
        int currentD;
        double frac;
        for (int n = 1; n <= 40; n++)
        {
            frac = (double)n / (4 * (n + 1));
            double diff = Abs(frac - targetFrac);
            if (diff < minDiff)
            {
                minDiff = diff;
                currentN = n;
            }
        }
        currentD = 4 * (currentN + 1);
        frac = (double)currentN / currentD;

        Console.WriteLine(targetFrac);

        Console.WriteLine(frac);

        // Compute the drift at the end of the 3rd millennium.
        // Note, this presupposes that the millennium started at 0 drift.
        // In fact, we'll probably start halfway through the millennium.
        int calendarMillennium_d = calendarMillenniumInDays(millennium, currentN);
        double currentDrift = tropicalMillennium_d - calendarMillennium_d;

        // Report.
        Console.WriteLine(
            $"For millennium {millennium} ({minYear}-{maxYear}), n = {currentN}, d = {currentD}, frac = {frac:F6}, drift at end of millennium is {currentDrift:F3} days ({currentDrift * 24:F1} hours)");

        Dictionary<int, (int minYear, int maxYear, int nLeapYears)> result = new ();
        int nLeapYears = calendarMillennium_d % 365;
        result[currentN] = (minYear, maxYear, nLeapYears);

        // Loop through remaining millennia and see when it makes sense to decrement n.
        for (millennium = 4; millennium <= 12; millennium++)
        {
            maxYear = millennium * 1000;
            minYear = maxYear - 999;

            // Get total solar days in this tropical millennium.
            tropicalMillennium_d = DurationUtility.GetTropicalMillenniumInSolarDays(millennium);

            // Get total calendar days in this tropical millennium for current value of n and next value of n.
            int calendarMillennium1_d = calendarMillenniumInDays(millennium, currentN);
            int nLeapYears1 = calendarMillennium1_d % 365;
            int calendarMillennium2_d = calendarMillenniumInDays(millennium, currentN - 1);
            int nLeapYears2 = calendarMillennium2_d % 365;

            // Calculate accumulated drift at the end of the millennium for the current value of n and next value of n.
            double newDrift1 = currentDrift + (tropicalMillennium_d - calendarMillennium1_d);
            double newDrift2 = currentDrift + (tropicalMillennium_d - calendarMillennium2_d);

            // if switching produces a better result, switch.
            if (Abs(newDrift2) < Abs(newDrift1))
            {
                Console.WriteLine();
                currentN--;
                currentDrift = newDrift2;
                result[currentN] = (minYear, maxYear, nLeapYears2);
            }
            else
            {
                currentDrift = newDrift1;
                result[currentN] = (result[currentN].minYear, maxYear,
                    result[currentN].nLeapYears + nLeapYears1);
            }

            currentD = 4 * (currentN + 1);
            frac = (double)currentN / currentD;

            Console.WriteLine(
                $"For millennium {millennium} ({minYear}-{maxYear}), n = {currentN}, d = {currentD}, frac = {frac:F6}, drift at end of millennium is {currentDrift:F3} days ({currentDrift * 24:F1} hours)");
            if (Abs(currentDrift) > maxDrift)
            {
                maxDrift = Abs(currentDrift);
            }
        }

        Console.WriteLine();
        foreach (var item in result)
        {
            int nYears = item.Value.maxYear - item.Value.minYear + 1;
            string label = $"For years {item.Value.minYear}-{item.Value.maxYear}:";
            Console.WriteLine(
                $"    {label,-22} {item.Key} leap years per {4 * (item.Key + 1)} years (actually {item.Value.nLeapYears} leap years in {nYears} years).");
        }
        Console.WriteLine();
        Console.WriteLine($"Max drift is {maxDrift:F3} days ({maxDrift * 24:F1} hours)");
    }

    public void Version5()
    {
        // Compute the length of a millennium in calendar days given a millennium number and values for s and n.
        static int calendarMillenniumInDays(int millennium, int s, int n)
        {
            int maxYear = millennium * 1000;
            int minYear = maxYear - 999;
            int d = s * (n + 1);
            int total_d = 0;
            for (int y = minYear; y <= maxYear; y++)
            {
                bool isLeapYear = y % s == 0 && y % d != 0;
                total_d += isLeapYear ? 366 : 365;
            }
            return total_d;
        }

        double currentDrift = 0;
        // double maxDrift = 0;

        // Remember the previous values.
        int prevS = 4;
        int prevN = 250;
        int prevB = 0;
        int prevMinYear = 2001;

        // Loop through the millennia.
        int maxMillennium = 100;
        for (int millennium = 3; millennium <= 12; millennium++)
        {
            // Step 1. Compute s for the current millennium.
            double tropicalMillennium_d =
                DurationUtility.GetTropicalMillenniumInSolarDays(millennium);
            double targetFrac = (tropicalMillennium_d / 1000).Frac();
            double sApprox = 1 / targetFrac;
            int newS = (int)(double.IsInteger(sApprox) ? sApprox - 1 : Floor(sApprox));

            // Step 2. Find the best-fit values of n and d for the current millennium.
            int maxYear = millennium * 1000;
            int minYear = maxYear - 999;

            // Remember our best-fit values.
            int newN = 0;
            int newB = 0;

            double minDrift = double.MaxValue;
            double minFracDiff = double.MaxValue;

            // Test values of n. Only values less than or equal to the current value allowed, to
            // reduce the number of changes.
            double frac;
            for (int n = prevN; n >= 1; n--)
            {
                // Compute d.
                int b = newS * (n + 1);

                // Compute frac.
                frac = (double)n / b;

                int calendarMillennium_d = calendarMillenniumInDays(millennium, newS, n);
                // int nLeapYears = calendarMillennium_d % 365;
                double newDrift = currentDrift + (tropicalMillennium_d - calendarMillennium_d);
                double absNewDrift = Abs(newDrift);
                double absMinDrift = Abs(minDrift);
                double fracDiff = Abs(targetFrac - frac);

                // Assume the rule is better if the drift is less, or if the drift is the same
                // (which means the number of leap years in the period is the same) but the fraction
                // is closer to the tropical year fraction.
                if (absNewDrift < absMinDrift
                    || (absNewDrift == absMinDrift && fracDiff < minFracDiff))
                {
                    minDrift = newDrift;
                    minFracDiff = fracDiff;

                    // Remember best values of n and b.
                    newN = n;
                    newB = b;
                }
            }

            // Update the drift.
            currentDrift += minDrift;

            // Update frac.
            frac = (double)newN / newB;

            // Report.
            Console.WriteLine(
                $"For millennium {millennium} ({minYear}-{maxYear}), s = {newS}, n = {newN}, b = {newB}, targetFrac = {targetFrac:F6}, frac = {frac:F6}, drift at end of millennium is {currentDrift:F3} days ({currentDrift * 24:F1} hours)");

            // Are we changing the rule?
            if (prevS != 0 && (newS != prevS || newN != prevN))
            {
                // Complete the report on the previous rule.
                string label = $"For years {prevMinYear}-{minYear - 1}:";
                // Console.WriteLine($"    {label,-22} {prevN} leap years per {prevB} years.");
                prevMinYear = maxYear + 1;
            }

            // If we're changing s, reset n.
            if (newS != prevS)
            {
                newN = 100;
            }

            // Update formula values to prepare for the next loop.
            prevS = newS;
            prevN = newN;
            prevB = newB;

            // if (Abs(currentDrift) > maxDrift)
            // {
            //     maxDrift = Abs(currentDrift);
            // }
        }

        // Finish output.
        string label2 = $"For years {prevMinYear}-{maxMillennium * 1000}:";
        // Console.WriteLine($"    {label2,-22} {prevN} leap years per {prevB} years.");
    }
}
