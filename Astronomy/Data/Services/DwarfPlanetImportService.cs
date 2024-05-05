using Galaxon.Astronomy.Data.Models;
using Galaxon.Astronomy.Data.Repositories;
using Galaxon.Quantities.Kinds;
using HtmlAgilityPack;

namespace Galaxon.Astronomy.Data.Services;

public class DwarfPlanetImportService(
    AstroDbContext astroDbContext,
    AstroObjectGroupRepository astroObjectGroupRepository,
    AstroObjectRepository astroObjectRepository)
{
    public async Task Import()
    {
        // Get the Sun.
        AstroObject? sun = astroObjectRepository.LoadByName("Sun", "Star");
        if (sun == null)
        {
            throw new Exception("Could not load Sun from database.");
        }

        try
        {
            string url = "https://en.wikipedia.org/wiki/List_of_possible_dwarf_planets";
            string tableClass = "sortable";

            string htmlContent = await GetHtmlContentAsync(url);
            HtmlDocument doc = new ();
            doc.LoadHtml(htmlContent);

            HtmlNode? table =
                doc.DocumentNode.SelectSingleNode($"//table[contains(@class,'{tableClass}')]");
            if (table != null)
            {
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

                            // Check if an official dwarf planet according to the IAU.
                            string[] validDwarfPlants =
                                ["Ceres", "Pluto", "Eris", "Haumea", "Makemake", "Quaoar"];
                            if (!validDwarfPlants.Contains(name))
                            {
                                continue;
                            }

                            // Load the dwarf planet record from the database, if present.
                            Console.WriteLine($"Dwarf planet: {name}");
                            AstroObject? dwarfPlanet =
                                astroObjectRepository.LoadByName(name, "Dwarf planet");

                            // Create or update the dwarf planet record as required.
                            if (dwarfPlanet == null)
                            {
                                // Create a new dwarf planet in the database.
                                Console.WriteLine($"Adding new dwarf planet {name}.");
                                dwarfPlanet = new AstroObject(name);
                                astroDbContext.AstroObjects.Add(dwarfPlanet);
                            }
                            else
                            {
                                // Update dwarf planet in the database.
                                Console.WriteLine($"Updating dwarf planet {name}.");
                                astroDbContext.AstroObjects.Attach(dwarfPlanet);
                            }

                            // Parent object.
                            dwarfPlanet.Parent = sun;

                            // Number.
                            Console.WriteLine($"Dwarf planet number: {number}");
                            dwarfPlanet.Number = number;

                            // Groups. (Just do the main one for now.)
                            astroObjectGroupRepository.AddToGroup(dwarfPlanet, "Dwarf planet");

                            // Save the dwarf planet object now to ensure it has an Id before
                            // attaching composition objects to it.
                            await astroDbContext.SaveChangesAsync();

                            // Physical parameters.
                            dwarfPlanet.Physical ??= new PhysicalRecord();

                            // Mean radius.
                            double radius = ParseUtility.ParseDouble(cells[1].InnerText.Trim());
                            Console.WriteLine($"Mean radius: {radius} km");
                            // Radius is provided in km, convert to m.
                            dwarfPlanet.Physical.MeanRadius = radius * 1000;

                            // Density.
                            double density = ParseUtility.ParseDouble(cells[2].InnerText.Trim());
                            Console.WriteLine($"Density: {density} g/cm3");
                            // Radius is provided in g/cm3, convert to kg/m3.
                            dwarfPlanet.Physical.Density = Density.GramsPerCm3ToKgPerM3(density);

                            // Albedo.
                            // Maybe do later.

                            // Save the physical parameters.
                            await astroDbContext.SaveChangesAsync();

                            Console.WriteLine();
                        }
                    }
                }
            }
            else
            {
                Console.WriteLine("Table with class 'sortable' not found.");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred: {ex.Message}");
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
        string idString = name.Substring(0, spaceIndex).Trim('(', ')');
        uint id = uint.Parse(idString);

        // Extract the name part
        string planetName = name.Substring(spaceIndex + 1);

        return (id, planetName);
    }
}
