namespace Data;

public class PlaceSequence
{
    public IReadOnlyList<string> GetPlaceSequence()
    {
        return File.ReadAllLines("place_sequence.txt");
    }
}