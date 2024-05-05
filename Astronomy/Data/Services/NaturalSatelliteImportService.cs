using Galaxon.Astronomy.Data.Models;
using Galaxon.Astronomy.Data.Repositories;
using Galaxon.Time;
using HtmlAgilityPack;

namespace Galaxon.Astronomy.Data.Services;

public class NaturalSatelliteImportService
{
    public async Task Import()
    {
        AstroDbContext astroDbContext = new ();
        AstroObjectGroupRepository astroObjectGroupRepository = new (astroDbContext);
        AstroObjectRepository astroObjectRepository =
            new (astroDbContext, astroObjectGroupRepository);

        try
        {
            string url = "https://en.wikipedia.org/wiki/List_of_natural_satellites";
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
                        Console.WriteLine();

                        HtmlNodeCollection? cells = row.SelectNodes(".//td");
                        if (cells != null)
                        {
                            // Get the satellite's name.
                            string satelliteName = cells[0].InnerText.Trim();
                            Console.WriteLine($"Satellite name: {satelliteName}");

                            // Handle issue with Namaka.
                            int offset = satelliteName == "Namaka" ? -1 : 0;

                            // Load the satellite record from the database, if present.
                            AstroObject? satellite =
                                astroObjectRepository.LoadByName(satelliteName, "Satellite");

                            // Create or update the satellite record as required.
                            if (satellite == null)
                            {
                                // Create a new satellite in the database.
                                Console.WriteLine($"Adding new satellite {satelliteName}.");
                                satellite = new AstroObject(satelliteName);
                                astroDbContext.AstroObjects.Add(satellite);
                            }
                            else
                            {
                                // Update satellite in the database.
                                Console.WriteLine($"Updating satellite {satelliteName}.");
                                astroDbContext.AstroObjects.Attach(satellite);
                            }

                            // Planet or dwarf planet the satellite orbits.
                            string parentName = cells[2 + offset].InnerText.Trim();
                            Console.WriteLine($"Parent name: {parentName}");
                            AstroObject? parent =
                                astroObjectRepository.LoadByName(parentName, "Planet");
                            if (parent == null)
                            {
                                // Try dwarf planet.
                                parent = astroObjectRepository.LoadByName(parentName,
                                    "Dwarf planet");
                                if (parent == null)
                                {
                                    Console.WriteLine(
                                        $"No parent object '{parentName}' found; skipping this item.");
                                    continue;
                                }
                            }
                            satellite.Parent = parent;

                            // Number.
                            uint? number =
                                ParseUtility.ExtractNumber(cells[3 + offset].InnerText.Trim());
                            Console.WriteLine($"Number: {number}");
                            satellite.Number = number;

                            // Sidereal period and if the satellite is retrograde or not.
                            string siderealPeriodText = cells[6 + offset].InnerText.Trim();
                            int rPos = siderealPeriodText.IndexOf("(r)",
                                StringComparison.InvariantCulture);
                            bool isRetrograde = false;
                            if (rPos != -1)
                            {
                                siderealPeriodText = siderealPeriodText[..rPos];
                                isRetrograde = true;
                            }
                            double siderealPeriod = ParseUtility.ParseDouble(siderealPeriodText);
                            Console.WriteLine($"Sidereal period: {siderealPeriod} days");

                            // Groups.
                            astroObjectGroupRepository.AddToGroup(satellite, "Satellite");
                            astroObjectGroupRepository.AddToGroup(satellite,
                                isRetrograde ? "Retrograde satellite" : "Prograde satellite");

                            // Save the satellite object now to ensure it has an Id before attaching
                            // composition objects to it.
                            await astroDbContext.SaveChangesAsync();

                            // Orbital parameters.
                            satellite.Orbit ??= new OrbitalRecord();

                            // Set the semi-major axis.
                            double semiMajorAxis =
                                ParseUtility.ParseDouble(cells[5 + offset].InnerText.Trim());
                            Console.WriteLine($"Semi-major axis: {semiMajorAxis} km");
                            // Semi-major axis is provided in km, convert to m.
                            satellite.Orbit.SemiMajorAxis = semiMajorAxis * 1000;

                            // Sidereal orbit period is provided in days, convert to seconds.
                            satellite.Orbit.SiderealOrbitPeriod =
                                siderealPeriod * TimeConstants.SECONDS_PER_DAY;

                            // Save the orbital parameters.
                            await astroDbContext.SaveChangesAsync();

                            // Physical parameters.
                            satellite.Physical ??= new PhysicalRecord();

                            // Set the radius.
                            double radius =
                                ParseUtility.ParseDouble(cells[4 + offset].InnerText.Trim());
                            Console.WriteLine($"Mean radius: {radius} km");
                            // Radius is provided in km, convert to m.
                            satellite.Physical.MeanRadius = radius * 1000;

                            // Save the physical parameters.
                            await astroDbContext.SaveChangesAsync();
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
}
