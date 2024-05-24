using System.Text.RegularExpressions;
using Galaxon.Astronomy.Algorithms.Records;
using Galaxon.Astronomy.Algorithms.Services;
using Galaxon.Astronomy.Data;
using Galaxon.Astronomy.Data.Enums;
using Galaxon.Astronomy.Data.Models;
using Galaxon.Astronomy.Data.Repositories;
using Galaxon.Astronomy.DataImport.DataTransferObjects;
using Galaxon.Core.Types;
using Galaxon.Time;
using Galaxon.Time.Extensions;
using HtmlAgilityPack;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Extensions;
using Newtonsoft.Json;

namespace Galaxon.Astronomy.DataImport.Services;

public class SeasonalMarkerImportService(
    AstroDbContext astroDbContext,
    AstroObjectRepository astroObjectRepository,
    SeasonalMarkerService seasonalMarkerService)
{
    /// <summary>
    /// Log an information message.
    /// </summary>
    private void LogInfo(string message, string planetName, int year, ESeasonalMarkerType type,
        string source, DateTime dt)
    {
        Slog.Information(
            "{Message}: planet = {Planet}, year = {Year}, Type = {Type}, DateTime from {Source} = {DateTime}",
            message, planetName, year, type.GetDisplayName(), source, dt.ToIsoString());
    }

    /// <summary>
    /// Look up a seasonal marker record in the database by DateTime within ±1 hour of a given
    /// DateTime.
    /// </summary>
    private SeasonalMarkerRecord? LookupRecord(DateTime dt)
    {
        return astroDbContext.SeasonalMarkers.FirstOrDefault(sm =>
            (sm.DateTimeUtcGalaxon != null
                && Abs(EF.Functions.DateDiffHour(sm.DateTimeUtcGalaxon, dt)!.Value) <= 1)
            || (sm.DateTimeUtcAstroPixels != null
                && Abs(EF.Functions.DateDiffHour(sm.DateTimeUtcAstroPixels, dt)!.Value) <= 1)
            || (sm.DateTimeUtcUsno != null
                && Abs(EF.Functions.DateDiffHour(sm.DateTimeUtcUsno, dt)!.Value) <= 1));
    }

    /// <summary>
    /// Compute and cache seasonal marker events.
    /// This will speed up the API and enables easy comparison with other sources using SQL.
    /// </summary>
    public async Task CacheCalculations()
    {
        // Load Earth.
        AstroObjectRecord earth = astroObjectRepository.LoadByName("Earth", "Planet");

        // Compute results for 1700-3000.
        // USNO provides results from 1700-2100.
        // AstroPixels provides results from 2001-2100.
        // The GetSeasonalMarkersInYear() method currently only supports up to year 3000.
        for (int year = 1700; year <= 3000; year++)
        {
            List<SeasonalMarkerEvent> smEvents =
                seasonalMarkerService.GetSeasonalMarkersInYear(year);
            foreach (SeasonalMarkerEvent smEvent in smEvents)
            {
                // Log it.
                LogInfo("Computed seasonal marker", earth.Name!, year, smEvent.SeasonalMarkerType,
                    "Galaxon", smEvent.DateTimeUtc);

                // Look for a matching record.
                SeasonalMarkerRecord? record = LookupRecord(smEvent.DateTimeUtc);

                if (record == null)
                {
                    // Insert new record.
                    record = new SeasonalMarkerRecord
                    {
                        AstroObjectId = earth.Id,
                        Year = year,
                        SeasonalMarkerType = smEvent.SeasonalMarkerType,
                        DateTimeUtcGalaxon = smEvent.DateTimeUtc
                    };
                    astroDbContext.SeasonalMarkers.Attach(record);
                    await astroDbContext.SaveChangesAsync();

                    // Log it.
                    LogInfo("Inserted seasonal marker record", earth.Name!, year,
                        smEvent.SeasonalMarkerType, "Galaxon", smEvent.DateTimeUtc);
                }
                else if (record.DateTimeUtcGalaxon != smEvent.DateTimeUtc)
                {
                    // Update existing record.
                    record.DateTimeUtcGalaxon = smEvent.DateTimeUtc;
                    await astroDbContext.SaveChangesAsync();

                    // Log it.
                    LogInfo("Updated seasonal marker record", earth.Name!, year,
                        smEvent.SeasonalMarkerType, "Galaxon", smEvent.DateTimeUtc);
                }
                else
                {
                    // No need to update.
                    LogInfo("Seasonal marker record already up-to-date", earth.Name!, year,
                        smEvent.SeasonalMarkerType, "Galaxon", smEvent.DateTimeUtc);
                }
            }
        }
    }

    /// <summary>
    /// Import seasonal markers from the USNO API.
    /// </summary>
    public async Task ImportFromUsno()
    {
        for (int year = 1700; year <= 2100; year++)
        {
            try
            {
                string apiUrl = $"https://aa.usno.navy.mil/api/seasons?year={year}";

                using HttpClient client = new ();
                HttpResponseMessage response = await client.GetAsync(apiUrl);

                if (response.IsSuccessStatusCode)
                {
                    await ImportFromUsnoJson(response);
                }
                else
                {
                    Slog.Error("Failed to retrieve data from USNO.");
                }
            }
            catch (Exception ex)
            {
                Slog.Error("Exception: {Exception}", ex.Message);
            }
        }
    }

    /// <summary>
    /// Extract seasonal markers from the USNO JSON response and import them into the database.
    /// </summary>
    /// <param name="response"></param>
    private async Task ImportFromUsnoJson(HttpResponseMessage response)
    {
        AstroObjectRecord earth = astroObjectRepository.LoadByName("Earth", "Planet");

        string jsonContent = await response.Content.ReadAsStringAsync();

        // Parse the JSON data to extract the seasonal marker data
        UsnoSeasonalMarkersForYear? usms =
            JsonConvert.DeserializeObject<UsnoSeasonalMarkersForYear>(jsonContent);
        if (usms?.data == null || usms.data.IsEmpty())
        {
            Slog.Error("Failed to extract data from the HTTP response.");
        }
        else
        {
            foreach (UsnoSeasonalMarker usm in usms.data)
            {
                ESeasonalMarkerType? seasonalMarkerType = usm switch
                {
                    { month: 3, phenom: "Equinox" } => ESeasonalMarkerType.NorthwardEquinox,
                    { month: 6, phenom: "Solstice" } => ESeasonalMarkerType.NorthernSolstice,
                    { month: 9, phenom: "Equinox" } => ESeasonalMarkerType.SouthwardEquinox,
                    { month: 12, phenom: "Solstice" } => ESeasonalMarkerType.SouthernSolstice,
                    _ => null
                };

                if (seasonalMarkerType == null)
                {
                    continue;
                }

                // Construct the datetime.
                DateOnly date = new (usm.year, usm.month, usm.day);
                TimeOnly time = TimeOnly.Parse(usm.time);
                DateTime dt = new (date, time, DateTimeKind.Utc);

                // Log it.
                LogInfo("Parsed seasonal marker", earth.Name!, usm.year, seasonalMarkerType.Value,
                    "USNO", dt);

                // Look for the record to update.
                SeasonalMarkerRecord? record = LookupRecord(dt);
                if (record == null)
                {
                    // Insert new record.
                    record = new SeasonalMarkerRecord
                    {
                        AstroObjectId = earth.Id,
                        Year = usm.year,
                        SeasonalMarkerType = seasonalMarkerType.Value,
                        DateTimeUtcGalaxon = dt
                    };
                    astroDbContext.SeasonalMarkers.Attach(record);
                    await astroDbContext.SaveChangesAsync();

                    // Log it.
                    LogInfo("Inserted seasonal marker record", earth.Name!, usm.year,
                        seasonalMarkerType.Value, "USNO", dt);
                }
                else if (record.DateTimeUtcUsno != dt)
                {
                    // Update existing record.
                    record.DateTimeUtcUsno = dt;
                    await astroDbContext.SaveChangesAsync();

                    // Log it.
                    LogInfo("Updated seasonal marker record", earth.Name!, usm.year,
                        seasonalMarkerType.Value, "USNO", dt);
                }
                else
                {
                    // Nothing to do.
                    LogInfo("Seasonal marker record already up-to-date", earth.Name!, usm.year,
                        seasonalMarkerType.Value, "USNO", dt);
                }
            }
        }
    }

    /// <summary>
    /// Import seasonal markers from the AstroPixels online ephemeris.
    /// See: <see href="https://www.astropixels.com/ephemeris/soleq2001.html"/>
    /// </summary>
    public async Task ImportFromAstroPixels()
    {
        // Use HttpClient to fetch the content.
        using HttpClient httpClient = new ();
        string html =
            await httpClient.GetStringAsync("https://www.astropixels.com/ephemeris/soleq2001.html");

        // Load HTML into the HtmlDocument.
        HtmlDocument htmlDoc = new ();
        htmlDoc.LoadHtml(html);

        // Prepare regular expressions.
        Regex rxDataLine = new (
            @"(?<year>\d{4})(\s+(?<month>[a-z]+)\s+(?<day>\d{2})\s+(?<hour>\d{2}):(?<minute>\d{2})){4}",
            RegexOptions.IgnoreCase);

        // Load Earth from the database.
        AstroObjectRecord earth = astroObjectRepository.LoadByName("Earth", "Planet");

        // Extract data from <pre> tags.
        foreach (HtmlNode? preNode in htmlDoc.DocumentNode.SelectNodes("//pre"))
        {
            // Get the non-empty lines from the <pre> tag.
            string[] lines = preNode.InnerText.Split(new[] { '\r', '\n' },
                StringSplitOptions.RemoveEmptyEntries);

            foreach (string line in lines)
            {
                MatchCollection matches = rxDataLine.Matches(line);
                if (matches.Count == 0)
                {
                    continue;
                }

                // Get the year.
                int year = int.Parse(matches[0].Groups["year"].Value);

                // Get the dates and times of the seasonal markers.
                for (int sm = 0; sm < 4; sm++)
                {
                    ESeasonalMarkerType seasonalMarkerType = (ESeasonalMarkerType)sm;

                    // Extract the date parts.
                    int month =
                        GregorianCalendarUtility.MonthNameToNumber(matches[0].Groups["month"]
                            .Captures[sm].Value);
                    int day = int.Parse(matches[0].Groups["day"].Captures[sm].Value);
                    int hour = int.Parse(matches[0].Groups["hour"].Captures[sm].Value);
                    int minute = int.Parse(matches[0].Groups["minute"].Captures[sm].Value);

                    // Construct the datetime.
                    DateTime dt = new (year, month, day, hour, minute, 0, DateTimeKind.Utc);

                    // Log it.
                    LogInfo("Parsed seasonal marker", earth.Name!, year, seasonalMarkerType,
                        "AstroPixels", dt);

                    // See if we need to update or insert a record, or do nothing.
                    SeasonalMarkerRecord? record = LookupRecord(dt);
                    if (record == null)
                    {
                        // Insert new record.
                        record = new SeasonalMarkerRecord
                        {
                            AstroObjectId = earth.Id,
                            Year = year,
                            SeasonalMarkerType = seasonalMarkerType,
                            DateTimeUtcAstroPixels = dt
                        };
                        astroDbContext.SeasonalMarkers.Add(record);
                        await astroDbContext.SaveChangesAsync();

                        // Log it.
                        LogInfo("Inserted seasonal marker record", earth.Name!, year,
                            seasonalMarkerType, "AstroPixels", dt);
                    }
                    else if (record.DateTimeUtcAstroPixels != dt)
                    {
                        // Update existing record.
                        record.DateTimeUtcAstroPixels = dt;
                        await astroDbContext.SaveChangesAsync();

                        // Log it.
                        LogInfo("Updated seasonal marker record", earth.Name!, year,
                            seasonalMarkerType, "AstroPixels", dt);
                    }
                    else
                    {
                        // Log it.
                        LogInfo("Seasonal marker record already up-to-date", earth.Name!, year,
                            seasonalMarkerType, "AstroPixels", dt);
                    }
                }
            }
        }
    }
}
