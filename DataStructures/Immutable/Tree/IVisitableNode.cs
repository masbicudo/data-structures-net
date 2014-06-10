using System.Collections.Generic;

namespace DataStructures.Immutable.Tree
{
    /// <summary>
    /// Represents a node in a tree structure,
    /// that allows the visitation of child nodes using the visitor pattern.
    /// </summary>
    /// <typeparam name="TValue">Type of value contained in each tree node.</typeparam>
    public interface IVisitableNode<TValue> : INode<TValue>, IImmutable
    {
        /// <summary>
        /// Gets a collection of nodes that are children of the current node.
        /// </summary>
        new ImmutableCollection<IVisitableNode<TValue>> Children { get; }

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