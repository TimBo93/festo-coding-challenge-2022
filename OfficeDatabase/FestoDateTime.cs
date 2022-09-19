using System.Text.RegularExpressions;
using Microsoft.VisualBasic.CompilerServices;

namespace OfficeDatabase;

public class FestoDateTime : IComparable<FestoDateTime>
{
    private static readonly Regex regex = new Regex("[0-9]+:[0-9]+", RegexOptions.IgnoreCase);

    public static bool TryParse(string input, out FestoDateTime? festoDateTime)
    {
        if (regex.IsMatch(input))
        {
            var split = regex.Matches(input).Select(x => x.Value).ToArray();
            festoDateTime = new FestoDateTime(input);
            return true;
        }

        festoDateTime = null;
        return false;
    }

    public FestoDateTime(string dateString)
    {
        var split = dateString.Split(":");
        Hour = int.Parse(split[0]);
        Minute = int.Parse(split[1]);
    }

    public FestoDateTime(int hour, int minute)
    {
        Hour = hour;
        Minute = minute;
    }

    public int Hour { get; }
    public int Minute { get; }

    public int CompareTo(FestoDateTime? other)
    {
        if (ReferenceEquals(this, other)) return 0;
        if (ReferenceEquals(null, other)) return 1;
        var hourComparison = Hour.CompareTo(other.Hour);
        if (hourComparison != 0) return hourComparison;
        return Minute.CompareTo(other.Minute);
    }

    public static int operator -(FestoDateTime a, FestoDateTime b)
    {
        var timeA = a.Hour * 60 + a.Minute;
        var timeB = b.Hour * 60 + b.Minute;
        return timeA - timeB;
    }

    public int TotalMinutes()
    {
        return Hour * 60 + Minute;
    }

    public FestoDateTime AddMinutes(int minutes)
    {
        var totalMinutes = TotalMinutes() + minutes;

        var hour = (int)Math.Floor(totalMinutes / 60d);
        var minute = totalMinutes % 60;

        return new FestoDateTime(hour, minute);
    }
}