using Galaxon.Astronomy.Algorithms.Records;
using Galaxon.Astronomy.Algorithms.Services;
using Galaxon.Astronomy.Data;
using Galaxon.Astronomy.Data.Enums;
using Galaxon.Astronomy.Data.Models;
using Galaxon.Astronomy.Data.Repositories;
using Galaxon.Astronomy.DataImport.DataTransferObjects;
using Galaxon.Core.Collections;
using Galaxon.Core.Exceptions;
using Galaxon.Time;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Extensions;
using Newtonsoft.Json;
using Serilog;

namespace Galaxon.Astronomy.DataImport.Services;

public class ApsideImportService(
    AstroDbContext astroDbContext,
    AstroObjectRepository astroObjectRepository,
    ApsideService apsideService)
{
    public void CacheCalculations(string planetName = "Earth")
    {
        // Define a range of years to compute values for.
        int minYear = 1500;
        int maxYear = 2500;

        // Load the planet.
        AstroObjectRecord planet = astroObjectRepository.LoadByName(planetName, "Planet");
        if (planet == null)
        {
            throw new Exception($"Could not load {planetName} from the database.");
        }

        // Get the orbital period.
        double? orbitalPeriodInSeconds = planet.Orbit?.SiderealOrbitPeriod ?? null;
        if (orbitalPeriodInSeconds == null)
        {
            throw new DataNotFoundException(
                $"Sidereal orbit period for {planetName} not found in database.");
        }
        double orbitalPeriodInDays = orbitalPeriodInSeconds.Value / TimeConstants.SECONDS_PER_DAY;

        // Get the start point of the period.
        DateTime dtStart = GregorianCalendarExtensions.GetYearStart(minYear);
        double jdttStart = TimeScales.DateTimeUniversalToJulianDateTerrestrial(dtStart);

        // Get the end point of the period.
        DateTime dtEnd = GregorianCalendarExtensions.GetYearEnd(maxYear);
        double jdttEnd = TimeScales.DateTimeUniversalToJulianDateTerrestrial(dtEnd);

        // Start at the start point.
        double jdttApprox = jdttStart;

        while (true)
        {
            ApsideEvent apsideEvent = apsideService.GetClosestApside(planet, jdttApprox);
            Console.WriteLine(
                $"{apsideEvent.ApsideType} for {planetName}, number {apsideEvent.ApsideNumber}, computed to occur at {apsideEvent.DateTimeUtc.ToIsoString()}");

            // Done?
            if (apsideEvent.JulianDateTerrestrial > jdttEnd)
            {
                break;
            }

            // If we're within range, update or insert a new record.
            if (apsideEvent.JulianDateTerrestrial >= jdttStart)
            {
                // Look for an existing record.
                ApsideRecord? apsideRecord;
                try
                {
                    apsideRecord = astroDbContext.Apsides.FirstOrDefault(apsideRec =>
                        apsideRec.AstroObjectId == planet.Id
                        && apsideRec.ApsideNumber == apsideEvent.ApsideNumber);
                }
                catch (Exception ex)
                {
                    Log.Error("{Exception}", ex.Message);
                    return;
                }

                if (apsideRecord == null)
                {
                    // Create a new record.
                    apsideRecord = new ApsideRecord();
                    astroDbContext.Apsides.Add(apsideRecord);
                }
                else
                {
                    // Update the existing record.
                    astroDbContext.Apsides.Attach(apsideRecord);
                }

                // Update the record.
                apsideRecord.AstroObjectId = planet.Id;
                apsideRecord.ApsideNumber = apsideEvent.ApsideNumber;
                apsideRecord.DateTimeUtcGalaxon = apsideEvent.DateTimeUtc;
                apsideRecord.RadiusGalaxon_AU = apsideEvent.Radius_AU;
                astroDbContext.SaveChanges();
            }

            // Go to the next approximate apside.
            jdttApprox = apsideEvent.JulianDateTerrestrial + orbitalPeriodInDays / 2;
        } // while
    }

    public async Task ImportApsidesFromUsno()
    {
        AstroObjectRecord earth = astroObjectRepository.LoadByName("Earth", "Planet");

        for (int year = 1700; year <= 2100; year++)
        {
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
                        Log.Error("Failed to retrieve data. Status code: {StatusCode}", response.StatusCode);
                    }
                    else
                    {
                        foreach (UsnoSeasonalMarker usm in usms.data)
                        {
                            EApsideType apsideType = default;

                            // Get the apside type.
                            if (usm.phenom == "Perihelion")
                            {
                                apsideType = EApsideType.Periapsis;
                            }
                            else if (usm.phenom == "Aphelion")
                            {
                                apsideType = EApsideType.Apoapsis;
                            }
                            else
                            {
                                continue;
                            }

                            // Construct the datetime.
                            DateOnly date = new (year, usm.month, usm.day);
                            TimeOnly time = TimeOnly.Parse(usm.time);
                            DateTime dt = new (date, time, DateTimeKind.Utc);

                            // Find the record to update.
                            ApsideRecord? existingApside =
                                astroDbContext.Apsides.FirstOrDefault(apsideRecord =>
                                    apsideRecord.AstroObjectId == earth.Id
                                    && Math.Abs(EF.Functions.DateDiffDay(apsideRecord.DateTimeUtcGalaxon, dt) ?? 2) <= 1);

                            if (existingApside == null)
                            {
                                Log.Warning("No matching record found for the {ApsideType} at {DateTime}.", apsideType.GetDisplayName(), dt.ToIsoString());
                            }
                            else
                            {
                                Log.Information("Updating record for the {ApsideType} at {DateTime}.", apsideType.GetDisplayName(), dt.ToIsoString());
                                existingApside.DateTimeUtcUsno = dt;
                                await astroDbContext.SaveChangesAsync();
                            }
                        }
                    }
                }
                else
                {
                    Log.Error("Failed to retrieve data. Status code: {StatusCode}", response.StatusCode);
                }
            }
            catch (Exception ex)
            {
                Log.Error("{Exception}", ex.Message);
            }
        }
    }

    public void ImportApsidesFromAstroPixels()
    {
        // Loop through all relevant URLS.
        for (int c = 1501; c <= 2401; c += 100)
        {
            string url = $"https://www.astropixels.com/ephemeris/perap/perap{c}.html";
        }
    }
}
