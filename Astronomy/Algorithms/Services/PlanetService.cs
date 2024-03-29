using Galaxon.Astronomy.Algorithms.Records;
using Galaxon.Astronomy.Data;
using Galaxon.Astronomy.Data.Models;
using Galaxon.Core.Collections;
using Galaxon.Core.Exceptions;
using Galaxon.Numerics.Algebra;
using Galaxon.Numerics.Geometry;
using Galaxon.Quantities;

namespace Galaxon.Astronomy.Algorithms.Services;

public class PlanetService(AstroDbContext astroDbContext)
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
        { 8, "Neptune" },
    };

    /// <summary>
    /// Calculate the position of a planet in heliocentric ecliptic coordinates.
    /// Algorithm is from AA2 p218.
    /// The result is a Coordinates object tuple with the 3 coordinate values as radians.
    ///     Longitude = the heliocentric longitude in radians, in range -PI..PI
    ///     Latitude = the heliocentric latitude in radians, in range -PI/2..PI/2
    ///     Radius = the orbital radius in metres.
    /// <see href="https://www.caglow.com/info/compute/vsop87"/>
    /// Original data files are from:
    /// <see href="ftp://ftp.imcce.fr/pub/ephem/planets/vsop87"/>
    /// </summary>
    /// <param name="planet">The planet.</param>
    /// <param name="JDTT">The Julian Ephemeris Day.</param>
    /// <returns>
    /// The planet's position in heliocentric ecliptic coordinates (radians).
    /// </returns>
    /// <exception cref="DataNotFoundException">
    /// If no VSOP87D data could be found for the planet.
    /// </exception>
    public Coordinates CalcPlanetPosition(AstroObject planet, double JDTT)
    {
        // Get the VSOP87D data for the planet from the database.
        // These aren't included in Load() so I may need to get them separately
        // rather than via the VSOP87DRecords property.
        var records = astroDbContext.VSOP87DRecords
            .Where(r => r.AstroObjectId == planet.Id)
            .ToList();

        // Check there are records.
        if (records.IsEmpty())
        {
            throw new DataNotFoundException($"No VSOP87D data found for planet {planet.Name}.");
        }

        // Get T in Julian millennia from the epoch J2000.0.
        double T = JulianDateService.JulianMillenniaSinceJ2000(JDTT);

        // Calculate the coefficients for each coordinate variable.
        Dictionary<char, double[]> coeffs = new ();
        foreach (VSOP87DRecord record in records)
        {
            if (!coeffs.ContainsKey(record.Variable))
            {
                coeffs[record.Variable] = new double[6];
            }
            double amplitude = record.Amplitude;
            double phase = record.Phase;
            double frequency = record.Frequency;
            coeffs[record.Variable][record.Exponent] += amplitude * Cos(phase + frequency * T);
        }

        // Calculate each coordinate variable.
        double L = Angles.WrapRadians(Polynomials.EvaluatePolynomial(coeffs['L'], T));
        double B = Angles.WrapRadians(Polynomials.EvaluatePolynomial(coeffs['B'], T));
        double R = Polynomials.EvaluatePolynomial(coeffs['R'], T)
            * LengthConstants.METRES_PER_ASTRONOMICAL_UNIT;
        return new Coordinates(L, B, R);
    }
}
