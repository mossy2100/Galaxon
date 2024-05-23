using Galaxon.Core.Exceptions;
using Galaxon.Numerics.Algebra;

namespace Galaxon.Astronomy.Algorithms.Utilities;

public static class DeltaTUtility
{
    /// <summary>
    /// Calculate ∆T in seconds using NASA's equations.
    /// Accepts a decimal year.
    ///
    /// ∆T is the difference between Terrestrial Time (TT; formerly known as Terrestrial Dynamical
    /// Time, TDT, or Dynamical Time, TD) and Universal Time (UT1; also commonly referred to simply
    /// as UT).
    /// Thus: ∆T = TT - UT1
    ///
    /// Equations in this method were copied from
    /// <see href="https://eclipse.gsfc.nasa.gov/SEcat5/deltatpoly.html"/>
    ///
    /// These appear to mostly agree with
    /// <see href="https://eclipsewise.com/help/deltatpoly2014.html"/>
    ///
    /// These are intended to be a simpler method for calculating ∆T than using tables as in Meeus
    /// AA2 and other sources. Estimates of ∆T are assumed to be reasonably accurate in the range
    /// 1620..2100, but since ∆T varies unpredictably, uncertainty in ∆T increases outside of this
    /// range.
    /// </summary>
    /// <seealso href="https://eclipse.gsfc.nasa.gov/SEcat5/uncertainty.html"/>
    /// <seealso href="https://maia.usno.navy.mil/products/deltaT"/>
    /// <seealso href="https://asa.hmnao.com/SecK/DeltaT.html"/>
    /// <seealso href="https://www.hermetic.ch/cal_stud/meeus1.htm"/>
    /// <param name="y">The year as a decimal value.</param>
    /// <returns>The calculated value for ∆T.</returns>
    public static double CalcDeltaT(double y)
    {
        double deltaT;

        // Get the year as an integer.
        int year = (int)Floor(y);

        // Calculate deltaT.
        switch (year)
        {
            // Polynomial representing long-term parabolic trend.
            case < -500:
            case > 2150:
                deltaT = Polynomials.EvaluatePolynomial([
                    -20,
                    0,
                    32
                ], (y - 1820) / 100);
                break;

            case >= -500 and <= 500:
                deltaT = Polynomials.EvaluatePolynomial([
                    10583.6,
                    -1014.41,
                    33.78311,
                    -5.952053,
                    -0.1798452,
                    0.022174192,
                    0.0090316521
                ], y / 100);
                break;

            case > 500 and <= 1600:
                deltaT = Polynomials.EvaluatePolynomial([
                    1574.2,
                    -556.01,
                    71.23472,
                    0.319781,
                    -0.8503463,
                    -0.005050998,
                    0.0083572073
                ], (y - 1000) / 100);
                break;

            case > 1600 and <= 1700:
                deltaT = Polynomials.EvaluatePolynomial([
                    120,
                    -0.9808,
                    -0.01532,
                    1.0 / 7129
                ], y - 1600);
                break;

            case > 1700 and <= 1800:
                deltaT = Polynomials.EvaluatePolynomial([
                    8.83,
                    0.1603,
                    -0.0059285,
                    0.00013336,
                    -1.0 / 1174000
                ], y - 1700);
                break;

            case > 1800 and <= 1860:
                deltaT = Polynomials.EvaluatePolynomial([
                    13.72,
                    -0.332447,
                    0.0068612,
                    0.0041116,
                    -0.00037436,
                    0.0000121272,
                    -0.0000001699,
                    0.000000000875
                ], y - 1800);
                break;

            case > 1860 and <= 1900:
                deltaT = Polynomials.EvaluatePolynomial([
                    7.62,
                    0.5737,
                    -0.251754,
                    0.01680668,
                    -0.0004473624,
                    1.0 / 233174
                ], y - 1860);
                break;

            case > 1900 and <= 1920:
                deltaT = Polynomials.EvaluatePolynomial([
                    -2.79,
                    1.494119,
                    -0.0598939,
                    0.0061966,
                    -0.000197
                ], y - 1900);
                break;

            case > 1920 and <= 1941:
                deltaT = Polynomials.EvaluatePolynomial([
                    21.20,
                    0.84493,
                    -0.0761,
                    0.0020936
                ], y - 1920);
                break;

            case > 1941 and <= 1961:
                deltaT = Polynomials.EvaluatePolynomial([
                    29.07,
                    0.407,
                    -1.0 / 233,
                    1.0 / 2547
                ], y - 1950);
                break;

            case > 1961 and <= 1986:
                deltaT = Polynomials.EvaluatePolynomial([
                    45.45,
                    1.067,
                    -1.0 / 260,
                    -1.0 / 718
                ], y - 1975);
                break;

            case > 1986 and <= 2005:
                deltaT = Polynomials.EvaluatePolynomial([
                    63.86,
                    0.3345,
                    -0.060374,
                    0.0017275,
                    0.000651814,
                    0.00002373599
                ], y - 2000);
                break;

            case > 2005 and <= 2050:
                deltaT = Polynomials.EvaluatePolynomial([
                    62.92,
                    0.32217,
                    0.005589
                ], y - 2000);
                break;

            case > 2050 and <= 2150:
                double u = (y - 1820) / 100;
                deltaT = -20 + 32 * u * u - 0.5628 * (2150 - y);
                break;
        }

        // Apply the lunar ephemeris correction for years outside the range 1955..2005.
        if (year is < 1955 or > 2005)
        {
            double t = y - 1955;
            deltaT -= 0.000012932 * t * t;
        }

        return deltaT;
    }

    /// <summary>
    /// Calculate the value for ∆T in seconds at a given Gregorian datetime.
    /// Defaults to the current date.
    /// ∆T = TT - UT1
    /// </summary>
    /// <param name="dt">A date.</param>
    /// <returns>Delta-T at that point in time.</returns>
    public static double CalcDeltaT(DateTime dt = new ())
    {
        return CalcDeltaT(TimeScalesUtility.DateTimeToDecimalYear(dt));
    }

    /// <summary>
    /// Calculate the value for ∆T in seconds for a given Gregorian year and month.
    /// </summary>
    /// <param name="year">The year.</param>
    /// <param name="month">The month.</param>
    /// <returns>Delta-T at the midpoint of the month.</returns>
    public static double CalcDeltaT(int year, int month)
    {
        return CalcDeltaT(year + (month - 0.5) / 12);
    }


    #region Meeus Delta-T methods
    //----------------------------------------------------------------------------------------------
    // This stuff can be removed later if I don't need it. Although, it should be kept somewhere.

    /// <summary>
    /// Copy of Table 10A in Astronomical Algorithms 2nd Ed. by Jean Meeus.
    /// </summary>
    private static readonly Dictionary<int, double> _DeltaTData = new ()
    {
        { 1620, 121.0 }, { 1622, 112.0 }, { 1624, 103.0 }, { 1626, 95.0 }, { 1628, 88.0 },
        { 1630, 82.0 }, { 1632, 77.0 }, { 1634, 72.0 }, { 1636, 68.0 }, { 1638, 63.0 },
        { 1640, 60.0 }, { 1642, 56.0 }, { 1644, 53.0 }, { 1646, 51.0 }, { 1648, 48.0 },
        { 1650, 46.0 }, { 1652, 44.0 }, { 1654, 42.0 }, { 1656, 40.0 }, { 1658, 38.0 },
        { 1660, 35.0 }, { 1662, 33.0 }, { 1664, 31.0 }, { 1666, 29.0 }, { 1668, 26.0 },
        { 1670, 24.0 }, { 1672, 22.0 }, { 1674, 20.0 }, { 1676, 18.0 }, { 1678, 16.0 },
        { 1680, 14.0 }, { 1682, 12.0 }, { 1684, 11.0 }, { 1686, 10.0 }, { 1688, 9.0 },
        { 1690, 8.0 }, { 1692, 7.0 }, { 1694, 7.0 }, { 1696, 7.0 }, { 1698, 7.0 },
        { 1700, 7.0 }, { 1702, 7.0 }, { 1704, 8.0 }, { 1706, 8.0 }, { 1708, 9.0 },
        { 1710, 9.0 }, { 1712, 9.0 }, { 1714, 9.0 }, { 1716, 9.0 }, { 1718, 10.0 },
        { 1720, 10.0 }, { 1722, 10.0 }, { 1724, 10.0 }, { 1726, 10.0 }, { 1728, 10.0 },
        { 1730, 10.0 }, { 1732, 10.0 }, { 1734, 11.0 }, { 1736, 11.0 }, { 1738, 11.0 },
        { 1740, 11.0 }, { 1742, 11.0 }, { 1744, 12.0 }, { 1746, 12.0 }, { 1748, 12.0 },
        { 1750, 12.0 }, { 1752, 13.0 }, { 1754, 13.0 }, { 1756, 13.0 }, { 1758, 14.0 },
        { 1760, 14.0 }, { 1762, 14.0 }, { 1764, 14.0 }, { 1766, 15.0 }, { 1768, 15.0 },
        { 1770, 15.0 }, { 1772, 15.0 }, { 1774, 15.0 }, { 1776, 16.0 }, { 1778, 16.0 },
        { 1780, 16.0 }, { 1782, 16.0 }, { 1784, 16.0 }, { 1786, 16.0 }, { 1788, 16.0 },
        { 1790, 16.0 }, { 1792, 15.0 }, { 1794, 15.0 }, { 1796, 14.0 }, { 1798, 13.0 },
        { 1800, 13.1 }, { 1802, 12.5 }, { 1804, 12.2 }, { 1806, 12.0 }, { 1808, 12.0 },
        { 1810, 12.0 }, { 1812, 12.0 }, { 1814, 12.0 }, { 1816, 12.0 }, { 1818, 11.9 },
        { 1820, 11.6 }, { 1822, 11.0 }, { 1824, 10.2 }, { 1826, 9.2 }, { 1828, 8.2 },
        { 1830, 7.1 }, { 1832, 6.2 }, { 1834, 5.6 }, { 1836, 5.4 }, { 1838, 5.3 },
        { 1840, 5.4 }, { 1842, 5.6 }, { 1844, 5.9 }, { 1846, 6.2 }, { 1848, 6.5 },
        { 1850, 6.8 }, { 1852, 7.1 }, { 1854, 7.3 }, { 1856, 7.5 }, { 1858, 7.6 },
        { 1860, 7.7 }, { 1862, 7.3 }, { 1864, 6.2 }, { 1866, 5.2 }, { 1868, 2.7 },
        { 1870, 1.4 }, { 1872, -1.2 }, { 1874, -2.8 }, { 1876, -3.8 }, { 1878, -4.8 },
        { 1880, -5.5 }, { 1882, -5.3 }, { 1884, -5.6 }, { 1886, -5.7 }, { 1888, -5.9 },
        { 1890, -6.0 }, { 1892, -6.3 }, { 1894, -6.5 }, { 1896, -6.2 }, { 1898, -4.7 },
        { 1900, -2.8 }, { 1902, -0.1 }, { 1904, 2.6 }, { 1906, 5.3 }, { 1908, 7.7 },
        { 1910, 10.4 }, { 1912, 13.3 }, { 1914, 16.0 }, { 1916, 18.2 }, { 1918, 20.2 },
        { 1920, 21.1 }, { 1922, 22.4 }, { 1924, 23.5 }, { 1926, 23.8 }, { 1928, 24.3 },
        { 1930, 24.0 }, { 1932, 23.9 }, { 1934, 23.9 }, { 1936, 23.7 }, { 1938, 24.0 },
        { 1940, 24.3 }, { 1942, 25.3 }, { 1944, 26.2 }, { 1946, 27.3 }, { 1948, 28.2 },
        { 1950, 29.1 }, { 1952, 30.0 }, { 1954, 30.7 }, { 1956, 31.4 }, { 1958, 32.2 },
        { 1960, 33.1 }, { 1962, 34.0 }, { 1964, 35.0 }, { 1966, 36.5 }, { 1968, 38.3 },
        { 1970, 40.2 }, { 1972, 42.2 }, { 1974, 44.5 }, { 1976, 46.5 }, { 1978, 48.5 },
        { 1980, 50.5 }, { 1982, 52.2 }, { 1984, 53.8 }, { 1986, 54.9 }, { 1988, 55.8 },
        { 1990, 56.9 }, { 1992, 58.3 }, { 1994, 60.0 }, { 1996, 61.6 }, { 1998, 63.0 }
    };

    /// <summary>
    /// Calculate ∆T for a given year, month, or date using the method from Astronomical Algorithms
    /// 2nd ed. (AA2) by Jean Meeus, pp77-80.
    /// This pretty closely tracks the NASA values for the range given in the table (1620-1998) but
    /// diverges significantly before and after that. I implemented this algorithm to compare the
    /// two, but I expect the NASA version is superior.
    /// </summary>
    /// <param name="y">The year as a floating point value.</param>
    /// <returns>The calculated value for ∆T.</returns>
    /// <exception cref="DataNotFoundException">
    /// If a ∆T entry expected to be found in the database table could not be found.
    /// </exception>
    public static double CalcDeltaTMeeus(double y)
    {
        // Get the year as an int.
        var year = (int)Floor(y);

        // Calculate deltaT.
        double deltaT;
        double t = (y - 2000.0) / 100.0;

        switch (year)
        {
            case < 948:
                deltaT = Polynomials.EvaluatePolynomial([2177, 497, 44.1], t);
                break;

            case >= 948 and < 1620:
            case >= 2000:
                deltaT = Polynomials.EvaluatePolynomial([102, 102, 25.3], t);
                if (y is >= 2000 and <= 2100)
                {
                    deltaT += 0.37 * (y - 2100);
                }
                break;

            case >= 1620 and < 2000:
                // Get the value from the lookup table for the even years before and after, and
                // interpolate.
                var year1 = (int)(Floor(y / 2) * 2);
                int year2 = year1 + 2;
                double deltaT1 = _DeltaTData[year1];
                double deltaT2 = year2 == 2000 ? CalcDeltaTMeeus(year2) : _DeltaTData[year2];
                deltaT = deltaT1 + (deltaT2 - deltaT1) * (y - year1) / (year2 - year1);
                break;
        }

        return deltaT;
    }

    #endregion Meeus Delta-T methods
}
