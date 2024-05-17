using Galaxon.Astronomy.Data;
using Galaxon.Astronomy.Data.Models;
using Galaxon.Astronomy.Data.Repositories;

namespace Galaxon.Astronomy.DataImport.Services;

public class Vsop87ImportService(AstroDbContext astroDbContext)
{
    /// <summary>
    /// Parse a VSOP87 data file downloaded from the VSOP87 ftp site.
    /// <see href="ftp://ftp.imcce.fr/pub/ephem/planets/vsop87"/>
    /// As per AA2 we're using VSOP87D data files, which contain values for
    /// heliocentric dynamical ecliptic and equinox of the date.
    /// <see href="https://www.caglow.com/info/compute/vsop87"/>
    /// </summary>
    public void ImportVsop87DataFile(int planetNumber)
    {
        string planetName = AstroObjectRepository.PLANET_NAMES[planetNumber]!;
        Console.WriteLine();
        Console.WriteLine($"Planet {planetName}");
        Console.WriteLine("===========================");

        // Get the data from the data file as an array of strings.
        string abbrev = planetName[..3].ToLower();
        string fileName = $"VSOP87D.{abbrev}";
        string dataFilePath = $"{AstroDbContext.DataDirectory()}/VSOP87/{fileName}";
        using StreamReader sr = new (dataFilePath);
        while (sr.ReadLine() is { } line)
        {
            Console.WriteLine($"Parsing {line}");

            // Get the code of body (planet number) (ib).
            string strCodeOfBody = line.Substring(2, 1);
            if (!byte.TryParse(strCodeOfBody, out byte codeOfBody))
            {
                Console.WriteLine("Could not read planet number (code of body), skipping line.");
                continue;
            }

            // Get the index of coordinate (ic).
            string strIndexOfCoordinate = line.Substring(3, 1);
            if (!byte.TryParse(strIndexOfCoordinate, out byte indexOfCoordinate))
            {
                throw new InvalidOperationException("Could not read variable index.");
            }
            Console.WriteLine($"Index of coordinate = {indexOfCoordinate}");

            // Get the degree alpha of time variable T (it).
            string strExponent = line.Substring(4, 1);
            if (!byte.TryParse(strExponent, out byte exponent))
            {
                throw new InvalidOperationException("Could not read exponent.");
            }
            Console.WriteLine($"Exponent = {exponent}");

            // Get the rank of the term within a series (n).
            string strRank = line.Substring(6, 5).Trim();
            if (!ushort.TryParse(strRank, out ushort rank))
            {
                throw new InvalidOperationException("Could not read rank.");
            }
            Console.WriteLine($"Rank = {rank}");

            // Get the amplitude (A).
            string strAmplitude = line.Substring(79, 18).Trim();
            if (!decimal.TryParse(strAmplitude, out decimal amplitude))
            {
                throw new InvalidOperationException("Could not read amplitude.");
            }
            Console.WriteLine($"Amplitude = {amplitude}");

            // Get the phase (B).
            string strPhase = line.Substring(97, 14).Trim();
            if (!decimal.TryParse(strPhase, out decimal phase))
            {
                throw new InvalidOperationException("Could not read phase.");
            }
            Console.WriteLine($"Phase = {phase}");

            // Get the frequency (C).
            string strFrequency = line.Substring(111, 20).Trim();
            if (!decimal.TryParse(strFrequency, out decimal frequency))
            {
                throw new InvalidOperationException("Could not read frequency.");
            }
            Console.WriteLine($"Frequency = {frequency}");

            // Look for an existing record.
            VSOP87DRecord? record = astroDbContext.VSOP87D.FirstOrDefault(record =>
                record.CodeOfBody == codeOfBody
                && record.IndexOfCoordinate == indexOfCoordinate
                && record.Exponent == exponent
                && record.Rank == rank);
            if (record == null)
            {
                // Add a new record.
                Console.WriteLine("Adding new record.");
                astroDbContext.VSOP87D.Add(new VSOP87DRecord
                {
                    CodeOfBody = codeOfBody,
                    IndexOfCoordinate = indexOfCoordinate,
                    Exponent = exponent,
                    Rank = rank,
                    Amplitude = amplitude,
                    Phase = phase,
                    Frequency = frequency
                });
                astroDbContext.SaveChanges();
            }
            else if (record.Amplitude != amplitude
                || record.Phase != phase
                || record.Frequency != frequency)
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
        for (int planetNum = 1; planetNum <= 8; planetNum++)
        {
            ImportVsop87DataFile(planetNum);
        }
    }
}
