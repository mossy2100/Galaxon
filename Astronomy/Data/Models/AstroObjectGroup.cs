using System.ComponentModel.DataAnnotations;

namespace Galaxon.Astronomy.Data.Models;

public class AstroObjectGroup : DataObject
{
    /// <summary>
    /// Group name.
    /// </summary>
    [MaxLength(30)]
    public string Name { get; set; } = "";

    /// <summary>
    /// Objects in the group (navigation property).
    /// </summary>
    public virtual List<AstroObject> Objects { get; set; } = [];

    /// <summary>
    /// Id of the parent group.
    /// </summary>
    public int? ParentId { get; set; }

    /// <summary>
    /// The parent group.
    /// </summary>
    public virtual AstroObjectGroup? Parent { get; set; }
}
