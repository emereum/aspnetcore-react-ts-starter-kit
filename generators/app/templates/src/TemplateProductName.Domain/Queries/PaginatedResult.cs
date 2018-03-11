using System;
using System.Collections.Generic;
using System.Linq;

namespace TemplateProductName.Domain.Queries
{
    public class PaginatedResult<TEntity>
    {
        public IEnumerable<TEntity> Entities { get; }
        public IPaginationOptions PaginationOptions { get; }

        public PaginatedResult(IEnumerable<TEntity> entities, IPaginationOptions paginationOptions)
        {
            PaginationOptions = paginationOptions;
            Entities = entities;
        }

        /// <summary>
        /// Maps from a PaginatedResult of one type to a PaginatedResult of
        /// another type.
        /// </summary>
        public PaginatedResult<TNew> Map<TNew>(Func<TEntity, TNew> mapper) =>
            new PaginatedResult<TNew>(Entities.Select(mapper).ToList(), PaginationOptions);
    }
}
