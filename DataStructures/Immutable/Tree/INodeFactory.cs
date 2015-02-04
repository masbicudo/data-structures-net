using System.Collections.Generic;
#if net40
using DataStructures.net40;
#endif

namespace DataStructures.Immutable.Tree
{
    /// <summary>
    /// Represents a factory of a specific node kind (root, branch or leaf).
    /// </summary>
    public interface ISpecificNodeFactory
    {
        /// <summary>
        /// Creates a new node specific to this factory (root, branch or leaf), with a value of `TNodeValue` type.
        /// </summary>
        /// <typeparam name="TNodeValue">The value type of the new node.</typeparam>
        /// <param name="children">Collection of children to pass to the new node. They must have the same value type as the new node.</param>
        /// <param name="value">The value of the new node.</param>
        /// <returns>A new node specific to this factory, with the given value type, containing the passed children and value.</returns>
        INode<TNodeValue> CreateNew<TNodeValue>(IReadOnlyList<INode<TNodeValue>> children, TNodeValue value);
    }
}
