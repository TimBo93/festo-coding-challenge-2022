using System.Collections.Immutable;
using System.Globalization;
using CsvHelper;

namespace OfficeDatabase
{
    public class OfficeDatabase
    {
        public IImmutableList<OfficeDatabaseModel> ReadFromCsv()
        {
            using var reader = new StreamReader("office_database.txt");
            using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);
            return csv.GetRecords<OfficeDatabaseModel>().ToImmutableList();
        }
    }
}