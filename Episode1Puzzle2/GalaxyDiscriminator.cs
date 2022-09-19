using Data;
using MathNet.Spatial.Euclidean;

namespace Episode1Puzzle2;

public class GalaxyDiscriminator
{
    private readonly Plane _model;

    private readonly List<Galaxy> _inlier = new();
    private readonly List<Galaxy> _unknown = new();
    private readonly List<Galaxy> _outlier = new();

    public IReadOnlyList<Galaxy> Inlier => _inlier;
    public IReadOnlyList<Galaxy> Unknown => _unknown;
    public IReadOnlyList<Galaxy> Outlier => _outlier;


    public GalaxyDiscriminator(Plane model)
    {
        _model = model;
    }

    public void InsertGalaxy(Galaxy galaxy)
    {
        // ReSharper disable once PossiblyImpureMethodCallOnReadonlyVariable
        var distance = _model.AbsoluteDistanceTo(galaxy.ToPoint3D());

        if (distance < 2)
        {
            _inlier.Add(galaxy);
            return;
        }

        if (distance > 10)
        {
            _outlier.Add(galaxy);
            return;
        }

        _inlier.Add(galaxy);
    }
}