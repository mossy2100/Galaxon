using Galaxon.Core.Strings;
using Galaxon.Time;

namespace Galaxon.ConsoleApp;

public class LeapWeekCalendar
{
    private static int num = 11;

    private static int den = 62;

    public static bool IsLeapYear(int y)
    {
        return y % den % 6 == 0;
    }

    public static int DaysInMonth(int y, int m)
    {
        if (m == 12) return IsLeapYear(y) ? 37 : 30;
        if (m % 3 == 1) return 31;
        return 30;
    }

    public static void FindIntercalationFraction()
    {
        double weeksPerYear = TimeConstants.DAYS_PER_TROPICAL_YEAR / TimeConstants.DAYS_PER_WEEK;
        double maxDiff = 30.0 / TimeConstants.SECONDS_PER_WEEK;
        FractionFinder.FindFraction(weeksPerYear, maxDiff, TimeConstants.SECONDS_PER_WEEK);
    }

    public static void FindIntercalationRule()
    {
        RuleFinder.FindRuleWith2Mods(num, den);
        // RuleFinder.FindRuleWith3Mods(num, den);
    }

    public static void VerifyIntercalationRule()
    {
        int numYears = den;
        int numLeapYears = num;
        int numCommonYears = numYears - numLeapYears;
        int numWeeks = numLeapYears * 53 + numCommonYears * 52;
        int numDays = numWeeks * 7;
        double numDays2 = numYears * TimeConstants.DAYS_PER_TROPICAL_YEAR;
        double diffDays = Math.Abs(numDays2 - numDays);
        double diffSeconds = diffDays * TimeConstants.SECONDS_PER_DAY;
        double diffSecondsPerYear = diffSeconds / numYears;
        Console.WriteLine($"{diffSecondsPerYear} seconds per year.");
    }

    public static void PrintLeapWeekPattern()
    {
        for (var y = 0; y < den; y++)
        {
            Console.Write(IsLeapYear(y) ? 1 : 0);
            Console.Write("  ");
            if (y % 6 == 5)
            {
                Console.WriteLine();
            }
        }
    }

    public static void PrintCalendarPages12()
    {
        List<PrintedMonth> printedMonths = new ();
        int dayOfWeek = 0;
        for (int m = 1; m <= 12; m++)
        {
            PrintedMonth printedMonth = new ();

            // Add the title and the days of the week.
            string title = GregorianCalendarExtensions.MonthNumberToName(m).PadBoth(28, ' ', false);
            printedMonth.AddLine(title);
            printedMonth.AddLine(" Mon Tue Wed Thu Fri Sat Sun");

            string curLine = "    ".Repeat(dayOfWeek);
            for (int d = 1; d <= DaysInMonth(0, m); d++)
            {
                curLine += $"{d,4}";

                // Go to next day.
                dayOfWeek++;

                // Check for new line.
                if (dayOfWeek == 7)
                {
                    dayOfWeek = 0;
                    printedMonth.AddLine(curLine);
                    curLine = "";
                }
            }

            // Add an incomplete line.
            if (curLine != "")
            {
                curLine = curLine.PadRight(28);
                printedMonth.AddLine(curLine);
            }

            printedMonths.Add(printedMonth);
        }

        string blankLine = "                            ";

        for (int row = 0; row < 4; row++)
        {
            // How many lines?
            int i = row * 3;
            int ln = 0;
            var pm1 = printedMonths[i];
            var pm2 = printedMonths[i + 1];
            var pm3 = printedMonths[i + 2];
            while (true)
            {
                string fullLine = $"{pm1.GetLine(ln)}   {pm2.GetLine(ln)}   {pm3.GetLine(ln)}";
                Console.WriteLine(fullLine);
                if (string.IsNullOrWhiteSpace(fullLine))
                {
                    break;
                }
                ln++;
            }
        }
    }

    /// <summary>
    /// Dictionary mapping month numbers (1-13) to Latin month names.
    /// </summary>
    public static readonly Dictionary<int, string> LatinMonthNames = new ()
    {
        { 1, "Unumber" },
        { 2, "Duober" },
        { 3, "Triaber" },
        { 4, "Quattuorber" },
        { 5, "Quinqueber" },
        { 6, "Sexber" },
        { 7, "September" },
        { 8, "October" },
        { 9, "November" },
        { 10, "December" },
        { 11, "Undecimber" },
        { 12, "Duodecimber" },
        { 13, "Tredecimber" }
    };

    public static void PrintCalendarPages13()
    {
        List<PrintedMonth> printedMonths = new ();
        int dayOfWeek = 0;
        for (int m = 1; m <= 13; m++)
        {
            PrintedMonth printedMonth = new ();

            // Add the title and the days of the week.
            string title = LatinMonthNames[m].PadBoth(28, ' ', false);
            printedMonth.AddLine(title);
            printedMonth.AddLine(" Mon Tue Wed Thu Fri Sat Sun");

            string curLine = "    ".Repeat(dayOfWeek);
            int daysInMonth = m == 13 ? 35 : 28;
            for (int d = 1; d <= daysInMonth; d++)
            {
                curLine += $"{d,4}";

                // Go to next day.
                dayOfWeek++;

                // Check for new line.
                if (dayOfWeek == 7)
                {
                    dayOfWeek = 0;
                    printedMonth.AddLine(curLine);
                    curLine = "";
                }
            }

            // Add an incomplete line.
            if (curLine != "")
            {
                curLine = curLine.PadRight(28);
                printedMonth.AddLine(curLine);
            }

            printedMonths.Add(printedMonth);
        }

        string blankLine = "                            ";

        for (int row = 0; row < 5; row++)
        {
            // How many lines?
            int i = row * 3;
            int ln = 0;
            while (true)
            {
                string line1 = i < printedMonths.Count ? printedMonths[i].GetLine(ln) : blankLine;
                string line2 = i + 1 < printedMonths.Count ? printedMonths[i + 1].GetLine(ln) : blankLine;
                string line3 = i + 2 < printedMonths.Count ? printedMonths[i + 2].GetLine(ln) : blankLine;
                string fullLine = $"{line1}   {line2}   {line3}";
                Console.WriteLine(fullLine);
                if (string.IsNullOrWhiteSpace(fullLine))
                {
                    break;
                }
                ln++;
            }
        }
    }
}
