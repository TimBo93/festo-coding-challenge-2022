namespace Episode1Start;

class Episode1Start
{
    public static void Main()
    {
        var sus1 = new Episode1Puzzle1.EpisodePuzzle1().SolveForSusPersons().Select(x => x.Name);
        var sus2 = new Episode1Puzzle2.EpisodePuzzle2().SolveForSusPersons().Select(x => x.Name);
        var sus3 = new Episode1Puzzle3.EpisodePuzzle3().SolveForSusPersons().Select(x => x.Name);

        var rogue = sus1.Intersect(sus2).Intersect(sus3);
        Console.WriteLine(rogue.First());
    }
}