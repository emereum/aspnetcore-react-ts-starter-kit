using System;
using System.Linq;
using System.Linq.Dynamic;
using TemplateProductName.Domain.Repositories;

namespace TemplateProductName.Domain.Queries
{
    /// <summary>
    /// Allows a set of data to be filtered by an overridden Filter method. The
    /// data are then automatically filtered by the pagination options specified
    /// on this class. Finally the results along with this class are returned
    /// for reuse in the next (if any) paginated query for this data.
    /// 
    /// To use this class create a subclass that implements PaginatedQuery and
    /// override the Filter method. The subclass can be accepted as a model to
    /// a controller action. It can also have additional properties used to
    /// drive the filtering process. The result from the Execute method should
    /// be returned to the client and re-submitted to retrieve subsequent pages.
    /// This result also supports .Map
    /// </summary>
    public abstract class PaginatedQuery<T> : IPaginationOptions, IGetQuery<T, PaginatedResult<T>> where T : class
    {
        public int Page { get; set; } = defaultPage;
        public int PageSize { get; set; } = defaultPageSize;
        public int Pages { get; set; }
        public string SortProperty { get; set; }
        public SortDirection SortDirection { get; set; }

        private static readonly int defaultPage = 1;
        private static readonly int defaultPageSize = 20;

        public PaginatedResult<T> Execute(IRepository repository, Type classType)
        {
            if (classType == null)
            {
                return Reduce(repository.Query<T>());
            }
            return Reduce(repository.Query<T>(classType));
        }

        public PaginatedResult<T> Reduce(IQueryable<T> entities)
        {
            if (Page < 1)
            {
                throw new InvalidOperationException("Page must be greater than zero.");
            }

            if (PageSize < 1)
            {
                throw new InvalidOperationException("Page size must be greater than zero.");
            }

            if (string.IsNullOrWhiteSpace(SortProperty))
            {
                throw new InvalidOperationException("Sort property must be specified.");
            }

            var query = Filter(entities);

            var count = query.Count();
            var result = query
                .OrderBy(SortProperty + " " + SortDirection)
                .Skip((Page - 1) * PageSize)
                .Take(PageSize)
                .AsEnumerable();

            Pages = Math.Max(1, (int)Math.Ceiling(count / (double)PageSize));

            return new PaginatedResult<T>(result, this);
        }

        /// <summary>
        /// Applies filtering rules before applying pagination. Override
        /// this in a subclass to implement custom filtering. By default
        /// no additional filtering beyond the pagination will be performed.
        /// </summary>
        protected virtual IQueryable<T> Filter(IQueryable<T> entities) => entities;
    }
}
