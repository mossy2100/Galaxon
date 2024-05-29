using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Galaxon.Astronomy.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdatedAstroObjectPropertiesInclComponents : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ColorBV",
                table: "Physicals");

            migrationBuilder.DropColumn(
                name: "ColorUB",
                table: "Physicals");

            migrationBuilder.DropColumn(
                name: "Density",
                table: "Physicals");

            migrationBuilder.DropColumn(
                name: "EscapeVelocity",
                table: "Physicals");

            migrationBuilder.RenameColumn(
                name: "Radiance",
                table: "Stellars",
                newName: "Radiance_W_sr_m2");

            migrationBuilder.RenameColumn(
                name: "Luminosity",
                table: "Stellars",
                newName: "Luminosity_W");

            migrationBuilder.RenameColumn(
                name: "SynodicRotationPeriod",
                table: "Rotationals",
                newName: "SynodicRotationPeriod_d");

            migrationBuilder.RenameColumn(
                name: "SiderealRotationPeriod",
                table: "Rotationals",
                newName: "SiderealRotationPeriod_d");

            migrationBuilder.RenameColumn(
                name: "Obliquity",
                table: "Rotationals",
                newName: "Obliquity_deg");

            migrationBuilder.RenameColumn(
                name: "NorthPoleRightAscension",
                table: "Rotationals",
                newName: "NorthPoleRightAscension_deg");

            migrationBuilder.RenameColumn(
                name: "NorthPoleDeclination",
                table: "Rotationals",
                newName: "NorthPoleDeclination_deg");

            migrationBuilder.RenameColumn(
                name: "EquatRotationVelocity",
                table: "Rotationals",
                newName: "EquatorialRotationalVelocity_km_s");

            migrationBuilder.RenameColumn(
                name: "Volume",
                table: "Physicals",
                newName: "Volume_km3");

            migrationBuilder.RenameColumn(
                name: "SurfaceGrav",
                table: "Physicals",
                newName: "SurfaceGravity_m_s2");

            migrationBuilder.RenameColumn(
                name: "SurfaceArea",
                table: "Physicals",
                newName: "SurfaceArea_km2");

            migrationBuilder.RenameColumn(
                name: "StdGravParam",
                table: "Physicals",
                newName: "StandardGravitationalParameter_m3_s2");

            migrationBuilder.RenameColumn(
                name: "SolarIrradiance",
                table: "Physicals",
                newName: "PolarRadius_km");

            migrationBuilder.RenameColumn(
                name: "RadiusC",
                table: "Physicals",
                newName: "MinSurfaceTemperature_K");

            migrationBuilder.RenameColumn(
                name: "RadiusB",
                table: "Physicals",
                newName: "MeanSurfaceTemperature_K");

            migrationBuilder.RenameColumn(
                name: "RadiusA",
                table: "Physicals",
                newName: "MeanRadius_km");

            migrationBuilder.RenameColumn(
                name: "MomentOfInertiaFactor",
                table: "Physicals",
                newName: "MaxSurfaceTemperature_K");

            migrationBuilder.RenameColumn(
                name: "MinSurfaceTemp",
                table: "Physicals",
                newName: "Mass_kg");

            migrationBuilder.RenameColumn(
                name: "MeanSurfaceTemp",
                table: "Physicals",
                newName: "EscapeVelocity_km_s");

            migrationBuilder.RenameColumn(
                name: "MeanRadius",
                table: "Physicals",
                newName: "EquatorialRadius_km");

            migrationBuilder.RenameColumn(
                name: "MaxSurfaceTemp",
                table: "Physicals",
                newName: "EquatorialRadius2_km");

            migrationBuilder.RenameColumn(
                name: "Mass",
                table: "Physicals",
                newName: "Density_g_cm3");

            migrationBuilder.RenameColumn(
                name: "HasRingSystem",
                table: "Physicals",
                newName: "HasRings");

            migrationBuilder.RenameColumn(
                name: "HasGlobalMagField",
                table: "Physicals",
                newName: "HasGlobalMagneticField");

            migrationBuilder.RenameColumn(
                name: "SynodicOrbitPeriod",
                table: "Orbitals",
                newName: "SynodicOrbitPeriod_d");

            migrationBuilder.RenameColumn(
                name: "SiderealOrbitPeriod",
                table: "Orbitals",
                newName: "SiderealOrbitPeriod_d");

            migrationBuilder.RenameColumn(
                name: "SemiMajorAxis",
                table: "Orbitals",
                newName: "SemiMajorAxis_km");

            migrationBuilder.RenameColumn(
                name: "Periapsis",
                table: "Orbitals",
                newName: "Periapsis_km");

            migrationBuilder.RenameColumn(
                name: "MeanMotion",
                table: "Orbitals",
                newName: "MeanMotion_deg_d");

            migrationBuilder.RenameColumn(
                name: "MeanAnomaly",
                table: "Orbitals",
                newName: "MeanAnomaly_deg");

            migrationBuilder.RenameColumn(
                name: "LongAscNode",
                table: "Orbitals",
                newName: "LongitudeAscendingNode_deg");

            migrationBuilder.RenameColumn(
                name: "Inclination",
                table: "Orbitals",
                newName: "Inclination_deg");

            migrationBuilder.RenameColumn(
                name: "AvgOrbitSpeed",
                table: "Orbitals",
                newName: "AverageOrbitalSpeed_km_s");

            migrationBuilder.RenameColumn(
                name: "ArgPeriapsis",
                table: "Orbitals",
                newName: "ArgumentPeriapsis_deg");

            migrationBuilder.RenameColumn(
                name: "Apoapsis",
                table: "Orbitals",
                newName: "Apoapsis_km");

            migrationBuilder.RenameColumn(
                name: "MinApparentMag",
                table: "Observationals",
                newName: "MinApparentMagnitude");

            migrationBuilder.RenameColumn(
                name: "MinAngularDiam",
                table: "Observationals",
                newName: "MinAngularDiameter_deg");

            migrationBuilder.RenameColumn(
                name: "MaxApparentMag",
                table: "Observationals",
                newName: "MaxApparentMagnitude");

            migrationBuilder.RenameColumn(
                name: "MaxAngularDiam",
                table: "Observationals",
                newName: "MaxAngularDiameter_deg");

            migrationBuilder.RenameColumn(
                name: "AbsMag",
                table: "Observationals",
                newName: "AbsoluteMagnitude");

            migrationBuilder.RenameColumn(
                name: "SurfacePressure",
                table: "Atmospheres",
                newName: "SurfacePressure_Pa");

            migrationBuilder.RenameColumn(
                name: "ScaleHeight",
                table: "Atmospheres",
                newName: "ScaleHeight_km");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Radiance_W_sr_m2",
                table: "Stellars",
                newName: "Radiance");

            migrationBuilder.RenameColumn(
                name: "Luminosity_W",
                table: "Stellars",
                newName: "Luminosity");

            migrationBuilder.RenameColumn(
                name: "SynodicRotationPeriod_d",
                table: "Rotationals",
                newName: "SynodicRotationPeriod");

            migrationBuilder.RenameColumn(
                name: "SiderealRotationPeriod_d",
                table: "Rotationals",
                newName: "SiderealRotationPeriod");

            migrationBuilder.RenameColumn(
                name: "Obliquity_deg",
                table: "Rotationals",
                newName: "Obliquity");

            migrationBuilder.RenameColumn(
                name: "NorthPoleRightAscension_deg",
                table: "Rotationals",
                newName: "NorthPoleRightAscension");

            migrationBuilder.RenameColumn(
                name: "NorthPoleDeclination_deg",
                table: "Rotationals",
                newName: "NorthPoleDeclination");

            migrationBuilder.RenameColumn(
                name: "EquatorialRotationalVelocity_km_s",
                table: "Rotationals",
                newName: "EquatRotationVelocity");

            migrationBuilder.RenameColumn(
                name: "Volume_km3",
                table: "Physicals",
                newName: "Volume");

            migrationBuilder.RenameColumn(
                name: "SurfaceGravity_m_s2",
                table: "Physicals",
                newName: "SurfaceGrav");

            migrationBuilder.RenameColumn(
                name: "SurfaceArea_km2",
                table: "Physicals",
                newName: "SurfaceArea");

            migrationBuilder.RenameColumn(
                name: "StandardGravitationalParameter_m3_s2",
                table: "Physicals",
                newName: "StdGravParam");

            migrationBuilder.RenameColumn(
                name: "PolarRadius_km",
                table: "Physicals",
                newName: "SolarIrradiance");

            migrationBuilder.RenameColumn(
                name: "MinSurfaceTemperature_K",
                table: "Physicals",
                newName: "RadiusC");

            migrationBuilder.RenameColumn(
                name: "MeanSurfaceTemperature_K",
                table: "Physicals",
                newName: "RadiusB");

            migrationBuilder.RenameColumn(
                name: "MeanRadius_km",
                table: "Physicals",
                newName: "RadiusA");

            migrationBuilder.RenameColumn(
                name: "MaxSurfaceTemperature_K",
                table: "Physicals",
                newName: "MomentOfInertiaFactor");

            migrationBuilder.RenameColumn(
                name: "Mass_kg",
                table: "Physicals",
                newName: "MinSurfaceTemp");

            migrationBuilder.RenameColumn(
                name: "HasRings",
                table: "Physicals",
                newName: "HasRingSystem");

            migrationBuilder.RenameColumn(
                name: "HasGlobalMagneticField",
                table: "Physicals",
                newName: "HasGlobalMagField");

            migrationBuilder.RenameColumn(
                name: "EscapeVelocity_km_s",
                table: "Physicals",
                newName: "MeanSurfaceTemp");

            migrationBuilder.RenameColumn(
                name: "EquatorialRadius_km",
                table: "Physicals",
                newName: "MeanRadius");

            migrationBuilder.RenameColumn(
                name: "EquatorialRadius2_km",
                table: "Physicals",
                newName: "MaxSurfaceTemp");

            migrationBuilder.RenameColumn(
                name: "Density_g_cm3",
                table: "Physicals",
                newName: "Mass");

            migrationBuilder.RenameColumn(
                name: "SynodicOrbitPeriod_d",
                table: "Orbitals",
                newName: "SynodicOrbitPeriod");

            migrationBuilder.RenameColumn(
                name: "SiderealOrbitPeriod_d",
                table: "Orbitals",
                newName: "SiderealOrbitPeriod");

            migrationBuilder.RenameColumn(
                name: "SemiMajorAxis_km",
                table: "Orbitals",
                newName: "SemiMajorAxis");

            migrationBuilder.RenameColumn(
                name: "Periapsis_km",
                table: "Orbitals",
                newName: "Periapsis");

            migrationBuilder.RenameColumn(
                name: "MeanMotion_deg_d",
                table: "Orbitals",
                newName: "MeanMotion");

            migrationBuilder.RenameColumn(
                name: "MeanAnomaly_deg",
                table: "Orbitals",
                newName: "MeanAnomaly");

            migrationBuilder.RenameColumn(
                name: "LongitudeAscendingNode_deg",
                table: "Orbitals",
                newName: "LongAscNode");

            migrationBuilder.RenameColumn(
                name: "Inclination_deg",
                table: "Orbitals",
                newName: "Inclination");

            migrationBuilder.RenameColumn(
                name: "AverageOrbitalSpeed_km_s",
                table: "Orbitals",
                newName: "AvgOrbitSpeed");

            migrationBuilder.RenameColumn(
                name: "ArgumentPeriapsis_deg",
                table: "Orbitals",
                newName: "ArgPeriapsis");

            migrationBuilder.RenameColumn(
                name: "Apoapsis_km",
                table: "Orbitals",
                newName: "Apoapsis");

            migrationBuilder.RenameColumn(
                name: "MinApparentMagnitude",
                table: "Observationals",
                newName: "MinApparentMag");

            migrationBuilder.RenameColumn(
                name: "MinAngularDiameter_deg",
                table: "Observationals",
                newName: "MinAngularDiam");

            migrationBuilder.RenameColumn(
                name: "MaxApparentMagnitude",
                table: "Observationals",
                newName: "MaxApparentMag");

            migrationBuilder.RenameColumn(
                name: "MaxAngularDiameter_deg",
                table: "Observationals",
                newName: "MaxAngularDiam");

            migrationBuilder.RenameColumn(
                name: "AbsoluteMagnitude",
                table: "Observationals",
                newName: "AbsMag");

            migrationBuilder.RenameColumn(
                name: "SurfacePressure_Pa",
                table: "Atmospheres",
                newName: "SurfacePressure");

            migrationBuilder.RenameColumn(
                name: "ScaleHeight_km",
                table: "Atmospheres",
                newName: "ScaleHeight");

            migrationBuilder.AddColumn<double>(
                name: "ColorBV",
                table: "Physicals",
                type: "double",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "ColorUB",
                table: "Physicals",
                type: "double",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "Density",
                table: "Physicals",
                type: "double",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "EscapeVelocity",
                table: "Physicals",
                type: "double",
                nullable: true);
        }
    }
}
