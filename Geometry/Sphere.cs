namespace Galaxon.Numerics.Geometry;

public class Sphere : Ellipsoid
{
    public Sphere(double radius) : base(radius, radius, radius) { }

    public double Radius
    {
        get => RadiusA;

        set
        {
            RadiusA = value;
            RadiusB = value;
            RadiusC = value;
        }
    }
}
