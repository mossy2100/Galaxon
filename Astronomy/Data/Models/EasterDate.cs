namespace Galaxon.Astronomy.Data.Models;

public class EasterDate : DataObject
{
    [Column(TypeName = "DATE")]
    public DateOnly Date { get; set; }
}
