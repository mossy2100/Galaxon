using Galaxon.Astronomy.Data;
using Galaxon.Astronomy.Data.Enums;
using Galaxon.Astronomy.Data.Models;
using Galaxon.Astronomy.Data.Repositories;
using Galaxon.Astronomy.DataImport.DataTransferObjects;
using Galaxon.Core.Collections;
using Galaxon.Time;
using Microsoft.OpenApi.Extensions;
using Newtonsoft.Json;

namespace Galaxon.Astronomy.DataImport.Services;

public class SeasonalMarkerImportService(
    AstroDbContext astroDbContext,
    AstroObjectRepository astroObjectRepository)
{
    public async Task Import()
    {
        AstroObjectRecord earth = astroObjectRepository.LoadByName("Earth", "Planet");

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
                            ESeasonalMarkerType seasonalMarkerType = default;
                            // EApsideType apside = default;

                            // Get the seasonal marker or apside type.
                            if (usm is { month: 3, phenom: "Equinox" })
                            {
                                isSeasonalMarker = true;
                                seasonalMarkerType = ESeasonalMarkerType.NorthwardEquinox;
                            }
                            else if (usm is { month: 6, phenom: "Solstice" })
                            {
                                isSeasonalMarker = true;
                                seasonalMarkerType = ESeasonalMarkerType.NorthernSolstice;
                            }
                            else if (usm is { month: 9, phenom: "Equinox" })
                            {
                                isSeasonalMarker = true;
                                seasonalMarkerType = ESeasonalMarkerType.SouthwardEquinox;
                            }
                            else if (usm is { month: 12, phenom: "Solstice" })
                            {
                                isSeasonalMarker = true;
                                seasonalMarkerType = ESeasonalMarkerType.SouthernSolstice;
                            }
                            else if (usm.phenom == "Perihelion")
                            {
                                isSeasonalMarker = false;
                                // apside = EApsideType.Periapsis;
                            }
                            else if (usm.phenom == "Aphelion")
                            {
                                isSeasonalMarker = false;
                                // apside = EApsideType.Apoapsis;
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
                                        Type = seasonalMarkerType,
                                        DateTimeUtcUsno = dt
                                    };
                                string seasonalMarkerTypeName = seasonalMarkerType.GetDisplayName();
                                Console.WriteLine(
                                    $"{seasonalMarkerTypeName,60}: {dt.ToIsoString()}");

                                // Update or insert the record.
                                SeasonalMarkerRecord? existingSeasonalMarker =
                                    astroDbContext.SeasonalMarkers.FirstOrDefault(sm =>
                                        sm.Type == seasonalMarkerType
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
                                // // Construct the Apside record.
                                // double k = Math.Round((TimeScales.DateTimeToDecimalYear(dt) - 2000) * 2) / 2;
                                // int orbitNumber = (int)Math.Floor(k);
                                // ApsideRecord newApsideRecord = new ()
                                // {
                                //     AstroObjectId = earth.Id,
                                //     OrbitNumber = orbitNumber,
                                //     ApsideNumber = (byte)apside,
                                //     DateTimeUtcUsno = dt
                                // };
                                // string apsideTypeName = apside.GetDisplayName();
                                // Console.WriteLine($"{apsideTypeName,60}: {dt.ToIsoString()}");
                                //
                                // // Update or insert the record.
                                // ApsideRecord? existingApside =
                                //     astroDbContext.Apsides.FirstOrDefault(a =>
                                //         a.AstroObjectId == earth.Id
                                //         && a.OrbitNumber == orbitNumber
                                //         && a.ApsideNumber == (byte)apside);
                                // if (existingApside == null)
                                // {
                                //     astroDbContext.Apsides.Add(newApsideRecord);
                                // }
                                // else if (existingApside.DateTimeUtcUsno != dt)
                                // {
                                //     existingApside.DateTimeUtcUsno = dt;
                                // }
                                // await astroDbContext.SaveChangesAsync();
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
