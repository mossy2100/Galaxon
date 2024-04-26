namespace Galaxon.Astronomy.AstroAPI.DataTransferObjects;

public record struct TropicalYearDto
{
    public double EphemerisDays { get; init; }

    public double SolarDays { get; init; }

    public TropicalYearDto(double ephemerisDays, double solarDays)
    {
        EphemerisDays = Math.Round(ephemerisDays, 9);
        SolarDays = Math.Round(solarDays, 9);
    }
}
