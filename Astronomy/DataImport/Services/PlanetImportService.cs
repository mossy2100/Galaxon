using Galaxon.Astronomy.Data;
using Galaxon.Astronomy.Data.Models;
using Galaxon.Astronomy.Data.Repositories;
using Galaxon.Quantities.Kinds;

namespace Galaxon.Astronomy.DataImport.Services;

public class PlanetImportService(
    AstroDbContext astroDbContext,
    AstroObjectRepository astroObjectRepository,
    AstroObjectGroupRepository astroObjectGroupRepository)
{
    /// <summary>
    /// Attempt to parse a double value from a data row.
    /// If successful, multiply the value by the multiplier before returning.
    /// Otherwise, return null.
    /// </summary>
    private static double? ParseDouble(string str, double multiplier = 1)
    {
        return double.TryParse(str, out double value) ? value * multiplier : null;
    }

    /// <summary>
    /// Initialize the planet data from the CSV file.
    /// </summary>
    public async Task Import()
    {
        // Get the Sun.
        AstroObjectRecord sun = astroObjectRepository.LoadByName("Sun", "Star");

        // Constant for converting from AU to km.
        const double KM_PER_AU = (double)Length.METRES_PER_ASTRONOMICAL_UNIT / 1000;

        // Number of columns in the worksheet.
        const int NUM_COLUMNS = 45;

        // Load the data from the Excel file.
        string xlsxPath = $"{AstroDbContext.DataDirectory()}/Planets/Planets.xlsx";
        List<List<string>> planetData = ExcelReader.ReadXlsxFile(xlsxPath);

        // Create or update the planet records.
        foreach (List<string> row in planetData)
        {
            // Skip non-data rows.
            if (row.Count < 2 || !uint.TryParse(row[1], out uint number))
            {
                Slog.Warning("Could not read the number; probably a header row: {Data}",
                    string.Join(", ", row));
                continue;
            }

            // Fill row with spaces if necessary, so we don't get index out of range errors.
            if (row.Count < NUM_COLUMNS)
            {
                int diff = NUM_COLUMNS - row.Count;
                for (int i = 0; i < diff; i++)
                {
                    row.Add("");
                }
            }

            // Get the name.
            string name = row[0];

            // Load the object record if there is one.
            AstroObjectRecord? planet = astroObjectRepository.Load(name, number);

            // Create a new record as needed.
            if (planet == null)
            {
                // Insert new planet data.
                planet = new AstroObjectRecord(name, number);
                astroDbContext.AstroObjects.Add(planet);

                Slog.Information("Inserting new data for astronomical object: {Name} ({Number})",
                    name, number);
            }
            else
            {
                // Update existing planet data.
                Slog.Information("Updating data for astronomical object: {Name} ({Number})", name,
                    number);
            }

            //--------------------------------------------------------------------------------------
            // Basic info.

            // Groups
            List<string> groups = row[2].Split(",").Select(g => g.Trim()).ToList();
            foreach (string group in groups)
            {
                astroObjectGroupRepository.AddToGroup(planet, group);
            }

            // The parent of planets and dwarf planets is the Sun.
            planet.Parent = sun;

            // Wikipedia URL.
            planet.WikipediaUrl = row[3];

            // Save the planet object now to ensure it has an id before attaching composition
            // objects to it.
            await astroDbContext.SaveChangesAsync();

            Slog.Information("Updated basic data for {Name}", name);

            //--------------------------------------------------------------------------------------
            // Orbit data.
            planet.Orbit ??= new OrbitRecord();

            // Apoapsis is provided in AU, convert to km.
            planet.Orbit.Apoapsis_km = ParseDouble(row[4], KM_PER_AU);

            // Periapsis is provided in AU, convert to km.
            planet.Orbit.Periapsis_km = ParseDouble(row[5], KM_PER_AU);

            // Semi-major axis is provided in AU, convert to km.
            planet.Orbit.SemiMajorAxis_km = ParseDouble(row[6], KM_PER_AU);

            // Eccentricity.
            planet.Orbit.Eccentricity = ParseDouble(row[7]);

            // Sidereal orbit period.
            planet.Orbit.SiderealOrbitPeriod_d = ParseDouble(row[8]);

            // Synodic orbit period.
            planet.Orbit.SynodicOrbitPeriod_d = ParseDouble(row[9]);

            // Average orbital speed.
            planet.Orbit.AverageOrbitalSpeed_km_s = ParseDouble(row[10]);

            // Mean anomaly.
            planet.Orbit.MeanAnomaly_deg = ParseDouble(row[11]);

            // Inclination to ecliptic.
            planet.Orbit.Inclination_deg = ParseDouble(row[12]);

            // Longitude of ascending node.
            planet.Orbit.LongitudeAscendingNode_deg = ParseDouble(row[13]);

            // Argument of perihelion.
            planet.Orbit.ArgumentPeriapsis_deg = ParseDouble(row[14]);

            // Save the orbit data.
            await astroDbContext.SaveChangesAsync();
            Slog.Information("Updated orbit data for {Name}", name);

            //--------------------------------------------------------------------------------------
            // Physical data.
            planet.Physical ??= new PhysicalRecord();

            // Equatorial radius.
            planet.Physical.EquatorialRadius_km = ParseDouble(row[15]);

            // Second equatorial radius.
            planet.Physical.EquatorialRadius2_km = ParseDouble(row[16]);

            // Polar radius.
            planet.Physical.PolarRadius_km = ParseDouble(row[17]);

            // Mean radius.
            planet.Physical.MeanRadius_km = ParseDouble(row[18]);

            // Flattening.
            planet.Physical.Flattening = ParseDouble(row[19]);

            // Surface area.
            planet.Physical.SurfaceArea_km2 = ParseDouble(row[20]);

            // Volume.
            planet.Physical.Volume_km3 = ParseDouble(row[21]);

            // Mass.
            planet.Physical.Mass_kg = ParseDouble(row[22]);

            // Density.
            planet.Physical.Density_kg_m3 = ParseDouble(row[23]);

            // Surface gravity.
            planet.Physical.SurfaceGravity_m_s2 = ParseDouble(row[24]);

            // Escape velocity.
            planet.Physical.EscapeVelocity_km_s = ParseDouble(row[25]);

            // Standard gravitational parameter.
            planet.Physical.StandardGravitationalParameter_m3_s2 = ParseDouble(row[26]);

            // Has global magnetic field.
            planet.Physical.HasGlobalMagneticField = row[27] == "Y";

            // Has rings.
            planet.Physical.HasRings = row[28] == "Y";

            // Geometric albedo.
            planet.Physical.GeometricAlbedo = ParseDouble(row[29]);

            // Minimum temperature.
            planet.Physical.MinSurfaceTemperature_K = ParseDouble(row[30]);

            // Mean temperature.
            planet.Physical.MeanSurfaceTemperature_K = ParseDouble(row[31]);

            // Maximum temperature.
            planet.Physical.MaxSurfaceTemperature_K = ParseDouble(row[32]);

            // Surface equivalent dose rate.
            planet.Physical.SurfaceEquivalentDoseRate_microSv_h = ParseDouble(row[33]);

            // Save the physical data.
            await astroDbContext.SaveChangesAsync();
            Slog.Information("Updated physical data for {Name}", name);

            //--------------------------------------------------------------------------------------
            // Rotation data.
            planet.Rotation ??= new RotationRecord();

            // Synodic rotation period.
            planet.Rotation.SynodicRotationPeriod_d = ParseDouble(row[34]);

            // Sidereal rotation period.
            planet.Rotation.SiderealRotationPeriod_d = ParseDouble(row[35]);

            // Equatorial rotational velocity.
            planet.Rotation.EquatorialRotationalVelocity_m_s = ParseDouble(row[36]);

            // Obliquity.
            planet.Rotation.Obliquity_deg = ParseDouble(row[37]);

            // North pole right ascension.
            planet.Rotation.NorthPoleRightAscension_deg = ParseDouble(row[38]);

            // North pole declination.
            planet.Rotation.NorthPoleDeclination_deg = ParseDouble(row[39]);

            // Save the rotation data.
            await astroDbContext.SaveChangesAsync();
            Slog.Information("Updated rotation data for {Name}", name);

            //--------------------------------------------------------------------------------------
            // Observation data.
            planet.Observation ??= new ObservationRecord();

            // Absolute magnitude.
            planet.Observation.AbsoluteMagnitude = ParseDouble(row[40]);

            // Save the observation data.
            await astroDbContext.SaveChangesAsync();
            Slog.Information("Updated observation data for {Name}", name);

            //--------------------------------------------------------------------------------------
            // Atmosphere data.

            // If the object has an atmosphere.
            bool hasAtmosphere = row[41] == "Y";

            if (hasAtmosphere)
            {
                // Make sure the atmosphere data record exists.
                planet.Atmosphere ??= new AtmosphereRecord();

                // If the atmosphere is a surface bounded exosphere.
                planet.Atmosphere.IsSurfaceBoundedExosphere = row[42] == "Y";

                // Surface pressure.
                planet.Atmosphere.SurfacePressure_Pa = ParseDouble(row[43]);

                // Scale height.
                planet.Atmosphere.ScaleHeight_km = ParseDouble(row[44], 1000);
            }
            else
            {
                planet.Atmosphere = null;
            }

            // Save the atmosphere data.
            await astroDbContext.SaveChangesAsync();
            Slog.Information("Updated atmosphere data for {Name}", name);
        }
    }
}
