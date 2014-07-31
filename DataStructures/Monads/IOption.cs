using System.Collections.Generic;

namespace DataStructures.Monads
{
    /// <summary>
    /// Option monad.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IOption<out T> :
        IEnumerable<T>
    {
        /// <summary>
        /// Gets the value of the option, if any, otherwise throws an exception.
        /// </summary>
        T Value { get; }

        /// <summary>
        /// Gets a value indicating whether this option has a value or not.
        /// </summary>
        bool HasValue { get; }
    }
}