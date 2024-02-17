namespace Galaxon.Core.Strings;

/// <summary>
/// May extend later with LowerSnake, LowerCamel, UpperSnake, UpperCamel, but I'd prefer to keep it
/// simple for now.
/// </summary>
public enum EStringCase
{
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
    /// Proper case. This is not the same as title case.
    /// In proper case, every word has the first letter upper case, and other letters lower-case.
    /// In title case, some short words like articles and prepositions are all lower-case.
    /// </summary>
    Proper,

    /// <summary>
    /// Mixed case (i.e. none of the above).
    /// </summary>
    Mixed
}
