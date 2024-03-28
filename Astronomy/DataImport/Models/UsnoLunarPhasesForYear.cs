namespace Galaxon.Astronomy.DataImport.Models;

public record UsnoLunarPhasesForYear
{
    public string apiversion = "";

    public int numphases;

    public List<UsnoLunarPhase>? phasedata;

    public int year;
}
