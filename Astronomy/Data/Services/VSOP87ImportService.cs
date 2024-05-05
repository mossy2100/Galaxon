// using Galaxon.Astronomy.Algorithms.Services;

using Galaxon.Astronomy.Data.Models;
using Galaxon.Astronomy.Data.Repositories;
using Galaxon.Numerics.Extensions.FloatingPoint;

namespace Galaxon.Astronomy.Data.Services;

public class VSOP87ImportService(
    AstroDbContext astroDbContext,
    AstroObjectRepository astroObjectRepository)
{
    /// <summary>
    /// Dictionary mapping planet numbers to English names.
    /// </summary>
    public static readonly Dictionary<int, string> PLANET_NAMES = new ()
    {
        { 1, "Mercury" },
        { 2, "Venus" },
        { 3, "Earth" },
        { 4, "Mars" },
        { 5, "Jupiter" },
        { 6, "Saturn" },
        { 7, "Uranus" },
        { 8, "Neptune" }
    };

    /// <summary>
    /// Parse a VSOP87 data file downloaded from the VSOP87 ftp site.
    /// <see href="ftp://ftp.imcce.fr/pub/ephem/planets/vsop87"/>
    /// As per AA2 we're using VSOP87D data files, which contain values for
    /// heliocentric dynamical ecliptic and equinox of the date.
    /// <see href="https://www.caglow.com/info/compute/vsop87"/>
    /// </summary>
    /// <param name="fileName">The name of the data file.</param>
    public void ImportVsop87DataFile(string fileName)
    {
        // Get the data from the data file as an array of strings.
        string dataFilePath = $"{AstroDbContext.DataDirectory()}/VSOP87/{fileName}";
        using StreamReader sr = new (dataFilePath);
        while (sr.ReadLine() is { } line)
        {
            Console.WriteLine($"Parsing {line}");

            // Get the planet number (called "code of body" in vsop87.doc).
            string strPlanetNum = line.Substring(2, 1);
            if (!byte.TryParse(strPlanetNum, out byte planetNum))
            {
                Console.WriteLine("Could not read planet number, skipping line.");
                continue;
            }

            // Attach the record to the right planet AstroObject.
            AstroObject? planet = astroObjectRepository.LoadByNumber(planetNum, "Planet");
            if (planet == null)
            {
                throw new InvalidOperationException($"Could not find planet number {planetNum}.");
            }
            Console.WriteLine($"Planet number {planetNum} => {planet.Name}");

            // Get the variable.
            string strVariableIndex = line.Substring(3, 1);
            if (!byte.TryParse(strVariableIndex, out byte variableIndex))
            {
                throw new InvalidOperationException("Could not read variable index.");
            }
            char variable = variableIndex switch
            {
                1 => 'L',
                2 => 'B',
                3 => 'R',
                _ => ' '
            };
            Console.WriteLine($"Variable = {variable}");

            // Get the exponent of T (called "degree alpha of time variable T" in vsop87.doc).
            string strExponent = line.Substring(4, 1);
            if (!byte.TryParse(strExponent, out byte exponent))
            {
                throw new InvalidOperationException("Could not read exponent.");
            }
            Console.WriteLine($"Exponent = {exponent}");

            // Get the index (rank) of the term within a series (n).
            string strIndex = line.Substring(6, 5).Trim();
            if (!ushort.TryParse(strIndex, out ushort index))
            {
                throw new InvalidOperationException("Could not read index.");
            }
            Console.WriteLine($"Index = {index}");

            // Get the amplitude (A).
            string strAmplitude = line.Substring(80, 18).Trim();
            if (!double.TryParse(strAmplitude, out double amplitude))
            {
                throw new InvalidOperationException("Could not read amplitude.");
            }
            Console.WriteLine($"Amplitude = {amplitude}");

            // Get the phase (B).
            string strPhase = line.Substring(98, 14).Trim();
            if (!double.TryParse(strPhase, out double phase))
            {
                throw new InvalidOperationException("Could not read phase.");
            }
            Console.WriteLine($"Phase = {phase}");

            // Get the frequency (C).
            string strFrequency = line.Substring(112, 20).Trim();
            if (!double.TryParse(strFrequency, out double frequency))
            {
                throw new InvalidOperationException("Could not read frequency.");
            }
            Console.WriteLine($"Frequency = {frequency}");

            // Look for an existing record.
            VSOP87DRecord? record = astroDbContext.VSOP87DRecords.FirstOrDefault(record =>
                record.AstroObjectId == planet.Id
                && record.Variable == variable
                && record.Exponent == exponent
                && record.Index == index);
            if (record == null)
            {
                // Add a new record.
                Console.WriteLine("Adding new record.");
                astroDbContext.VSOP87DRecords.Add(new VSOP87DRecord
                {
                    AstroObjectId = planet.Id,
                    Variable = variable,
                    Exponent = exponent,
                    Index = index,
                    Amplitude = amplitude,
                    Phase = phase,
                    Frequency = frequency
                });
                astroDbContext.SaveChanges();
            }
            else if (!record.Amplitude.FuzzyEquals(amplitude)
                || !record.Phase.FuzzyEquals(phase)
                || !record.Frequency.FuzzyEquals(frequency))
            {
                // Update the record.
                Console.WriteLine("Updating record.");
                record.Amplitude = amplitude;
                record.Phase = phase;
                record.Frequency = frequency;
                astroDbContext.SaveChanges();
            }
        }
    }

    public void Import()
    {
        for (byte planetNum = 1; planetNum <= 8; planetNum++)
        {
            string name = PLANET_NAMES[planetNum];
            string abbrev = name[..3].ToLower();
            ImportVsop87DataFile($"VSOP87D.{abbrev}");
        }
    }
}
