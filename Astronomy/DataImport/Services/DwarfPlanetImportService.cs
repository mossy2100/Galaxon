using Galaxon.Astronomy.Data;
using Galaxon.Astronomy.Data.Models;
using Galaxon.Astronomy.Data.Repositories;
using Galaxon.Astronomy.DataImport.Utilities;
using Galaxon.Core.Exceptions;
using Galaxon.Quantities.Kinds;
using HtmlAgilityPack;

namespace Galaxon.Astronomy.DataImport.Services;

public class DwarfPlanetImportService(
    AstroDbContext astroDbContext,
    AstroObjectGroupRepository astroObjectGroupRepository,
    AstroObjectRepository astroObjectRepository)
{
    /// <summary>
    /// Import dwarf planets from the Wikipedia page.
    /// </summary>
    public async Task Import()
    {
        // Get the Sun.
        AstroObjectRecord sun = astroObjectRepository.LoadByName("Sun", "Star");

        try
        {
            string url = "https://en.wikipedia.org/wiki/List_of_possible_dwarf_planets";
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
            if (rows != null)
            {
                foreach (HtmlNode? row in rows)
                {
                    HtmlNodeCollection? cells = row.SelectNodes(".//td | .//th");
                    if (cells != null)
                    {
                        if (cells.Count < 10)
                        {
                            continue;
                        }

                        // Get the dwarf planet's number and name.
                        string numberAndName = cells[0].InnerText.Trim();
                        (uint number, string name) = ParseDwarfPlanetName(numberAndName);

                        // Load the dwarf planet record from the database, if present.
                        Slog.Information("Possible dwarf planet: {Name}", name);

                        // Check if it's a recognised one by looking for the check mark in the cell.
                        bool isOfficial = cells[8]
                            .Descendants("img")
                            .Any(img =>
                                img
                                    .GetAttributeValue("alt", string.Empty)
                                    .Equals("Yes", StringComparison.OrdinalIgnoreCase));
                        if (isOfficial)
                        {
                            Slog.Information("Recognised by IAU.");
                        }
                        else
                        {
                            Slog.Information("Not recognised by the IAU, skipping.");
                            continue;
                        }

                        AstroObjectRecord? dwarfPlanet;
                        try
                        {
                            // Try to load it from the database.
                            dwarfPlanet = astroObjectRepository.LoadByName(name, "Dwarf planet");

                            Slog.Information(
                                "Dwarf planet record already exists in the database, updating.");
                        }
                        catch (DataNotFoundException)
                        {
                            // Create a new dwarf planet in the database.
                            dwarfPlanet = new AstroObjectRecord(name);
                            astroDbContext.AstroObjects.Add(dwarfPlanet);

                            Slog.Information(
                                "Dwarf planet record not found in the database, inserting.");
                        }

                        // Number.
                        dwarfPlanet.Number = number;

                        // Parent object.
                        dwarfPlanet.Parent = sun;

                        // Groups. (Just do the main one for now.)
                        astroObjectGroupRepository.AddToGroup(dwarfPlanet, "Dwarf planet");

                        Slog.Information("Number = {Number}, Parent = Sun, Group = 'Dwarf planet'", number);

                        // Save the dwarf planet object now to ensure it has an id before
                        // attaching composition objects to it.
                        await astroDbContext.SaveChangesAsync();

                        // Physical parameters.
                        dwarfPlanet.Physical ??= new PhysicalRecord();

                        // Mean radius.
                        double radius = ParseUtility.ParseDouble(cells[1].InnerText.Trim());
                        // Radius is provided in km, convert to m.
                        dwarfPlanet.Physical.MeanRadius = radius * 1000;

                        // Density.
                        double density = ParseUtility.ParseDouble(cells[2].InnerText.Trim());
                        // Radius is provided in g/cm3, convert to kg/m3.
                        dwarfPlanet.Physical.Density = Density.GramsPerCm3ToKgPerM3(density);

                        Slog.Information("Mean radius = {Radius} km, Density = {Density} g/cm3", radius, density);

                        // Save the physical parameters.
                        await astroDbContext.SaveChangesAsync();
                    }
                }
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

    private static (uint, string) ParseDwarfPlanetName(string name)
    {
        // Find the first space character
        int spaceIndex = name.IndexOf(' ');
        if (spaceIndex == -1)
        {
            throw new ArgumentException("Invalid dwarf planet name format.");
        }

        // Extract the number part, if possible.
        string idString = name[..spaceIndex].Trim('(', ')');
        uint id = uint.Parse(idString);

        // Extract the name part
        string planetName = name[(spaceIndex + 1)..];

        return (id, planetName);
    }
}
