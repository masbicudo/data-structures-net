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
        /// Gets the set of child nodes of the current node.
        /// </summary>
        new ImmutableCollection<IVisitableNode<TValue>> Children { get; }

        /// <summary>
        /// Applies the visitor pattern to the current tree node,
        /// allowing the return of additional data along with the node in each visit.
        /// </summary>
        /// <typeparam name="TData">Type of the additional data object.</typeparam>
        /// <param name="visitor">Delegate the will be called when visiting a tree node.</param>
        /// <remarks>
        /// This is suitable for the construction of new trees based on the current tree,
        /// even if the other tree if of a different type.
        /// </remarks>
        /// <returns>A VisitResult struct, containing the visited node (or a replacement), and the additional data produced by the visitor.</returns>
        IEnumerable<VisitResult<TValue, TData>> Visit<TData>(Visitor<TValue, TData> visitor);

        /// <summary>
        /// Applies the visitor pattern to the current tree node.
        /// </summary>
        /// <param name="visitor">Delegate the will be called when visiting a tree node.</param>
        /// <remarks>
        /// This is suitable for the modification of the current tree (which will result in a new immutable tree).
        /// </remarks>
        /// <returns>A VisitResult struct, containing the visited node (or a replacement).</returns>
        IEnumerable<VisitResult<TValue>> Visit(Visitor<TValue> visitor);
    }
}