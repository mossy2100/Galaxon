using Galaxon.Astronomy.Algorithms.Utilities;
using Galaxon.Time.Extensions;

namespace Galaxon.ConsoleApp.Services;

public class Metonic
{
    /// <summary>
    /// Display the length of relevant cycles in the time of Meton and now.
    /// </summary>
    public static void CycleLengthChanges()
    {
        int yearThen = -433;
        int yearNow = 2024;

        double dayLengthThen_s = DurationUtility.GetSolarDayInSeconds(yearThen);
        Console.WriteLine($"The solar day length in {yearThen} was {dayLengthThen_s} days.");
        double dayLengthNow_s = DurationUtility.GetSolarDayInSeconds(yearNow);
        Console.WriteLine($"The solar day length in {yearNow} is {dayLengthNow_s} seconds.");
        double dayDiff_s = dayLengthNow_s - dayLengthThen_s;
        string dayIncreaseVerb = dayLengthNow_s > dayLengthThen_s ? "increased" : "decreased";
        TimeSpan tsDayDiff = TimeSpan.FromSeconds(Abs(dayDiff_s));
        Console.WriteLine($"The solar day has {dayIncreaseVerb} by {TimeSpanExtensions.GetTimeString(tsDayDiff)}.");

        Console.WriteLine();

        double lunationLengthThen_d = DurationUtility.GetLunationInEphemerisDaysForYear(yearThen);
        Console.WriteLine($"The lunation length in {yearThen} was {lunationLengthThen_d} days.");
        double lunationLengthNow_d = DurationUtility.GetLunationInEphemerisDaysForYear(yearNow);
        Console.WriteLine($"The lunation length in {yearNow} is {lunationLengthNow_d} days.");
        double lunationDiff_d = lunationLengthNow_d - lunationLengthThen_d;
        string lunationIncreaseVerb = lunationLengthNow_d > lunationLengthThen_d ? "increased" : "decreased";
        TimeSpan tsLunationDiff = TimeSpan.FromDays(Abs(lunationDiff_d));
        Console.WriteLine($"The lunation has {lunationIncreaseVerb} by {TimeSpanExtensions.GetTimeString(tsLunationDiff)}.");

        Console.WriteLine();

        double yearLengthThen_d = DurationUtility.GetTropicalYearInEphemerisDaysForYear(yearThen);
        Console.WriteLine($"The tropical year length in {yearThen} was {yearLengthThen_d} days.");
        double yearLengthNow_d = DurationUtility.GetTropicalYearInEphemerisDaysForYear(yearNow);
        Console.WriteLine($"The tropical year length in {yearNow} is {yearLengthNow_d} days.");
        double yearDiff_d = yearLengthNow_d - yearLengthThen_d;
        string yearIncreaseVerb = yearLengthNow_d > yearLengthThen_d ? "increased" : "decreased";
        TimeSpan tsYearDiff = TimeSpan.FromDays(Abs(yearDiff_d));
        Console.WriteLine($"The tropical year has {yearIncreaseVerb} by {TimeSpanExtensions.GetTimeString(tsYearDiff)}.");
    }
}
