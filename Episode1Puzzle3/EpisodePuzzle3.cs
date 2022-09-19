using Base;
using Data;

namespace Episode1Puzzle3;

public class EpisodePuzzle3 : IEpisodePuzzleSolver
{
    public static void Main()
    {
        Console.WriteLine(new EpisodePuzzle3().SolveForSusPersons().Sum(x => x.Id));
    }

    public static bool CheckIfHistoryMatchesSuspiciousSequence(IEnumerable<SecurityLogRecord> personHistory,
        IEnumerable<string> suspiciousPlaceSequence)
    {
        var personHistoryList = personHistory.ToList();
        var suspiciousPlacesList = suspiciousPlaceSequence.ToList();

        if (personHistoryList.Count != suspiciousPlacesList.Count * 2)
        {
            return false;
        }

        for (int i = 0; i < suspiciousPlacesList.Count; i++)
        {
            var logItemIn = personHistoryList[2 * i];
            if (logItemIn.EntryMode != EntryMode.In || logItemIn.Place != suspiciousPlacesList[i])
            {
                return false;
            }


            var logItemOut = personHistoryList[2 * i + 1];
            if (logItemOut.EntryMode != EntryMode.Out || logItemOut.Place != suspiciousPlacesList[i])
            {
                return false;
            }
        }

        return true;
    }

    public IEnumerable<Person> SolveForSusPersons()
    {
        var suspiciousPlacesSequence = new PlaceSequence().GetPlaceSequence();
        var securityLogRecords = new SecurityLog().ParseSecurityLog();

        var placesPerPerson = securityLogRecords.GroupBy(x => x.Name);

        var susPersons = placesPerPerson.Where(logRecords =>
        {
            var history = logRecords.OrderBy(x => x.Time).ToList();
            return CheckIfHistoryMatchesSuspiciousSequence(history, suspiciousPlacesSequence);
        }).Select(x => x.Key).ToList();

        var population = new Population().ReadFromFile();

        return susPersons.Select(x => population.First(person => person.Name == x));
    }
}