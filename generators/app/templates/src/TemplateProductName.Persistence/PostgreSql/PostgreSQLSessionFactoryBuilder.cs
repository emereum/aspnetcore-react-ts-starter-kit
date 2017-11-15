using System.Reflection;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using NHibernate;
using NHibernate.Cfg;
using TemplateProductName.Persistence.PostgreSQL.Conventions;

namespace TemplateProductName.Persistence.PostgreSQL
{
    internal class PostgreSqlSessionFactoryBuilder
    {
        public static ISessionFactory Build(string connectionString, string mappingAssemblyName)
        {
            var mappingAssembly = Assembly.Load(mappingAssemblyName);

            return Fluently.Configure()
                .Database(PostgreSQLConfiguration.PostgreSQL82.ConnectionString(connectionString))
                .ExposeConfiguration(x => Configure(x, mappingAssembly))
                .Mappings(x =>
                {
                    // Load mappings which define how domain classes are mapped
                    // to the database
                    x.FluentMappings.AddFromAssembly(mappingAssembly);

                    // Customise NHibernate's conventions for constructing a new
                    // schema
                    x.FluentMappings.Conventions.Add<PersistStringsAsTextConvention>();
                    x.FluentMappings.Conventions.Add<PersistEnumsAsIntegerConvention>();
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
