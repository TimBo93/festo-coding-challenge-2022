using OfficeDatabase;

namespace Data;

public class SecurityLogRecord
{
    public SecurityLogRecord(EntryMode entryMode, string place, FestoDateTime time, string name)
    {
        EntryMode = entryMode;
        Place = place;
        Time = time;
        Name = name;
    }

    public FestoDateTime Time { get; }

    public EntryMode EntryMode { get; }

    public string Place { get; }

    public string Name { get; }
}