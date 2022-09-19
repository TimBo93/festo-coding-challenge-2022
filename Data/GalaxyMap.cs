namespace Data
{
    public class GalaxyMap
    {
        public IReadOnlyList<Galaxy> ReadFromFile()
        {
            var lines = File.ReadAllLines("galaxy_map.txt");
            return lines.Select(ParseString).ToList();
        }

        private Galaxy ParseString(string line)
        {
            var splitted = line.Split(":");
            var name = splitted[0].TrimEnd();

            var coords = splitted[1].Replace("(", "").Replace(")", "").Split(",");
            int x = Convert.ToInt32(coords[0]);
            int y = Convert.ToInt32(coords[1]);
            int z = Convert.ToInt32(coords[2]);

            return new Galaxy(name, x, y, z);
        }
    }
}
