namespace Episode2Start;

internal class Program
{
    static void Main(string[] args)
    {
        var sus1 = new Episode2Puzzle1.Episode2Puzzle1().SolveForSusPersons().Select(x => x.Name);
        var sus2 = new Episode2Puzzle2.Episode2Puzzle2().SolveForSusPersons().Select(x => x.Name);
        var sus3 = new Episode2Puzzle3.Episode2Puzzle3().SolveForSusPersons().Select(x => x.Name);

        var rogue = sus1.Intersect(sus2).Intersect(sus3);
        Console.WriteLine(rogue.First());
    }
}