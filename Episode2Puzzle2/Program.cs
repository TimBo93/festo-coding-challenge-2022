using Base;
using Data;
using MathNet.Spatial.Euclidean;

namespace Episode2Puzzle2;

public class Episode2Puzzle2 : IEpisodePuzzleSolver
{
    public IEnumerable<Person> SolveForSusPersons()
    {
        var parsedRoutes = new TradeRoutes().ReadFromFile();
        var galaxies = new GalaxyMap().ReadFromFile();

        var routes = parsedRoutes.Select(r => new EnrichedRoute(
            galaxies.Single(x => x.Name == r.Galaxy1),
            galaxies.Single(x => x.Name == r.Galaxy2),
            r.DistanceMode
        )).ToList();

        var susGalaxies = galaxies.Where(g => IsPossiblyRogueGalaxy(routes, g)).Select(x => x.Name).ToHashSet();

        var population = new Population().ReadFromFile();
        return susGalaxies.Select(x => population.Where(p => x == p.Home)).SelectMany(x => x);
    }

    public static void Main()
    {
        Console.WriteLine(new Episode2Puzzle2().SolveForSusPersons().Sum(x => x.Id));
    }

    private static bool IsPossiblyRogueGalaxy(List<EnrichedRoute> routeConstraints, Galaxy galaxyToCheck)
    {
        var galaxyToCheckPoint = galaxyToCheck.ToPoint3D();

        foreach (var routeToCheck in routeConstraints)
        {
            if (routeToCheck.FromGalaxy.Name == galaxyToCheck.Name || routeToCheck.ToGalaxy.Name == galaxyToCheck.Name)
            {
                if (routeToCheck.DistanceMode == DistanceMode.Ok) continue;
                return false;
            }

            var from = routeToCheck.FromGalaxy.ToPoint3D();
            var to = routeToCheck.ToGalaxy.ToPoint3D();

            var route = new LineSegment3D(from, to);
            var shortestDistance = route.LineTo(galaxyToCheckPoint);
            var shortestDistanceLength = shortestDistance.Length;

            if ((routeToCheck.DistanceMode == DistanceMode.Ok && shortestDistanceLength <= 10)
                || (routeToCheck.DistanceMode == DistanceMode.TooFar && shortestDistanceLength > 10))
                continue;

            return false;
        }

        return true;
    }
}

internal static class GalaxyExtension
{
    public static Point3D ToPoint3D(this Galaxy galaxy)
    {
        return new Point3D(galaxy.X, galaxy.Y, galaxy.Z);
    }
}

internal class EnrichedRoute
{
    public EnrichedRoute(Galaxy fromGalaxy, Galaxy toGalaxy, DistanceMode distanceMode)
    {
        FromGalaxy = fromGalaxy;
        ToGalaxy = toGalaxy;
        DistanceMode = distanceMode;
    }

    public Galaxy FromGalaxy { get; }
    public Galaxy ToGalaxy { get; }
    public DistanceMode DistanceMode { get; }
}