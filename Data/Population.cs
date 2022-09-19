namespace Data;

public class Population
{
    public IReadOnlyList<Person> ReadFromFile()
    {
        var resultSet = new List<Person>();

        var lines = File.ReadAllLines("population.txt");

        PlainTextPerson? plainTextPerson = null;
        foreach (var line in lines)
        {
            if (line.Contains("Name:"))
            {
                plainTextPerson = new PlainTextPerson();
                plainTextPerson.Name = line.Split(":")[1].TrimStart();
                continue;
            }

            if (line.Contains("ID:"))
            {
                plainTextPerson!.Id = line.Split(":")[1].TrimStart();
                continue;
            }

            if (line.Contains("Home Planet"))
            {
                plainTextPerson!.Home = line.Split(":")[1].TrimStart();
                continue;
            }

            if (line.Contains("|"))
            {
                plainTextPerson!.BloodSample.Add(line.Split("|")[1]);
                continue;
            }

            if (string.IsNullOrEmpty(line) && plainTextPerson != null)
            {
                resultSet.Add(plainTextPerson.ToPerson());
                plainTextPerson = null;
            }
        }

        return resultSet;
    }
}