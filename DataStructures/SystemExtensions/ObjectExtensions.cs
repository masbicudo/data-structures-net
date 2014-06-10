using System.Collections.Generic;

namespace DataStructures.SystemExtensions
{
    public static class ObjectExtensions
    {
        /// <summary>
        /// Gets an unit set composed of the single passed element.
        /// </summary>
        /// <typeparam name="T">Type of the set elements.</typeparam>
        /// <param name="obj">Single object to be in the set.</param>
        /// <returns>A set composed of a single element.</returns>
        public static IEnumerable<T> ToUnitSet<T>(this T obj)
        {
            yield return obj;
        }
    }
}
