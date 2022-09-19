using Data;

namespace FinalEpisodePuzzle1;

internal class FinalEpisodePuzzle1
{
    public static void Main()
    {
        Console.WriteLine(new FinalEpisodePuzzle1().Solve());
    }

    private string Solve()
    {
        var passages = new ScrapScan().ReadFromFile().ToList();

        return passages.Where(x => CanFreePassage(x)).Select(x => x.Id).OrderBy(x => x).Select(x => x.ToString())
            .Aggregate((a, b) => $"{a}-{b}");
    }

    public bool CanFreePassage(Passage passage)
    {
        var removableItems = new HashSet<string>();

        var stacks = new List<(string, Stack<string>)>();
        foreach (var item in passage.Items)
        {
            if (item.BlockedBy.Count == 1 && item.BlockedBy[0] == "-")
            {
                removableItems.Add(item.Name);
                continue;
            }

            var itemStack = new Stack<string>();
            foreach (var blockedBy in item.BlockedBy) itemStack.Push(blockedBy);
            stacks.Add((item.Name, itemStack));
        }

        bool found;
        do
        {
            found = false;
            foreach (var (name, stack) in stacks.Where(x =>
                             x switch
                             {
                                 var (_, s) => s.Count > 0
                             })
                         .Where(x => x switch
                         {
                             var (_, s) => removableItems.Contains(s.Peek())
                         }))
            {
                var removedItem = stack.Pop();
                if (stack.Count == 0) removableItems.Add(name);
                found = true;
            }
        } while (found);

        return stacks.All(x => x.Item2.Count == 0);
    }
}