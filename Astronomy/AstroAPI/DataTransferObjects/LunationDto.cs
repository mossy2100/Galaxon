namespace Galaxon.Astronomy.AstroAPI.DataTransferObjects;

public record struct LunationDto
{
    public double EphemerisDays { get; init; }

    public double SolarDays { get; init; }

    public LunationDto(double ephemerisDays, double solarDays)
    {
        EphemerisDays = Math.Round(ephemerisDays, 9);
        SolarDays = Math.Round(solarDays, 9);
    }
}
