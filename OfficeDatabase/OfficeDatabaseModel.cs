using CsvHelper.Configuration.Attributes;

namespace OfficeDatabase;

public class OfficeDatabaseModel
{
    [Index(0)]
    public string Username { get; set; } = null!;

    [Index(1)]
    public long Id { get; set; }


    [Index(2)]
    public long AccessKey { get; set; }


    [Index(3)]
    public string FirstLoginTime { get; set; } = null!;
}