namespace Galaxon.Astronomy.DataImport.Models;

public record UsnoSeasonalMarker
{
    public int day;

    public int month;

    public string phenom = "";

    public string time = "";

    public int year;
}
