using Galaxon.Astronomy.Algorithms.Records;
using Galaxon.Astronomy.Algorithms.Utilities;
using Galaxon.Astronomy.Data;
using Galaxon.Astronomy.Data.Models;
using Galaxon.Astronomy.Data.Repositories;
using Galaxon.Core.Collections;
using Galaxon.Core.Exceptions;
using Galaxon.Numerics.Algebra;

namespace Galaxon.Astronomy.Algorithms.Services;

public class PlanetService(AstroDbContext astroDbContext, AstroObjectRepository astroObjectRepository)
{
    /// <summary>
    /// Calculate the position of a planet in heliocentric ecliptic coordinates.
    /// Algorithm is from AA2 p218.
    /// The result is a Coordinates object tuple with the 3 coordinate values as radians.
    ///     Longitude = the heliocentric longitude in radians, in range -PI..PI
    ///     Latitude = the heliocentric latitude in radians, in range -PI/2..PI/2
    ///     Radius = the orbital radius (distance to Sun) in astronomical units (AU).
    /// <see href="https://www.caglow.com/info/compute/vsop87"/>
    /// Original data files are from:
    /// <see href="ftp://ftp.imcce.fr/pub/ephem/planets/vsop87"/>
    /// </summary>
    /// <param name="planet">The planet.</param>
    /// <param name="jdtt">The Julian Ephemeris Day.</param>
    /// <returns>
    /// The planet's position in heliocentric ecliptic coordinates (radians).
    /// </returns>
    /// <exception cref="DataNotFoundException">
    /// If no VSOP87D data could be found for the planet.
    /// </exception>
    public Coordinates CalcPlanetPosition(AstroObjectRecord planet, double jdtt)
    {
        // Get the VSOP87D data for the planet from the database.
        List<VSOP87DRecord> records = astroDbContext.VSOP87D.Where(r => r.CodeOfBody == planet.Number).ToList();

        // Check there are records.
        if (records.IsEmpty())
        {
            throw new DataNotFoundException($"No VSOP87D data found for planet {planet.Name}.");
        }

        // Get T in Julian millennia from the epoch J2000.0.
        double T = JulianDateUtility.JulianMillenniaSinceJ2000(jdtt);

        // Calculate the coefficients for each coordinate variable.
        Dictionary<byte, double[]> coeffs = new ();
        foreach (VSOP87DRecord record in records)
        {
            if (!coeffs.ContainsKey(record.IndexOfCoordinate))
            {
                coeffs[record.IndexOfCoordinate] = new double[6];
            }
            double amplitude = (double)record.Amplitude;
            double phase = (double)record.Phase;
            double frequency = (double)record.Frequency;
            coeffs[record.IndexOfCoordinate][record.Exponent] += amplitude * Cos(phase + frequency * T);
        }

        // Calculate each coordinate variable.
        double longitude_rad = WrapRadians(Polynomials.EvaluatePolynomial(coeffs[1], T));
        double latitude_rad = WrapRadians(Polynomials.EvaluatePolynomial(coeffs[2], T));
        double radius_AU = Polynomials.EvaluatePolynomial(coeffs[3], T);
        return new Coordinates(longitude_rad, latitude_rad, radius_AU);
    }

    /// <summary>
    /// Calculate the position of a planet in heliocentric ecliptic coordinates.
    /// </summary>
    /// <param name="planetName">The planet's name.</param>
    /// <param name="jdtt">The Julian Ephemeris Day.</param>
    /// <returns>
    /// The planet's position in heliocentric ecliptic coordinates (radians).
    /// </returns>
    /// <exception cref="DataNotFoundException">
    /// If the planet couldn't be found in the database or no VSOP87D data could be found for the
    /// planet.
    /// </exception>
    public Coordinates CalcPlanetPosition(string planetName, double jdtt)
    {
        // Load the planet.
        AstroObjectRecord planet = astroObjectRepository.LoadByName(planetName, "Planet");

        // Call the other method.
        return CalcPlanetPosition(planet, jdtt);
    }
}
