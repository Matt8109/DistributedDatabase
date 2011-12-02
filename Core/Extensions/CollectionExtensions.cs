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

        /// <summary>
        /// Adds a value to a list, but without throwing an error
        /// if it already exists. 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list">The list.</param>
        /// <param name="valueToAdd">The value to add.</param>
        public static void SilentAdd<T>(this List<T> list, T valueToAdd)
        {
            if (list.Contains(valueToAdd))
                return;
       
            list.Add(valueToAdd);
        }
    }
}
