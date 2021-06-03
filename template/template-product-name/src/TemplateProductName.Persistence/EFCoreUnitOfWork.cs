using Microsoft.EntityFrameworkCore;
using TemplateProductName.Domain.Repositories;

namespace TemplateProductName.Persistence
{
    public class EFCoreUnitOfWork : IUnitOfWork
    {
        private readonly DbContext context;

        public EFCoreUnitOfWork(DbContext context) => this.context = context;

        public void Complete() => context.Database.CurrentTransaction.Commit();

        public void Rollback() => context.Database.CurrentTransaction.Rollback();
    }
}
