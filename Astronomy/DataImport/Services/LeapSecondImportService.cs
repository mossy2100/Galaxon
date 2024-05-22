using System.Text.RegularExpressions;
using Galaxon.Astronomy.Data;
using Galaxon.Astronomy.Data.Models;
using Galaxon.Time;
using Galaxon.Time.Extensions;
using HtmlAgilityPack;

namespace Galaxon.Astronomy.DataImport.Services;

public class LeapSecondImportService(AstroDbContext astroDbContext)
{
    /// <summary>
    /// NIST web page showing a table of leap seconds.
    /// An alternate source could be <see href="https://en.wikipedia.org/wiki/Leap_second"/>, but I
    /// expect NIST is more likely to be correct and maintained up to date (but I could be wrong).
    /// As yet I haven't found a more authoritative source of leap seconds online. The IERS
    /// bulletins don't cover the entire period from 1972.
    /// </summary>
    private const string _NIST_LEAP_SECONDS_URL =
        "https://www.nist.gov/pml/time-and-frequency-division/time-realization/leap-seconds";

    /// <summary>
    /// URL of the IERS Bulletin C index.
    /// </summary>
    private const string _BULLETIN_INDEX_URL =
        "https://datacenter.iers.org/products/eop/bulletinc/";

    /// <summary>
    /// Download the leap seconds from the NIST website.
    /// </summary>
    public async Task ImportNistWebPage()
    {
        Slog.Information("Parsing NIST web page.");

        using HttpClient httpClient = new ();

        // Load the HTML table as a string.
        // Fetch HTML content from the bulletins URL
        string htmlContent = await httpClient.GetStringAsync(_NIST_LEAP_SECONDS_URL);

        // Parse HTML content using HtmlAgilityPack.
        HtmlDocument htmlDocument = new ();
        htmlDocument.LoadHtml(htmlContent);

        // Get the tables.
        HtmlNodeCollection? tables = htmlDocument.DocumentNode.SelectNodes("//table");
        if (tables == null || tables.Count == 0)
        {
            throw new InvalidDataException("No tables found.");
        }

        // Get the table cells.
        HtmlNode? leapSecondTable = tables[0];
        HtmlNodeCollection? cells = leapSecondTable.SelectNodes(".//td");
        if (cells == null || cells.Count == 0)
        {
            throw new InvalidDataException("No table data found.");
        }

        // Loop through the cells looking for dates.
        List<DateOnly> dates = new ();
        Regex rxDate = new (@"^(?<year>\d{4})-(?<month>\d{2})-(?<day>\d{2})$");
        foreach (HtmlNode? cell in cells)
        {
            MatchCollection matches = rxDate.Matches(cell.InnerText.Trim());
            if (matches.Count == 1)
            {
                int year = int.Parse(matches[0].Groups["year"].Value);
                int month = int.Parse(matches[0].Groups["month"].Value);
                int day = int.Parse(matches[0].Groups["day"].Value);
                DateOnly date = new (year, month, day);
                dates.Add(date);
            }
        }

        // Sort, then add any new ones to database.
        if (dates.Count > 0)
        {
            foreach (DateOnly d in dates.OrderBy(d => d.GetTotalDays()))
            {
                LeapSecondRecord? leapSecond =
                    astroDbContext.LeapSeconds.FirstOrDefault(ls => ls.Date == d);
                if (leapSecond == null)
                {
                    // Add the new record.
                    leapSecond = new LeapSecondRecord();
                    leapSecond.Date = d;
                    // All the leap seconds in that table are positive.
                    leapSecond.Value = 1;
                    astroDbContext.LeapSeconds.Add(leapSecond);
                    await astroDbContext.SaveChangesAsync();
                }
            }
        }
    }

    /// <summary>
    /// Every 6 months, scrape the IERS bulletins at
    /// <see href="https://datacenter.iers.org/products/eop/bulletinc/"/>
    /// and parse them to get the latest leap second dates.
    /// Compare data with table here:
    /// <see href="https://www.nist.gov/pml/time-and-frequency-division/time-realization/leap-seconds"/>
    /// </summary>
    /// <remarks>
    /// TODO Set up a cron job to check the IERS website periodically.
    /// </remarks>
    public async Task ImportIersBulletins(bool updateExisting = false)
    {
        Slog.Information("Importing IERS Bulletin Cs.");

        using HttpClient httpClient = new ();

        try
        {
            // Fetch HTML content from the bulletins URL
            string htmlContent = await httpClient.GetStringAsync(_BULLETIN_INDEX_URL);

            // Parse HTML content using HtmlAgilityPack.
            HtmlDocument htmlDocument = new ();
            htmlDocument.LoadHtml(htmlContent);

            // Extract individual bulletin URLs
            List<string> bulletinUrls = new ();
            HtmlNodeCollection? bulletinNodes =
                htmlDocument.DocumentNode.SelectNodes("//a[contains(@href, 'bulletinc-')]");
            if (bulletinNodes != null)
            {
                foreach (HtmlNode? node in bulletinNodes)
                {
                    string href = node.GetAttributeValue("href", "");
                    string bulletinUrl = new Uri(new Uri(_BULLETIN_INDEX_URL), href).AbsoluteUri;
                    bulletinUrls.Add(bulletinUrl);
                }
            }

            // Prepare regular expressions.
            // Get the month names.
            string enMonths = string.Join('|', GregorianCalendarUtility.GetMonthNames().Values);
            string frMonths = string.Join('|', GregorianCalendarUtility.GetMonthNames("fr").Values);
            string rxsMonths = $"{enMonths}|{frMonths}";
            // Regular expression to match the bulletin URL.
            string rxsBulletinUrl = @"bulletinc-(\d+)\.txt$";
            // Regular expression to match the publish city and date.
            string rxsDatePublished = $@"Paris,\s(?<day>\d{{1,2}})\s(?<month>{rxsMonths})\s(?<year>\d{{4}})";
            // Regular expression to match any of the "no leap second" phrases.
            string rxsNoLeapSecond = "(NO|No) ((posi|nega)tive )?leap second will be introduced";
            // Regular expression to match the leap second phrases.
            string rxsLeapSecond =
                $@"A (?<sign>positive|negative) leap second will be introduced at the end of (?<month>{enMonths}) (?<year>\d{{4}}).";

            // Loop through individual bulletin URLs and process them
            foreach (string bulletinUrl in bulletinUrls)
            {
                Slog.Information("Bulletin URL: {BulletinUrl}", bulletinUrl);

                // Get the bulletin number.
                Match matchBulletinUrl = Regex.Match(bulletinUrl, rxsBulletinUrl);
                if (!matchBulletinUrl.Success)
                {
                    throw new InvalidOperationException("Could not get bulletin number.");
                }
                int bulletinNumber = int.Parse(matchBulletinUrl.Groups[1].Value);
                Slog.Information("Bulletin number: {BulletinNumber}", bulletinNumber);

                // Ignore Bulletin C 10.
                if (bulletinNumber == 10)
                {
                    Slog.Information("Ignoring bulletin {BulletinNumber}.", bulletinNumber);
                    continue;
                }

                // Get the existing record if there is one.
                IersBulletinCRecord? iersBulletinC = astroDbContext.IersBulletinCs.FirstOrDefault(
                    ls => ls.BulletinNumber == bulletinNumber);

                if (iersBulletinC == null)
                {
                    Slog.Information("Existing leap second record not found, creating a new one.");
                    iersBulletinC = new IersBulletinCRecord
                    {
                        BulletinNumber = bulletinNumber
                    };
                    astroDbContext.IersBulletinCs.Add(iersBulletinC);
                }
                else
                {
                    if (!updateExisting)
                    {
                        Slog.Information("Existing leap second record found, not updating.");
                        continue;
                    }
                    else
                    {
                        Slog.Information("Existing leap second record found, updating.");
                    }
                }

                // Update leap second record.
                iersBulletinC.BulletinUrl = bulletinUrl;
                iersBulletinC.DateTimeParsed = DateTime.Now;

                // Read content of bulletin.
                string bulletinText = await httpClient.GetStringAsync(bulletinUrl);

                // Get the publish date.
                Match matchDatePublished =
                    Regex.Match(bulletinText, rxsDatePublished, RegexOptions.IgnoreCase);
                if (!matchDatePublished.Success)
                {
                    throw new InvalidOperationException("Could not read publish date.");
                }
                int publishDay = int.Parse(matchDatePublished.Groups["day"].Value);
                string monthName = matchDatePublished.Groups["month"].Value;
                int publishMonth;
                try
                {
                    // Try to match the English month name.
                    publishMonth = GregorianCalendarUtility.MonthNameToNumber(monthName);
                }
                catch
                {
                    try
                    {
                        // Try French.
                        publishMonth = GregorianCalendarUtility.MonthNameToNumber(monthName, "fr");
                    }
                    catch
                    {
                        throw new InvalidOperationException("Unknown month name.");
                    }
                }
                int publishYear = int.Parse(matchDatePublished.Groups["year"].Value);
                DateOnly datePublished = new (publishYear, publishMonth, publishDay);
                iersBulletinC.DatePublished = datePublished;
                Slog.Information("Bulletin publish date = {PublishDate}",
                    datePublished.ToIsoString());

                // See if there's a leap second or not.
                if (Regex.IsMatch(bulletinText, rxsNoLeapSecond))
                {
                    // No leap second.
                    iersBulletinC.Value = 0;
                    iersBulletinC.LeapSecondDate = null;
                    Slog.Information("No leap second.");
                }
                else
                {
                    // Get the leap second date and value.
                    MatchCollection matches = Regex.Matches(bulletinText, rxsLeapSecond);
                    if (matches.Count == 0)
                    {
                        throw new InvalidOperationException("Could not detect leap second value.");
                    }

                    GroupCollection groups = matches[0].Groups;
                    iersBulletinC.Value = (sbyte)(groups["sign"].Value == "positive" ? 1 : -1);
                    int month = GregorianCalendarUtility.MonthNameToNumber(groups["month"].Value);
                    int year = int.Parse(groups["year"].Value);
                    iersBulletinC.LeapSecondDate = GregorianCalendarUtility.GetMonthLastDay(year, month);

                    Slog.Information("Leap second value = {Value}", iersBulletinC.Value);
                    Slog.Information("Leap second date = {Date}",
                        iersBulletinC.LeapSecondDate.Value.ToIsoString());

                    // Update or insert the leap second record.
                    LeapSecondRecord? leapSecond = astroDbContext.LeapSeconds.FirstOrDefault(ls =>
                        ls.Date == iersBulletinC.LeapSecondDate);
                    if (leapSecond == null)
                    {
                        // Create new record.
                        leapSecond = new LeapSecondRecord();
                        leapSecond.Date = iersBulletinC.LeapSecondDate.Value;
                        leapSecond.Value = iersBulletinC.Value;
                        astroDbContext.LeapSeconds.Add(leapSecond);
                    }
                }

                // Update the database.
                await astroDbContext.SaveChangesAsync();
            }
        }
        catch (Exception ex)
        {
            Slog.Error("{Message}", ex.Message);
        }
    }
}
