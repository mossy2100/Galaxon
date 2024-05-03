using System.Text;

namespace Galaxon.Quantities;

public class CompoundUnit : List<Unit>
{
    /// <summary>
    /// Convert the compound unit to a string.
    /// Two format codes are supportedL
    ///     A = ASCII. This will use '^' to indicate an exponent, and '*' for multiply.
    ///     U = Unicode. This will use superscript characters to indicate an exponent, and '⋅' for multiply.
    /// </summary>
    /// <param name="format">The format code.</param>
    /// <returns>The Unit as a string.</returns>
    public string ToString(string format)
    {
        // Guard.
        if (format != "A" && format != "U")
        {
            throw new ArgumentOutOfRangeException(nameof(format),
                "Must be 'A' for ASCII or 'U' for Unicode.");
        }

        StringBuilder sb = new ();

        // Append the units. They should already be tidy.
        for (int i = 0; i < Count; i++)
        {
            Unit unit = this[i];

            string strOperator;
            string strUnit;
            string strSymbol = unit.BaseUnit.Symbol;

            if (unit.Exponent < 0)
            {
                strOperator = "/";
                strUnit = new Unit(unit.BaseUnit, unit.Prefix, -unit.Exponent).ToString(format);
            }
            else
            {
                // For units with positive exponents, put a space before the first one, or a
                // multiply sign before the others. If niceFormat = true, the multiply sign will be
                // a bullet, otherwise it will be a period.
                // We don't want spaces before degree, arcminute, or arcsecond symbols.
                strOperator = i == 0
                    ? (strSymbol[0] == '°' || strSymbol[0] == '′' || strSymbol[0] == '″' ? "" : " ")
                    : (format == "U" ? "⋅" : ".");
                strUnit = unit.ToString(format);
            }

            sb.Append($"{strOperator}{strUnit}");
        }

        return sb.ToString();
    }

    /// <inheritdoc />
    public override string ToString()
    {
        return ToString("U");
    }
}
