using System.ComponentModel.DataAnnotations;

namespace Galaxon.Astronomy.Data.Models;

public abstract class Entity
{
    // Primary key.
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
}
