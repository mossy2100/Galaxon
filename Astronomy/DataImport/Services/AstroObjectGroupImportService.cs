using Galaxon.Astronomy.Data.Models;
using Galaxon.Astronomy.Data.Repositories;

namespace DataImport.Services;

public class AstroObjectGroupImportService(
    AstroObjectGroupRepository astroObjectGroupRepository)
{
    /// <summary>
    /// Initialize all the groups.
    /// </summary>
    public void InitAstroObjectGroups()
    {
        // Stars.
        AstroObjectGroup star = astroObjectGroupRepository.CreateOrUpdate("Star");
        astroObjectGroupRepository.CreateOrUpdate("Hypergiant", star);
        astroObjectGroupRepository.CreateOrUpdate("Supergiant", star);

        AstroObjectGroup giantStar = astroObjectGroupRepository.CreateOrUpdate("Giant", star);
        astroObjectGroupRepository.CreateOrUpdate("Subgiant", giantStar);
        astroObjectGroupRepository.CreateOrUpdate("Bright giant", giantStar);
        astroObjectGroupRepository.CreateOrUpdate("Red giant", giantStar);
        astroObjectGroupRepository.CreateOrUpdate("Yellow giant", giantStar);
        astroObjectGroupRepository.CreateOrUpdate("Blue giant", giantStar);
        astroObjectGroupRepository.CreateOrUpdate("White giant", giantStar);

        AstroObjectGroup mainSequence =
            astroObjectGroupRepository.CreateOrUpdate("Main sequence", star);
        astroObjectGroupRepository.CreateOrUpdate("Red dwarf", mainSequence);
        astroObjectGroupRepository.CreateOrUpdate("Orange dwarf", mainSequence);
        astroObjectGroupRepository.CreateOrUpdate("Yellow dwarf", mainSequence);
        astroObjectGroupRepository.CreateOrUpdate("Blue main sequence star", mainSequence);
        astroObjectGroupRepository.CreateOrUpdate("White dwarf", mainSequence);

        astroObjectGroupRepository.CreateOrUpdate("Subdwarf", star);
        astroObjectGroupRepository.CreateOrUpdate("Brown dwarf", star);

        // Planets.
        AstroObjectGroup planet = astroObjectGroupRepository.CreateOrUpdate("Planet");
        astroObjectGroupRepository.CreateOrUpdate("Terrestrial planet", planet);

        AstroObjectGroup giantPlanet =
            astroObjectGroupRepository.CreateOrUpdate("Giant planet", planet);
        astroObjectGroupRepository.CreateOrUpdate("Gas giant", giantPlanet);
        astroObjectGroupRepository.CreateOrUpdate("Ice giant", giantPlanet);

        // Planetoids.
        AstroObjectGroup minorPlanet = astroObjectGroupRepository.CreateOrUpdate("Minor planet");
        astroObjectGroupRepository.CreateOrUpdate("Centaur", minorPlanet);
        astroObjectGroupRepository.CreateOrUpdate("Trojan", minorPlanet);
        astroObjectGroupRepository.CreateOrUpdate("Quasi-satellite", minorPlanet);

        AstroObjectGroup dwarfPlanet =
            astroObjectGroupRepository.CreateOrUpdate("Dwarf planet", minorPlanet);
        astroObjectGroupRepository.CreateOrUpdate("Plutoid", dwarfPlanet);

        AstroObjectGroup asteroid =
            astroObjectGroupRepository.CreateOrUpdate("Asteroid", minorPlanet);
        astroObjectGroupRepository.CreateOrUpdate("Potentially hazardous asteroid", asteroid);

        AstroObjectGroup nea =
            astroObjectGroupRepository.CreateOrUpdate("Near Earth asteroid", asteroid);
        astroObjectGroupRepository.CreateOrUpdate("Apohele asteroid", nea);
        astroObjectGroupRepository.CreateOrUpdate("Aten asteroid", nea);
        astroObjectGroupRepository.CreateOrUpdate("Apollo asteroid", nea);
        astroObjectGroupRepository.CreateOrUpdate("Amor asteroid", nea);

        AstroObjectGroup sssb =
            astroObjectGroupRepository.CreateOrUpdate("Small Solar System body");
        astroObjectGroupRepository.CreateOrUpdate("Comet", sssb);

        AstroObjectGroup tno = astroObjectGroupRepository.CreateOrUpdate("Trans-Neptunian Object");
        astroObjectGroupRepository.CreateOrUpdate("Oort cloud", tno);

        AstroObjectGroup kbo = astroObjectGroupRepository.CreateOrUpdate("Kuper Belt Object", tno);
        astroObjectGroupRepository.CreateOrUpdate("Cubewano", kbo);
        AstroObjectGroup resonentKbo =
            astroObjectGroupRepository.CreateOrUpdate("Resonant KBO", kbo);
        astroObjectGroupRepository.CreateOrUpdate("Plutino", resonentKbo);

        AstroObjectGroup sdo =
            astroObjectGroupRepository.CreateOrUpdate("Scattered-disc object", tno);
        astroObjectGroupRepository.CreateOrUpdate("Resonant SDO", sdo);

        AstroObjectGroup etno =
            astroObjectGroupRepository.CreateOrUpdate("Extreme Trans-Neptunian object", tno);
        AstroObjectGroup detached =
            astroObjectGroupRepository.CreateOrUpdate("Detached object", etno);
        astroObjectGroupRepository.CreateOrUpdate("Sednoid", detached);

        // Satellites.
        AstroObjectGroup satellite = astroObjectGroupRepository.CreateOrUpdate("Satellite");
        astroObjectGroupRepository.CreateOrUpdate("Regular satellite", satellite);
        astroObjectGroupRepository.CreateOrUpdate("Irregular satellite", satellite);
        astroObjectGroupRepository.CreateOrUpdate("Prograde satellite", satellite);
        astroObjectGroupRepository.CreateOrUpdate("Retrograde satellite", satellite);
    }
}
