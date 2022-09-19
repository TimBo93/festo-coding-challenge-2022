using Base;
using Data;

namespace Episode3Puzzle1;

public class Episode3Puzzle1 : IEpisodePuzzleSolver
{
    static void Main(string[] args)
    {
        Console.WriteLine(new Episode3Puzzle1().SolveForSusPersons().Sum(x => x.Id));
    }

    public IEnumerable<Person> SolveForSusPersons()
    {
        return new Population().ReadFromFile().Where(x => x.BloodSample.IsPicoGen3());
    }
}