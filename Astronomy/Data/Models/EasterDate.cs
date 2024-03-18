namespace Galaxon.Astronomy.Data.Models;

public class EasterDate : Entity
{
    [Column(TypeName = "date")]
    public DateOnly Date { get; set; }
}
