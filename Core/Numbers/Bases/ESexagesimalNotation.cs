namespace Galaxon.Core.Numbers.Bases;

/// <summary>
/// Enum for the different notations used to represent sexagesimal numbers.
/// </summary>
public enum ESexagesimalNotation
{
    /// <summary>
    /// A method of writing sexagesimal numbers using degrees, arcminutes, and arcseconds notation,
    /// which uses the degree character and prime characters.
    /// The degrees part can be any integer, not limited to the range of a sexagesimal value.
    /// The arcminutes part will be a sexagesimal digit (0-59).
    /// The arcseconds part can be a sexagesimal digit or a decimal value in the range 0-59.999...
    /// e.g. 123°15′6.789″
    /// </summary>
    Angle,

    /// <summary>
    /// A method of writing sexagesimal numbers using colons to separate digits.
    /// The hours part can be any integer, not limited to the range of a sexagesimal value.
    /// The minutes part will be a sexagesimal digit (0-59).
    /// The seconds part can be a sexagesimal digit or a decimal value in the range 0-59.999 (etc.).
    /// The minutes and seconds parts are left-padded with a 0 to make at least 2 digits, if
    /// necessary.
    /// e.g. 123:45:06.789
    /// </summary>
    Colons,

    /// <summary>
    /// A method of writing sexagesimal numbers using h, m, and s to indicate hours, minutes, and
    /// seconds.
    /// The hours part can be any integer, not limited to the range of a sexagesimal value.
    /// The minutes part will be a sexagesimal digit (0-59).
    /// The seconds part can be a sexagesimal digit or a decimal value in the range 0-59.999...
    /// e.g. 123h45m6.789s
    /// </summary>
    TimeUnits,

    /// <summary>
    /// Neugebauer notation. A method of writing sexagesimal numbers using semicolons and commas.
    /// <see href="https://en.wikipedia.org/wiki/Sexagesimal#Notations"/>
    /// The semicolon separates the integer and fractional part. Commas separate digits before and
    /// after the semicolon.
    /// Each sexagesimal digit is represented as decimal value in the range 0-59.
    /// e.g. 29;31,50,8,20
    /// </summary>
    Neugebauer,

    /// <summary>
    /// A method of writing sexagesimal numbers using minutes, seconds, thirds, and fourths
    /// notation, which uses the degree character and prime characters.
    /// Each sexagesimal digit is represented as decimal value in the range 0-59.
    /// It's similar to Angle except all digits are limited to sexagesimal range.
    /// e.g. 36‷25‶15‵1°15′2″36‴49⁗
    /// </summary>
    Primes
}
