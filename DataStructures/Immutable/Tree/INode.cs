using System.Collections.Generic;

namespace DataStructures.Immutable.Tree
{
    /// <summary>
    /// Represents a node of a tree structure.
    /// </summary>
    /// <typeparam name="TValue">Type of value contained in each tree node.</typeparam>
    public interface INode<out TValue> : IImmutable
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
        TValue Value { get; }

        /// <summary>
        /// Gets the set of child nodes of the current node.
        /// </summary>
        IReadOnlyList<INode<TValue>> Children { get; }

        /// <summary>
        /// Applies the visitor pattern to the current tree node.
        /// </summary>
        /// <typeparam name="TResult">Type of the resulting tree node. Any type is allowed, even non INode&lt;T&gt; objects.</typeparam>
        /// <param name="visitor">
        /// Delegate that will be called when visiting a tree node, to change it,
        /// and to add the already visited children.
        /// </param>
        /// <remarks>
        /// This is suitable for the construction of new trees based on the current tree,
        /// even if the other tree if of a different type.
        /// </remarks>
        /// <returns>
        /// An object of TResult type, that corresponds to the visited node, 
        /// that results from applying the `visitor` delegate to the current node.
        /// </returns>
        IEnumerable<TResult> Visit<TResult>(Visitor<TValue, TResult> visitor);
    }
}