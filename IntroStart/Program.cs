// See https://aka.ms/new-console-template for more information


using Intro1Puzzle1;
using Intro1Puzzle2;
using Intro1Puzzle3;

foreach (var thief in new OfficeDatabase.OfficeDatabase().ReadFromCsv()
             .Where(x => Puzzle1.Predicate(x))
             .Where(x => Puzzle2.Predicate(x))
             .Where(x => Puzzle3.Predicate(x)))
{
    Console.WriteLine(thief.Username);
}