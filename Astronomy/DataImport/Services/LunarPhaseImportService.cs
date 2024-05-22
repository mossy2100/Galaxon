using System.Globalization;
using System.Text.RegularExpressions;
using Galaxon.Astronomy.Algorithms.Records;
using Galaxon.Astronomy.Algorithms.Services;
using Galaxon.Astronomy.Data;
using Galaxon.Astronomy.Data.Enums;
using Galaxon.Astronomy.Data.Models;
using Galaxon.Astronomy.DataImport.DataTransferObjects;
using Galaxon.Core.Collections;
using Galaxon.Core.Strings;
using Galaxon.Core.Types;
using Galaxon.Time;
using Galaxon.Time.Utilities;
using HtmlAgilityPack;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace Galaxon.Astronomy.DataImport.Services;

public class LunarPhaseImportService(AstroDbContext astroDbContext, MoonService moonService)
{
    /// <summary>
    /// Get the links to the AstroPixels lunar phase data tables.
    /// <see href="http://astropixels.com/ephemeris/phasescat/phasescat.html"/>
    /// </summary>
    /// <returns></returns>
    /// <exception cref="InvalidOperationException"></exception>
    public async Task<List<string>> GetAstroPixelsEphemerisPageUrls()
    {
        List<string> result = new ();
        string indexUrl = "https://astropixels.com/ephemeris/phasescat/phasescat.html";
        string tableTitle = "Moon Phases in Common Era (CE)";

        // Use HttpClient to fetch the content.
        using HttpClient httpClient = new ();
        string html = await httpClient.GetStringAsync(indexUrl);

        // Load HTML into the HtmlDocument.
        HtmlDocument htmlDoc = new ();
        htmlDoc.LoadHtml(html);

        // Process tables by looking for specific <thead> texts.
        HtmlNode? thead = htmlDoc.DocumentNode.SelectSingleNode(
            $"//thead[tr/td[contains(text(), '{tableTitle}')]]");
        HtmlNode? table = thead?.Ancestors("table").FirstOrDefault();

        // Get all the links in the table.
        HtmlNodeCollection? links = table?.SelectNodes(".//a");
        if (links == null)
        {
            throw new InvalidOperationException("Could not find any links.");
        }

        // Parse the links and add to results.
        foreach (HtmlNode link in links)
        {
            string linkHref = link.GetAttributeValue("href", string.Empty).Trim();
            if (!string.IsNullOrEmpty(linkHref))
            {
                // Ensure the URL is absolute
                Uri linkUri = new (new Uri(indexUrl), linkHref);
                result.Add(linkUri.AbsoluteUri);
            }
        }

        return result;
    }

    /// <summary>
    /// Parse one AstroPixels lunar phase data page.
    /// </summary>
    /// <param name="url"></param>
    /// <returns></returns>
    /// <exception cref="InvalidOperationException"></exception>
    public async Task ImportAstroPixelsPage(string url)
    {
        JulianCalendar jcal = JulianCalendarUtility.JulianCalendarInstance;

        // Use HttpClient to fetch the content.
        using HttpClient httpClient = new ();
        string html = await httpClient.GetStringAsync(url);

        // Load HTML into the HtmlDocument.
        HtmlDocument htmlDoc = new ();
        htmlDoc.LoadHtml(html);

        // Get all the divs with the data. There should be 10 with data, but there can be additional
        // ones without data. They won't cause anything to break though.
        HtmlNodeCollection divs = htmlDoc.DocumentNode.SelectNodes("//div[@class='pbox1']");

        // Prepare regular expressions.
        Regex rxDataLine = new (
            @"(?<year>\d{4})?\s+(([a-z]{3})\s{1,2}(\d{1,2})\s{2}(\d{2}):(\d{2})\s+([a-z])?){1,4}",
            RegexOptions.IgnoreCase);
        Regex rxDateTime = new (
            @"(?<month>[a-z]{3})\s{1,2}(?<day>\d{1,2})\s{2}(?<hour>\d{2}):(?<minute>\d{2})",
            RegexOptions.IgnoreCase);

        // Loop through the data divs and parse each one.
        foreach (HtmlNode div in divs)
        {
            // Reset the year.
            int year = 0;

            // Get the lines of test from the <pre> tag.
            HtmlNode? pre = div.SelectSingleNode(".//pre");
            if (pre == null)
            {
                continue;
            }

            string text = pre.InnerHtml.StripTags();
            string[] lines = text.Split("\n");

            foreach (string line in lines)
            {
                Console.WriteLine(line);
                MatchCollection matches = rxDataLine.Matches(line);
                if (matches.Count == 0)
                {
                    continue;
                }

                // Get the year, if present.
                string yearStr = matches[0].Groups["year"].Value;
                if (!string.IsNullOrWhiteSpace(yearStr))
                {
                    year = int.Parse(yearStr);
                }
                // If the year isn't set, that's a problem. This shouldn't happen.
                if (year == 0)
                {
                    throw new InvalidOperationException("Unknown year.");
                }

                // Get the dates and times of the phases, if present.
                for (byte phaseTypeIndex = 0; phaseTypeIndex < 4; phaseTypeIndex++)
                {
                    string dateStr = line.Substring(8 + phaseTypeIndex * 18, 13);
                    MatchCollection dateTimeMatches = rxDateTime.Matches(dateStr);
                    if (dateTimeMatches.Count == 0)
                    {
                        continue;
                    }

                    // Extract the date parts.
                    int month =
                        GregorianCalendarUtility.MonthNameToNumber(dateTimeMatches[0].Groups["month"].Value);
                    int day = int.Parse(dateTimeMatches[0].Groups["day"].Value);
                    int hour = int.Parse(dateTimeMatches[0].Groups["hour"].Value);
                    int minute = int.Parse(dateTimeMatches[0].Groups["minute"].Value);

                    // Construct the datetime.
                    DateTime dt;
                    // Check for Julian date.
                    if (JulianCalendarUtility.IsValidDate(year, month, day))
                    {
                        // Construct a DateTime (Gregorian) from the Julian date parts.
                        dt = jcal.ToDateTime(year, month, day, hour, minute, 0, 0);
                        DateTime.SpecifyKind(dt, DateTimeKind.Utc);
                    }
                    else
                    {
                        // Handle error in the AstroPixels data.
                        if (year == 3869 && month == 1 && day == 1 && hour == 0 && minute == 0)
                        {
                            year = 3870;
                        }

                        // Construct the DateTime.
                        dt = new DateTime(year, month, day, hour, minute, 0, DateTimeKind.Utc);
                    }

                    // Get the Lunation Number.
                    int LN = GetLunationNumber(dt);
                    ELunarPhaseType phaseTypeType = (ELunarPhaseType)phaseTypeIndex;

                    // See if we need to update or insert a record.
                    LunarPhaseRecord? existingLunarPhase = LookupRecord(dt);
                    if (existingLunarPhase == null)
                    {
                        // Insert new record.
                        LunarPhaseRecord newLunarPhase = new ()
                        {
                            LunationNumber = LN,
                            PhaseType = phaseTypeType,
                            DateTimeUtcAstroPixels = dt
                        };
                        astroDbContext.LunarPhases.Add(newLunarPhase);
                    }
                    else
                    {
                        // Update existing record.
                        existingLunarPhase.LunationNumber = LN;
                        existingLunarPhase.PhaseType = phaseTypeType;
                        existingLunarPhase.DateTimeUtcAstroPixels = dt;
                    }
                    await astroDbContext.SaveChangesAsync();
                }
            }
        }
    }

    /// <summary>
    /// Extract lunar phase data from the AstroPixels web pages and copy them to the database.
    /// </summary>
    public async Task ImportAstroPixels()
    {
        // Get the links to the catalog pages (CE only).
        List<string> urls = await GetAstroPixelsEphemerisPageUrls();

        // One page at a time.
        foreach (string url in urls)
        {
            // Import all the lunar phases on this page.
            await ImportAstroPixelsPage(url);
        }
    }

    /// <summary>
    /// Import the lunar phase data from the USNO API.
    /// </summary>
    /// <exception cref="InvalidOperationException"></exception>
    public async Task ImportUsno()
    {
        for (int year = 1700; year <= 2100; year++)
        {
            string apiUrl = $"https://aa.usno.navy.mil/api/moon/phases/year?year={year}";
            using HttpClient client = new ();
            HttpResponseMessage response = await client.GetAsync(apiUrl);
            if (response.IsSuccessStatusCode)
            {
                string jsonContent = await response.Content.ReadAsStringAsync();
                // Parse the JSON data to extract the seasonal marker data
                UsnoLunarPhasesForYear? ulps =
                    JsonConvert.DeserializeObject<UsnoLunarPhasesForYear>(jsonContent);
                if (ulps?.phasedata == null || ulps.phasedata.IsEmpty())
                {
                    Console.WriteLine(
                        $"Failed to retrieve data. Status code: {response.StatusCode}");
                }
                else
                {
                    foreach (UsnoLunarPhase ulp in ulps.phasedata)
                    {
                        Console.WriteLine();

                        // Get the phase and phase number.
                        ELunarPhaseType phaseType;
                        bool parsedPhase;
                        if (ulp.phase == "Last Quarter")
                        {
                            phaseType = ELunarPhaseType.ThirdQuarter;
                            parsedPhase = true;
                        }
                        else
                        {
                            parsedPhase = EnumExtensions.TryParse(ulp.phase, out phaseType);
                        }
                        if (!parsedPhase)
                        {
                            throw new InvalidOperationException("Error parsing lunar phase.");
                        }
                        byte phaseTypeIndex = (byte)phaseType;

                        // Construct the datetime.
                        DateOnly date = new (year, ulp.month, ulp.day);
                        TimeOnly time = TimeOnly.Parse(ulp.time);
                        DateTime dt = new (date, time, DateTimeKind.Utc);

                        // Get the Lunation Number and phase type.
                        int lunationNumber = GetLunationNumber(dt);
                        ELunarPhaseType phaseTypeType = (ELunarPhaseType)phaseTypeIndex;

                        // Print.
                        Console.WriteLine(
                            $"Lunation {lunationNumber:10}, Phase {ulp.phase,20} => {dt.ToIsoString()}");

                        // See if we need to update or insert a record.
                        LunarPhaseRecord? existingLunarPhase = LookupRecord(dt);
                        if (existingLunarPhase == null)
                        {
                            // Insert new record.
                            LunarPhaseRecord newLunarPhase = new ()
                            {
                                LunationNumber = lunationNumber,
                                PhaseType = phaseTypeType,
                                DateTimeUtcUsno = dt
                            };
                            astroDbContext.LunarPhases.Add(newLunarPhase);
                        }
                        else
                        {
                            // Update existing record.
                            existingLunarPhase.LunationNumber = lunationNumber;
                            existingLunarPhase.PhaseType = phaseTypeType;
                            existingLunarPhase.DateTimeUtcUsno = dt;
                        }
                        await astroDbContext.SaveChangesAsync();
                    }
                }
            }
            else
            {
                Console.WriteLine(
                    $"Failed to retrieve data. Status code: {response.StatusCode}");
            }
        }
    }

    /// <summary>
    /// Calculate the Lunation Number from a DateTime.
    /// </summary>
    private static int GetLunationNumber(DateTime phaseDateTime)
    {
        TimeSpan timeSinceLunation0 = phaseDateTime - TimeConstants.LUNATION_0_START;

        // Round off to nearest phase.
        int phaseCount =
            (int)Round((double)timeSinceLunation0.Ticks / TimeConstants.TICKS_PER_LUNAR_PHASE);

        // Calculate the Lunation Number.
        return (int)Floor(phaseCount / 4.0);
    }

    /// <summary>
    /// Look up a lunar phase record in the database by DateTime within ±24 hours of a given
    /// DateTime.
    /// </summary>
    public LunarPhaseRecord? LookupRecord(DateTime dt)
    {
        return astroDbContext.LunarPhases
            .FirstOrDefault(lp =>
                (lp.DateTimeUtcAstroPixels != null
                    && Abs(EF.Functions.DateDiffHour(lp.DateTimeUtcAstroPixels, dt)!
                        .Value)
                    <= TimeConstants.HOURS_PER_DAY)
                || (lp.DateTimeUtcUsno != null
                    && Abs(EF.Functions.DateDiffHour(lp.DateTimeUtcUsno, dt)!.Value)
                    <= TimeConstants.HOURS_PER_DAY)
            );
    }

    public async Task CacheCalculations()
    {
        for (int year = 1; year <= 4000; year++)
        {
            List<LunarPhaseEvent> phaseEvents = moonService.GetPhasesInYear(year);
            foreach (LunarPhaseEvent phaseEvent in phaseEvents)
            {
                // Look for a matching record.
                LunarPhaseRecord? phaseRecord = astroDbContext.LunarPhases.FirstOrDefault(
                    phaseRecord =>
                        phaseRecord.LunationNumber == phaseEvent.LunationNumber
                        && phaseRecord.PhaseType == phaseEvent.PhaseType);

                if (phaseRecord == null)
                {
                    // Insert new record.
                    Slog.Information(
                        "Inserting new record: LN = {LN}, Phase = {Phase}, DateTime = {DateTime}.",
                        phaseEvent.LunationNumber, phaseEvent.PhaseType.ToString(),
                        phaseEvent.DateTimeUtc.ToIsoString());
                    phaseRecord = new LunarPhaseRecord
                    {
                        LunationNumber = phaseEvent.LunationNumber,
                        PhaseType = phaseEvent.PhaseType,
                        DateTimeUtcGalaxon = phaseEvent.DateTimeUtc,
                    };
                }
                else
                {
                    // Update existing record.
                    Slog.Information(
                        "Updating existing record: LN = {LN}, Phase = {Phase}, DateTime = {DateTime}.",
                        phaseEvent.LunationNumber, phaseEvent.PhaseType.ToString(),
                        phaseEvent.DateTimeUtc.ToIsoString());
                    phaseRecord.DateTimeUtcGalaxon = phaseEvent.DateTimeUtc;
                }

                // Update the database.
                await astroDbContext.SaveChangesAsync();
            }
        }
    }
}
