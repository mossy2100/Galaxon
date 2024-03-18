namespace Galaxon.Astronomy.Data.Models;

public class DeltaTRecord : Entity
{
    [Column(TypeName = "smallint")]
    public int Year { get; set; }

    public double DeltaT { get; set; }
}
