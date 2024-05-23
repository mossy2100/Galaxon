using System.ComponentModel.DataAnnotations;

namespace Galaxon.Astronomy.Data.Models;

public abstract class DatabaseRecord
{
    /// <summary>
    /// Primary key.
    /// </summary>
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
}
