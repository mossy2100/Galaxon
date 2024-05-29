using Galaxon.Astronomy.Data;
using Galaxon.Astronomy.Data.Models;
using Galaxon.Astronomy.Data.Repositories;
using Galaxon.Astronomy.DataImport.Utilities;
using Galaxon.Core.Exceptions;
using Galaxon.Time;
using HtmlAgilityPack;

namespace Galaxon.Astronomy.DataImport.Services;

public class NaturalSatelliteImportService(
    AstroDbContext astroDbContext,
    AstroObjectRepository astroObjectRepository,
    AstroObjectGroupRepository astroObjectGroupRepository)
{
    public async Task Import()
    {
        try
        {
            string url = "https://en.wikipedia.org/wiki/List_of_natural_satellites";
            string tableClass = "sortable";

            string htmlContent = await GetHtmlContentAsync(url);
            HtmlDocument doc = new ();
            doc.LoadHtml(htmlContent);

            HtmlNode? table =
                doc.DocumentNode.SelectSingleNode($"//table[contains(@class,'{tableClass}')]");
            if (table == null)
            {
                throw new InvalidOperationException("Table with class 'sortable' not found.");
            }

            HtmlNodeCollection? rows = table.SelectNodes(".//tbody/tr");
            if (rows == null)
            {
                throw new InvalidOperationException("Table doesn't have any rows.");
            }

            // Loop through the rows, processing the satellite information.
            foreach (HtmlNode? row in rows)
            {
                HtmlNodeCollection? cells = row.SelectNodes(".//td");
                if (cells == null)
                {
                    continue;
                }

                // Get the satellite's name.
                string satelliteName = cells[0].InnerText.Trim();
                Slog.Information("Satellite name: {Name}", satelliteName);

                // Handle issue with Namaka.
                int offset = cells.Count - 12;

                // Load the satellite record from the database, if present.
                AstroObjectRecord record;
                bool insert = false;
                bool update = false;
                try
                {
                    // Update satellite in the database.
                    record = astroObjectRepository.LoadByName(satelliteName, "Satellite");
                    insert = true;

                    Slog.Information("Satellite already exists in the database.");
                }
                catch (DataNotFoundException)
                {
                    // Create a new satellite in the database.
                    record = new AstroObjectRecord(satelliteName);
                    update = true;

                    Slog.Information("Satellite not found in the database.");
                }

                // Planet or dwarf planet the satellite orbits.
                string parentName = cells[2 + offset].InnerText.Trim();
                Slog.Information("Parent name: {ParentName}", parentName);
                AstroObjectRecord? parent = null;
                try
                {
                    parent = astroObjectRepository.LoadByName(parentName, "Planet");
                }
                catch (DataNotFoundException)
                {
                    // Try dwarf planet.
                    try
                    {
                        parent = astroObjectRepository.LoadByName(parentName, "Dwarf planet");
                    }
                    catch (DataNotFoundException)
                    {
                        Slog.Warning(
                            "Parent object '{ParentName}' not found; skipping this satellite.",
                            parentName);
                        update = false;
                        insert = false;
                    }
                }

                if (insert)
                {
                    Slog.Information("Inserting record.");
                }
                else if (update)
                {
                    astroDbContext.AstroObjects.Add(record);
                    Slog.Information("Updating record.");
                }
                else
                {
                    continue;
                }

                // Parent.
                record.Parent = parent;

                // Number.
                uint? number = ParseUtility.ExtractNumber(cells[3 + offset].InnerText.Trim());
                Slog.Information("Number = {Number}", number);
                record.Number = number;

                // Sidereal period and if the satellite is retrograde or not.
                string siderealPeriodText = cells[6 + offset].InnerText.Trim();
                int rPos = siderealPeriodText.IndexOf("(r)", StringComparison.InvariantCulture);
                bool isRetrograde = false;
                if (rPos != -1)
                {
                    siderealPeriodText = siderealPeriodText[..rPos];
                    isRetrograde = true;
                }
                double siderealPeriod_d = ParseUtility.ParseDouble(siderealPeriodText);
                Slog.Information("Sidereal period: {SiderealPeriod} days", siderealPeriod_d);

                // Groups.
                astroObjectGroupRepository.AddToGroup(record, "Satellite");
                astroObjectGroupRepository.AddToGroup(record,
                    isRetrograde ? "Retrograde satellite" : "Prograde satellite");

                // Save the satellite object now to ensure it has an id before attaching
                // composition objects to it.
                await astroDbContext.SaveChangesAsync();

                // ---------------------------------------------------------------------------------
                // Orbit data.
                record.Orbit ??= new OrbitRecord();

                // Semi-major axis.
                double semiMajorAxis_km =
                    ParseUtility.ParseDouble(cells[5 + offset].InnerText.Trim());
                Slog.Information("Semi-major axis: {SemiMajorAxis} km", semiMajorAxis_km);
                record.Orbit.SemiMajorAxis_km = semiMajorAxis_km;

                // Sidereal orbit period.
                record.Orbit.SiderealOrbitPeriod_d = siderealPeriod_d;

                // Save the orbit data.
                await astroDbContext.SaveChangesAsync();

                // ---------------------------------------------------------------------------------
                // Physical data.
                record.Physical ??= new PhysicalRecord();

                // Mean radius.
                double radius_km = ParseUtility.ParseDouble(cells[4 + offset].InnerText.Trim());
                Slog.Information("Mean radius: {Radius} km", radius_km);
                record.Physical.MeanRadius_km = radius_km;

                // Save the physical parameters.
                await astroDbContext.SaveChangesAsync();
            }
        }
        catch (Exception ex)
        {
            Slog.Error("Error: {Message}", ex.Message);
        }
    }

    private static async Task<string> GetHtmlContentAsync(string url)
    {
        using HttpClient client = new ();
        HttpResponseMessage response = await client.GetAsync(url);
        if (response.IsSuccessStatusCode)
        {
            return await response.Content.ReadAsStringAsync();
        }

        throw new Exception($"Failed to retrieve data. Status code: {response.StatusCode}");
    }
}
