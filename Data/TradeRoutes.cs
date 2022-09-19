namespace Data;

public class TradeRoutes
{
    public IReadOnlyList<TradeRoute> ReadFromFile()
    {
        var result = new List<TradeRoute>();

        var lines = File.ReadAllLines("trade_routes.txt");
        foreach (var line in lines)
        {
            var routeStatus = line.Split(":");
            var routePart = routeStatus[0];
            var statusPart = routeStatus[1];

            var galaxies = routePart.Split("-");
            var fromGalaxy = galaxies[0].TrimStart().TrimEnd();
            var toGalaxy = galaxies[1].TrimStart().TrimEnd();

            result.Add(new TradeRoute(fromGalaxy, toGalaxy,
                statusPart.Contains("Ok") ? DistanceMode.Ok : DistanceMode.TooFar));
        }

        return result;
    }
}

public class TradeRoute
{
    public TradeRoute(string galaxy1, string galaxy2, DistanceMode distanceMode)
    {
        Galaxy1 = galaxy1;
        Galaxy2 = galaxy2;
        DistanceMode = distanceMode;
    }

    public string Galaxy1 { get; }
    public string Galaxy2 { get; }
    public DistanceMode DistanceMode { get; }
}

public enum DistanceMode
{
    Ok,
    TooFar
}