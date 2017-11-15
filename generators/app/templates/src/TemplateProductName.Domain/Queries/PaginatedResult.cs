using System;
using System.Collections.Generic;
using System.Linq;

namespace TemplateProductName.Domain.Queries
{
    public class PaginatedResult<T>
    {
        public IEnumerable<T> Entities { get; }
        public IPaginationOptions PaginationOptions { get; }

        public PaginatedResult(IEnumerable<T> entities, IPaginationOptions paginationOptions)
        {
            PaginationOptions = paginationOptions;
            Entities = entities;
        }

        /// <summary>
        /// Maps from a PaginatedResult of one type to a PaginatedResult of
        /// another type.
        /// </summary>
        public PaginatedResult<U> Map<U>(Func<T, U> mapper)
        {
            return new PaginatedResult<U>(Entities.Select(mapper).ToList(), PaginationOptions);
        }
    }
}
