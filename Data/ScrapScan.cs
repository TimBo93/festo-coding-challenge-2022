using MathNet.Numerics.Distributions;

namespace Data;

public class ScrapScan
{
    public IEnumerable<Passage> ReadFromFile()
    {
        var lines = File.ReadAllLines("scrap_scan.txt");

        int passage = -1;
        var currentPassageItems = new List<Item>();
        foreach (var line in lines)
        {
            if (string.IsNullOrWhiteSpace(line))
            {
                var createdPassage = new Passage(passage, currentPassageItems.ToList());
                yield return createdPassage;
                currentPassageItems = new ();
                continue;
            }

            if (line.Contains("Passage"))
            {
                passage = int.Parse(line.Split(" ")[1]);
                continue;
            }

            var split = line.Split(":");
            var itemName = split[0];
            var blockedByItems = split[1].Split(",").Select(x => x.TrimStart().TrimEnd());
            currentPassageItems.Add(new Item(itemName, blockedByItems.ToList()));
        }
    }
}

public class Passage
{
    public Passage(int id, IReadOnlyList<Item> items)
    {
        Id = id;
        Items = items;
    }

    public int Id { get; }
    public IReadOnlyList<Item> Items { get; }
}

public class Item
{
    public string Name { get; }
    public IReadOnlyList<string> BlockedBy { get; }

    public Item(string name, IReadOnlyList<string> blockedBy)
    {
        Name = name;
        BlockedBy = blockedBy;
    }
}