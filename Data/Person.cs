namespace Data;

public class Person
{
    public Person(string name, long id, string home, BloodSample bloodSample)
    {
        Name = name;
        Id = id;
        Home = home;
        BloodSample = bloodSample;
    }

    public string Name { get; }
    public long Id { get; }
    public string Home { get; }
    public BloodSample BloodSample { get; }
}