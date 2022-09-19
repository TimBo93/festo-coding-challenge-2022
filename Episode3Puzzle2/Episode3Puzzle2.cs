using System.Collections.Immutable;
using System.Collections.ObjectModel;
using Base;
using Data;

namespace Episode3Puzzle2;

internal class Episode3Puzzle2 : IEpisodePuzzleSolver
{
    private static void Main(string[] args)
    {
        Console.WriteLine(new Episode3Puzzle2().SolveForSusPersons().Sum(x => x.Id));
    }

    private int MinimalDistance(string from, string to, Dictionary<string, PlanetNode> planetNodes)
    {
        var minimalDistanceMap = planetNodes.ToDictionary(x => x.Key, _ => (int?) null);
        var reachablePlanets = new HashSet<string>{from};

        var minimalDistance = 0;
        while (true)
        {
            foreach (var reachablePlanet in reachablePlanets)
            {
                minimalDistanceMap[reachablePlanet] = minimalDistance;

                if (reachablePlanet == to)
                {
                    return minimalDistance;
                }
            }

            var newlyReachablePlanets = reachablePlanets
                .Select(x => planetNodes[x].Neighbors.AsEnumerable()).SelectMany(x => x)
                .Where(x => minimalDistanceMap[x] == null)
                .ToList();

            if (newlyReachablePlanets.Count == 0)
            {
                // no connection found
                return int.MaxValue;
            }

            reachablePlanets = newlyReachablePlanets.ToHashSet();
            
            minimalDistance++;
        }
    }

    public IEnumerable<Person> SolveForSusPersons()
    {
        var galaxies = new GalaxyMap().ReadFromFile();
        var galaxyByName = new ReadOnlyDictionary<string, Galaxy>(galaxies.ToImmutableDictionary(x => x.Name));
        var planets = galaxies.Select(x => new PlanetNode(x.Name, galaxyByName)).ToDictionary(p => p.Name);

        var distanceAssumptions = new SignalRanging().ReadFromFile();

        //var distance = new Episode3Puzzle2().MinimalDistance("Berseus", "Aurus II", planets);

        var allSusGalaxies = GetAllSusGalaxies(galaxies, distanceAssumptions, planets).ToList();

        var population = new Population().ReadFromFile();

        return population.Where(x => allSusGalaxies.Select(g => g.Name).Contains(x.Home));

    }

    private IEnumerable<Galaxy> GetAllSusGalaxies(IReadOnlyList<Galaxy> galaxies, IReadOnlyList<SignalRange> distanceAssumptions, Dictionary<string, PlanetNode> planets)
    {
        foreach (var galaxy in galaxies)
        {
            // check all conditions
            if (CheckAllAssumptions(distanceAssumptions, galaxy.Name, planets))
            {
                yield return galaxy;
            }
        }
    }

    private bool CheckAllAssumptions(IReadOnlyList<SignalRange> distanceAssumptions, string galaxy, Dictionary<string, PlanetNode> planets)
    {
        foreach (var distanceAssumption in distanceAssumptions)
        {
            var distance = MinimalDistance(galaxy, distanceAssumption.Planet, planets);
            if (distance != distanceAssumption.Delay)
            {
                return false;
            }
        }

        return true;
    }
}