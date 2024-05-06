using Galaxon.Astronomy.Data.Models;
using Galaxon.Core.Exceptions;

namespace Galaxon.Astronomy.Data.Repositories;

public class AstroObjectRepository(
    AstroDbContext astroDbContext,
    AstroObjectGroupRepository astroObjectGroupRepository)
{
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
    /// <param name="astroObjectName">The object's name.</param>
    /// <param name="astroObjectNumber">The object's number.</param>
    /// <param name="groupName">The name of the group to search, e.g. "Planet", "Asteroid",
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
    public AstroObject? Load(string? astroObjectName, int? astroObjectNumber,
        string? groupName = null)
    {
        // Check we have a name or number.
        if (astroObjectName == null && astroObjectNumber == null)
        {
            throw new ArgumentException("Either the name or number (or both) must be specified.");
        }

        // Match on name if specified (case-insensitive).
        IQueryable<AstroObject> query = astroDbContext.AstroObjects;
        if (!string.IsNullOrWhiteSpace(astroObjectName))
        {
            // Can't use string.Equals() here without requiring enumeration first. Using ToLower()
            // should be faster.
            query = query.Where(ao =>
                ao.Name != null && ao.Name.ToLower() == astroObjectName.ToLower());
        }

        // Match on number if specified.
        if (astroObjectNumber != null)
        {
            if (astroObjectNumber <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(astroObjectNumber),
                    "Object number must be positive.");
            }

            query = query.Where(ao => ao.Number == astroObjectNumber);
        }

        // Enumerate.
        List<AstroObject> results = query.ToList();

        // Filter by group if specified.
        if (groupName != null)
        {
            results = results.Where(ao => astroObjectGroupRepository.IsInGroup(ao, groupName))
                .ToList();
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
    public AstroObject LoadByName(string astroObjectName, string? groupName = null)
    {
        AstroObject? obj = Load(astroObjectName, null, groupName);

        if (obj == null)
        {
            string groupName2 = groupName == null ? "object" : groupName.ToLower();
            throw new DataNotFoundException($"Could not find {groupName2} {astroObjectName} in the database.");
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
    /// <param name="astroObjectNumber">The object's number.</param>
    /// <param name="groupName">The name of the group to search, e.g. "Planet", "Asteroid",
    /// etc.</param>
    /// <returns>The matching AstroObject or null if no match was found.</returns>
    public AstroObject LoadByNumber(int astroObjectNumber, string? groupName = null)
    {
        AstroObject? obj = Load(null, astroObjectNumber, groupName);

        if (obj == null)
        {
            string groupName2 = groupName == null ? "object" : groupName.ToLower();
            throw new DataNotFoundException($"Could not find {groupName2} {astroObjectNumber} in the database.");
        }

        return obj;
    }

    /// <summary>
    /// Load all AstroObjects in a group.
    /// Examples:
    ///     LoadAllInGroup("Planet");
    /// </summary>
    /// <param name="groupName">The name of the group, e.g. "Planet", "Asteroid", "Plutoid",
    /// etc.</param>
    /// <returns>The matching AstroObjects.</returns>
    public List<AstroObject> LoadByGroup(string groupName)
    {
        // Get objects with matching name.
        return astroDbContext.AstroObjects.ToList()
            .Where(ao => astroObjectGroupRepository.IsInGroup(ao, groupName)).ToList();
    }
}
