using DataStructures.Immutable;
using System;
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
            if (enumerable == null)
                throw new ArgumentNullException("enumerable");

            // ReSharper disable once SuspiciousTypeConversion.Global - the fact is: `ImmutableCollection<T>` implements `IImmutablePrototype<ImmutableCollection<T>>`
            var immutableProto = enumerable as IImmutablePrototype<ImmutableCollection<T>>;
            if (immutableProto != null)
                return immutableProto.ToImmutable();

            return new ImmutableCollection<T>(enumerable);
        }

        /// <summary>
        /// Converts the enumerable to an immutable collection object,
        /// when the enumerable has got elements.
        /// Otherwise, returns the `whenEmpty` collection.
        /// </summary>
        /// <typeparam name="T">Type of the items of the collection.</typeparam>
        /// <param name="enumerable">The source enumerable object, whose elements will be copied to the immutable collection.</param>
        /// <param name="whenEmpty">The default collection to use, if the passed enumerable is empty.</param>
        /// <returns>An immutable collection composed of the elements of the enumerable.</returns>
        public static ImmutableCollection<T> ToImmutable<T>(this IEnumerable<T> enumerable, ImmutableCollection<T> whenEmpty)
        {
            if (enumerable == null)
                throw new ArgumentNullException("enumerable");

            // if `enumerable` is an immutable prototype
            // ReSharper disable once SuspiciousTypeConversion.Global - resharper is crazy! fact: `ImmutableCollection<T>` implements `IImmutablePrototype<ImmutableCollection<T>>`
            var protoSource = enumerable as IImmutablePrototype<ImmutableCollection<T>>;
            if (protoSource != null)
            {
                var result = protoSource.ToImmutable();
                return result == null || result.Count == 0 ? whenEmpty : result;
            }

            using (var iterator = enumerable.GetEnumerator())
            {
                // if enumeration is empty, we return the `whenEmpty`
                if (!iterator.MoveNext())
                    return whenEmpty;

                var proto = new ImmutableCollection<T>.Prototype { iterator.Current };

                while (iterator.MoveNext())
                    proto.Add(iterator.Current);

                return proto.ToImmutable();
            }
        }
    }
}