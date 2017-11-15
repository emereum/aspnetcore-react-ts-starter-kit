using System.Reflection;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using NHibernate;
using NHibernate.Cfg;

namespace TemplateProductName.Persistence.Oracle
{
    internal class OracleSessionFactoryBuilder
    {
        public static ISessionFactory Build(string connectionString, string mappingAssemblyName)
        {
            var mappingAssembly = Assembly.Load(mappingAssemblyName);

            return Fluently.Configure()
                .Database(OracleManagedDataClientConfiguration.Oracle10.ConnectionString(connectionString))
                .ExposeConfiguration(x => Configure(x, mappingAssembly))
                .Mappings(x =>
                {
                    // Load mappings which define how domain classes are mapped
                    // to the database
                    x.FluentMappings.AddFromAssembly(mappingAssembly);
                })
                .BuildSessionFactory();
        }

        private static void Configure(Configuration configuration, Assembly mappingAssembly)
        {
            // Update database schema
            //new SchemaUpdate(configuration).Execute(false, true);

#if DEBUG
            // Log all SQL queries to the debug output
            configuration.SetInterceptor(new SqlLoggingInterceptor());
#endif

            // Add all listeners from the mapping assembly
            configuration.AddListenersFromAssembly(mappingAssembly);
        }
    }
}
