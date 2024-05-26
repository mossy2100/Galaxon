namespace Galaxon.Astronomy.Data.Models;

public class DeltaTRecord : DatabaseRecord
{
    [Column(TypeName = "decimal(6, 2)")]
    public decimal DecimalYear { get; set; }

    /// <summary>
    /// Value of delta-T at the specified time.
    /// </summary>
    [Column(TypeName = "decimal(9, 4)")]
    public decimal DeltaT { get; set; }
}
