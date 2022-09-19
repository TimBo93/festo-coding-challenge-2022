namespace Data;

public class BloodSample
{
    private readonly List<string> _bloodSample;

    public BloodSample(List<string> bloodSample)
    {
        _bloodSample = bloodSample;
    }

    public bool IsPicoGen1()
    {
        var reversePico = "ocip";
        return HorizontalStrings.Any(x => x.Contains("pico")) || HorizontalStrings.Any(x => x.Contains(reversePico)) ||
               VerticalStrings.Any(x => x.Contains("pico")) || VerticalStrings.Any(x => x.Contains(reversePico));
    }

    public bool IsPicoGen2()
    {
        var searchString = "picoico";


        for (int x = 0; x < Width; x++)
        {
            for (int y = 0; y < Height; y++)
            {
                if (Search(searchString, x, y))
                {
                    return true;
                }
            }
        }

        return false;
    }

    private int Width => HorizontalStrings.First().Length;
    private int Height => VerticalStrings.First().Length;

    private string GetAt(int posX, int posY)
    {
        return _bloodSample[posY].Substring(posX, 1);
    }

    private bool Search(string currentSearchString, int posX, int posY)
    {
        if (currentSearchString.Length == 0)
        {
            return true;
        }

        var head = currentSearchString[0].ToString();
        if (head != GetAt(posX, posY))
        {
            return false;
        }

        var body = currentSearchString.Substring(1);


        // left
        if (posX > 0 && Search(body, posX - 1, posY))
        {
            return true;
        }

        // right
        if (posX < Width - 1 && Search(body, posX + 1, posY))
        {
            return true;
        }

        // top
        if (posY > 0 && Search(body, posX, posY - 1))
        {
            return true;
        }

        // bottom
        if (posY < Height - 1 && Search(body, posX, posY + 1))
        {
            return true;
        }

        return false;
    }

    private IEnumerable<string> HorizontalStrings => _bloodSample.ToList();

    private IEnumerable<string> VerticalStrings
    {
        get
        {
            var numCharsPerLine = _bloodSample[0].Length;
            for (var i = 0; i < numCharsPerLine; i++)
            {
                var verticalString = "";
                foreach (var sample in _bloodSample)
                {
                    verticalString += sample[i];
                }

                yield return verticalString;
            }
        }
    }

    public bool IsPicoGen3()
    {
        var fragmentsPic = this.FindAllFragments("pic").ToList();
        var fragmentsOpi = this.FindAllFragments("opi").ToList();
        var fragmentsCop = this.FindAllFragments("cop").ToList();
        var fragmentsIco = this.FindAllFragments("ico").ToList();

        if (!fragmentsPic.Any()
            || !fragmentsOpi.Any()
            || !fragmentsCop.Any()
            || !fragmentsIco.Any())
        {
            return false;
        }

        return fragmentsPic
            .SelectMany(x => fragmentsOpi.Where(f => !f.Intersects(x)),
                (fragment1, fragment2) => fragment1.UnionWith(fragment2))
            .SelectMany(x => fragmentsCop.Where(f => !f.Intersects(x)),
                (fragment1, fragment2) => fragment1.UnionWith(fragment2))
            .SelectMany(x => fragmentsIco.Where(f => !f.Intersects(x)),
                (fragment1, fragment2) => fragment1.UnionWith(fragment2))
            .Any();
    }

    private IEnumerable<Fragment> FindAllFragments(string searchString)
    {
        var fragments = new List<Fragment>();
        for (int x = 0; x < Width; x++)
        {
            for (int y = 0; y < Height; y++)
            {
                SearchFragments(searchString, x, y, new List<(int, int)>(), fragments);
            }
        }

        return fragments;
    }

    private void SearchFragments(string currentSearchString, int posX, int posY, IReadOnlyList<(int, int)> usedFields,
        List<Fragment> foundFragments)
    {
        var history = usedFields.ToList();
        history.Add((posX, posY));

        var head = currentSearchString[0].ToString();
        if (head != GetAt(posX, posY))
        {
            return;
        }

        var body = currentSearchString.Substring(1);
        if (body == "")
        {
            foundFragments.Add(new Fragment(history));
            return;
        }

        // left
        if (posX > 0)
        {
            SearchFragments(body, posX - 1, posY, history.ToList(), foundFragments);
        }

        // right
        if (posX < Width - 1)
        {
            SearchFragments(body, posX + 1, posY, history.ToList(), foundFragments);
        }

        // top
        if (posY > 0)
        {
            SearchFragments(body, posX, posY - 1, history.ToList(), foundFragments);
        }

        // bottom
        if (posY < Height - 1)
        {
            SearchFragments(body, posX, posY + 1, history.ToList(), foundFragments);
        }
    }
}

class Fragment
{
    public Fragment(IReadOnlyList<(int, int)> usedFields)
    {
        UsedFields = usedFields;
    }

    public IReadOnlyList<(int, int)> UsedFields { get; }

    public Fragment UnionWith(Fragment other)
    {
        return new Fragment(UsedFields.Union(other.UsedFields).ToList());
    }

    public bool Intersects(Fragment other)
    {
        return UsedFields.Any(x => other.UsedFields.Contains(x));
    }
}