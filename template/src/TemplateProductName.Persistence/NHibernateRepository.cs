using System;
using System.Collections.Generic;
using System.Linq;
using NHibernate;
using TemplateProductName.Domain.Repositories;

namespace TemplateProductName.Persistence
{
    public class NHibernateRepository : IRepository
    {
        public IUnitOfWork UnitOfWork { get; }
        private readonly ISession session;

        public NHibernateRepository(ISession session)
        {
            this.session = session;
            UnitOfWork = new NHibernateUnitOfWork(session);
        }

        public T Get<T>(object id) where T : class
        {
            if (id == null)
            {
                return null;
            }

            return session.Get<T>(id);
        }

        public T Get<T>(int? id) where T : class
        {
            if (!id.HasValue)
            {
                return null;
            }

            return session.Get<T>(id.Value);
        }

        public T Get<T>(object id, Type type) where T : class => (T)session.Get(type, id);

        public IEnumerable<T> List<T>() where T : class => session.Query<T>().AsEnumerable();

        public IEnumerable<T> List<T>(Type type) where T : class => session.CreateCriteria(type).List<T>();

        public IQueryable<T> Query<T>() where T : class => session.Query<T>();

        /// <summary>
        /// Query the specified runtime type. Can be used when querying via a
        /// base class. Specify the base class as the generic type then the
        /// concrete type as the first parameter. This is done through
        /// reflection and may not be performant.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="type"></param>
        /// <returns></returns>
        public IQueryable<T> Query<T>(Type type) where T : class
        {
            var closedMethod = typeof(NHibernateRepository)
                .GetMethods()
                .Single(x => x.Name == "Query" && !x.GetParameters().Any())
                .MakeGenericMethod(type);

            return (IQueryable<T>)closedMethod.Invoke(this, new object[0]);
        }

        public void Add<T>(T entity) where T : class => session.Save(entity);

        public void Remove<T>(T entity) where T : class => session.Delete(entity);
    }
}
