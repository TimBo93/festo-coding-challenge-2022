using Data;

namespace Episode1Puzzle2;

internal class GalaxySample
{
    public GalaxySample(Galaxy galaxy1, Galaxy galaxy2, Galaxy galaxy3)
    {
        Galaxy1 = galaxy1;
        Galaxy2 = galaxy2;
        Galaxy3 = galaxy3;
    }

    public Galaxy Galaxy1 { get; }
    public Galaxy Galaxy2 { get; }
    public Galaxy Galaxy3 { get; }
}