using System;
using System.Collections.Generic;
using DataStructures.Immutable.Tree;

namespace DataStructures.Immutable
{
    /// <summary>
    /// Represents a tree builder that is abled to build an `INode&lt;T&gt;` given one of the source tree nodes,
    /// and delegates capable of getting the children and value of a given source tree node.
    /// </summary>
    /// <remarks>
    /// The `IImmutableTreeBuilder` interface differs from `IImmutableSpecificTreeBuilder`
    /// in the need for delegates to teach how to get the children and value of a source tree node.
    /// </remarks>
    public interface IImmutableTreeBuilder
    {
        /// <summary>
        /// Builds a root node of an immutable tree, given the root object of another tree representation,
        /// and delegates abled to get the children and value of any source tree node.
        /// </summary>
        /// <param name="rootData">Object representing the source tree root.</param>
        /// <param name="childGetter">Delegate that gets the children of any source tree node.</param>
        /// <param name="valueGetter">Delegate that gets the value of any source tree node.</param>
        /// <returns>An immutable tree root node, that corresponds to the passed root object.</returns>
        INode<TValue> BuildRoot<TData, TValue>(
            TData rootData,
            Func<TData, IEnumerable<TData>> childGetter,
            Func<TData, TValue> valueGetter);

        /// <summary>
        /// Builds a branch or leaf node of an immutable tree, given the corresponding object of another tree representation,
        /// and delegates abled to get the children and value of any source tree node.
        /// </summary>
        /// <param name="nodeData">Object representing the source tree node.</param>
        /// <param name="childGetter">Delegate that gets the children of any source tree node.</param>
        /// <param name="valueGetter">Delegate that gets the value of any source tree node.</param>
        /// <returns>An immutable tree branch or leaf node, that corresponds to the passed object.</returns>
        INode<TValue> BuildBranchOrLeaf<TData, TValue>(
            TData nodeData,
            Func<TData, IEnumerable<TData>> childGetter,
            Func<TData, TValue> valueGetter);
    }
}