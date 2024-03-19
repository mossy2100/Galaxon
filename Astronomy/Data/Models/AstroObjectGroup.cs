using System.ComponentModel.DataAnnotations;

namespace Galaxon.Astronomy.Data.Models;

public class AstroObjectGroup : DataObject
{
    // Group name.
    [MaxLength(30)]
    public string Name { get; set; } = "";

    // Objects in the group (navigation property).
    public virtual List<AstroObject> Objects { get; set; } = [];

    // Parent group.
    public virtual int? ParentId { get; set; }

    public virtual AstroObjectGroup? Parent { get; set; }
}
