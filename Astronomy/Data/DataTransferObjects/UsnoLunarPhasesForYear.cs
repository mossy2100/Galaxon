namespace Galaxon.Astronomy.Data.DataTransferObjects;

public record UsnoLunarPhasesForYear
{
    public string apiversion = "";

    public int numphases;

    public List<UsnoLunarPhase>? phasedata;

    public int year;
}
