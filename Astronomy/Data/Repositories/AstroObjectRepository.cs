using Galaxon.Astronomy.Data.Models;
using Galaxon.Core.Exceptions;

namespace Galaxon.Astronomy.Data.Repositories;

public class AstroObjectRepository(
    AstroDbContext astroDbContext,
    AstroObjectGroupRepository astroObjectGroupRepository)
{
    /// <summary>
    /// Array mapping planet numbers to English names.
    /// </summary>
    public static readonly string?[] PLANET_NAMES =
    [
        null,
        "Mercury",
        "Venus",
        "Earth",
        "Mars",
        "Jupiter",
        "Saturn",
        "Uranus",
        "Neptune"
    ];

    /// <summary>
    /// Load an AstroObject from the database by specifying either an object name or number, or
    /// both, and optional group name.
    /// Examples:
    ///     Load("Earth", null);
    ///     Load("Ceres", null, "Dwarf planet");
    ///     Load("Ceres", 1, "Dwarf planet");
    ///     Load(null, 4, "Planet");
    /// If there's more than one matching result, an exception will be thrown
    /// </summary>
    /// <param name="name">The object's name.</param>
    /// <param name="number">The object's number.</param>
    /// <param name="group">The name of the group to search, e.g. "Planet", "Asteroid",
    /// "Plutoid", etc.</param>
    /// <returns>The matching AstroObject or null if no match was found.</returns>
    /// <exception cref="ArgumentException">
    /// If neither the object name nor the number is specified.
    /// </exception>
    /// <exception cref="ArgumentOutOfRangeException">
    /// If the object number is specified but isn't positive.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    /// If more than one match is found.
    /// </exception>
    public AstroObjectRecord? Load(string? name, int? number, string? group = null)
    {
        // Check we have a name or number.
        if (name == null && number == null)
        {
            throw new ArgumentException("Either the name or number (or both) must be specified.");
        }

        // Match on name if specified (case-insensitive).
        IQueryable<AstroObjectRecord> query = astroDbContext.AstroObjects;
        if (!string.IsNullOrWhiteSpace(name))
        {
            // Can't use string.Equals() here without requiring enumeration first. Using ToLower()
            // should be faster.
            query = query.Where(ao => ao.Name != null && ao.Name.ToLower() == name.ToLower());
        }

        // Match on number if specified.
        if (number != null)
        {
            if (number <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(number),
                    "Object number must be positive.");
            }

            query = query.Where(ao => ao.Number != null && ao.Number.Value == number);
        }

        // Enumerate.
        List<AstroObjectRecord> results = query.ToList();

        // Filter by group if specified.
        if (group != null)
        {
            results = results.Where(ao => astroObjectGroupRepository.IsInGroup(ao, group)).ToList();
        }

        // Check if we got multiple results.
        if (results.Count > 1)
        {
            throw new InvalidOperationException("More than one result was found.");
        }

        return results.FirstOrDefault();
    }

    /// <summary>
    /// Load an AstroObject from the database by specifying an object name and optional group name.
    /// Examples:
    ///     Load("Earth");
    ///     Load("Ceres", "Dwarf planet");
    /// If there's not exactly one matching result, an exception will be thrown.
    /// </summary>
    /// <param name="astroObjectName">The object's name.</param>
    /// <param name="groupName">The name of the group to search, e.g. "Planet", "Asteroid",
    /// "Plutoid", etc.</param>
    /// <returns>The matching AstroObject or null if no match was found.</returns>
    public AstroObjectRecord LoadByName(string astroObjectName, string? groupName = null)
    {
        AstroObjectRecord? obj = Load(astroObjectName, null, groupName);

        if (obj == null)
        {
            string groupName2 = groupName == null ? "object" : groupName.ToLower();
            throw new DataNotFoundException(
                $"Could not find {groupName2} {astroObjectName} in the database.");
        }

        return obj;
    }

    /// <summary>
    /// Load an AstroObject from the database by specifying an object number and optional group
    /// name.
    /// Examples:
    ///     Load(2, "Planet");
    ///     Load(134340);
    /// If there's more than one matching result, an exception will be thrown.
    /// </summary>
    /// <param name="number">The object's number.</param>
    /// <param name="group">The name of the group to search, e.g. "Planet", "Asteroid",
    /// etc.</param>
    /// <returns>The matching AstroObject or null if no match was found.</returns>
    public AstroObjectRecord LoadByNumber(int number, string? group = null)
    {
        AstroObjectRecord? obj = Load(null, number, group);

        if (obj == null)
        {
            string groupName2 = group == null ? "object" : group.ToLower();
            throw new DataNotFoundException(
                $"Could not find {groupName2} {number} in the database.");
        }

        return obj;
    }

    /// <summary>
    /// Load all AstroObjects in a group.
    /// Examples:
    ///     LoadAllInGroup("Planet");
    /// </summary>
    /// <param name="group">The name of the group, e.g. "Planet", "Asteroid", "Plutoid",
    /// etc.</param>
    /// <returns>The matching AstroObjects.</returns>
    public List<AstroObjectRecord> LoadByGroup(string group)
    {
        return astroDbContext
            .AstroObjects
            .ToList()
            .Where(ao => astroObjectGroupRepository.IsInGroup(ao, group))
            .ToList();
    }
}
