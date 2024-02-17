namespace Galaxon.Numerics.Geometry;

public interface IShape3D
{
    double SurfaceArea { get; }

    double Volume { get; }
}
