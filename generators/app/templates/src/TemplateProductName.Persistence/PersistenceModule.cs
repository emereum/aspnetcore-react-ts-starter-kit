using System.Data;
using System.Diagnostics.CodeAnalysis;
using Autofac;
using Autofac.Core;
using NHibernate;
using TemplateProductName.Domain.Repositories;
using TemplateProductName.Persistence.PostgreSQL;

namespace TemplateProductName.Persistence
{
    [ExcludeFromCodeCoverage]
    public class PersistenceModule : Module
    {
        private readonly string connectionString;
        private readonly string mappingAssembly;

        public PersistenceModule(string connectionString, string mappingAssembly)
        {
            this.connectionString = connectionString;
            this.mappingAssembly = mappingAssembly;
        }

        protected override void Load(ContainerBuilder builder)
        {
            // Session factory builder
            builder.Register(x => PostgreSqlSessionFactoryBuilder.Build(connectionString, mappingAssembly))
                .As<ISessionFactory>()
                .SingleInstance();

            // ISession
            builder.Register(x =>
                {
                    var session = x.Resolve<ISessionFactory>().OpenSession();
                    session.FlushMode = FlushMode.Commit;
                    return session;
                })
                .As<ISession>()
                .InstancePerLifetimeScope()
                .ExternallyOwned()
                .OnActivated(BeginSession)
                .OnRelease(EndSession);

            // IDbConnection (exposed because there may be cases where we need
            // to use Dapper)
            builder.Register(x => x.Resolve<ISession>().Connection)
                .As<IDbConnection>()
                .InstancePerLifetimeScope()
                .ExternallyOwned();

            // IUnitOfWork
            builder.RegisterType<NHibernateUnitOfWork>()
                .As<IUnitOfWork>()
                .InstancePerLifetimeScope()
                .ExternallyOwned();

            // IRepository
            builder.RegisterType<NHibernateRepository>()
                .As<IRepository>()
                .InstancePerLifetimeScope();
        }

        private void BeginSession(IActivatedEventArgs<ISession> args)
        {
            var session = args.Instance;
            session.BeginTransaction();
        }

        /// <summary>
        /// Closes an open session. If the session contains an active
        /// transaction, this method will attempt to commit it. If unsuccessful,
        /// the transaction will be rolled back. This method always disposes the
        /// supplied session object, regardless of whether or not the commit is
        /// successful.
        /// </summary>
        /// <param name="session">The session to close.</param>
        private void EndSession(ISession session)
        {
            try
            {
                // If the user hasn't closed the transaction through an
                // IUnitOfWork, close it now.
                if (session.Transaction.IsActive)
                {
                    session.Transaction.Commit();
                }
            }
            catch
            {
                session.Transaction.Rollback();
                throw;
            }
            finally
            {
                session.Close();
                session.Dispose();
            }
        }
    }
}
