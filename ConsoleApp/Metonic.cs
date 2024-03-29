using Galaxon.Astronomy.Algorithms.Services;
using Galaxon.Time;

namespace Galaxon.ConsoleApp;

public class Metonic
{
    /// <summary>
    /// Display the length of relevant cycles in the time of Meton and now.
    /// </summary>
    public static void CycleLengthChanges()
    {
        int yearThen = -433;
        int yearNow = 2024;

        double dayLengthThen = EarthService.CalcSolarDayLength(yearThen / 100.0);
        Console.WriteLine($"The solar day length in {yearThen} was {dayLengthThen} days.");
        double dayLengthNow = EarthService.CalcSolarDayLength(yearNow / 100.0);
        Console.WriteLine($"The solar day length in {yearNow} is {dayLengthNow} seconds.");
        double dayDiff = dayLengthNow - dayLengthThen;
        string dayIncreaseVerb = dayLengthNow > dayLengthThen ? "increased" : "decreased";
        TimeSpan tsDayDiff = TimeSpan.FromSeconds(Math.Abs(dayDiff));
        Console.WriteLine($"The solar day has {dayIncreaseVerb} by {TimeSpanConversion.GetTimeString(tsDayDiff)}.");

        Console.WriteLine();

        double lunationLengthThen = LunaService.CalcLengthOfLunation(yearThen / 100.0);
        Console.WriteLine($"The lunation length in {yearThen} was {lunationLengthThen} days.");
        double lunationLengthNow = LunaService.CalcLengthOfLunation(yearNow / 100.0);
        Console.WriteLine($"The lunation length in {yearNow} is {lunationLengthNow} days.");
        double lunationDiff = lunationLengthNow - lunationLengthThen;
        string lunationIncreaseVerb = lunationLengthNow > lunationLengthThen ? "increased" : "decreased";
        TimeSpan tsLunationDiff = TimeSpan.FromDays(Math.Abs(lunationDiff));
        Console.WriteLine($"The lunation has {lunationIncreaseVerb} by {TimeSpanConversion.GetTimeString(tsLunationDiff)}.");

        Console.WriteLine();

        double yearLengthThen = EarthService.CalcTropicalYearLength(yearThen / 100.0);
        Console.WriteLine($"The tropical year length in {yearThen} was {yearLengthThen} days.");
        double yearLengthNow = EarthService.CalcTropicalYearLength(yearNow / 100.0);
        Console.WriteLine($"The tropical year length in {yearNow} is {yearLengthNow} days.");
        double yearDiff = yearLengthNow - yearLengthThen;
        string yearIncreaseVerb = yearLengthNow > yearLengthThen ? "increased" : "decreased";
        TimeSpan tsYearDiff = TimeSpan.FromDays(Math.Abs(yearDiff));
        Console.WriteLine($"The tropical year has {yearIncreaseVerb} by {TimeSpanConversion.GetTimeString(tsYearDiff)}.");
    }
}
