using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DistributedDatabase.Core.Extensions
{
    /// <summary>
    /// Extension methods for collection objects.
    /// </summary>
    public static class CollectionExtensions
    {
        public static void ForEachElement<T>(this IEnumerable<T> list, Action<T> action)
        {
            foreach (T item in list)
            {
                action(item);
            }
        }
    }
}
