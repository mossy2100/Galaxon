using System.Text.RegularExpressions;
using Galaxon.Astronomy.Data;
using Galaxon.Astronomy.Data.Models;
using Galaxon.Time;
using HtmlAgilityPack;
using Serilog;

namespace DataImport.Services;

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
        Log.Information("Parsing NIST web page.");

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
                LeapSecond? leapSecond =
                    astroDbContext.LeapSeconds.FirstOrDefault(ls => ls.LeapSecondDate == d);
                if (leapSecond == null)
                {
                    // Add the new record.
                    leapSecond = new LeapSecond();
                    leapSecond.LeapSecondDate = d;
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
    public async Task ImportIersBulletins()
    {
        Log.Information("Importing IERS Bulletin Cs.");

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

            // Regular expression to match any of the "no leap second" phrases.
            Regex rxNoLeapSecond =
                new ("(NO|No) ((posi|nega)tive )?leap second will be introduced");
            string months = string.Join('|', GregorianCalendarExtensions.GetMonthNames().Values);
            Regex rxLeapSecond = new (
                $@"A (?<sign>positive|negative) leap second will be introduced at the end of (?<month>{months}) (?<year>\d{{4}}).");

            // Loop through individual bulletin URLs and process them
            foreach (string bulletinUrl in bulletinUrls)
            {
                Log.Information("Bulletin URL: {BulletinUrl}", bulletinUrl);

                // Get the bulletin number.
                int bulletinNumber;
                string pattern = @"bulletinc-(\d+)\.txt$";
                Match match = Regex.Match(bulletinUrl, pattern);
                if (match.Success)
                {
                    bulletinNumber = int.Parse(match.Groups[1].Value);
                    // Console.WriteLine($"Bulletin C number: {bulletinNumber}");
                    Log.Information("Bulletin number: {BulletinNumber}", bulletinNumber);
                }
                else
                {
                    string error = "PARSE ERROR: Could not get bulletin number.";
                    Log.Error("{Error}", error);
                    throw new InvalidOperationException(error);
                }

                // Ignore Bulletin C 10.
                if (bulletinNumber == 10)
                {
                    // Console.WriteLine($"Ignoring bulletin {bulletinNumber}.");
                    Log.Information("Ignoring bulletin {BulletinNumber}.", bulletinNumber);
                    continue;
                }

                // Get the existing record if there is one.
                IersBulletinC? iersBulletinC = astroDbContext.IersBulletinCs.FirstOrDefault(
                    ls => ls.BulletinNumber == bulletinNumber);
                if (iersBulletinC == null)
                {
                    // Console.WriteLine("Existing leap second record not found.");
                    Log.Information("Existing leap second record not found.");
                    iersBulletinC = new IersBulletinC();
                    astroDbContext.IersBulletinCs.Add(iersBulletinC);
                }
                else
                {
                    // Console.WriteLine("Existing leap second record found.");
                    Log.Information("Existing leap second record found.");
                    continue;
                }

                // Update leap second record.
                iersBulletinC.BulletinNumber = bulletinNumber;
                iersBulletinC.BulletinUrl = bulletinUrl;
                iersBulletinC.DateTimeParsed = DateTime.Now;

                // Parse content of bulletin.
                string bulletinText = await httpClient.GetStringAsync(bulletinUrl);
                if (rxNoLeapSecond.IsMatch(bulletinText))
                {
                    iersBulletinC.Value = 0;
                    iersBulletinC.LeapSecondDate = null;
                    // Console.WriteLine("No leap second.");
                    Log.Information("No leap second.");
                }
                else
                {
                    MatchCollection matches = rxLeapSecond.Matches(bulletinText);
                    if (matches.Count == 0)
                    {
                        string error = "PARSE ERROR: Could not detect leap second value.";
                        Log.Information("{Error}", error);
                        throw new InvalidOperationException(error);
                    }

                    GroupCollection groups = matches[0].Groups;
                    iersBulletinC.Value = (sbyte)(groups["sign"].Value == "positive" ? 1 : -1);
                    int month =
                        GregorianCalendarExtensions.MonthNameToNumber(groups["month"].Value);
                    int year = int.Parse(groups["year"].Value);
                    iersBulletinC.LeapSecondDate =
                        GregorianCalendarExtensions.GetMonthLastDay(year, month);

                    // Update or insert the leap second record.
                    LeapSecond? leapSecond = astroDbContext.LeapSeconds.FirstOrDefault(ls =>
                        ls.LeapSecondDate == iersBulletinC.LeapSecondDate);
                    if (leapSecond == null)
                    {
                        // Create new record.
                        leapSecond = new LeapSecond();
                        leapSecond.LeapSecondDate = iersBulletinC.LeapSecondDate.Value;
                        leapSecond.Value = iersBulletinC.Value;
                        astroDbContext.LeapSeconds.Add(leapSecond);
                    }
                }
                // Console.WriteLine($"Value = {iersBulletinC.Value}");
                Log.Information("Value = {Value}", iersBulletinC.Value);
                // Console.WriteLine($"Leap second date = {iersBulletinC.LeapSecondDate}");
                Log.Information("Leap second date = {Date}",
                    iersBulletinC.LeapSecondDate.ToString());

                // Update or insert the record.
                await astroDbContext.SaveChangesAsync();
            }
        }
        catch (Exception ex)
        {
            Log.Information("Error occurred: {Message}", ex.Message);
            // Console.WriteLine($"Error occurred: {ex.Message}");
        }
    }
}
