using Base;

namespace Intro1Puzzle2;

public class Puzzle2
{
    public static readonly IntroPuzzlePredicate Predicate = x => (x.AccessKey & 8) != 0;

    public static void Main()
    {
        Console.WriteLine(new OfficeDatabase.OfficeDatabase().ReadFromCsv()
            .Where(x => Predicate(x))
            .Sum(x => x.Id));
    }
}