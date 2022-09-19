using Data;

namespace Episode3Puzzle2;

internal class PlanetNode
{
    public PlanetNode(string planet, IReadOnlyDictionary<string, Galaxy> galaxies)
    {
        Name = planet;
        var myPlanet = galaxies[planet];
        var myPosition = myPlanet.ToPoint3D();

        Neighbors = galaxies
            .Where(x => x.Key != Name)
            .Where(x => x.Value.ToPoint3D().DistanceTo(myPosition) <= 50)
            .Select(x => x.Key)
            .ToList()
            .AsReadOnly();
    }

    public string Name { get; }

    public IReadOnlyList<string> Neighbors { get; }
}