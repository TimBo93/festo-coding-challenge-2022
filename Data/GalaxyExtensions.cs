using MathNet.Spatial.Euclidean;

namespace Data;

public static class GalaxyExtensions
{
    public static Point3D ToPoint3D(this Galaxy galaxy)
    {
        return new Point3D(galaxy.X, galaxy.Y, galaxy.Z);
    }
}