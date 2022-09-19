using Data;

namespace FinalEpisodePuzzle2;

internal class FinalEpisodePuzzle2
{
    public static void Main()
    {
        Console.WriteLine(new FinalEpisodePuzzle2().Solve());
    }

    private string Solve()
    {
        var bunkerGold = new BunkerGold().ReadFromFile().ToList();

        var bestBunker = bunkerGold.Select(x => new { treasure = SolveHighestTreasure(x), name = x.PlanetName })
            .MaxBy(x => x.treasure)!;

        return $"{bestBunker.name}{bestBunker.treasure}";
    }

    private int SolveHighestTreasure(PlanetBunker planetBunker)
    {
        // building chain data-structure
        IChainItem currentChainItem = new RootChainItem();
        foreach (var planetBunkerBunkerColumn in planetBunker.BunkerColumns)
        {
            var chainItem = new ChainItem(currentChainItem, planetBunkerBunkerColumn);
            currentChainItem = chainItem;
        }

        return Math.Max(currentChainItem.GetMaxValueAllowOnlyTop(), currentChainItem.GetMaxValueAllowOnlyBottom());
    }
}

internal class ChainItem : IChainItem
{
    private readonly int _maxValueIfOnlyBottomIsAllowed;
    private readonly int _maxValueIfOnlyTopIsAllowed;

    public ChainItem(IChainItem previouseChainItem, TopDownPair topDownPair)
    {
        _maxValueIfOnlyTopIsAllowed = Math.Max(topDownPair.Top + previouseChainItem.GetMaxValueAllowOnlyBottom(),
            previouseChainItem.GetMaxValueAllowOnlyTop());

        _maxValueIfOnlyBottomIsAllowed = Math.Max(topDownPair.Bottom + previouseChainItem.GetMaxValueAllowOnlyTop(),
            previouseChainItem.GetMaxValueAllowOnlyBottom());
    }

    public int GetMaxValueAllowOnlyTop()
    {
        return _maxValueIfOnlyTopIsAllowed;
    }

    public int GetMaxValueAllowOnlyBottom()
    {
        return _maxValueIfOnlyBottomIsAllowed;
    }
}

internal class RootChainItem : IChainItem
{
    public int GetMaxValueAllowOnlyTop()
    {
        return 0;
    }

    public int GetMaxValueAllowOnlyBottom()
    {
        return 0;
    }
}

internal interface IChainItem
{
    int GetMaxValueAllowOnlyTop();
    int GetMaxValueAllowOnlyBottom();
}