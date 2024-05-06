using Galaxon.Astronomy.Data.DataTransferObjects;
using Galaxon.Astronomy.Data.Enums;
using Galaxon.Astronomy.Data.Models;
using Galaxon.Astronomy.Data.Repositories;
using Galaxon.Core.Collections;
using Galaxon.Time;
using Microsoft.OpenApi.Extensions;
using Newtonsoft.Json;

namespace Galaxon.Astronomy.Data.Services;

public class SeasonalMarkerImportService(
    AstroDbContext astroDbContext,
    AstroObjectRepository astroObjectRepository)
{
    public async Task Import()
    {
        AstroObject earth = astroObjectRepository.LoadByName("Earth", "Planet");

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
                            bool isSeasonalMarker;
                            ESeasonalMarker seasonalMarker = default;
                            EApside apside = default;

                            // Get the seasonal marker or apside type.
                            if (usm.month == 3 && usm.phenom == "Equinox")
                            {
                                isSeasonalMarker = true;
                                seasonalMarker = ESeasonalMarker.NorthwardEquinox;
                            }
                            else if (usm.month == 6 && usm.phenom == "Solstice")
                            {
                                isSeasonalMarker = true;
                                seasonalMarker = ESeasonalMarker.NorthernSolstice;
                            }
                            else if (usm.month == 9 && usm.phenom == "Equinox")
                            {
                                isSeasonalMarker = true;
                                seasonalMarker = ESeasonalMarker.SouthwardEquinox;
                            }
                            else if (usm.month == 12 && usm.phenom == "Solstice")
                            {
                                isSeasonalMarker = true;
                                seasonalMarker = ESeasonalMarker.SouthernSolstice;
                            }
                            else if (usm.phenom == "Perihelion")
                            {
                                isSeasonalMarker = false;
                                apside = EApside.Periapsis;
                            }
                            else if (usm.phenom == "Aphelion")
                            {
                                isSeasonalMarker = false;
                                apside = EApside.Apoapsis;
                            }
                            else
                            {
                                Console.WriteLine("Error: Invalid data.");
                                continue;
                            }

                            // Construct the datetime.
                            DateOnly date = new (year, usm.month, usm.day);
                            TimeOnly time = TimeOnly.Parse(usm.time);
                            DateTime dt = new (date, time, DateTimeKind.Utc);

                            if (isSeasonalMarker)
                            {
                                // Construct the SeasonalMarker record.
                                SeasonalMarkerRecord newSeasonalMarkerRecord =
                                    new ()
                                    {
                                        MarkerNumber = (byte)seasonalMarker,
                                        DateTimeUtcUsno = dt
                                    };
                                string seasonalMarkerTypeName = seasonalMarker.GetDisplayName();
                                Console.WriteLine(
                                    $"{seasonalMarkerTypeName,60}: {dt.ToIsoString()}");

                                // Update or insert the record.
                                SeasonalMarkerRecord? existingSeasonalMarker =
                                    astroDbContext.SeasonalMarkers.FirstOrDefault(sm =>
                                        sm.MarkerNumber == (byte)seasonalMarker
                                        && sm.DateTimeUtcUsno != null
                                        && sm.DateTimeUtcUsno.Value.Year == year);
                                if (existingSeasonalMarker == null)
                                {
                                    astroDbContext.SeasonalMarkers.Add(newSeasonalMarkerRecord);
                                    await astroDbContext.SaveChangesAsync();
                                }
                                else if (existingSeasonalMarker.DateTimeUtcUsno != dt)
                                {
                                    existingSeasonalMarker.DateTimeUtcUsno = dt;
                                    await astroDbContext.SaveChangesAsync();
                                }
                            }
                            else
                            {
                                // Construct the Apside record.
                                int k = dt.Year - 2000;
                                ApsideRecord newApsideRecord = new ()
                                {
                                    AstroObjectId = earth.Id,
                                    CycleNumber = k,
                                    ApsideNumber = (byte)apside,
                                    DateTimeUtcUsno = dt
                                };
                                string apsideTypeName = apside.GetDisplayName();
                                Console.WriteLine($"{apsideTypeName,60}: {dt.ToIsoString()}");

                                // Update or insert the record.
                                ApsideRecord? existingApside =
                                    astroDbContext.Apsides.FirstOrDefault(a =>
                                        a.AstroObjectId == earth.Id
                                        && a.ApsideNumber == (byte)apside
                                        && a.DateTimeUtcUsno != null
                                        && a.DateTimeUtcUsno.Value.Year == year
                                        && a.DateTimeUtcUsno.Value.Month == usm.month);
                                if (existingApside == null)
                                {
                                    astroDbContext.Apsides.Add(newApsideRecord);
                                    await astroDbContext.SaveChangesAsync();
                                }
                                else if (existingApside.DateTimeUtcUsno != dt)
                                {
                                    existingApside.DateTimeUtcUsno = dt;
                                    await astroDbContext.SaveChangesAsync();
                                }
                            }
                        }
                    }
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
}
