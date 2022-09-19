using Base;
using Data;

namespace Episode1Puzzle2;

public class EpisodePuzzle2 : IEpisodePuzzleSolver
{

    public static void Main()
    {
        Console.WriteLine(new EpisodePuzzle2().SolveForSusPersons().Sum(p => p.Id));
    }

    public IEnumerable<Person> SolveForSusPersons()
    {

        var galaxies = new GalaxyMap().ReadFromFile();

        // Ansatz mit RANSAC (https://de.wikipedia.org/wiki/RANSAC-Algorithmus)
        var ransac = new Ransac(galaxies);
        var discriminator = ransac.Calculate();

        var population = new Population().ReadFromFile();

        return discriminator.Outlier.Select(x => population.Where(p => x.Name == p.Home)).SelectMany(x => x);
    }
}

