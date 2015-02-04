using System.Collections.Generic;
using DataStructures.Immutable.Tree;
#if net40
using DataStructures.net40;
#endif

namespace DataStructures.Immutable
{
    /// <summary>
    /// Represents a readable forest, composed of rooted trees and disconnected branches and leaves.
    /// </summary>
    /// <typeparam name="TValue">Type of the value contained in each tree node.</typeparam>
    public interface IReadableForest<out TValue>
    {
        /// <summary>
        /// Gets a set of nodes contained directly in this forest.
        /// </summary>
        IReadOnlyList<INode<TValue>> Nodes { get; }

        /// <summary>
        /// Gets a set of root nodes contained directly in this forest.
        /// </summary>
        IEnumerable<INode<TValue>> RootNodes { get; }

        /// <summary>
        /// Gets a set of non-root nodes contained directly in this forest.
        /// </summary>
        IEnumerable<INode<TValue>> DisconnectedNodes { get; }

        IReadableForest<TResult> Visit<TResult>(Visitor<TValue, INode<TResult>> visitor);
    }
}