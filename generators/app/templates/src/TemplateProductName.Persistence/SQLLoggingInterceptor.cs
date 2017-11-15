using System.Diagnostics;
using NHibernate;
using NHibernate.SqlCommand;

namespace TemplateProductName.Persistence
{
    public class SqlLoggingInterceptor : EmptyInterceptor
    {
        public override SqlString OnPrepareStatement(SqlString sql)
        {
            Debug.WriteLine($"SqlLoggingInterceptor: {sql}");
            return sql;
        }
    }
}
