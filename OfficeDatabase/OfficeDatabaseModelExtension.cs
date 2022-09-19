namespace OfficeDatabase;

public static class OfficeDatabaseModelExtension
{
    public static FestoDateTime ToFestoDateTime(this OfficeDatabaseModel model)
    {
        var parts = model.FirstLoginTime.Split(":");
        var hour = Convert.ToInt32(parts[0]);
        var minute = Convert.ToInt32(parts[1]);
        return new FestoDateTime(hour, minute);

    }
}