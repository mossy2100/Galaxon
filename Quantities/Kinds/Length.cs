namespace Galaxon.Quantities.Kinds;

public static class Length
{
    /// <summary>
    /// Number of metres per Astronomical Unit.
    /// <see href="https://en.wikipedia.org/wiki/Astronomical_unit"/>
    /// </summary>
    public const long METRES_PER_ASTRONOMICAL_UNIT = 149_597_870_700;

    /// <summary>
    /// Number of metres per light year.
    /// <see href="https://en.wikipedia.org/wiki/Light-year"/>
    /// </summary>
    public const long METRES_PER_LIGHT_YEAR = 9_460_730_472_580_800;

    /// <summary>
    /// Number of kilometres per light year.
    /// </summary>
    public const double KM_PER_LIGHT_YEAR = METRES_PER_LIGHT_YEAR / 1000.0;

    /// <summary>
    /// Number of metres per parsec.
    /// <see href="https://en.wikipedia.org/wiki/Parsec"/>
    /// </summary>
    public const double METRES_PER_PARSEC = 3.0857e16;
}
