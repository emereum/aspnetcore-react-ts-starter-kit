using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace TemplateProductName.Common
{
    public static class EnumerableExtensions
    {
        public static void ForEach<T>(this IEnumerable<T> items, Action<T> action)
        {
            foreach (var item in items)
            {
                action(item);
            }
        }

        public static string Aggregate(this IEnumerable<string> items, string separator) =>
            string.Join(separator, items);

        public static IQueryable<TSource> WhereIf<TSource>(this IQueryable<TSource> items, bool condition, Expression<Func<TSource, bool>> predicate) =>
            condition
                ? items.Where(predicate)
                : items;

        public static IEnumerable<TSource> WhereIf<TSource>(this IEnumerable<TSource> items, bool condition, Func<TSource, bool> predicate) =>
            condition
                ? items.Where(predicate)
                : items;
    }
}
