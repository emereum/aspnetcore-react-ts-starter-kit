using System.Collections.Generic;
using System.Data;

namespace TemplateProductName.Persistence
{
    public static class DataReaderExtensions
    {
        public static IEnumerable<IDataRecord> AsEnumerable(this IDataReader reader)
        {
            while (reader.Read())
            {
                yield return reader;
            }
        }
    }
}
