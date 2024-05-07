using Galaxon.Astronomy.Algorithms.Records;
using Galaxon.Astronomy.Algorithms.Services;
using Galaxon.Astronomy.Data;
using Galaxon.Astronomy.Data.Models;
using Galaxon.Astronomy.Data.Repositories;
using Galaxon.Core.Exceptions;
using Galaxon.Time;
using Microsoft.EntityFrameworkCore;

namespace DataImport.Services;

public class ApsideImportService(
    AstroDbContext astroDbContext,
    AstroObjectRepository astroObjectRepository,
    ApsideService apsideService)
{
    public void CacheCalculations()
    {
        // Define a range of years to compute values for.
        int minYear = 1700;
        int maxYear = 2100;

        // Get the start point of the period.
        DateTime dtStart = GregorianCalendarExtensions.GetYearStart(minYear);
        double jdttStart = TimeScales.DateTimeUniversalToJulianDateTerrestrial(dtStart);

        // Get the start point of the period.
        DateTime dtEnd = GregorianCalendarExtensions.GetYearEnd(maxYear);
        double jdttEnd = TimeScales.DateTimeUniversalToJulianDateTerrestrial(dtEnd);

        // Loop through the planets.
        // foreach (string planetName in Vsop87ImportService.PLANET_NAMES.Values)
        // {
        string planetName = "Earth";
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
            throw new DataNotFoundException("Orbital period not found in database.");
        }
        double orbitalPeriodInDays = orbitalPeriodInSeconds.Value / TimeConstants.SECONDS_PER_DAY;

        // Start at the start point.
        double jdttApprox = jdttStart;

        while (true)
        {
            ApsideEvent apsideEvent = apsideService.GetClosestApside(planet, jdttApprox);
            Console.WriteLine(
                $"Apside ({apsideEvent.Type}) computed to occur at {apsideEvent.DateTimeUtc.ToIsoString()}");
            double jdttEvent = apsideEvent.JulianDateTerrestrial;

            // Done?
            if (jdttEvent > jdttEnd)
            {
                break;
            }

            // If we're within range, update or insert a new record.
            if (jdttEvent >= jdttStart)
            {
                // Look for an existing record.
                ApsideRecord? apsideRecord = astroDbContext.Apsides.FirstOrDefault(a =>
                    a.AstroObjectId == planet.Id
                    && a.Orbit == apsideEvent.Orbit
                    && a.Type == apsideEvent.Type);

                if (apsideRecord == null)
                {
                    // Create a new record.
                    apsideRecord = new ApsideRecord
                    {
                        AstroObjectId = planet.Id,
                        Orbit = apsideEvent.Orbit,
                        Type = apsideEvent.Type,
                        DateTimeUtc = apsideEvent.DateTimeUtc
                    };
                    astroDbContext.Apsides.Add(apsideRecord);
                }
                else
                {
                    // Update the existing record.
                    apsideRecord.DateTimeUtc = apsideEvent.DateTimeUtc;
                    astroDbContext.Apsides.Attach(apsideRecord);
                }

                // Update the database.
                astroDbContext.SaveChanges();
            }

            // Go to the next approximate apside.
            jdttApprox = jdttEvent + orbitalPeriodInDays / 2;
        } // while
    }
}
