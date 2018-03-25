using System;
using System.Linq;
using System.Linq.Dynamic;

namespace TemplateProductName.Common.Pagination
{
    public static class PaginationExtensions
    {
        /// <summary>
        /// Paginates an IQueryable using the specified pagination options.
        /// Returns the subset of paginated data, wrapped in a PaginatedResult
        /// which contains pagination options that can be reused on subsequent
        /// requests.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="queryable"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        public static PaginatedResult<T> Paginate<T>(this IQueryable<T> queryable, IPaginationOptions options)
        {
            if (options.Page == null || options.Page < 1)
            {
                throw new InvalidOperationException("Page must be greater than zero.");
            }

            if (options.PageSize == null || options.PageSize < 1)
            {
                throw new InvalidOperationException("Page size must be greater than zero.");
            }

            if (string.IsNullOrWhiteSpace(options.SortProperty))
            {
                throw new InvalidOperationException("Sort property must be specified.");
            }

            if (options.SortDirection == null)
            {
                throw new InvalidOperationException("Sort direction must be specified.");
            }

            var totalCount = queryable.Count();
            var results = queryable
                .OrderBy(options.SortProperty + " " + options.SortDirection)
                .Skip((options.Page.Value - 1) * options.PageSize.Value)
                .Take(options.PageSize.Value)
                .AsEnumerable();
            var pages = Math.Max(1, (int)Math.Ceiling(totalCount/ (double)options.PageSize.Value));

            return new PaginatedResult<T>(results, new PaginationOptions
            {
                Page = options.Page,
                Pages = pages,
                PageSize = options.PageSize,
                SortDirection = options.SortDirection,
                SortProperty = options.SortProperty
            });
        }
    }
}
