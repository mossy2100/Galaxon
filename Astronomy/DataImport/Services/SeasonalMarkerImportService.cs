using Galaxon.Astronomy.Data;
using Galaxon.Astronomy.Data.Enums;
using Galaxon.Astronomy.Data.Models;
using Galaxon.Astronomy.DataImport.Models;
using Galaxon.Core.Collections;
using Galaxon.Core.Types;
using Galaxon.Time;
using Newtonsoft.Json;

namespace Galaxon.Astronomy.DataImport.Services;

public class SeasonalMarkerImportService
{
    public async Task Import()
    {
        using AstroDbContext astroDbContext = new ();

        for (int year = 1700; year <= 2100; year++)
        {
            Console.WriteLine();

            try
            {
                string apiUrl = $"https://aa.usno.navy.mil/api/seasons?year={year}";

                using HttpClient client = new HttpClient();
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
                            EApsideType apsideType = default;

                            // Get the seasonal marker or apside type.
                            if (usm.month == 3 && usm.phenom == "Equinox")
                            {
                                isSeasonalMarker = true;
                                seasonalMarkerType = ESeasonalMarkerType.NorthwardEquinox;
                            }
                            else if (usm.month == 6 && usm.phenom == "Solstice")
                            {
                                isSeasonalMarker = true;
                                seasonalMarkerType = ESeasonalMarkerType.NorthernSolstice;
                            }
                            else if (usm.month == 9 && usm.phenom == "Equinox")
                            {
                                isSeasonalMarker = true;
                                seasonalMarkerType = ESeasonalMarkerType.SouthwardEquinox;
                            }
                            else if (usm.month == 12 && usm.phenom == "Solstice")
                            {
                                isSeasonalMarker = true;
                                seasonalMarkerType = ESeasonalMarkerType.SouthernSolstice;
                            }
                            else if (usm.phenom == "Perihelion")
                            {
                                isSeasonalMarker = false;
                                apsideType = EApsideType.Periapsis;
                            }
                            else if (usm.phenom == "Aphelion")
                            {
                                isSeasonalMarker = false;
                                apsideType = EApsideType.Apoapsis;
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
                                SeasonalMarker newSeasonalMarker = new SeasonalMarker
                                {
                                    Type = seasonalMarkerType,
                                    DateTimeUtcUsno = dt
                                };
                                string seasonalMarkerTypeName =
                                    seasonalMarkerType.GetDescriptionOrName();
                                Console.WriteLine(
                                    $"{seasonalMarkerTypeName,60}: {dt.ToIsoString()}");

                                // Update or insert the record.
                                SeasonalMarker? existingSeasonalMarker =
                                    astroDbContext.SeasonalMarkers.FirstOrDefault(sm =>
                                        sm.Type == seasonalMarkerType
                                        && sm.DateTimeUtcUsno.Year == year);
                                if (existingSeasonalMarker == null)
                                {
                                    astroDbContext.SeasonalMarkers.Add(newSeasonalMarker);
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
                                Apside newApside = new Apside
                                {
                                    Type = apsideType,
                                    DateTimeUtcUsno = dt
                                };
                                string apsideTypeName = apsideType.GetDescriptionOrName();
                                Console.WriteLine($"{apsideTypeName,60}: {dt.ToIsoString()}");

                                // Update or insert the record.
                                Apside? existingApside = astroDbContext.Apsides.FirstOrDefault(a =>
                                    a.Type == apsideType
                                    && a.DateTimeUtcUsno.Year == year
                                    && a.DateTimeUtcUsno.Month == usm.month);
                                if (existingApside == null)
                                {
                                    astroDbContext.Apsides.Add(newApside);
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
