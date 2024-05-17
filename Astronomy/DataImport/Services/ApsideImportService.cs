using System.Globalization;
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
using HtmlAgilityPack;
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
        // The AstroPixels ephemeris shows results for years 1501-2500, but unfortunately there
        // appear to be numerous wrong (by 1) year numbers before 2000.
        int minYear = 2001;
        int maxYear = 2500;

        // Load the planet.
        AstroObjectRecord planet = astroObjectRepository.LoadByName(planetName, "Planet");
        if (planet == null)
        {
            throw new Exception($"Could not load {planetName} from the database.");
        }

        // Get the orbital period.
        double? orbitalPeriod_s = planet.Orbit?.SiderealOrbitPeriod ?? null;
        if (orbitalPeriod_s == null)
        {
            throw new DataNotFoundException(
                $"Sidereal orbit period for {planetName} not found in database.");
        }
        double orbitalPeriod_d = orbitalPeriod_s.Value / TimeConstants.SECONDS_PER_DAY;

        // Get the start point of the period.
        DateTime dtStart = GregorianCalendarUtility.GetYearStart(minYear);
        double jdttStart = TimeScales.DateTimeUniversalToJulianDateTerrestrial(dtStart);

        // Get the end point of the period.
        DateTime dtEnd = GregorianCalendarUtility.GetYearEnd(maxYear);
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
                apsideRecord.ApsideType = apsideEvent.ApsideType;
                apsideRecord.DateTimeUtcGalaxon = apsideEvent.DateTimeUtc;
                apsideRecord.RadiusGalaxon_AU = apsideEvent.Radius_AU;
                astroDbContext.SaveChanges();
            }

            // Go to the next approximate apside.
            jdttApprox = apsideEvent.JulianDateTerrestrial + orbitalPeriod_d / 2;
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
                    await ImportApsidesFromUsnoJson(response, earth);
                }
                else
                {
                    Log.Error("Failed to retrieve data. Status code: {StatusCode}",
                        response.StatusCode);
                }
            }
            catch (Exception ex)
            {
                Log.Error("{Exception}", ex.Message);
            }
        }
    }

    private async Task ImportApsidesFromUsnoJson(HttpResponseMessage response,
        AstroObjectRecord earth)
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
                EApsideType apsideType;

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
                DateOnly date = new (usms.year, usm.month, usm.day);
                TimeOnly time = TimeOnly.Parse(usm.time);
                DateTime dt = new (date, time, DateTimeKind.Utc);

                // Find the record to update.
                ApsideRecord? existingApside =
                    astroDbContext.Apsides.FirstOrDefault(apsideRecord =>
                        apsideRecord.AstroObjectId == earth.Id
                        && Math.Abs(EF.Functions.DateDiffDay(apsideRecord.DateTimeUtcGalaxon, dt)
                            ?? 2)
                        <= 1);

                if (existingApside == null)
                {
                    Log.Warning("No matching record found for the {ApsideType} at {DateTime}.",
                        apsideType.GetDisplayName(), dt.ToIsoString());
                }
                else
                {
                    Log.Information("Updating record for the {ApsideType} at {DateTime}.",
                        apsideType.GetDisplayName(), dt.ToIsoString());
                    existingApside.DateTimeUtcUsno = dt;
                    await astroDbContext.SaveChangesAsync();
                }
            }
        }
    }

    public async Task ImportApsidesFromAstroPixels()
    {
        AstroObjectRecord earth = astroObjectRepository.LoadByName("Earth", "Planet");
        JulianCalendar jcal = new ();

        // Loop through all relevant URLS.
        for (int startYear = 1501; startYear <= 2401; startYear += 100)
        {
            try
            {
                string url = $"https://www.astropixels.com/ephemeris/perap/perap{startYear}.html";

                // Use HttpClient to fetch the content.
                using HttpClient httpClient = new ();
                string html = await httpClient.GetStringAsync(url);

                // Load HTML into the HtmlDocument.
                HtmlDocument htmlDoc = new ();
                htmlDoc.LoadHtml(html);

                // Extract information from <pre> tags.
                foreach (HtmlNode? preNode in htmlDoc.DocumentNode.SelectNodes("//pre"))
                {
                    string[] lines = preNode.InnerText.Split(new[] { '\r', '\n' },
                        StringSplitOptions.RemoveEmptyEntries);
                    foreach (string line in lines)
                    {
                        // Split the line by multiple spaces (assumes space is the delimiter).
                        string[] parts =
                            System.Text.RegularExpressions.Regex.Split(line.Trim(), @"\s+");

                        // To see if this is a data record, check array has the right number of
                        // parts, and that the first part is an integer.
                        if (parts.Length != 17 || !int.TryParse(parts[0], out int year))
                        {
                            continue;
                        }

                        // Loop through the two apside types.
                        int i = 1;
                        foreach (EApsideType apsideType in Enum.GetValues(typeof(EApsideType)))
                        {
                            // Get the date and time parts.
                            int apsideYear = parts[i] == "Dec" ? year - 1 : year;
                            int apsideMonth = GregorianCalendarUtility.MonthNameToNumber(parts[i]);
                            int apsideDay = int.Parse(parts[i + 1]);
                            TimeOnly apsideTime = TimeOnly.Parse(parts[i + 2]);

                            // Convert the date parts to a DateTime.
                            DateTime apsideDateTime;
                            // Check if it's a Julian or Gregorian date.
                            if (GregorianCalendarUtility.IsJulianDate(apsideYear, apsideMonth,
                                apsideDay))
                            {
                                apsideDateTime = jcal.ToDateTime(apsideYear, apsideMonth,
                                    apsideDay, apsideTime.Hour, apsideTime.Minute,
                                    apsideTime.Second, 0);
                                DateTime.SpecifyKind(apsideDateTime, DateTimeKind.Utc);
                            }
                            else
                            {
                                apsideDateTime = new (apsideYear, apsideMonth, apsideDay,
                                    apsideTime.Hour, apsideTime.Minute, apsideTime.Second,
                                    DateTimeKind.Utc);
                            }

                            // Get the distance (radius).
                            double apsideDistance = double.Parse(parts[i + 3]);

                            // Log the extracted data.
                            Log.Information(
                                "Apside type: {ApsideType}, Datetime: {ApsideDateTime}, Distance: {ApsideDistance} AU",
                                apsideType.GetDisplayName(), apsideDateTime.ToIsoString(),
                                apsideDistance);

                            // Update the apside record.
                            ApsideRecord? existingApside =
                                astroDbContext.Apsides.FirstOrDefault(apsideRecord =>
                                    apsideRecord.AstroObjectId == earth.Id
                                    && apsideRecord.ApsideType == apsideType
                                    && Math.Abs(
                                        EF.Functions.DateDiffMinute(apsideRecord.DateTimeUtcGalaxon,
                                            apsideDateTime)
                                        ?? int.MaxValue)
                                    <= 10);
                            if (existingApside == null)
                            {
                                Log.Warning(
                                    "No matching record found for the {ApsideType} apside at {DateTime}.",
                                    apsideType.GetDisplayName(), apsideDateTime.ToIsoString());
                            }
                            else
                            {
                                Log.Information(
                                    "Updating record for the {ApsideType} at {DateTime}.",
                                    apsideType.GetDisplayName(), apsideDateTime.ToIsoString());
                                existingApside.DateTimeUtcAstroPixels = apsideDateTime;
                                existingApside.RadiusAstroPixels_AU = apsideDistance;
                                await astroDbContext.SaveChangesAsync();
                            }

                            // Adjust field offset for the next iteration.
                            i += 7;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Error("{Exception}", ex.Message);
            }
        }
    }
}
