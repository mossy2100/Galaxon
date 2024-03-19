namespace Galaxon.Astronomy.Data.Models;

public class EasterDate : DataObject
{
    [Column(TypeName = "date")]
    public DateOnly Date { get; set; }
}
