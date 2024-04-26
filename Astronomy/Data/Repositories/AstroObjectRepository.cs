using Galaxon.Astronomy.Data.Models;

namespace Galaxon.Astronomy.Data.Repositories;

public class AstroObjectRepository(AstroDbContext astroDbContext)
{
    /// <summary>
    /// Load an AstroObject from the database by specifying an object name and optional group name.
    /// Examples:
    ///     Load("Earth");
    ///     Load("Ceres", "Dwarf planet");
    /// If there is more than one matching result, throw an exception.
    /// </summary>
    /// <param name="astroObjectName">The object's name (or number, as a string).</param>
    /// <param name="groupName">The name of the group to search, e.g. "Planet", "Asteroid",
    /// "Plutoid", etc.</param>
    /// <returns>The matching AstroObject.</returns>
    /// <exception cref="ArgumentNullException">
    /// If the object name is null or whitespace.
    /// </exception>
    public AstroObject? Load(string astroObjectName, string? groupName = null)
    {
        // Guard.
        if (string.IsNullOrWhiteSpace(astroObjectName))
        {
            throw new ArgumentNullException(nameof(astroObjectName),
                "Object name cannot be null or blank.");
        }

        // Get objects with matching name.
        var results = astroDbContext.AstroObjects.ToList().Where(ao => ao.IsMatch(astroObjectName))
            .ToList();

        // Filter by group if specified and necessary.
        if (results.Count > 0 && groupName != null)
        {
            results = results.Where(ao => AstroObjectGroupRepository.IsInGroup(ao, groupName))
                .ToList();
        }

        // Check if we got multiple results.
        if (results.Count > 1)
        {
            throw new InvalidOperationException("More than one result found.");
        }

        return results.FirstOrDefault();
    }

    /// <summary>
    /// Load an AstroObject from the database by specifying an object number and optional group
    /// name.
    /// Examples:
    ///     Load(2, "Planet");
    ///     Load(134340);
    /// If there is more than one matching result, throw an exception.
    /// </summary>
    /// <param name="astroObjectNumber">The object's number.</param>
    /// <param name="groupName">The name of the group to search, e.g. "Planet", "Asteroid",
    /// etc.</param>
    /// <returns>The matching AstroObject.</returns>
    /// <exception cref="ArgumentNullException">
    /// If the object name is null or whitespace.
    /// </exception>
    public AstroObject? Load(int astroObjectNumber, string? groupName = null)
    {
        return Load(astroObjectNumber.ToString(), groupName);
    }

    /// <summary>
    /// Load all AstroObjects in a group.
    /// Examples:
    ///     LoadAllInGroup("Planet");
    /// </summary>
    /// <param name="groupName">The name of the group, e.g. "Planet", "Asteroid", "Plutoid",
    /// etc.</param>
    /// <returns>The matching AstroObjects.</returns>
    public List<AstroObject> LoadAllInGroup(string groupName)
    {
        // Get objects with matching name.
        return astroDbContext.AstroObjects.ToList()
            .Where(ao => AstroObjectGroupRepository.IsInGroup(ao, groupName)).ToList();
    }
}
