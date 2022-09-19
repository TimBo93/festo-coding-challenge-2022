namespace Data;

public class Galaxy
{
    public Galaxy(string name, int x, int y, int z)
    {
        Name = name;
        X = x;
        Y = y;
        Z = z;
    }

    public int X { get; }
    public int Y { get; }
    public int Z { get; }
    public string Name { get; }
}