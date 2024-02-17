using System.Numerics;
using Galaxon.Core.Strings;

namespace Galaxon.Core.Numbers;

/// <summary>
/// Extension methods for IBinaryInteger{T}.
/// </summary>
public static class XBinaryInteger
{
    #region Superscript and subscript

    /// <summary>
    /// Map from normal integer characters to their superscript versions.
    /// </summary>
    public static readonly Dictionary<char, string> SuperscriptChars = new ()
    {
        { '0', "⁰" },
        { '1', "¹" },
        { '2', "²" },
        { '3', "³" },
        { '4', "⁴" },
        { '5', "⁵" },
        { '6', "⁶" },
        { '7', "⁷" },
        { '8', "⁸" },
        { '9', "⁹" },
        { '-', "⁻" }
    };

    /// <summary>
    /// Map from normal integer characters to their subscript versions.
    /// </summary>
    public static readonly Dictionary<char, string> SubscriptChars = new ()
    {
        { '0', "₀" },
        { '1', "₁" },
        { '2', "₂" },
        { '3', "₃" },
        { '4', "₄" },
        { '5', "₅" },
        { '6', "₆" },
        { '7', "₇" },
        { '8', "₈" },
        { '9', "₉" },
        { '-', "₋" }
    };

    /// <summary>
    /// Render an integer as a superscript string.
    /// This is useful for formatting exponents, numerators, and other numeric superscripts when
    /// HTML is unavailable.
    /// See <see href="https://en.wikipedia.org/wiki/Unicode_subscripts_and_superscripts"/>
    /// See <see href="https://rupertshepherd.info/resource_pages/superscript-letters-in-unicode"/>
    /// See <see href="https://unicode-table.com/en/"/>
    /// </summary>
    /// <param name="n">The integer.</param>
    /// <returns>The string of superscript characters.</returns>
    public static string ToSuperscript<T>(this T n) where T : IBinaryInteger<T>
    {
        return $"{n}".ToSuperscript();
    }

    /// <summary>
    /// Render an integer as a subscript string.
    /// This is useful for formatting denominators, the number of atoms in a molecule, atomic
    /// numbers, and other numeric subscripts when HTML is unavailable.
    /// </summary>
    /// <param name="n">The integer.</param>
    /// <returns>The string of subscript characters.</returns>
    public static string ToSubscript<T>(this T n) where T : IBinaryInteger<T>
    {
        return $"{n}".ToSubscript();
    }

    #endregion Superscript and subscript
}
