namespace Galaxon.Core.Strings;

/// <summary>
/// May extend later with LowerSnake, LowerCamel, UpperSnake, UpperCamel, but I'd prefer to keep it
/// simple for now.
/// </summary>
public enum EStringCase
{
    /// <summary>
    /// No case. Applies to empty strings, or strings of whitespace, or strings not containing
    /// letters (e.g. numbers).
    /// </summary>
    None,

    /// <summary>
    /// Lower case.
    /// </summary>
    Lower,

    /// <summary>
    /// Upper case.
    /// </summary>
    Upper,

    /// <summary>
    /// Upper case, first letter only.
    /// </summary>
    UpperFirstLetter,

    /// <summary>
    /// Proper case.
    /// NB: This is not the same as title case.
    /// In proper case, every word has the first letter upper case, and other letters lower-case.
    /// In title case, some short words like articles and prepositions are all lower-case.
    /// </summary>
    Proper,

    /// <summary>
    /// Mixed case.
    /// </summary>
    Mixed
}
