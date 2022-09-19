namespace Data;

internal class PlainTextPerson
{
    public string Name { get; set; } = null!;
    public string Id { get; set; } = null!;
    public string Home { get; set; } = null!;
    public List<string> BloodSample { get; } = new();

    public Person ToPerson()
    {
        return new Person(Name, long.Parse(Id), Home, new BloodSample(BloodSample));
    }
}