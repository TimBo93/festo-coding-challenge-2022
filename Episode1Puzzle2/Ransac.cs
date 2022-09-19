using Data;
using MathNet.Spatial.Euclidean;

namespace Episode1Puzzle2;

public class Ransac
{
    private readonly List<Galaxy> _galaxies;
    private readonly Random _rand = new(DateTime.Now.Millisecond);

    public Ransac(IEnumerable<Galaxy> galaxies)
    {
        _galaxies = galaxies.ToList();
    }

    public GalaxyDiscriminator Calculate()
    {
        for (int i = 0; i < 10_000; i++)
        {
            var galaxyDiscriminator = CalculateOneStep();
            if (galaxyDiscriminator.Unknown.Count == 0)
            {
                Console.WriteLine($"found a perfect solution after {i + 1} iterations.");
                return galaxyDiscriminator;
            }
        }

        throw new Exception("no perfect solution found");
    }

    internal GalaxyDiscriminator CalculateOneStep()
    {
        var randomGalaxySample = this.GetRandomGalaxySample();
        var model = this.CalculateModelFromRandomSample(randomGalaxySample);

        var galaxyDiscriminator = new GalaxyDiscriminator(model);
        _galaxies.ForEach(galaxy => galaxyDiscriminator.InsertGalaxy(galaxy));
        return galaxyDiscriminator;
    }

    internal Plane CalculateModelFromRandomSample(GalaxySample galaxySample)
    {
        Point3D pos1;
        Point3D pos2;
        Point3D pos3;

        do
        {
            pos1 = galaxySample.Galaxy1.ToPoint3D();
            pos2 = galaxySample.Galaxy2.ToPoint3D();
            pos3 = galaxySample.Galaxy3.ToPoint3D();
        } while (pos1 == pos2 || pos1 == pos3 || pos2 == pos3);

        return Plane.FromPoints(pos1, pos2, pos3);
    }

    internal GalaxySample GetRandomGalaxySample()
    {
        return new GalaxySample(GetOneRandomGalaxy(), GetOneRandomGalaxy(), GetOneRandomGalaxy());
    }

    internal Galaxy GetOneRandomGalaxy()
    {
        return _galaxies[_rand.Next(_galaxies.Count)];
    }
}