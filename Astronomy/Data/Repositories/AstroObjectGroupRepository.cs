using Galaxon.Astronomy.Data.Models;
using Galaxon.Core.Exceptions;
using Galaxon.Core.Strings;

namespace Galaxon.Astronomy.Data.Repositories;

public class AstroObjectGroupRepository(AstroDbContext astroDbContext)
{
    /// <summary>
    /// Load an AstroObjectGroup from the database.
    /// </summary>
    /// <param name="name">The group name.</param>
    /// <returns>The found object or null.</returns>
    public AstroObjectGroupRecord? Load(string name)
    {
        return astroDbContext.AstroObjectGroups.FirstOrDefault(g => g.Name == name);
    }

    /// <summary>
    /// Create or update a group.
    /// </summary>
    /// <param name="name">The group name.</param>
    /// <param name="parent">The group's parent.</param>
    /// <returns>The updated group.</returns>
    public AstroObjectGroupRecord CreateOrUpdate(string name, AstroObjectGroupRecord? parent = null)
    {
        AstroObjectGroupRecord? group = Load(name);
        if (group == null)
        {
            // The group was not found in the database. Create a new one.
            group = new AstroObjectGroupRecord
            {
                Name = name,
                Parent = parent
            };
            astroDbContext.AstroObjectGroups.Add(group);
        }
        else
        {
            // The group was found.
            group.Parent = parent;
            astroDbContext.AstroObjectGroups.Update(group);
        }
        astroDbContext.SaveChanges();
        return group;
    }

    /// <summary>
    /// Check if the object is in a certain group.
    /// </summary>
    /// <param name="astroObjectRecord">The AstroObject to look for.</param>
    /// <param name="groupRecord">The AstroObjectGroup to look in.</param>
    /// <returns>If the object is in the specified group.</returns>
    public bool IsInGroup(AstroObjectRecord astroObjectRecord, AstroObjectGroupRecord groupRecord)
    {
        return astroObjectRecord.Groups?.Contains(groupRecord) ?? false;
    }

    /// <summary>
    /// Check if the object is in a certain group.
    /// </summary>
    /// <param name="astroObjectRecord">The AstroObject to look for.</param>
    /// <param name="groupName">The name of the group to check (case sensitive).</param>
    /// <returns>If the object is in the specified group.</returns>
    public bool IsInGroup(AstroObjectRecord astroObjectRecord, string groupName)
    {
        return astroObjectRecord.Groups != null
            && astroObjectRecord.Groups.Any(group => groupName.EqualsIgnoreCase(group.Name));
    }

    /// <summary>
    /// Add the object to a group, if it's not already a member.
    /// </summary>
    public void AddToGroup(AstroObjectRecord astroObjectRecord, AstroObjectGroupRecord groupRecord)
    {
        if (!IsInGroup(astroObjectRecord, groupRecord))
        {
            astroObjectRecord.Groups ??= [];
            astroObjectRecord.Groups.Add(groupRecord);
        }
    }

    /// <summary>
    /// Add the object to a group, if it's not already a member.
    /// </summary>
    /// <param name="astroObjectRecord">The AstroObject to add to the group.</param>
    /// <param name="groupName">The group name.</param>
    public void AddToGroup(AstroObjectRecord astroObjectRecord, string groupName)
    {
        AstroObjectGroupRecord? group = Load(groupName);
        if (group == null)
        {
            throw new DataNotFoundException($"Group '{groupName}' not found.");
        }
        AddToGroup(astroObjectRecord, group);
    }
}
