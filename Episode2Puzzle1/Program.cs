using Base;
using Data;

namespace Episode2Puzzle1;

public class Episode2Puzzle1 : IEpisodePuzzleSolver
{
    public static void Main()
    {
        Console.WriteLine(new Episode2Puzzle1().SolveForSusPersons().Sum(x => x.Id));
    }

    public IEnumerable<Person> SolveForSusPersons()
    {
        var population = new Population().ReadFromFile();
        return population.Where(p => p.BloodSample.IsPicoGen2());
    }
}