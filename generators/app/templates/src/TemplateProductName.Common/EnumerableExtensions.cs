using System;
using System.Collections.Generic;

namespace TemplateProductName.Common
{
    public static class EnumerableExtensions
    {
        public static void Each<T>(this IEnumerable<T> items, Action<T> action)
        {
            foreach (var item in items)
            {
                action(item);
            }
        }

        public static string Aggregate(this IEnumerable<string> items, string separator)
        {
            return string.Join(separator, items);
        }
    }
}
