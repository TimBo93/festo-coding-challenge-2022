using Data;

namespace Base;

public interface IEpisodePuzzleSolver
{
    IEnumerable<Person> SolveForSusPersons();
}
