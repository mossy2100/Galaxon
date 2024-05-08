namespace Galaxon.Time;

/// <summary>
/// Represents a Gregorian calendar month with its length in days and localized names.
/// </summary>
/// <param name="Length">The number of days in the month (non-leap year).</param>
/// <param name="Names">
/// A dictionary mapping language codes (e.g., 'en', 'fr') to month names.
/// </param>
public readonly record struct GregorianMonth(int Length, Dictionary<string, string> Names)
{
    /// <summary>
    /// Gets the Gregorian months.
    /// </summary>
    public static Dictionary<int, GregorianMonth> Months => _months ??= CreateMonths();

    /// <summary>
    /// Cache of month info.
    /// </summary>
    private static Dictionary<int, GregorianMonth>? _months;

    /// <summary>
    /// Creates the Gregorian months and returns them in the static cache property.
    /// </summary>
    private static Dictionary<int, GregorianMonth> CreateMonths()
    {
        return new Dictionary<int, GregorianMonth>
        {
            {
                1,
                new GregorianMonth(
                    31,
                    new Dictionary<string, string>
                    {
                        { "en", "January" },
                        { "fr", "Janvier" }
                    }
                )
            },
            {
                2,
                new GregorianMonth(
                    28,
                    new Dictionary<string, string>
                    {
                        { "en", "February" },
                        { "fr", "Février" }
                    }
                )
            },
            {
                3,
                new GregorianMonth(
                    31,
                    new Dictionary<string, string>
                    {
                        { "en", "March" },
                        { "fr", "Mars" }
                    }
                )
            },
            {
                4,
                new GregorianMonth(
                    30,
                    new Dictionary<string, string>
                    {
                        { "en", "April" },
                        { "fr", "Avril" }
                    }
                )
            },
            {
                5,
                new GregorianMonth(
                    31,
                    new Dictionary<string, string>
                    {
                        { "en", "May" },
                        { "fr", "Mai" }
                    }
                )
            },
            {
                6,
                new GregorianMonth(
                    30,
                    new Dictionary<string, string>
                    {
                        { "en", "June" },
                        { "fr", "Juin" }
                    }
                )
            },
            {
                7,
                new GregorianMonth(
                    31,
                    new Dictionary<string, string>
                    {
                        { "en", "July" },
                        { "fr", "Juillet" }
                    }
                )
            },
            {
                8,
                new GregorianMonth(
                    31,
                    new Dictionary<string, string>
                    {
                        { "en", "August" },
                        { "fr", "Août" }
                    }
                )
            },
            {
                9,
                new GregorianMonth(
                    30,
                    new Dictionary<string, string>
                    {
                        { "en", "September" },
                        { "fr", "Septembre" }
                    }
                )
            },
            {
                10,
                new GregorianMonth(
                    31,
                    new Dictionary<string, string>
                    {
                        { "en", "October" },
                        { "fr", "Octobre" }
                    }
                )
            },
            {
                11,
                new GregorianMonth(
                    30,
                    new Dictionary<string, string>
                    {
                        { "en", "November" },
                        { "fr", "Novembre" }
                    }
                )
            },
            {
                12,
                new GregorianMonth(
                    31,
                    new Dictionary<string, string>
                    {
                        { "en", "December" },
                        { "fr", "Décembre" }
                    }
                )
            }
        };
    }

    /// <summary>
    /// Gets the month name in the specified language.
    /// </summary>
    /// <param name="languageCode">The language code (e.g., "en", "fr").</param>
    /// <returns>The name of the month in the specified language.</returns>
    public static string GetName(int monthNumber, string languageCode = "en")
    {
        try
        {
            return Months[monthNumber].Names[languageCode];
        }
        catch
        {
            throw new InvalidOperationException(
                $"No month name found for month number {monthNumber} in language {languageCode}.");
        }
    }

    /// <summary>
    /// Returns a dictionary mapping month numbers to names in the specified language.
    /// </summary>
    /// <param name="languageCode">The language code to retrieve names (default is "en").</param>
    /// <returns>A dictionary mapping month numbers to names in the specified language.</returns>
    public static Dictionary<int, string> GetNames(string languageCode = "en")
    {
        Dictionary<int, string> result = new ();

        foreach (KeyValuePair<int, GregorianMonth> month in Months)
        {
            try
            {
                result[month.Key] = month.Value.Names[languageCode];
            }
            catch (KeyNotFoundException)
            {
                throw new InvalidOperationException(
                    $"No month name found for month number {month.Key} in language {languageCode}.");
            }
        }

        return result;
    }

    /// <summary>
    /// Get a month number (1-12) given a name in a specified language (default is English).
    /// Fails if zero or more than one match is found.
    /// </summary>
    /// <param name="monthName">The month name or abbreviation (case-insensitive).</param>
    /// <param name="languageCode">
    /// The language code specifying which set of names to search for a match (default is "en").
    /// </param>
    /// <returns>The month number.</returns>
    /// <exception cref="ArgumentException">
    /// Thrown when the provided month name doesn't produce a unique result.
    /// </exception>
    public static int GetNumber(string monthName, string languageCode = "en")
    {
        // Find matches in the specified language.
        var matches = Months
            .Where(pair =>
            {
                try
                {
                    return GetName(pair.Key, languageCode).StartsWith(monthName, StringComparison.CurrentCultureIgnoreCase);
                }
                catch (KeyNotFoundException)
                {
                    return false;
                }
            })
            .Select(pair => pair.Key)
            .ToList();

        // Handle failure modes.
        if (matches.Count == 0)
        {
            throw new ArgumentException("Invalid month name or abbreviation.", nameof(monthName));
        }
        if (matches.Count > 1)
        {
            throw new ArgumentException("More than one match found.", nameof(monthName));
        }

        // Return the result.
        return matches[0];
    }
}
