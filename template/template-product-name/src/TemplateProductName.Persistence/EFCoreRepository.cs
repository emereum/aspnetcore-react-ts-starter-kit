using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using TemplateProductName.Domain.Repositories;

namespace TemplateProductName.Persistence
{
    public class EFCoreRepository : IRepository
    {
        public IUnitOfWork UnitOfWork { get; }
        private readonly DbContext context;

        public EFCoreRepository(DbContext context)
        {
            this.context = context;
            UnitOfWork = new EFCoreUnitOfWork(context);
        }

        public T Get<T>(object id) where T : class => id != null ? context.Set<T>().Find(id) : null;
        public T Get<T>(int? id) where T : class => id.HasValue ? context.Set<T>().Find(id.Value) : null;
        public T Get<T>(object id, Type type) where T : class => throw new NotImplementedException();
        public IEnumerable<T> List<T>() where T : class => context.Set<T>().AsEnumerable();
        public IEnumerable<T> List<T>(Type type) where T : class => throw new NotImplementedException();
        public IQueryable<T> Query<T>() where T : class => context.Set<T>();
        public IQueryable<T> Query<T>(Type type) where T : class => throw new NotImplementedException();
        public void Add<T>(T entity) where T : class => context.Set<T>().Add(entity);
        public void Remove<T>(T entity) where T : class => context.Set<T>().Remove(entity);
    }
}
