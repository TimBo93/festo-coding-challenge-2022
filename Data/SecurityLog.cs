using OfficeDatabase;

namespace Data;

public class SecurityLog
{
    public IReadOnlyList<SecurityLogRecord> ParseSecurityLog()
    {
        var securityLogs = new List<SecurityLogRecord>();
        var lines = File.ReadAllLines("security_log.txt");


        var currentPlace = "";
        FestoDateTime currentTime = null;
        foreach (var line in lines)
        {
            if (line.StartsWith("Place:"))
            {
                currentPlace = line.Split(":")[1].TrimStart();
                continue;
            }

            if (FestoDateTime.TryParse(line, out var parsedFestoDateTime))
            {
                currentTime = parsedFestoDateTime!;
                continue;
            }

            if (line.StartsWith("in:"))
            {
                var namePart = line.Split(":")[1];
                var names = namePart.Split(",");
                foreach (var name in names)
                {
                    securityLogs.Add(new SecurityLogRecord(EntryMode.In, currentPlace, currentTime!,
                        name.TrimStart().TrimStart()));
                }
            }

            if (line.StartsWith("out:"))
            {
                var namePart = line.Split(":")[1];
                var names = namePart.Split(",");
                foreach (var name in names)
                {
                    securityLogs.Add(new SecurityLogRecord(EntryMode.Out, currentPlace, currentTime!,
                        name.TrimStart().TrimStart()));
                }
            }
        }

        return securityLogs;
    }
}