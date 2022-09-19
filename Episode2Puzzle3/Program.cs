using Base;
using Data;

namespace Episode2Puzzle3;

public class Episode2Puzzle3 : IEpisodePuzzleSolver
{
    public static void Main()
    {
        Console.WriteLine(new Episode2Puzzle3().SolveForSusPersons().Sum(x => x.Id));
    }

    private bool IsRelevantPerson(IGrouping<string, SecurityLogRecord> logByPerson)
    {
        var materializedList = logByPerson.OrderBy(x => x.Time).ToList();
        var timeSlots = GetTimeSlots(materializedList).ToList();

        if (timeSlots.Contains(79)) // trivial solution
        {
            return true;
        }

        return SolveCombinations(timeSlots, 0);
    }

    private IEnumerable<int> GetTimeSlots(List<SecurityLogRecord> log)
    {
        for (int i = 0; i < log.Count; i += 2)
        {
            var from = log[i];
            var to = log[i + 1];

            yield return to.Time - from.Time;
        }
    }

    private bool SolveCombinations(IReadOnlyList<int> currentItems, int currentGal)
    {
        if (currentGal == 79)
        {
            return true;
        }

        if (currentGal > 79)
        {
            return false;
        }

        if (currentItems.Count == 0)
        {
            return false;
        }

        var head = currentItems.First();
        var body = currentItems.Skip(1).ToList();

        return SolveCombinations(body, currentGal + head) // this slot was "work"
               || SolveCombinations(body, currentGal); // this slot was "free"
    }

    public IEnumerable<Person> SolveForSusPersons()
    {
        var securityLogs = new SecurityLog().ParseSecurityLog();
        var logByPerson = securityLogs.GroupBy(x => x.Name);

        var susPersonNames = logByPerson.Where(IsRelevantPerson).Select(x => x.Key).ToList();

        return new Population().ReadFromFile().Where(x => susPersonNames.Contains(x.Name));
    }
}