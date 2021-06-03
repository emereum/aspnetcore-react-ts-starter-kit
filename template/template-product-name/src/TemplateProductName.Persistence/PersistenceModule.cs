using System.Data;
using System.Diagnostics.CodeAnalysis;
using Autofac;
using Autofac.Core;
using Microsoft.EntityFrameworkCore;
using TemplateProductName.Domain.Repositories;

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
            // DbContext
            builder.Register(x =>
                {
                    var optionsBuilder = new DbContextOptionsBuilder<TemplateProductNameDbContext>();
                    optionsBuilder.UseNpgsql(connectionString);
                    return new TemplateProductNameDbContext(optionsBuilder.Options, mappingAssembly);
                })
                .As<DbContext>()
                .InstancePerLifetimeScope()
                .ExternallyOwned()
                .OnActivated(BeginSession)
                .OnRelease(EndSession);

            // IDbConnection (exposed because there may be cases where we need
            // to use Dapper)
            builder.Register(x => x.Resolve<DbContext>().Database.GetDbConnection())
                .As<IDbConnection>()
                .InstancePerLifetimeScope()
                .ExternallyOwned();

            // IUnitOfWork
            builder.RegisterType<EFCoreUnitOfWork>()
                .As<IUnitOfWork>()
                .InstancePerLifetimeScope()
                .ExternallyOwned();

            // IRepository
            builder.RegisterType<EFCoreRepository>()
                .As<IRepository>()
                .InstancePerLifetimeScope();
        }

        private void BeginSession(IActivatedEventArgs<DbContext> args)
        {
            var context = args.Instance;
            context.Database.BeginTransaction();
        }

        /// <summary>
        /// Closes an open session. If the session contains an active
        /// transaction, this method will attempt to commit it. If unsuccessful,
        /// the transaction will be rolled back. This method always disposes the
        /// supplied session object, regardless of whether or not the commit is
        /// successful.
        /// </summary>
        /// <param name="session">The session to close.</param>
        private void EndSession(DbContext context)
        {
            try
            {
                context.SaveChanges();

                // If the user hasn't closed the transaction through an
                // IUnitOfWork, close it now.
                if (context.Database.CurrentTransaction != null)
                {
                    context.Database.CurrentTransaction.Commit();
                }
            }
            catch
            {
                context.Database.CurrentTransaction.Rollback();
                throw;
            }
            finally
            {
                context.Dispose();
            }
        }
    }
}
