using DataStructures.Immutable.Tree;

namespace DataStructures.Immutable
{
    /// <summary>
    /// Represents a tree builder that knows how to get the children and value of a node of the source tree,
    /// and is abled to build `INode&lt;T&gt;` given one of the source tree nodes.
    /// </summary>
    /// <typeparam name="TData">Type of node from the source tree.</typeparam>
    /// <typeparam name="TValue">Type of data that should be store in each tree node.</typeparam>
    /// <remarks>
    /// The `IImmutableSpecificTreeBuilder` interface differs from `IImmutableTreeBuilder`
    /// in the need for delegates to teach how to get the children and value of a source tree node.
    /// The specific interface knows how to get children and value internally.
    /// </remarks>
    public interface IImmutableSpecificTreeBuilder<in TData, out TValue>
    {
        /// <summary>
        /// Builds a root node of an immutable tree, given the root object of another tree representation.
        /// </summary>
        /// <param name="rootData">Object representing the source tree root.</param>
        /// <returns>An immutable tree root node, that corresponds to the passed root object.</returns>
        INode<TValue> BuildRoot(TData rootData);

        /// <summary>
        /// Builds a branch or leaf node of an immutable tree, given the corresponding object of another tree representation.
        /// </summary>
        /// <param name="nodeData">Object representing the source tree node.</param>
        /// <returns>An immutable tree branch or leaf node, that corresponds to the passed object.</returns>
        INode<TValue> BuildBranchOrLeaf(TData nodeData);
    }
}