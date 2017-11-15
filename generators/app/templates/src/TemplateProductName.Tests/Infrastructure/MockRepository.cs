using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TemplateProductName.Domain.Repositories;

namespace TemplateProductName.Tests.Infrastructure
{
    public class MockRepository : IRepository
    {
        public IUnitOfWork UnitOfWork { get { throw new NotImplementedException(); } }

        private readonly Dictionary<Type, ICollection> allEntities = new Dictionary<Type, ICollection>();

        public void WithEntities<T>(ICollection<T> entities)
        {
            var type = typeof(T);

            allEntities[type] = (ICollection)entities;
        }

        public void WithEntities<T>(params T[] entities)
        {
            var type = typeof(T);

            allEntities[type] = entities;
        }


        public T Get<T>(object id) where T : class
        {
            throw new NotImplementedException();
        }

        public T Get<T>(int? id) where T : class
        {
            throw new NotImplementedException();
        }

        public T Get<T>(object id, Type type) where T : class
        {
            throw new NotImplementedException();
        }

        public IEnumerable<T> List<T>() where T : class
        {
            if (allEntities.ContainsKey(typeof(T)))
            {
                return (IEnumerable<T>)allEntities[typeof(T)];
            }

            return Enumerable.Empty<T>().AsQueryable();
        }

        public IEnumerable<T> List<T>(Type type) where T : class
        {
            throw new NotImplementedException();
        }

        public IQueryable<T> Query<T>() where T : class
        {
            if (allEntities.ContainsKey(typeof(T)))
            {
                return ((IEnumerable<T>)allEntities[typeof(T)]).AsQueryable();
            }

            return Enumerable.Empty<T>().AsQueryable();
        }

        public IQueryable<T> Query<T>(Type type) where T : class
        {
            throw new NotImplementedException();
        }

        public void Add<T>(T entity) where T : class
        {
            throw new NotImplementedException();
        }

        public void Remove<T>(T entity) where T : class
        {
            throw new NotImplementedException();
        }
    }
}
