namespace DataImport.DataTransferObjects;

public record UsnoLunarPhasesForYear
{
    public string apiversion = "";

    public int numphases;

    public List<UsnoLunarPhase>? phasedata;

    public int year;
}
