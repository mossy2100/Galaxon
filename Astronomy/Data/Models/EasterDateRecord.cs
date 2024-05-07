namespace Galaxon.Astronomy.Data.Models;

public class EasterDateRecord : DatabaseRecord
{
    [Column(TypeName = "date")]
    public DateOnly Date { get; set; }
}
