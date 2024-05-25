using System.Globalization;
using System.Text.RegularExpressions;
using Galaxon.Astronomy.Algorithms.Records;
using Galaxon.Astronomy.Algorithms.Services;
using Galaxon.Astronomy.Algorithms.Utilities;
using Galaxon.Astronomy.Data;
using Galaxon.Astronomy.Data.Enums;
using Galaxon.Astronomy.Data.Models;
using Galaxon.Astronomy.Data.Repositories;
using Galaxon.Astronomy.DataImport.DataTransferObjects;
using Galaxon.Core.Exceptions;
using Galaxon.Core.Types;
using Galaxon.Time;
using Galaxon.Time.Extensions;
using HtmlAgilityPack;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Extensions;
using Newtonsoft.Json;

namespace Galaxon.Astronomy.DataImport.Services;

public class ApsideImportService(
    AstroDbContext astroDbContext,
    AstroObjectRepository astroObjectRepository,
    ApsideService apsideService)
{
    /// <summary>
    /// Log an information message.
    /// </summary>
    private void LogInfo(string message, string planetName, int orbitNumber, EApsideType apsideType,
        DateTime dt, double? radius_AU = null)
    {
        string strRadius = radius_AU == null ? "<unknown>" : $"{radius_AU:F6}";
        Slog.Information(
            "{Message}: Planet = {Planet}, Orbit = {Orbit}, Type = {Type}, DateTime = {DateTime}, Radius = {Radius} AU",
            message, planetName, orbitNumber, apsideType.GetDisplayName(), dt.ToIsoString(),
            strRadius);
    }

    /// <summary>
    /// Look up an apside record in the database by DateTime within ±1 hour of a given DateTime.
    /// </summary>
    private ApsideRecord? LookupRecord(DateTime dt)
    {
        return astroDbContext.Apsides.FirstOrDefault(a =>
            (a.DateTimeUtcGalaxon != null
                && Abs(EF.Functions.DateDiffHour(a.DateTimeUtcGalaxon, dt)!.Value) <= 1)
            || (a.DateTimeUtcAstroPixels != null
                && Abs(EF.Functions.DateDiffHour(a.DateTimeUtcAstroPixels, dt)!.Value) <= 1)
            || (a.DateTimeUtcUsno != null
                && Abs(EF.Functions.DateDiffHour(a.DateTimeUtcUsno, dt)!.Value) <= 1));
    }

    /// <summary>
    /// Compute and cache apside events.
    /// This will speed up the API and enables easy comparison with other sources using SQL.
    /// </summary>
    public async Task CacheCalculations(string planetName = "Earth")
    {
        // Compute results for 1700-3000.
        // USNO provides values from 1700-2100.
        // AstroPixels seems to provide good results for 2000-2500 but seems to contain errors in
        // year numbers before 2000.
        int minYear = 1700;
        int maxYear = 3000;

        // Load the planet.
        AstroObjectRecord planet = astroObjectRepository.LoadByName(planetName, "Planet");

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
        double jdttStart = JulianDateUtility.DateTimeUniversalToJulianDateTerrestrial(dtStart);

        // Get the end point of the period.
        DateTime dtEnd = GregorianCalendarUtility.GetYearEnd(maxYear);
        double jdttEnd = JulianDateUtility.DateTimeUniversalToJulianDateTerrestrial(dtEnd);

        // Start at the start point.
        double jdttApprox = jdttStart;

        while (true)
        {
            ApsideEvent apsideEvent = apsideService.GetClosestApside(planet, jdttApprox);

            // Log it.
            LogInfo("Computed apside", planetName, apsideEvent.OrbitNumber, apsideEvent.ApsideType,
                apsideEvent.DateTimeUtc, apsideEvent.Radius_AU);

            // Done?
            if (apsideEvent.JulianDateTerrestrial > jdttEnd)
            {
                break;
            }

            // If we're within range, update or insert a new record.
            if (apsideEvent.JulianDateTerrestrial >= jdttStart)
            {
                // Look for an existing record.
                ApsideRecord? record = LookupRecord(apsideEvent.DateTimeUtc);

                // Insert or update the record, or do nothing.
                if (record == null)
                {
                    // Insert a new record.
                    record = new ApsideRecord
                    {
                        AstroObjectId = apsideEvent.Planet.Id,
                        OrbitNumber = apsideEvent.OrbitNumber,
                        ApsideType = apsideEvent.ApsideType,
                        DateTimeUtcGalaxon = apsideEvent.DateTimeUtc
                    };
                    astroDbContext.Apsides.Add(record);
                    await astroDbContext.SaveChangesAsync();

                    // Log it.
                    Slog.Information("Inserted apside record.");
                }
                else if (record.DateTimeUtcGalaxon != apsideEvent.DateTimeUtc)
                {
                    // Update the existing record.
                    record.DateTimeUtcGalaxon = apsideEvent.DateTimeUtc;
                    await astroDbContext.SaveChangesAsync();

                    // Log it.
                    Slog.Information("Updated apside record.");
                }
                else
                {
                    // Nothing to do.
                    Slog.Information("Apside record already up-to-date.");
                }
            }

            // Go to the next approximate apside.
            jdttApprox = apsideEvent.JulianDateTerrestrial + orbitalPeriod_d / 2;
        } // while
    }

    public async Task ImportFromUsno()
    {
        AstroObjectRecord earth = astroObjectRepository.LoadByName("Earth", "Planet");

        for (int year = 1700; year <= 2100; year++)
        {
            try
            {
                await ImportFromUsnoYear(year, earth);
            }
            catch (Exception ex)
            {
                Slog.Error("Failed to import apside data from USNO for year {Year}: {Exception}",
                    year, ex.Message);
                throw;
            }
        }
    }

    private async Task ImportFromUsnoYear(int year, AstroObjectRecord earth)
    {
        string apiUrl = $"https://aa.usno.navy.mil/api/seasons?year={year}";

        using HttpClient client = new ();
        HttpResponseMessage response = await client.GetAsync(apiUrl);

        if (!response.IsSuccessStatusCode)
        {
            throw new InvalidOperationException(
                $"Failed to retrieve data. Status code: {response.StatusCode}");
        }

        // Get the JSON response.
        string jsonContent = await response.Content.ReadAsStringAsync();

        // Parse the JSON data to extract the seasonal marker data
        UsnoSeasonalMarkersForYear? usms =
            JsonConvert.DeserializeObject<UsnoSeasonalMarkersForYear>(jsonContent);

        // Check we got data.
        if (usms?.data == null || usms.data.IsEmpty())
        {
            throw new InvalidOperationException("Failed to find data in the API response.");
        }

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
            DateOnly apsideDate = new (usms.year, usm.month, usm.day);
            TimeOnly apsideTime = TimeOnly.Parse(usm.time);
            DateTime apsideDateTime = new (apsideDate, apsideTime, DateTimeKind.Utc);

            // Get the orbit number.
            int orbitNumber = OrbitNumber(apsideDateTime);

            // Log it.
            LogInfo("Parsed apside from USNO", earth.Name!, orbitNumber, apsideType,
                apsideDateTime);

            // Find the record to update.
            ApsideRecord? record = LookupRecord(apsideDateTime);

            if (record == null)
            {
                // Insert new record.
                record = new ApsideRecord
                {
                    AstroObjectId = earth.Id,
                    OrbitNumber = usms.year,
                    ApsideType = apsideType,
                    DateTimeUtcUsno = apsideDateTime
                };
                astroDbContext.Apsides.Add(record);
                await astroDbContext.SaveChangesAsync();

                // Log it.
                Slog.Information("Inserted apside record.");
            }
            else if (record.DateTimeUtcUsno != apsideDateTime)
            {
                // Update existing record.
                record.DateTimeUtcUsno = apsideDateTime;
                await astroDbContext.SaveChangesAsync();

                // Log it.
                Slog.Information("Updated apside record.");
            }
            else
            {
                // Log it.
                Slog.Information("Apside record already up-to-date.");
            }
        }
    }

    /// <summary>
    /// Import apside event data from the AstroPixels online ephemeris.
    /// See: <see href="https://www.astropixels.com/ephemeris/ephemeris.html"/>
    /// </summary>
    public async Task ImportFromAstroPixels()
    {
        // Load Earth.
        AstroObjectRecord earth = astroObjectRepository.LoadByName("Earth", "Planet");

        // Get year range to parse.
        int minYear = 1501;
        int maxYear = 2500;

        // Loop through all relevant URLS.
        for (int startYear = 1501; startYear <= 2401; startYear += 100)
        {
            try
            {
                await ImportFromAstroPixelsPage(startYear, minYear, maxYear, earth);
            }
            catch (Exception ex)
            {
                Slog.Error(
                    "Failed to import apside data from AstroPixels for start year {Year}: {Exception}",
                    startYear, ex.Message);
                throw;
            }
        }
    }

    private async Task ImportFromAstroPixelsPage(int startYear, int minYear, int maxYear,
        AstroObjectRecord earth)
    {
        // Get URL of page to parse.
        string url = $"https://www.astropixels.com/ephemeris/perap/perap{startYear}.html";

        // Fetch the content.
        using HttpClient httpClient = new ();
        string html = await httpClient.GetStringAsync(url);

        // Load HTML into an HtmlDocument.
        HtmlDocument htmlDoc = new ();
        htmlDoc.LoadHtml(html);

        // Define newline characters.
        char[] newlineChars = ['\r', '\n'];

        // Extract data from <pre> tags.
        foreach (HtmlNode? preNode in htmlDoc.DocumentNode.SelectNodes("//pre"))
        {
            // Get the lines.
            string[] lines =
                preNode.InnerText.Split(newlineChars, StringSplitOptions.RemoveEmptyEntries);

            foreach (string line in lines)
            {
                // Split the line into parts by whitespaces.
                string[] parts = Regex.Split(line.Trim(), @"\s+");

                // To see if this is a data record, check array has the right number of
                // parts, and that the first part is an integer.
                // Also, skip data from years outside the range we want data for.
                if (parts.Length != 17
                    || !int.TryParse(parts[0], out int year)
                    || year < minYear
                    || year > maxYear)
                {
                    continue;
                }

                // Loop through the two apside types.
                for (int apsideTypeIndex = 0; apsideTypeIndex <= 1; apsideTypeIndex++)
                {
                    await ImportFromAstroPixelsEvent(apsideTypeIndex, parts, year, earth);
                }
            }
        }
    }

    private async Task ImportFromAstroPixelsEvent(int apsideTypeIndex, string[] parts, int year,
        AstroObjectRecord earth)
    {
        // Get the Julian Calendar instance.
        JulianCalendar jcal = JulianCalendarUtility.JulianCalendarInstance;

        // Get the apside type.
        EApsideType apsideType = (EApsideType)apsideTypeIndex;

        // Get the offset.
        int j = apsideTypeIndex * 7 + 1;

        // Get the date and time parts.
        int apsideYear = parts[j] == "Dec" ? year - 1 : year;
        int apsideMonth = GregorianCalendarUtility.MonthNameToNumber(parts[j]);
        int apsideDay = int.Parse(parts[j + 1]);
        TimeOnly apsideTime = TimeOnly.Parse(parts[j + 2]);

        // Convert the date parts to a DateTime.
        DateTime apsideDateTime;
        // Check if it's a Julian or Gregorian date.
        if (JulianCalendarUtility.IsValidDate(apsideYear, apsideMonth, apsideDay))
        {
            apsideDateTime = jcal.ToDateTime(apsideYear, apsideMonth, apsideDay, apsideTime.Hour,
                apsideTime.Minute, apsideTime.Second, 0);
            DateTime.SpecifyKind(apsideDateTime, DateTimeKind.Utc);
        }
        else
        {
            apsideDateTime = new DateTime(apsideYear, apsideMonth, apsideDay, apsideTime.Hour,
                apsideTime.Minute, apsideTime.Second, DateTimeKind.Utc);
        }

        // Get the distance (radius).
        double apsideRadius_AU = double.Parse(parts[j + 3]);

        // Get the orbit number.
        int orbitNumber = OrbitNumber(apsideDateTime);

        // Log the extracted data.
        LogInfo("Parsed apside from AstroPixels", earth.Name!, orbitNumber, apsideType,
            apsideDateTime, apsideRadius_AU);

        // See if we need to insert or update the apside record.
        ApsideRecord? record = LookupRecord(apsideDateTime);
        if (record == null)
        {
            // Insert new record.
            record = new ApsideRecord
            {
                AstroObjectId = earth.Id,
                OrbitNumber = orbitNumber,
                ApsideType = apsideType,
                DateTimeUtcAstroPixels = apsideDateTime,
                RadiusAstroPixels_AU = apsideRadius_AU
            };
            astroDbContext.Apsides.Add(record);
            await astroDbContext.SaveChangesAsync();

            // Log it.
            Slog.Information("Inserted apside record.");
        }
        else if (record.DateTimeUtcAstroPixels != apsideDateTime)
        {
            // Update existing record.
            record.DateTimeUtcAstroPixels = apsideDateTime;
            record.RadiusAstroPixels_AU = apsideRadius_AU;
            await astroDbContext.SaveChangesAsync();

            // Log it.
            Slog.Information("Updated apside record.");
        }
        else
        {
            // Log it.
            Slog.Information("Apside record already up-to-date.");
        }
    }

    /// <summary>
    /// Get the orbit number (Earth only) given an apside DateTime.
    /// </summary>
    private static int OrbitNumber(DateTime apsideDateTime)
    {
        int orbitNumber = apsideDateTime.Year;
        if (apsideDateTime.Month == 12)
        {
            orbitNumber++;
        }
        return orbitNumber;
    }
}
