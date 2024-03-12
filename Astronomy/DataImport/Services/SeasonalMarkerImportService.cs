using System.Text.RegularExpressions;
using Galaxon.Astronomy.Data;
using Galaxon.Astronomy.Data.Models;

namespace Galaxon.Astronomy.DataImport.Services;

public class SeasonalMarkerImportService
{
    /// <summary>
    /// Extract the seasonal marker data from the text file and copy the data
    /// into the database.
    /// </summary>
    public static void ParseSeasonalMarkerData()
    {
        using AstroDbContext astroDbContext = new ();

        // Lookup table to help with the parsing.
        Dictionary<string, int> MonthAbbrevs = new ()
        {
            { "Mar", 3 },
            { "Jun", 6 },
            { "Sep", 9 },
            { "Dec", 12 }
        };

        // Get the data from the data file as an array of strings.
        var dataFilePath =
            $"{AstroDbContext.DataDirectory()}/Seasonal markers/SeasonalMarkers2001-2100.txt";
        string[] lines = File.ReadAllLines(dataFilePath);

        // Convert the lines into our internal data structure.
        Regex rx = new ("\\s+");
        foreach (string line in lines)
        {
            string[] words = rx.Split(line);

            if (words.Length <= 1 || words[0] == "Year")
            {
                continue;
            }

            // Extract the dates from the row.
            var year = int.Parse(words[1]);
            for (var i = 0; i < 4; i++)
            {
                int j = i * 3;
                string monthAbbrev = words[j + 2];
                int month = MonthAbbrevs[monthAbbrev];
                var dayOfMonth = int.Parse(words[j + 3]);
                string[] time = words[j + 4].Split(":");
                var hour = int.Parse(time[0]);
                var minute = int.Parse(time[1]);

                // Construct the new DateTime object.
                DateTime seasonalMarkerDateTime = new (year, month, dayOfMonth, hour, minute, 0,
                    DateTimeKind.Utc);

                // Check if there is already an entry in the database table
                // for this seasonal marker.
                SeasonalMarker? sm = astroDbContext.SeasonalMarkers?
                    .FirstOrDefault(sm => sm.DateTimeUTC.Year == year && sm.MarkerNumber == i);

                // Add a new row or update the existing row as required.
                if (sm == null)
                {
                    // Add a new row.
                    astroDbContext.SeasonalMarkers!.Add(new SeasonalMarker
                    {
                        MarkerNumber = i,
                        DateTimeUTC = seasonalMarkerDateTime
                    });
                }
                else
                {
                    // Update the row.
                    sm.DateTimeUTC = seasonalMarkerDateTime;
                }
            }
        }

        astroDbContext.SaveChanges();
    }
}
