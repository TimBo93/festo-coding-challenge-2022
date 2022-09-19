using Base;
using Data;

namespace Episode1Puzzle1;

public class EpisodePuzzle1 : IEpisodePuzzleSolver
{
    public static void Main()
    {
        Console.WriteLine(new EpisodePuzzle1().SolveForSusPersons().Sum(x => x.Id));
    }

    public IEnumerable<Person> SolveForSusPersons()
    {
        var population = new Population().ReadFromFile();
        return population.Where(x => x.BloodSample.IsPicoGen1());
    }
}