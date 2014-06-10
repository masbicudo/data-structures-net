using DataStructures.Immutable;
using System.Collections.Generic;

namespace DataStructures
{
    public static class EnumerableExtensions
    {
        /// <summary>
        /// Converts the enumerable to an immutable collection object.
        /// </summary>
        /// <typeparam name="T">Type of the items of the collection.</typeparam>
        /// <param name="enumerable">The source enumerable object, whose elements will be copied to the immutable collection.</param>
        /// <returns>An immutable collection composed of the elements of the enumerable.</returns>
        public static ImmutableCollection<T> ToImmutable<T>(this IEnumerable<T> enumerable)
        {
            // ReSharper disable once SuspiciousTypeConversion.Global - the fact is: `ImmutableCollection<T>` implements `IImmutablePrototype<ImmutableCollection<T>>`
            var immutableProto = enumerable as IImmutablePrototype<ImmutableCollection<T>>;
            if (immutableProto != null)
                return immutableProto.ToImmutable();

            return new ImmutableCollection<T>(enumerable);
        }
    }
}
