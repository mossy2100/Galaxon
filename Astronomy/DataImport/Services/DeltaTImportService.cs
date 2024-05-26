using Galaxon.Astronomy.Algorithms.Utilities;
using Galaxon.Astronomy.Data;
using Galaxon.Astronomy.Data.Models;
using HtmlAgilityPack;

namespace Galaxon.Astronomy.DataImport.Services;

public class DeltaTImportService(HttpClient httpClient, AstroDbContext astroDbContext)
{
    /// <summary>
    /// Import delta-T data from the internet.
    /// </summary>
    public async Task Import()
    {
        await ImportFromNasaDeltaTHistoric();
        await ImportFromUsnoDeltaTHistoric();
        await ImportFromUsnoDeltaTMonthly();
        await ImportFromUsnoDeltaTPredicted();
    }

    /// <summary>
    /// Import monthly delta-T data.
    /// <see href="https://maia.usno.navy.mil/products/deltaT"/>
    /// </summary>
    public async Task ImportFromUsnoDeltaTMonthly()
    {
        const string URL = "https://maia.usno.navy.mil/ser7/deltat.data";

        try
        {
            // Download data file from USNO.
            string dataContent = await httpClient.GetStringAsync(URL);

            using StringReader reader = new (dataContent);

            while (await reader.ReadLineAsync() is { } line)
            {
                string[] parts = line.Split(new[] { ' ', '\t' },
                    StringSplitOptions.RemoveEmptyEntries);

                if (parts.Length != 4)
                {
                    continue;
                }

                // Parse the date and delta-T
                if (int.TryParse(parts[0], out int year)
                    && int.TryParse(parts[1], out int month)
                    && int.TryParse(parts[2], out int day)
                    && decimal.TryParse(parts[3], out decimal deltaT))
                {
                    // Convert date parts to decimal year.
                    DateTime date = new (year, month, day, 0, 0, 0, DateTimeKind.Utc);
                    decimal decimalYear =
                        decimal.Round((decimal)TimeScalesUtility.DateTimeToDecimalYear(date), 2);

                    // Log it.
                    Slog.Information(
                        "Parsed delta-T record from monthly data: Date = {Date}, DecimalYear = {DecimalYear}, DeltaT = {DeltaT}",
                        date, decimalYear, deltaT);

                    // Insert or update record as needed.
                    await UpsertRecord(decimalYear, deltaT);
                }
            }
        }
        catch (Exception ex)
        {
            // Log the exception.
            Slog.Error(
                "An error occurred while importing monthly delta-T data from USNO: {Message}",
                ex.Message);
            if (ex.InnerException != null)
            {
                Slog.Error("Inner exception: {Message}", ex.InnerException.Message);
            }
            throw;
        }
    }

    /// <summary>
    /// Import historic delta-T data.
    /// <see href="https://maia.usno.navy.mil/products/deltaT"/>
    /// </summary>
    public async Task ImportFromUsnoDeltaTHistoric()
    {
        const string URL = "https://maia.usno.navy.mil/ser7/historic_deltat.data";

        try
        {
            // Download data file from USNO.
            string dataContent = await httpClient.GetStringAsync(URL);

            using StringReader reader = new (dataContent);

            while (await reader.ReadLineAsync() is { } line)
            {
                string[] parts = line.Split(new[] { ' ', '\t' },
                    StringSplitOptions.RemoveEmptyEntries);

                if (parts.Length != 5)
                {
                    continue;
                }

                // Parse the date and delta-T
                if (decimal.TryParse(parts[0], out decimal decimalYear)
                    && decimal.TryParse(parts[1], out decimal deltaT))
                {
                    // Log it.
                    Slog.Information(
                        "Parsed delta-T record from historic data: DecimalYear = {DecimalYear}, DeltaT = {DeltaT}",
                        decimalYear, deltaT);

                    // Insert or update record as needed.
                    await UpsertRecord(decimalYear, deltaT);
                }
            }
        }
        catch (Exception ex)
        {
            // Log the exception.
            Slog.Error(
                "An error occurred while importing historic delta-T data from USNO: {Message}",
                ex.Message);
            if (ex.InnerException != null)
            {
                Slog.Error("Inner exception: {Message}", ex.InnerException.Message);
            }
            throw;
        }
    }

    private async Task UpsertRecord(decimal decimalYear, decimal deltaT)
    {
        // Look for a matching delta-T record.
        DeltaTRecord? record =
            astroDbContext.DeltaTRecords.FirstOrDefault(dt => dt.DecimalYear == decimalYear);

        // Check if this record already exists
        if (record == null)
        {
            // Insert a new record.
            record = new DeltaTRecord
            {
                DecimalYear = decimalYear,
                DeltaT = deltaT
            };

            // Add this record to the database
            astroDbContext.DeltaTRecords.Add(record);
            await astroDbContext.SaveChangesAsync();

            // Log it.
            Slog.Information("Inserted new delta-T record.");
        }
        else if (record.DeltaT != deltaT)
        {
            // Update existing record.
            record.DeltaT = deltaT;
            await astroDbContext.SaveChangesAsync();

            // Log it.
            Slog.Information("Updated existing delta-T record.");
        }
        else
        {
            // Log it.
            Slog.Information("delta-T record already up-to-date.");
        }
    }

    /// <summary>
    /// Import predicted delta-T data.
    /// <see href="https://maia.usno.navy.mil/products/deltaT"/>
    /// </summary>
    public async Task ImportFromUsnoDeltaTPredicted()
    {
        const string URL = "https://maia.usno.navy.mil/ser7/deltat.preds";

        try
        {
            // Download data file from USNO.
            string dataContent = await httpClient.GetStringAsync(URL);

            using StringReader reader = new (dataContent);

            while (await reader.ReadLineAsync() is { } line)
            {
                string[] parts = line.Split(new[] { ' ', '\t' },
                    StringSplitOptions.RemoveEmptyEntries);

                // Parse the date and delta-T.
                if (decimal.TryParse(parts[1], out decimal decimalYear)
                    && decimal.TryParse(parts[2], out decimal deltaT))
                {
                    // Log it.
                    Slog.Information(
                        "Parsed delta-T record from predicted data: DecimalYear = {DecimalYear}, DeltaT = {DeltaT}",
                        decimalYear, deltaT);

                    // Insert or update record as needed.
                    await UpsertRecord(decimalYear, deltaT);
                }
            }
        }
        catch (Exception ex)
        {
            // Log the exception.
            Slog.Error(
                "An error occurred while importing predicted delta-T data from USNO: {Message}",
                ex.Message);
            if (ex.InnerException != null)
            {
                Slog.Error("Inner exception: {Message}", ex.InnerException.Message);
            }
            throw;
        }
    }

    /// <summary>
    /// Import historic delta-T data from NASA.
    /// <see href="https://eclipse.gsfc.nasa.gov/LEcat5/deltat.html"/>
    /// </summary>
    public async Task ImportFromNasaDeltaTHistoric()
    {
        string URL = "https://eclipse.gsfc.nasa.gov/LEcat5/deltat.html";
        try
        {
            // Get the HTML from the NASA page.
            var response = await httpClient.GetAsync(URL);
            response.EnsureSuccessStatusCode();
            var htmlContent = await response.Content.ReadAsStringAsync();

            // Load the HTML into an HtmlDocument.
            HtmlDocument htmlDoc = new ();
            htmlDoc.LoadHtml(htmlContent);

            // Find the first data table with class "datatab".
            HtmlNode? dataTable =
                htmlDoc.DocumentNode.SelectSingleNode("//table[@class='datatab']");

            // Iterate over the rows in the tbody of the table.
            foreach (HtmlNode? row in dataTable.SelectNodes("tbody/tr"))
            {
                HtmlNodeCollection? cells = row.SelectNodes("td");
                if (cells != null && cells.Count >= 2)
                {
                    // Parse the decimal year and delta-T from the first two td elements
                    if (decimal.TryParse(cells[0].InnerText.Trim(), out decimal decimalYear)
                        && decimal.TryParse(cells[1].InnerText.Trim(), out decimal deltaT))
                    {
                        // Check if the year is less than 1657
                        if (decimalYear < 1657)
                        {
                            // Log it.
                            Slog.Information(
                                "Parsed delta-T record from NASA historic data: DecimalYear = {DecimalYear}, DeltaT = {DeltaT}",
                                decimalYear, deltaT);

                            // Insert or update record as needed.
                            await UpsertRecord(decimalYear, deltaT);
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            // Log the exception.
            Slog.Error(
                "An error occurred while importing historic delta-T data from NASA: {Message}",
                ex.Message);
            if (ex.InnerException != null)
            {
                Slog.Error("Inner exception: {Message}", ex.InnerException.Message);
            }
            throw;
        }
    }
}
