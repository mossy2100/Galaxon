using System.Text.RegularExpressions;
using Galaxon.Astronomy.Data;
using Galaxon.Astronomy.Data.Models;
using Galaxon.Time;
using Microsoft.Extensions.Logging;

namespace Galaxon.Astronomy.DataImport.Services;

public class EasterDateImportService(
    ILogger<EasterDateImportService> logger,
    AstroDbContext astroDbContext)
{
    /// <summary>
    /// Parse the data file from the US Census Bureau.
    /// </summary>
    /// <see href="https://www.census.gov/data/software/x13as/genhol/easter-dates.html"/>
    internal void ImportEasterDates1600_2099()
    {
        logger.LogInformation("Parsing easter dates 1600-2999 from {Url}.",
            "https://www.census.gov/data/software/x13as/genhol/easter-dates.html");

        var csvFile =
            $"{AstroDbContext.DataDirectory()}/Easter/Easter Sunday Dates 1600-2099.csv";
        using StreamReader reader = new (csvFile);

        while (!reader.EndOfStream)
        {
            string? line = reader.ReadLine()?.Trim();
            if (string.IsNullOrEmpty(line))
            {
                continue;
            }

            string[] values = line.Split(',');
            if (values.Length != 3)
            {
                continue;
            }

            try
            {
                var year = int.Parse(values[0]);
                var month = int.Parse(values[1]);
                var day = int.Parse(values[2]);
                DateOnly newEasterDate = new (year, month, day);

                // See if we already have one for this year.
                EasterDate? existingEasterDate = astroDbContext.EasterDates
                    .FirstOrDefault(ed => ed.Date.Year == year);

                // Add or update the record as needed.
                if (existingEasterDate == null)
                {
                    // Add a record.
                    // Console.WriteLine($"Adding new Easter date {newEasterDate}");
                    logger.LogInformation("Adding new Easter date {NewEasterDate}",
                        newEasterDate.ToIsoString());
                    astroDbContext.EasterDates.Add(new EasterDate { Date = newEasterDate });
                }
                else if (existingEasterDate.Date != newEasterDate)
                {
                    // Update the record.
                    // Console.WriteLine(
                    //     $"Dates for {year} are not the same! Existing = {existingEasterDate.Date}, new = {newEasterDate}");
                    logger.LogWarning(
                        "Dates for {Year} are not the same. Not updating record. Existing = {ExistingEasterDate}, new = {NewEasterDate}",
                        year, existingEasterDate.Date.ToIsoString(), newEasterDate.ToIsoString());
                }
                else
                {
                    // Console.WriteLine($"Dates for {year} are the same, nothing to do.");
                    logger.LogInformation("Dates for {Year} are the same, nothing to do.", year);
                }
            }
            catch
            {
                // Console.WriteLine($"Invalid line in CSV: {line}");
                logger.LogError("Invalid line in CSV: {Line}", line);
            }
        }

        astroDbContext.SaveChanges();
    }

    /// <summary>
    /// Parse the data file from the Astronomical Society of South Australia.
    /// </summary>
    /// <see href="https://www.assa.org.au/edm"/>
    internal void ImportEasterDates1700_2299()
    {
        logger.LogInformation("Parsing easter dates 1600-2999 from {Url}.",
            "https://www.assa.org.au/edm");

        var htmlFile =
            $"{AstroDbContext.DataDirectory()}/Easter/Easter Sunday Dates 1700-2299.html";
        Regex rx = new (@"(\d{1,2})(st|nd|rd|th) (March|April) (\d{4})");

        using StreamReader reader = new (htmlFile);

        while (!reader.EndOfStream)
        {
            string? line = reader.ReadLine()?.Trim();
            if (string.IsNullOrEmpty(line))
            {
                continue;
            }

            MatchCollection matches = rx.Matches(line);
            if (matches.Count <= 0)
            {
                continue;
            }
            foreach (Match match in matches)
            {
                var year = int.Parse(match.Groups[4].Value);
                int month = match.Groups[3].Value == "March" ? 3 : 4;
                var day = int.Parse(match.Groups[1].Value);
                DateOnly newEasterDate = new (year, month, day);
                // See if we already have one for this year.
                EasterDate? existingEasterDate = astroDbContext.EasterDates
                    .FirstOrDefault(ed => ed.Date.Year == year);
                if (existingEasterDate == null)
                {
                    astroDbContext.EasterDates.Add(new EasterDate { Date = newEasterDate });
                }
                else
                {
                    // Check if they are different.
                    if (existingEasterDate.Date != newEasterDate)
                    {
                        logger.LogWarning(
                            "Dates for {Year} are not the same. Not updating record. Existing = {ExistingEasterDate}, new = {NewEasterDate}",
                            year, existingEasterDate.Date.ToIsoString(),
                            newEasterDate.ToIsoString());
                    }
                    else
                    {
                        // Console.WriteLine($"Dates for {year} are the same, nothing to do.");
                        logger.LogInformation("Dates for {Year} are the same, nothing to do.",
                            year);
                    }
                }
            }
        }

        astroDbContext.SaveChanges();
    }
}
