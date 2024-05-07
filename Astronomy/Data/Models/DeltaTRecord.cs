namespace Galaxon.Astronomy.Data.Models;

public class DeltaTRecord : DatabaseRecord
{
    [Column(TypeName = "SMALLINT")]
    public int Year { get; set; }

    public double DeltaT { get; set; }
}
