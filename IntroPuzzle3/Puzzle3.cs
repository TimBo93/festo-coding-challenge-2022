using Base;
using OfficeDatabase;

namespace Intro1Puzzle3;

public class Puzzle3
{
    private static readonly FestoDateTime Threshold = new FestoDateTime(7, 14); 
    public static readonly IntroPuzzlePredicate Predicate = x => x.ToFestoDateTime().CompareTo(Threshold) < 0;

    public static void Main()
    {
        Console.WriteLine(new OfficeDatabase.OfficeDatabase().ReadFromCsv()
            .Where(x => Predicate(x))
            .Sum(x => x.Id));
    }
}