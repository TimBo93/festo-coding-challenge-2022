// See https://aka.ms/new-console-template for more information

using Base;

namespace Intro1Puzzle1;

public class Puzzle1
{
    public static readonly IntroPuzzlePredicate Predicate = x => x.Id.ToString().Contains("814");

    public static void Main()
    {
        Console.WriteLine(new OfficeDatabase.OfficeDatabase().ReadFromCsv()
            .Where(x => Predicate(x))
            .Sum(x => x.Id));
    }
}