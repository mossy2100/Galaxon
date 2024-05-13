using System.Globalization;
using Galaxon.Astronomy.Calendars;

namespace Galaxon.Tests.Calendars;

public class CalendarTesting
{
    public static void ShowCalendarInfo(Calendar cal)
    {
        Console.WriteLine(cal);
        foreach (int era in cal.Eras)
        {
            Console.WriteLine($"Era {era}");
        }
    }

    public static void RunCalendarTests()
    {
        GregorianCalendar gcal = new();
        ShowCalendarInfo(gcal);

        JapaneseCalendar jcal = new();
        ShowCalendarInfo(jcal);

        HarmonyCalendar hcal = new ();

        for (int year = 2002; year <= 2010; year++)
        {
            Console.WriteLine(
                $"\nThe {year} NVE occurs at {hcal.GetSeasonalMarker(year, 0)}.");
            DateOnly[] testValues =
            {
                new(year, 3, 1),
                new(year, 3, 20),
                new(year, 3, 21),
                new(year, 3, 31)
            };
            foreach (DateOnly d in testValues)
            {
                HarmonyDate hd = new(d);
                Console.WriteLine($"{d} is day {hd.DayOfYear} of Harmony Year {hd.Year}.");
            }
        }

        Console.WriteLine();

        HarmonyCalendar hc = new();

        for (int y = 2002; y <= 2099; y++)
        {
            Console.Write($"There are {hc.GetDaysInYear(y)} days in Harmony Year {y}.");
            string isLeapYearString = hc.IsLeapYear(y) ? "IS" : "IS NOT";
            Console.WriteLine($" {y} {isLeapYearString} a leap year.");
        }
        Console.WriteLine();
    }
}
