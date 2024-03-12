using System.Text.RegularExpressions;
using Galaxon.Astronomy.Data;
using Galaxon.Astronomy.Data.Enums;
using Galaxon.Astronomy.Data.Models;

namespace Galaxon.Astronomy.DataImport.Services;

public class LunarPhaseDataImportService
{
    /// <summary>
    /// Extract the lunar phase data from the web pages captured from
    /// AstroPixels and copy the data into the database.
    /// </summary>
    public static void ParseLunarPhaseData()
    {
        Regex rxDataLine =
            new (@"(\d{4})?\s+(([a-z]{3})\s+(\d{1,2})\s+(\d{2}):(\d{2})\s+([a-z])?){1,4}",
                RegexOptions.IgnoreCase);
        int? year = null;
        string? curYearStr = null;

        using AstroDbContext astroDbContext = new ();

        for (var century = 0; century < 20; century++)
        {
            int startYear = century * 100 + 1;
            int endYear = (century + 1) * 100;
            string[] lines = File.ReadAllLines(
                $"{AstroDbContext.DataDirectory()}/Lunar phases/AstroPixels - Moon Phases_ {startYear:D4} to {endYear:D4}.html");
            foreach (string line in lines)
            {
                MatchCollection matches = rxDataLine.Matches(line);
                if (matches.Count <= 0)
                {
                    continue;
                }

                // Get the year, if present.
                string yearStr = line.Substring(1, 4);
                if (yearStr.Trim() != "")
                {
                    curYearStr = yearStr;
                    year = int.Parse(yearStr);
                }

                // The dates are meaningless without the year. This
                // shouldn't happen, but if we don't know the year, skip
                // parsing the dates.
                if (year == null)
                {
                    continue;
                }

                // Get the dates and times of the phases, if present.
                for (var phase = 0; phase < 4; phase++)
                {
                    string dateStr = line.Substring(8 + phase * 18, 13);
                    if (dateStr.Trim() == "")
                    {
                        continue;
                    }
                    bool parseOk = DateTime.TryParse($"{curYearStr} {dateStr}",
                        out DateTime phaseDateTime);
                    if (parseOk)
                    {
                        // Set the datetime to UTC.
                        phaseDateTime = DateTime.SpecifyKind(phaseDateTime, DateTimeKind.Utc);

                        // Store phase information in the database.
                        if (astroDbContext.LunarPhases != null)
                        {
                            astroDbContext.LunarPhases.Add(new LunarPhase
                            {
                                PhaseType = (ELunarPhase)phase,
                                DateTimeUTC = phaseDateTime
                            });
                            astroDbContext.SaveChanges();
                        }
                    }
                }
            }
        }
    }
}
