﻿using Galaxon.Astronomy.Algorithms.Records;
using Galaxon.Astronomy.Algorithms.Utilities;
using Galaxon.Numerics.Algebra;

namespace Galaxon.Astronomy.Algorithms.Services;

public class SunService(PlanetService planetService)
{
    /// <summary>
    /// Calculation the variation in the Sun's longitude in radians.
    /// </summary>
    /// <param name="jdtt">The Julian Date in Terrestrial Time.</param>
    /// <returns></returns>
    public static double CalcVariationInSunLongitude(double jdtt)
    {
        double TM = JulianDateUtility.JulianMillenniaSinceJ2000(jdtt);
        double TM2 = TM * TM;

        double deltaLambdaInArcseconds = 3548.193
            + 118.568 * SinDegrees(87.5287 + 359_993.7286 * TM)
            + 2.476 * SinDegrees(85.0561 + 719_987.4571 * TM)
            + 1.376 * SinDegrees(27.8502 + 4452_671.1152 * TM)
            + 0.119 * SinDegrees(73.1375 + 450_368.8564 * TM)
            + 0.114 * SinDegrees(337.2264 + 329_644.6718 * TM)
            + 0.086 * SinDegrees(222.5400 + 659_289.3436 * TM)
            + 0.078 * SinDegrees(162.8136 + 9224_659.7915 * TM)
            + 0.054 * SinDegrees(82.5823 + 1079_981.1857 * TM)
            + 0.052 * SinDegrees(171.5189 + 225_184.4282 * TM)
            + 0.034 * SinDegrees(30.3214 + 4092_677.3866 * TM)
            + 0.033 * SinDegrees(119.8105 + 337_181.4711 * TM)
            + 0.023 * SinDegrees(247.5418 + 299_295.6151 * TM)
            + 0.023 * SinDegrees(325.1526 + 315_559.5560 * TM)
            + 0.021 * SinDegrees(155.1241 + 675_553.2846 * TM)
            + 7.311 * TM * SinDegrees(333.4515 + 359_993.7286 * TM)
            + 0.305 * TM * SinDegrees(330.9814 + 719_987.4571 * TM)
            + 0.010 * TM * SinDegrees(328.5170 + 1079_981.1857 * TM)
            + 0.309 * TM2 * SinDegrees(241.4518 + 359_993.7286 * TM)
            + 0.021 * TM2 * SinDegrees(205.0482 + 719_987.4571 * TM)
            + 0.004 * TM2 * SinDegrees(297.8610 + 4452_671.1152 * TM)
            + 0.010 * TM2 * SinDegrees(154.7066 + 359_993.7286 * TM);

        return deltaLambdaInArcseconds / ARCSECONDS_PER_RADIAN;
    }

    /// <summary>
    /// Calculate apparent solar latitude and longitude for a given instant specified as a Julian
    /// Date in Terrestrial Time (a.k.a. jdtt, a.k.a. Julian Ephemeris Day or JDE).
    /// This method uses the higher accuracy algorithm from AA2 Ch25 p166 (p174 in PDF)
    /// </summary>
    /// <param name="jdtt">The Julian Ephemeris Day.</param>
    /// <returns>The longitude of the Sun (Ls) in radians at the given
    /// instant.</returns>
    public Coordinates CalcPosition(double jdtt)
    {
        // Get the Earth's heliocentric position.
        (double earthLong_rad, double earthLat_rad, double earthDist_AU) =
            planetService.CalcPlanetPosition("Earth", jdtt);

        // Reverse to get the mean dynamical ecliptic and equinox of the date.
        double sunLong_rad = WrapRadians(earthLong_rad + PI);
        double sunLat_rad = WrapRadians(-earthLat_rad);
        double sunDist_AU = earthDist_AU;

        // Convert to FK5.
        // This gives the true ("geometric") longitude of the Sun referred to the mean equinox of
        // the date.
        double T = JulianDateUtility.JulianCenturiesSinceJ2000(jdtt);
        double lambdaPrime = Polynomials.EvaluatePolynomial(
            [sunLong_rad, -DegreesToRadians(1.397), -DegreesToRadians(0.000_31)], T);
        sunLong_rad -= DMSToRadians(0, 0, 0.090_33);
        sunLat_rad += DMSToRadians(0, 0, 0.039_16) * (Cos(lambdaPrime) - Sin(lambdaPrime));

        // The Sun's longitude obtained thus far is the true ("geometric") longitude of the Sun
        // referred to the mean equinox of the date.

        // Calculate and add the nutation in longitude.
        Nutation nutation = NutationService.CalcNutation(jdtt);
        sunLong_rad += nutation.Longitude;

        // Calculate and add the aberration.
        double deltaLambda_rad = CalcVariationInSunLongitude(jdtt);
        double aberration = -0.005_775_518 * sunDist_AU * deltaLambda_rad;
        sunLong_rad += aberration;

        // Make sure coordinates are in the standard range (signed).
        sunLong_rad = WrapRadians(sunLong_rad);
        sunLat_rad = WrapRadians(sunLat_rad);

        return new Coordinates(sunLong_rad, sunLat_rad, sunDist_AU);
    }
}
