using System.Collections.Generic;

namespace DataStructures.Immutable.Tree
{
    /// <summary>
    /// Represents a tree-node visitor, that is used to process tree-nodes and maybe transform these nodes.
    /// </summary>
    /// <typeparam name="TValue">Type of the value contained in the tree-nodes.</typeparam>
    /// <typeparam name="TResult">Type of results returned by processing a tree-node.</typeparam>
    /// <param name="node">Node to be processed.</param>
    /// <param name="children">Already processed child nodes results.</param>
    /// <returns> An enumeration of results, that represents the visitor result. </returns>
    /// <remarks>
    /// <para>
    /// Anything can be done inside the visitor with the passed parameters,
    /// in order to construct the results.
    /// </para>
    /// <para>
    /// By yielding no results, the node is deleted from the resulting tree.
    /// </para>
    /// <para>
    /// By yielding the very same input node, the tree keeps unchanged;
    /// Or, if a single new node is returned, then the old node is replaced by the new one.
    /// </para>
    /// <para>
    /// By yielding multiple results, a single node is replaced by many nodes.
    /// It is possible for example to return the list of children,
    /// thus replacing the node with its children.
    /// </para>
    /// <para>
    /// When using the visitor pattern to change an immutable tree,
    /// and a node is replaced in the tree, every node up to the root must also be replaced.
    /// To detect that a child node has changed, the children parameter can be compared with the node.Children collection.
    /// If any has changed, then it means that a new immutable node must be constructed to replace the current one.
    /// To make it easier to do this, you can use the extension method RecreateNodeIfNeeded, passing the new children.
    /// </para>
    /// </remarks>
    public delegate IEnumerable<TResult> Visitor<in TValue, TResult>(INode<TValue> node, IEnumerable<TResult> children);
}