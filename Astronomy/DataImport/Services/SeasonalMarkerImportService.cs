using Galaxon.Astronomy.Data;
using Galaxon.Astronomy.Data.Enums;
using Galaxon.Astronomy.Data.Models;
using Galaxon.Astronomy.DataImport.DataTransferObjects;
using Galaxon.Core.Collections;
using Galaxon.Time;
using Microsoft.OpenApi.Extensions;
using Newtonsoft.Json;

namespace Galaxon.Astronomy.DataImport.Services;

public class SeasonalMarkerImportService(AstroDbContext astroDbContext)
{
    public async Task ImportFromUsno()
    {
        for (int year = 1700; year <= 2100; year++)
        {
            Console.WriteLine();

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
                    Console.WriteLine(
                        $"Failed to retrieve data. Status code: {response.StatusCode}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
        }
    }

    private async Task ImportFromUsnoJson(HttpResponseMessage response)
    {
        string jsonContent = await response.Content.ReadAsStringAsync();

        // Parse the JSON data to extract the seasonal marker data
        UsnoSeasonalMarkersForYear? usms =
            JsonConvert.DeserializeObject<UsnoSeasonalMarkersForYear>(jsonContent);
        if (usms?.data == null || usms.data.IsEmpty())
        {
            Console.WriteLine(
                $"Failed to retrieve data. Status code: {response.StatusCode}");
        }
        else
        {
            foreach (UsnoSeasonalMarker usm in usms.data)
            {
                ESeasonalMarkerType? seasonalMarkerType;

                // Get the seasonal marker type.
                seasonalMarkerType = usm switch
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

                // Provide feedback.
                string seasonalMarkerTypeName = seasonalMarkerType.GetDisplayName();
                Console.WriteLine($"{seasonalMarkerTypeName,60}: {dt.ToIsoString()}");

                // See if we need to update or insert.
                SeasonalMarkerRecord? existingSeasonalMarker =
                    astroDbContext.SeasonalMarkers.FirstOrDefault(sm =>
                        sm.Type == seasonalMarkerType
                        && sm.DateTimeUtcUsno != null
                        && sm.DateTimeUtcUsno.Value.Year == usm.year);
                if (existingSeasonalMarker == null)
                {
                    // Insert a new record.
                    SeasonalMarkerRecord newSeasonalMarkerRecord = new ()
                    {
                        Type = seasonalMarkerType.Value,
                        DateTimeUtcUsno = dt
                    };
                    astroDbContext.SeasonalMarkers.Add(newSeasonalMarkerRecord);
                    await astroDbContext.SaveChangesAsync();
                }
                else if (existingSeasonalMarker.DateTimeUtcUsno != dt)
                {
                    // Update the existing record.
                    existingSeasonalMarker.DateTimeUtcUsno = dt;
                    await astroDbContext.SaveChangesAsync();
                }
            }
        }
    }
}
