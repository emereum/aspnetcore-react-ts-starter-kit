using System;
using System.Collections.Generic;
using System.Linq;

namespace TemplateProductName.Domain.Repositories
{
    public interface IRepository
    {
        T Get<T>(object id) where T : class;
        T Get<T>(int? id) where T : class;
        T Get<T>(object id, Type type) where T : class;
        IEnumerable<T> List<T>() where T : class;
        IEnumerable<T> List<T>(Type type) where T : class;
        IQueryable<T> Query<T>() where T : class;
        IQueryable<T> Query<T>(Type type) where T : class;
        void Add<T>(T entity) where T : class;
        void Remove<T>(T entity) where T : class;

        /// <summary>
        /// This allows a unit of work to be explicitly completed or rolled
        /// back. This can be useful when side-effects need to be performed only
        /// after a unit of work has been successful. For example, if an e-mail
        /// should be sent to notify that something has been created, the unit
        /// of work should be completed before sending the e-mail to ensure the
        /// e-mail is not sent if the work fails for an unexpected reason.
        /// 
        /// It is assumed that a unit of work is automatically created for the
        /// lifetime of this IRepository so it is not the consumer's
        /// responsibility to begin the unit of work.
        /// </summary>
        /// <returns></returns>
        IUnitOfWork UnitOfWork { get; }
    }
}
