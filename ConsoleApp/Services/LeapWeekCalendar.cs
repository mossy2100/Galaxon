using System.Globalization;
using Galaxon.Core.Strings;
using Galaxon.Time;

namespace Galaxon.ConsoleApp.Services;

public class LeapWeekCalendar
{
    public static bool IsLeapYear2(int y, int den, int a, int r)
    {
        return y % den % a == r;
    }

    public static bool IsLeapYear3(int y, int den, int a, int b, int r)
    {
        return y % den % a % b == r;
    }

    public static bool IsLeapYear4(int y, int den, int a, int b, int c, int r)
    {
        return y % den % a % b % c == r;
    }

    public static int DaysInMonth(int y, int m)
    {
        if (m == 12) return IsLeapYear3(y, 355, 17, 5, 2) ? 37 : 30;
        if (m % 3 == 1) return 31;
        return 30;
    }

    public static void FindIntercalationFraction()
    {
        double weeksPerYear = TimeConstants.DAYS_PER_TROPICAL_YEAR / TimeConstants.DAYS_PER_WEEK;
        double maxDiff = 30.0 / TimeConstants.SECONDS_PER_WEEK;
        FractionFinder.FindFraction(weeksPerYear, maxDiff, ETimeUnit.Week);
    }

    public static void FindIntercalationRule()
    {
        int n = 74;
        int d = 417;
        Console.WriteLine();
        Console.WriteLine($"Searching for intercalation formula for fraction {n}/{d}...");

        // Console.WriteLine();
        // Console.WriteLine("2-modulo solutions:");
        // RuleFinder.FindRuleWith2Mods(n, d);
        //
        // Console.WriteLine();
        // Console.WriteLine("3-modulo solutions:");
        // RuleFinder.FindRuleWith3Mods(n, d);

        Console.WriteLine();
        Console.WriteLine("4-modulo solutions:");
        RuleFinder.FindRuleWith4Mods(n, d);
    }

    public static void VerifyIntercalationRule()
    {
        int numYears = 355;
        int numLeapYears = 63;
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
        int den = 417;
        int a = 45;
        int b = 17;
        int c = 6;
        int r = 0;

        Console.Write("  ");

        for (var y = 0; y < den; y++)
        {
            Console.Write(IsLeapYear4(y, den, a, b, c, r) ? 1 : 0);
            Console.Write(" ");
            if (y % den % a == a - 1)
            {
                Console.WriteLine();
                Console.Write("  ");
            }
            if (y % den % a % b == b - 1)
            {
                Console.Write("  ");
            }
        }
        Console.WriteLine();
    }

    public static void PrintCalendarPages12a()
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

    public static void PrintCalendarPages12b()
    {
        List<PrintedMonth> printedMonths = new ();
        int dayOfWeek = 0;
        int[] longMonths = [1, 4, 7, 10, 12];

        for (int m = 1; m <= 12; m++)
        {
            PrintedMonth printedMonth = new ();

            // Add the title and the days of the week.
            string title = LatinMonthNames[m].PadBoth(28, ' ', false);
            printedMonth.AddLine(title);
            printedMonth.AddLine(" Mon Tue Wed Thu Fri Sat Sun");

            string curLine = "    ".Repeat(dayOfWeek);
            int daysInMonth = longMonths.Contains(m) ? 35 : 28;
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

    public static void PrintIsoWeekCalendarLeapYearPattern()
    {
        GregorianCalendar gc = new ();
        Dictionary<int, bool> leapYears = new ();
        Console.Write("  ");
        for (int y = 2001; y < 2401; y++)
        {
            if (ISOWeek.GetWeeksInYear(y) == 53)
            {
                leapYears.Add(y, true);
                Console.Write(" 1 ");
            }
            else
            {
                Console.Write(" 0 ");
            }
            if (y % 25 == 0)
            {
                Console.WriteLine();
                Console.Write("  ");
            }
        }
        Console.WriteLine();
        Console.WriteLine($"Total number of leap years = {leapYears.Count}");
    }

    private static void PrintIsoWeek(int y, int w)
    {
        string strW = w.ToString().ZeroPad();
        Console.Write($"Week {strW}:");
        for (int i = 0; i < 7; i++)
        {
            DayOfWeek dow = i == 6 ? DayOfWeek.Sunday : (DayOfWeek)(i + 1);
            DateTime d = ISOWeek.ToDateTime(y, w, dow);
            Console.Write($"  {d.GetDateOnly().ToIsoString()}");
        }
        Console.WriteLine();
    }

    public static void PrintExampleIsoWeekCalendar()
    {
        Console.WriteLine("             Mon         Tue         Wed         Thu         Fri         Sat         Sun");
        for (int y = 2025; y <= 2034; y++)
        {
            Console.WriteLine(y);
            PrintIsoWeek(y, 1);
            Console.WriteLine("...");
            PrintIsoWeek(y, ISOWeek.GetWeeksInYear(y));
            Console.WriteLine();
            Console.WriteLine();
        }
    }

    public static void PrintYearsEndOnSunday()
    {
        Console.WriteLine("Years starting on Monday.");
        for (int y = 2024; y <= 2100; y++)
        {
            DateOnly d = new DateOnly(y, 1, 1);
            if (d.DayOfWeek == DayOfWeek.Monday)
            {
                Console.Write($"{y}, ");
            }
        }
        Console.WriteLine();
    }
}
