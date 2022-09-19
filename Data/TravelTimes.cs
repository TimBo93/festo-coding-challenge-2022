namespace Data;

public class TravelTimes
{
    public IReadOnlyList<TravelTime> ReadFromFile()
    {
        var result = new List<TravelTime>();
        var lines = File.ReadAllLines("travel_times.txt");

        foreach (var line in lines)
        {
            var split = line.Split(":");
            var cityName = split[0].TrimEnd();
            var travelTime = int.Parse(split[1].TrimStart().TrimEnd());
            result.Add(new TravelTime(cityName, travelTime));
        }

        return result;
    }
}

public class TravelTime
{
    public TravelTime(string city, int time)
    {
        City = city;
        Time = time;
    }

    public string City { get; }

    public int Time { get; }
}