
namespace Data;

public class BunkerGold
{
    public IEnumerable<PlanetBunker> ReadFromFile()
    {
        var lines = File.ReadAllLines("bunker_gold.txt");

        for (var i = 0; i < lines.Length;)
        {
            var planetName = lines[i++];
            var topLine = lines[i++].Split(",").Select(x => x.TrimStart().TrimEnd()).Select(int.Parse).ToList();
            var bottomLine = lines[i++].Split(",").Select(x => x.TrimStart().TrimEnd()).Select(int.Parse).ToList();
            i++;

            var bunkerColumns = topLine.Zip(bottomLine).Select(x => new TopDownPair(x.First, x.Second));
            yield return new PlanetBunker(planetName, bunkerColumns.ToList());
        }
    }
}

public class PlanetBunker
{
    public PlanetBunker(string planetName, IReadOnlyList<TopDownPair> bunkerColumns)
    {
        PlanetName = planetName;
        BunkerColumns = bunkerColumns;
    }

    public string PlanetName { get; }

    public IReadOnlyList<TopDownPair> BunkerColumns { get; }
}

public class TopDownPair
{
    public TopDownPair(int top, int bottom)
    {
        Top = top;
        Bottom = bottom;
    }

    public int Top { get; }
    public int Bottom { get; }
}