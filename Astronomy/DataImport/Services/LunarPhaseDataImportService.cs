using System.Globalization;
using System.Text.RegularExpressions;
using Galaxon.Astronomy.Data;
using Galaxon.Astronomy.Data.Enums;
using Galaxon.Astronomy.Data.Models;
using Galaxon.Core.Strings;
using Galaxon.Time;
using HtmlAgilityPack;

namespace Galaxon.Astronomy.DataImport.Services;

public class LunarPhaseDataImportService(AstroDbContext astroDbContext)
{
    /// <summary>
    /// Get the links to the AstroPixels lunar phase data tables.
    /// <see href="http://astropixels.com/ephemeris/phasescat/phasescat.html"/>
    /// </summary>
    /// <returns></returns>
    /// <exception cref="InvalidOperationException"></exception>
    public async Task<List<string>> GetEphemerisPageUrls()
    {
        var result = new List<string>();
        string indexUrl = "http://astropixels.com/ephemeris/phasescat/phasescat.html";
        string tableTitle = "Moon Phases in Common Era (CE)";

        // Use HttpClient to fetch the content.
        using var httpClient = new HttpClient();
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
    public async Task<List<LunarPhase>> ImportPage(string url)
    {
        List<LunarPhase> lunarPhases = new ();
        JulianCalendar jc = new ();

        // Use HttpClient to fetch the content.
        using var httpClient = new HttpClient();
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
                for (int phaseType = 0; phaseType < 4; phaseType++)
                {
                    string dateStr = line.Substring(8 + phaseType * 18, 13);
                    MatchCollection dateTimeMatches = rxDateTime.Matches(dateStr);
                    if (dateTimeMatches.Count == 0)
                    {
                        continue;
                    }

                    // Extract the date parts.
                    int month = GregorianCalendarExtensions.MonthNameToNumber(
                        dateTimeMatches[0].Groups["month"].Value);
                    int day = int.Parse(dateTimeMatches[0].Groups["day"].Value);
                    int hour = int.Parse(dateTimeMatches[0].Groups["hour"].Value);
                    int minute = int.Parse(dateTimeMatches[0].Groups["minute"].Value);

                    // Construct the datetime.
                    DateTime phaseDateTime;
                    // Check for Julian date.
                    int dateNumber = year * 10000 + month * 100 + day;
                    if (dateNumber < 1582_10_15)
                    {
                        // Construct a DateTime (Gregorian) from the Julian date parts.
                        phaseDateTime = jc.ToDateTime(year, month, day, hour, minute, 0, 0);
                        DateTime.SpecifyKind(phaseDateTime, DateTimeKind.Utc);
                    }
                    else
                    {
                        // Handle error in the AstroPixels data.
                        if (year == 3869 && month == 1 && day == 1 && hour == 0 && minute == 0)
                        {
                            year = 3870;
                        }

                        // Construct the DateTime.
                        phaseDateTime = new DateTime(year, month, day, hour, minute, 0,
                            DateTimeKind.Utc);
                    }

                    // Construct the new LunarPhase and add it to the results.
                    LunarPhase lunarPhase = new LunarPhase
                    {
                        Type = (ELunarPhaseType)phaseType,
                        DateTimeUtcAstroPixels = phaseDateTime
                    };
                    lunarPhases.Add(lunarPhase);
                }
            }
        }

        return lunarPhases;
    }

    /// <summary>
    /// Extract lunar phase data from the AstroPixels web pages and copy them to the database.
    /// </summary>
    public async Task Import()
    {
        // Get the links to the catalog pages (CE only).
        List<string> urls = await GetEphemerisPageUrls();

        // One page at a time.
        foreach (string url in urls)
        {
            // Get all the lunar phases on this page.
            List<LunarPhase> lunarPhases = await ImportPage(url);

            // Loop through and add the new ones to the database.
            foreach (LunarPhase lunarPhase in lunarPhases)
            {
                // See if this lunar phase is already in the database. If not, add it.
                if (!astroDbContext.LunarPhases.Any(lp =>
                    lp.Type == lunarPhase.Type
                    && lp.DateTimeUtcAstroPixels == lunarPhase.DateTimeUtcAstroPixels))
                {
                    // Store phase information in the database.
                    astroDbContext.LunarPhases.Add(lunarPhase);
                }
            }

            // Update the database. Doing this once per page is easier to debug, because it only
            // generates a single insert statement with all the new lunar phase data in it.
            await astroDbContext.SaveChangesAsync();
        }
    }
}
