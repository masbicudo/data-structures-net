using System.Collections.Generic;

namespace DataStructures.Immutable.Tree
{
    /// <summary>
    /// Represents a node of a tree structure.
    /// </summary>
    /// <typeparam name="TItem">Type of value contained in each tree node.</typeparam>
    public interface INode<out TItem>
    {
        /// <summary>
        /// Gets a value indicating whether the node is the root of a tree.
        /// </summary>
        bool IsRoot { get; }

        /// <summary>
        /// Gets a value indicating whether the node is a brach,
        /// that is, have one or more children.
        /// </summary>
        bool IsBranch { get; }

        /// <summary>
        /// Gets a value indicating whether the node is a leaf.
        /// </summary>
        bool IsLeaf { get; }

        /// <summary>
        /// Gets a value indicating whether the node is internal,
        /// that is: neither root nor leaf.
        /// </summary>
        bool IsInternalNode { get; }

        /// <summary>
        /// Gets the value held by the current node.
        /// </summary>
        TItem Value { get; }

        /// <summary>
        /// Gets the set of child nodes of the current node.
        /// </summary>
        IReadOnlyCollection<INode<TItem>> Children { get; }

        IEnumerable<INode<TItem>> AllLeaves { get; }
    }
}