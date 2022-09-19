using System.Collections.Immutable;
using Base;
using Data;
using OfficeDatabase;

namespace Episode3Puzzle3;

public class Episode3Puzzle3 : IEpisodePuzzleSolver
{
    private readonly FestoDateTime _crimeStart = new(11, 0);
    private readonly FestoDateTime _crimeEnd = new(13, 0);


    static void Main(string[] args)
    {
        Console.WriteLine(new Episode3Puzzle3().SolveForSusPersons().Sum(x => x.Id));
    }

    public IEnumerable<Person> SolveForSusPersons()
    {
        var population = new Population().ReadFromFile();
        var personsWithoutAlibi = GetAllPersonNamesWithoutAlibi().ToList();
        return population.Where(x => personsWithoutAlibi.Contains(x.Name));
    }

    private IEnumerable<string> GetAllPersonNamesWithoutAlibi()
    {
        // this is a dummy node.
        // Because we do not know where the subject was before the first and after the last log entry.
        const string dummyPlace = "DUMMY-PLACE";
        var dummyPlaceTravelTime = new TravelTime(dummyPlace, 0);
        var dummySecurityLogRecordOut =
            new SecurityLogRecord(EntryMode.Out, dummyPlace, new FestoDateTime(0, 0), "any");
        var dummySecurityLogRecordIn =
            new SecurityLogRecord(EntryMode.In, dummyPlace, new FestoDateTime(23, 59), "any");

        var log = new SecurityLog().ParseSecurityLog();


        var travelTimes = new TravelTimes().ReadFromFile();
        var travelTimeWithDummy = new List<TravelTime>();
        travelTimeWithDummy.Add(dummyPlaceTravelTime);
        travelTimeWithDummy.AddRange(travelTimes);

        var travelTimeLibrary = travelTimeWithDummy.ToImmutableDictionary(x => x.City);

        var logByPerson = log.GroupBy(x => x.Name);

        foreach (var personLog in logByPerson)
        {
            var orderedPersonLog = personLog.OrderBy(x => x.Time).ToList();

            var enrichedPersonLog = new List<SecurityLogRecord>();
            enrichedPersonLog.Add(dummySecurityLogRecordOut); // he comes from anywhere
            enrichedPersonLog.AddRange(orderedPersonLog);
            enrichedPersonLog.Add(dummySecurityLogRecordIn); // he goes to anywhere

            if (!CheckAlibi(enrichedPersonLog, travelTimeLibrary))
            {
                yield return personLog.Key;
            }
        }
    }

    private bool CheckAlibi(List<SecurityLogRecord> securityLogRecords,
        ImmutableDictionary<string, TravelTime> travelTimeLibrary)
    {
        for (int i = 0; i < securityLogRecords.Count; i += 2)
        {
            var from = securityLogRecords[i];
            var to = securityLogRecords[i + 1];

            if (CheckIfCrimeCouldHappenInBetween(from, to, travelTimeLibrary))
            {
                return false;
            }
        }

        return true;
    }

    private bool CheckIfCrimeCouldHappenInBetween(SecurityLogRecord from, SecurityLogRecord to,
        IImmutableDictionary<string, TravelTime> travelTimeLibrary)
    {
        var earliestArrival = from.Time.AddMinutes(travelTimeLibrary[from.Place].Time);
        if (earliestArrival.CompareTo(_crimeStart) < 0)
        {
            earliestArrival = _crimeStart;
        }

        var latestCheckout = to.Time.AddMinutes(-travelTimeLibrary[to.Place].Time);
        if (latestCheckout.CompareTo(_crimeEnd) > 0)
        {
            latestCheckout = _crimeEnd;
        }

        var diff = latestCheckout - earliestArrival;

        return diff >= 20;
    }
}