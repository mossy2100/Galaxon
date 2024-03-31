using Galaxon.ConsoleApp.Services;

namespace Galaxon.ConsoleApp;

class Program
{
    static void Main()
    {
        // LeapWeekCalendar.FindIntercalationFraction();
        // LeapWeekCalendar.FindIntercalationRule();
        // LeapWeekCalendar.VerifyIntercalationRule();
        // LeapWeekCalendar.PrintLeapWeekPattern();
        // LeapWeekCalendar.PrintCalendarPages13();
        LeapWeekCalendar.TestLeapYearPattern(11, 62, 17, 6);
        // TropicalYear.GetAverageTropicalLengthPerMillennium();
        // RuleFinder.FindRuleWith3Mods(121, 500);
        // LunisolarCalendar.FindEpoch();
    }
}
