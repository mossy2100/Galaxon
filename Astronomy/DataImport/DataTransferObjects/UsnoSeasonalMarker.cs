namespace Galaxon.Astronomy.DataImport.DataTransferObjects;

public record UsnoSeasonalMarker
{
    public int day;

    public int month;

    public string phenom = "";

    public string time = "";

    public int year;
}