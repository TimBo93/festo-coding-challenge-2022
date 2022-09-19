namespace Data
{
    public class SignalRanging
    {
        public IReadOnlyList<SignalRange> ReadFromFile()
        {
            List<SignalRange> signalRanges = new();
            var lines = File.ReadAllLines("signal_ranging.txt");
            foreach (var line in lines)
            {
                var split = line.Split(":");
                var planet = split[0];
                var delay = split[1].TrimStart();
                signalRanges.Add(new SignalRange()
                {
                    Planet = planet,
                    Delay = int.Parse(delay)
                });
            }
            return signalRanges;
        }
    }
}
