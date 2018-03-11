using System.Linq;

namespace TemplateProductName.Domain.Queries
{
    /// <summary>
    /// A query which simply filters a set of data
    /// </summary>
    public interface IQuery<T>
    {
        IQueryable<T> Filter(IQueryable<T> entities);
    }

    /// <summary>
    /// A query which filters a set of data (T) then reduces to a single new
    /// data type (U)
    /// </summary>
    public interface IGetQuery<T, U>
    {
        U Reduce(IQueryable<T> entities);
    }

    /// <summary>
    /// A query which returns a single element from a set of data.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IGetQuery<T>
    {
        T Reduce(IQueryable<T> entities);
    }
}
