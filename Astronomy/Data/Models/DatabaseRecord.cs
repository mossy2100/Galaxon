using System.ComponentModel.DataAnnotations;

namespace Galaxon.Astronomy.Data.Models;

public abstract class DatabaseRecord
{
    // Primary key.
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
}
