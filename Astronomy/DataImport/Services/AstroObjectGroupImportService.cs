using Galaxon.Astronomy.Data.Models;
using Galaxon.Astronomy.Data.Repositories;

namespace Galaxon.Astronomy.DataImport.Services;

public class AstroObjectGroupImportService(
    AstroObjectGroupRepository astroObjectGroupRepository)
{
    /// <summary>
    /// Initialize all the groups.
    /// </summary>
    public void InitAstroObjectGroups()
    {
        // Stars.
        AstroObjectGroupRecord star = astroObjectGroupRepository.CreateOrUpdate("Star");
        astroObjectGroupRepository.CreateOrUpdate("Hypergiant", star);
        astroObjectGroupRepository.CreateOrUpdate("Supergiant", star);

        AstroObjectGroupRecord giantStar = astroObjectGroupRepository.CreateOrUpdate("Giant", star);
        astroObjectGroupRepository.CreateOrUpdate("Subgiant", giantStar);
        astroObjectGroupRepository.CreateOrUpdate("Bright giant", giantStar);
        astroObjectGroupRepository.CreateOrUpdate("Red giant", giantStar);
        astroObjectGroupRepository.CreateOrUpdate("Yellow giant", giantStar);
        astroObjectGroupRepository.CreateOrUpdate("Blue giant", giantStar);
        astroObjectGroupRepository.CreateOrUpdate("White giant", giantStar);

        AstroObjectGroupRecord mainSequence =
            astroObjectGroupRepository.CreateOrUpdate("Main sequence", star);
        astroObjectGroupRepository.CreateOrUpdate("Red dwarf", mainSequence);
        astroObjectGroupRepository.CreateOrUpdate("Orange dwarf", mainSequence);
        astroObjectGroupRepository.CreateOrUpdate("Yellow dwarf", mainSequence);
        astroObjectGroupRepository.CreateOrUpdate("Blue main sequence star", mainSequence);
        astroObjectGroupRepository.CreateOrUpdate("White dwarf", mainSequence);

        astroObjectGroupRepository.CreateOrUpdate("Subdwarf", star);
        astroObjectGroupRepository.CreateOrUpdate("Brown dwarf", star);

        // Planets.
        AstroObjectGroupRecord planet = astroObjectGroupRepository.CreateOrUpdate("Planet");
        astroObjectGroupRepository.CreateOrUpdate("Terrestrial planet", planet);

        AstroObjectGroupRecord giantPlanet =
            astroObjectGroupRepository.CreateOrUpdate("Giant planet", planet);
        astroObjectGroupRepository.CreateOrUpdate("Gas giant", giantPlanet);
        astroObjectGroupRepository.CreateOrUpdate("Ice giant", giantPlanet);

        // Planetoids.
        AstroObjectGroupRecord minorPlanet = astroObjectGroupRepository.CreateOrUpdate("Minor planet");
        astroObjectGroupRepository.CreateOrUpdate("Centaur", minorPlanet);
        astroObjectGroupRepository.CreateOrUpdate("Trojan", minorPlanet);
        astroObjectGroupRepository.CreateOrUpdate("Quasi-satellite", minorPlanet);

        AstroObjectGroupRecord dwarfPlanet =
            astroObjectGroupRepository.CreateOrUpdate("Dwarf planet", minorPlanet);
        astroObjectGroupRepository.CreateOrUpdate("Plutoid", dwarfPlanet);

        AstroObjectGroupRecord asteroid =
            astroObjectGroupRepository.CreateOrUpdate("Asteroid", minorPlanet);
        astroObjectGroupRepository.CreateOrUpdate("Potentially hazardous asteroid", asteroid);

        AstroObjectGroupRecord nea =
            astroObjectGroupRepository.CreateOrUpdate("Near Earth asteroid", asteroid);
        astroObjectGroupRepository.CreateOrUpdate("Apohele asteroid", nea);
        astroObjectGroupRepository.CreateOrUpdate("Aten asteroid", nea);
        astroObjectGroupRepository.CreateOrUpdate("Apollo asteroid", nea);
        astroObjectGroupRepository.CreateOrUpdate("Amor asteroid", nea);

        AstroObjectGroupRecord sssb =
            astroObjectGroupRepository.CreateOrUpdate("Small Solar System body");
        astroObjectGroupRepository.CreateOrUpdate("Comet", sssb);

        AstroObjectGroupRecord tno = astroObjectGroupRepository.CreateOrUpdate("Trans-Neptunian Object");
        astroObjectGroupRepository.CreateOrUpdate("Oort cloud", tno);

        AstroObjectGroupRecord kbo = astroObjectGroupRepository.CreateOrUpdate("Kuper Belt Object", tno);
        astroObjectGroupRepository.CreateOrUpdate("Cubewano", kbo);
        AstroObjectGroupRecord resonentKbo =
            astroObjectGroupRepository.CreateOrUpdate("Resonant KBO", kbo);
        astroObjectGroupRepository.CreateOrUpdate("Plutino", resonentKbo);

        AstroObjectGroupRecord sdo =
            astroObjectGroupRepository.CreateOrUpdate("Scattered-disc object", tno);
        astroObjectGroupRepository.CreateOrUpdate("Resonant SDO", sdo);

        AstroObjectGroupRecord etno =
            astroObjectGroupRepository.CreateOrUpdate("Extreme Trans-Neptunian object", tno);
        AstroObjectGroupRecord detached =
            astroObjectGroupRepository.CreateOrUpdate("Detached object", etno);
        astroObjectGroupRepository.CreateOrUpdate("Sednoid", detached);

        // Satellites.
        AstroObjectGroupRecord satellite = astroObjectGroupRepository.CreateOrUpdate("Satellite");
        astroObjectGroupRepository.CreateOrUpdate("Regular satellite", satellite);
        astroObjectGroupRepository.CreateOrUpdate("Irregular satellite", satellite);
        astroObjectGroupRepository.CreateOrUpdate("Prograde satellite", satellite);
        astroObjectGroupRepository.CreateOrUpdate("Retrograde satellite", satellite);
    }
}
