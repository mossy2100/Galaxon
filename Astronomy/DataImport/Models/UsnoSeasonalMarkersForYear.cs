namespace Galaxon.Astronomy.DataImport.Models;

public record UsnoSeasonalMarkersForYear
{
    public string apiversion = "";

    public List<UsnoSeasonalMarker>? data;

    public bool dst;

    public int tz;

    public int year;
}
