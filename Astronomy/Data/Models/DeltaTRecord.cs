namespace Galaxon.Astronomy.Data.Models;

public class DeltaTRecord : DataObject
{
    [Column(TypeName = "SMALLINT")]
    public int Year { get; set; }

    public double DeltaT { get; set; }
}
