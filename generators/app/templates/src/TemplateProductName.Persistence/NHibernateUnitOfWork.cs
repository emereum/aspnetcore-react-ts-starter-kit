using NHibernate;
using TemplateProductName.Domain.Repositories;

namespace TemplateProductName.Persistence
{
    class NHibernateUnitOfWork : IUnitOfWork
    {
        private readonly ISession session;

        public NHibernateUnitOfWork(ISession session)
        {
            this.session = session;
        }

        public void Complete()
        {
            session.Transaction.Commit();
        }

        public void Rollback()
        {
            session.Transaction.Rollback();
        }
    }
}
