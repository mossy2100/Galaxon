namespace Bases;

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
    /// A method of writing times using h, m, and s to indicate hours, minutes, and seconds.
    /// The hours part can be any integer, not limited to the range of a sexagesimal value.
    /// The minutes part will be a sexagesimal digit (0-59).
    /// The seconds part can be a sexagesimal digit or a decimal value in the range 0-59.999...
    /// e.g. 123h 45m 6.789s
    /// </summary>
    TimeUnits
}
