using Galaxon.Core.Collections;
using Galaxon.Core.Exceptions;
using Galaxon.Numerics.Geometry;
using Galaxon.Quantities.Kinds;
using Galaxon.Time;

namespace Galaxon.Quantities;

public class BaseUnit
{
    #region Properties

    /// <summary>
    /// The unit's symbol, e.g. "m", "s", "rad", "°F", etc.
    /// </summary>
    public string Symbol { get; set; }

    /// <summary>
    /// The unit's name, e.g. "metre", "second", "radian", "degree Fahrenheit", etc.
    /// </summary>
    public string Name { get; set; }

    // The physical amount this unit measures, e.g. length, area, mass, temperature, etc.
    // Not convinced I need this yet. It would mainly be necessary for ensuring compatibility
    // when adding units, but this can be determined by converting both to SI base units.
    // public EUnitKind Kind { get; set; }

    /// <summary>
    /// Set of multiplier prefixes that are valid for this unit.
    /// </summary>
    public List<UnitPrefix>? ValidPrefixes { get; set; }

    /// <summary>
    /// This unit expressed in equivalent metric units.
    /// For example:
    ///     1 mile = 1609.34 meters
    ///     1 hour = 3600 seconds
    ///     1 kcal = 4184 joules
    /// etc.
    /// Must be null for SI base units.
    /// </summary>
    public Quantity? MetricEquivalent { get; set; }

    /// <summary>
    /// Method for non-standard conversion to metric, such as converting from Celsius to Kelvin.
    /// </summary>
    public Func<double, double>? ToMetric { get; set; }

    /// <summary>
    /// Method for non-standard conversion from metric, such as converting from Kelvin to Celsius.
    /// </summary>
    public Func<double, double>? FromMetric { get; set; }

    /// <summary>
    /// A unique integer for ordering units.
    /// </summary>
    public int Order { get; set; }

    #endregion Properties

    #region Constructors

    public BaseUnit(string symbol = "", string name = "", List<UnitPrefix>? validPrefixes = null,
        double? metricAmount = null, string? metricUnitsString = null,
        Func<double, double>? toMetric = null, Func<double, double>? fromMetric = null)
    {
        Symbol = symbol;
        Name = name;
        ValidPrefixes = validPrefixes;
        MetricEquivalent = metricAmount != null && metricUnitsString != null
            ? new Quantity(metricAmount.Value, metricUnitsString)
            : null;
        ToMetric = toMetric;
        FromMetric = fromMetric;
    }

    #endregion Constructors

    #region Known units

    /// <summary>
    /// Internal collection of all known units.
    /// </summary>
    private static readonly List<BaseUnit> _AllKnown = new ();

    /// <summary>
    /// Get all the known units.
    /// </summary>
    /// <returns></returns>
    public static List<BaseUnit> AllKnown
    {
        get
        {
            if (_AllKnown.Count == 0)
            {
                InitializeKnown();
            }
            return _AllKnown;
        }
    }

    /// <summary>
    /// Add a new base unit to the internal collection.
    /// </summary>
    /// <param name="baseUnit"></param>
    public static void Add(BaseUnit baseUnit)
    {
        // Check we don't already have one with this symbol.
        if (Get(baseUnit.Symbol) != null)
        {
            throw new ArgumentInvalidException(nameof(baseUnit),
                $"There is already a unit with the symbol '{baseUnit.Symbol}' in the set of known units.");
        }

        // Get the next order number and add the new unit to the collection.
        int maxOrder = _AllKnown.IsEmpty() ? 0 : _AllKnown.Select(bu => bu.Order).Max();
        baseUnit.Order = maxOrder + 1;
        _AllKnown.Add(baseUnit);
    }

    /// <summary>
    /// Add the SI base units to the internal collection.
    /// Even though kilograms are the SI base unit, grams are used here instead of "kg" because of
    /// how prefixes are handled. Grams are converted back to kg in Quantity.ToMetric().
    /// <see href="https://en.wikipedia.org/wiki/International_System_of_Units"/>
    /// <see href="https://en.wikipedia.org/wiki/SI_base_unit"/>
    /// These have to be in a specific order.
    /// <see href="https://en.wikipedia.org/wiki/International_System_of_Quantities#Derived_quantities"/>
    /// </summary>
    private static void AddSiBaseUnits()
    {
        Add(new BaseUnit("m", "meter", UnitPrefix.Metric));
        Add(new BaseUnit("g", "gram", UnitPrefix.Metric));
        Add(new BaseUnit("s", "second", UnitPrefix.Metric));
        Add(new BaseUnit("A", "ampere", UnitPrefix.Metric));
        Add(new BaseUnit("K", "kelvin", UnitPrefix.Metric));
        Add(new BaseUnit("mol", "mole", UnitPrefix.Metric));
        Add(new BaseUnit("cd", "candela", UnitPrefix.Metric));
    }

    /// <summary>
    /// Add SI derived units to the internal collection.
    /// <see href="https://en.wikipedia.org/wiki/SI_derived_unit"/>
    /// </summary>
    public static void AddSiDerivedUnits()
    {
        Add(new BaseUnit("Hz", "hertz", UnitPrefix.Metric, 1, "/s"));
        Add(new BaseUnit("rad", "radian"));
        Add(new BaseUnit("sr", "steradian"));
        Add(new BaseUnit("N", "newton", UnitPrefix.Metric, 1, "kg.m/s2"));
        Add(new BaseUnit("Pa", "pascal", UnitPrefix.Metric, 1, "kg/m/s2"));
        Add(new BaseUnit("J", "joule", UnitPrefix.Metric, 1, "kg.m2/s2"));
        Add(new BaseUnit("W", "watt", UnitPrefix.Metric, 1, "kg.m2/s3"));
        Add(new BaseUnit("C", "coulomb", UnitPrefix.Metric, 1, "s.A"));
        Add(new BaseUnit("V", "volt", UnitPrefix.Metric, 1, "kg.m2/s3/A"));
        Add(new BaseUnit("F", "farad", UnitPrefix.Metric, 1, "s4.A2/kg/m2"));
        Add(new BaseUnit("Ω", "ohm", UnitPrefix.Metric, 1, "kg.m2/s3/A2"));
        Add(new BaseUnit("S", "siemens", UnitPrefix.Metric, 1, "s3.A2/kg/m2"));
        Add(new BaseUnit("Wb", "weber", UnitPrefix.Metric, 1, "kg.m2/s2/A"));
        Add(new BaseUnit("T", "tesla", UnitPrefix.Metric, 1, "kg/s2/A"));
        Add(new BaseUnit("H", "henry", UnitPrefix.Metric, 1, "kg.m2/s2/A2"));
        Add(new BaseUnit("°C", "Celsius", null, 1, "K", Temperature.CelsiusToKelvin,
            Temperature.KelvinToCelsius));
        Add(new BaseUnit("lm", "lumen", UnitPrefix.Metric, 1, "cd"));
        Add(new BaseUnit("lx", "lux", UnitPrefix.Metric, 1, "cd/m2"));
        Add(new BaseUnit("Bq", "becquerel", UnitPrefix.Metric, 1, "s-1"));
        Add(new BaseUnit("Gy", "gray", UnitPrefix.Metric, 1, "m2/s2"));
        Add(new BaseUnit("Sv", "sievert", UnitPrefix.Metric, 1, "m2/s2"));
        Add(new BaseUnit("kat", "katal", UnitPrefix.Metric, 1, "mol/s"));
    }

    /// <summary>
    /// Add support for non-SI units accepted for use with SI (plus a few more).
    /// Most come from the Wikipedia article but a few extra have been added.
    /// <see href="https://en.wikipedia.org/wiki/International_System_of_Units#Non-SI_units_accepted_for_use_with_SI"/>
    /// <see href="https://en.wikipedia.org/wiki/Unit_of_time"/>
    /// </summary>
    public static void AddSiAcceptedUnits()
    {
        // Time.
        Add(new BaseUnit("min", "minute", null, TimeConstants.SECONDS_PER_MINUTE, "s"));
        Add(new BaseUnit("h", "hour", null, TimeConstants.SECONDS_PER_HOUR, "s"));
        Add(new BaseUnit("d", "day", null, TimeConstants.SECONDS_PER_DAY, "s"));

        // Length.
        Add(new BaseUnit("AU", "astronomical unit", null, Length.METRES_PER_ASTRONOMICAL_UNIT,
            "m"));

        // Angles.
        Add(new BaseUnit("°", "degree", null, RADIANS_PER_DEGREE, "rad"));
        Add(new BaseUnit("′", "arcminute", null, RADIANS_PER_ARCMINUTE, "rad"));
        Add(new BaseUnit("″", "arcsecond", null, RADIANS_PER_ARCSECOND, "rad"));

        // Area.
        Add(new BaseUnit("ha", "hectare", null, 10_000, "m2"));

        // Volume.
        // Add(new BaseUnit("l", "liter", UnitPrefix.Metric, 0.001, "m3"));
        Add(new BaseUnit("L", "liter", UnitPrefix.Metric, 0.001, "m3"));

        // Mass.
        Add(new BaseUnit("t", "tonne", UnitPrefix.LargeMetric, 1000, "kg"));
        Add(new BaseUnit("Da", "dalton", UnitPrefix.LargeMetric, 1.660_539_040e-27, "kg"));

        // Energy.
        Add(new BaseUnit("eV", "electron volt", UnitPrefix.Metric, 1.602_176_634e-19, "J"));
    }

    /// <summary>
    /// Some common units that don't fit into the other groups.
    /// Re metric prefixes and years:
    /// <see href="https://en.wikipedia.org/wiki/Kyr"/>
    /// <see href="https://en.wikipedia.org/wiki/Myr"/>
    /// <see href="https://en.wikipedia.org/wiki/Billion_years"/>
    /// <see href="https://en.wikipedia.org/wiki/Year#Abbreviations_yr_and_ya"/>
    /// </summary>
    public static void AddCommonUnits()
    {
        // Time.
        Add(new BaseUnit("w", "week", null, 604_800, "s"));
        // Calculated from (365.2425 d) * (86400 s/d) / (12 mon/y).
        Add(new BaseUnit("mon", "month", null, 2_629_746, "s"));

        // The value given here for converting a year to seconds is based on a Gregorian calendar
        // year of 365.2425 d * 86400 s/d.
        // SI specifies the symbol "a" (annum) for year. However, it's also common to use "y" or "yr".
        Add(new BaseUnit("a", "year", UnitPrefix.LargeMetric, 31_556_952, "s"));
        // Add(new BaseUnit("yr", "year", UnitPrefix.GetMultiple("k,M,B,G"), 31_556_952, "s"));

        // Length.
        Add(new BaseUnit("ly", "light year", null, 9_460_730_472_580_800, "m"));
        Add(new BaseUnit("pc", "parsec", null, 3.085_677_581_491_3673e+16, "m"));

        // Energy.
        Add(new BaseUnit("cal", "small calorie", UnitPrefix.GetMultiple("k"), 4.184, "J"));
        // 1 Cal (upper-case 'C') == 1 kcal.
        // Add(new BaseUnit("Cal", "big calorie", null, 4184, "J"));

        // Pressure.
        Add(new BaseUnit("bar", "bar", UnitPrefix.Metric, 100_000, "Pa"));
        Add(new BaseUnit("atm", "atmosphere", null, 101_325, "Pa"));
    }

    /// <summary>
    /// Add support for most common imperial units.
    /// <see href="https://en.wikipedia.org/wiki/Imperial_units"/>
    /// </summary>
    public static void AddImperialUnits()
    {
        // Length.
        Add(new BaseUnit("in", "inch", null, 0.0254, "m"));
        Add(new BaseUnit("ft", "foot", null, 0.3048, "m"));
        Add(new BaseUnit("yd", "yard", null, 0.9144, "m"));
        Add(new BaseUnit("mi", "mile", null, 1609.344, "m"));

        // Area.
        Add(new BaseUnit("ac", "acre", null, 4046.856_4224, "m2"));

        // Volume.
        Add(new BaseUnit("pt", "imperial pint", null, 568.261_25, "mL"));
        Add(new BaseUnit("qt", "imperial quart", null, 1.136_5225, "L"));
        Add(new BaseUnit("gal", "imperial gallon", null, 4546.09, "L"));

        // Mass.
        Add(new BaseUnit("oz", "ounce", null, 28.349_523_125, "g"));
        Add(new BaseUnit("lb", "pound", null, 453.592_37, "g"));
        Add(new BaseUnit("st", "stone", null, 6.350_293_18, "kg"));
        Add(new BaseUnit("ton", "long ton", null, 1016.046_9088, "kg"));

        // Force.
        Add(new BaseUnit("lbf", "pound force", null, 4.448_221_615_2605, "N"));

        // Pressure.
        Add(new BaseUnit("psi", "pound force per square inch", null, 6894.757_293_168_36,
            "Pa"));
    }

    /// <summary>
    /// Add support for some of the most common US Customary units.
    /// <see href="https://en.wikipedia.org/wiki/United_States_customary_units"/>
    /// </summary>
    public static void AddUsCustomaryUnits()
    {
        // Volume.
        Add(new BaseUnit("USpt", "US liquid pint", null, 473.176_473, "mL"));
        Add(new BaseUnit("USqt", "US liquid quart", null, 946.352_946, "mL"));
        Add(new BaseUnit("USgal", "US liquid gallon", null, 3.785_411_784, "L"));

        // Mass.
        Add(new BaseUnit("tn", "short ton", null, 907_184.74, "kg"));

        // Temperature.
        Add(new BaseUnit("°F", "fahrenheit", null, 5.0 / 9, "K",
            Temperature.FahrenheitToKelvin, Temperature.KelvinToFahrenheit));
    }

    /// <summary>
    /// Add support for binary (data storage) units.
    /// </summary>
    public static void AddBinaryUnits()
    {
        List<UnitPrefix> prefixes = UnitPrefix.Combine(UnitPrefix.Binary, UnitPrefix.LargeMetric);
        Add(new BaseUnit("b", "bit", prefixes, 0.125, "B"));
        Add(new BaseUnit("B", "byte", prefixes));
    }

    /// <summary>
    /// Add support for most common currency units.
    /// May add more later if there's interest, possibly including currency codes.
    /// Prefixes must follow the number even when the currency symbol precedes the number.
    /// </summary>
    public static void AddCurrencyUnits()
    {
        List<UnitPrefix> prefixes = UnitPrefix.GetMultiple("k,K,M,B,T");
        Add(new BaseUnit("$", "dollar", prefixes));
        Add(new BaseUnit("€", "euro", prefixes));
        Add(new BaseUnit("£", "pound", prefixes));
        Add(new BaseUnit("¥", "yuan", prefixes));
        Add(new BaseUnit("₹", "rupee", prefixes));
        Add(new BaseUnit("₩", "won", prefixes));
        Add(new BaseUnit("฿", "baht", prefixes));
        Add(new BaseUnit("₽", "ruble", prefixes));
    }

    public const string CurrencySymbols = "$€£¥₹₩฿₽";

    /// <summary>
    /// Initialize the internal collection of known base units.
    /// </summary>
    private static void InitializeKnown()
    {
        if (_AllKnown.Count > 0)
        {
            _AllKnown.Clear();
        }
        AddSiBaseUnits();
        AddSiDerivedUnits();
        AddSiAcceptedUnits();
        AddCommonUnits();
        AddImperialUnits();
        AddUsCustomaryUnits();
        AddBinaryUnits();
        AddCurrencyUnits();
    }

    /// <summary>
    /// Look for clashes in the complete known symbol set.
    /// Ideally there will be none.
    /// </summary>
    /// <returns>A list of symbols that have clashes.</returns>
    public static IEnumerable<string> Clashes()
    {
        List<string> symbols = new ();
        List<string> clashes = new ();

        // Useful function.
        void addWithCheck(string prefixPlusSymbol)
        {
            if (symbols.Contains(prefixPlusSymbol))
            {
                clashes.Add(prefixPlusSymbol);
            }
            else
            {
                symbols.Add(prefixPlusSymbol);
            }
        }

        // Check all possible symbols for each unit.
        foreach (BaseUnit baseUnit in AllKnown)
        {
            addWithCheck(baseUnit.Symbol);
            if (baseUnit.ValidPrefixes == null)
            {
                continue;
            }
            foreach (UnitPrefix prefix in baseUnit.ValidPrefixes)
            {
                addWithCheck($"{prefix.Symbol}{baseUnit.Symbol}");
            }
        }

        return clashes;
    }

    #endregion Known units

    #region String methods

    /// <inheritdoc />
    public override string ToString()
    {
        return Symbol;
    }

    #endregion String methods.

    #region Miscellaneous methods

    /// <summary>
    /// Return the base unit corresponding to the given symbol.
    /// </summary>
    /// <param name="symbol"></param>
    /// <returns></returns>
    public static BaseUnit? Get(string symbol)
    {
        return AllKnown.FirstOrDefault(baseUnit => symbol == baseUnit.Symbol);
    }

    #endregion Miscellaneous methods
}
