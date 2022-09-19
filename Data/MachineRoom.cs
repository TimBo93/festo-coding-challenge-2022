namespace Data;

public class MachineRoom
{
    public IEnumerable<Cable> ReadFromFile()
    {
        var lines = File.ReadAllLines("machine_room.txt");

        foreach (var line in lines)
        {
            if (string.IsNullOrEmpty(line))
            {
                continue;
            }

            var parts = line.Split(':');
            var id = int.Parse(parts[0]);
            var fromTo = parts[1].Split("-");
            var from = fromTo[0].TrimStart().TrimEnd();
            var to = fromTo[1].TrimStart().TrimEnd();
            var thickness = int.Parse(parts[2].TrimStart().TrimEnd());

            yield return new Cable(id, from, to, thickness);
        }
    }
}

public class Cable
{
    public Cable(int id, string from, string to, int thickness)
    {
        Id = id;
        From = from;
        To = to;
        Thickness = thickness;
    }

    public int Id { get; }
    public string From { get; }
    public string To { get; }
    public int Thickness { get; }
}