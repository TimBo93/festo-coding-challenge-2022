namespace Episode3Start;

internal class Program
{
    static void Main(string[] args)
    {
        var sus1 = new Episode3Puzzle1.Episode3Puzzle1().SolveForSusPersons().Select(x => x.Name);
        //var sus2 = new Episode1Puzzle2.EpisodePuzzle2().SolveForSusPersons().Select(x => x.Name);
        var sus3 = new Episode3Puzzle3.Episode3Puzzle3().SolveForSusPersons().Select(x => x.Name);

        var rogue = sus1.Intersect(sus3);
        Console.WriteLine(rogue.First());
    }
}