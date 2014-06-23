using System;
using System.Collections.Generic;
using DataStructures.Immutable.Tree;

namespace DataStructures.Immutable
{
    /// <summary>
    /// Represents a tree builder that knows how to get the children and value of a node of the source tree,
    /// and is abled to build `INode&lt;T&gt;` given one of the source tree nodes.
    /// </summary>
    /// <typeparam name="TData">Type of node from the source tree.</typeparam>
    /// <typeparam name="TValue">Type of data that should be store in each tree node.</typeparam>
    public class ImmutableSpecificTreeBuilder<TData, TValue> : IImmutableSpecificTreeBuilder<TData, TValue>
    {
        private readonly IImmutableTreeBuilder innerBuilder;
        private readonly Func<TData, IEnumerable<TData>> childGetter;
        private readonly Func<TData, TValue> valueGetter;

        public ImmutableSpecificTreeBuilder(IImmutableTreeBuilder innerBuilder, Func<TData, IEnumerable<TData>> childGetter, Func<TData, TValue> valueGetter)
        {
            this.innerBuilder = innerBuilder;
            this.childGetter = childGetter;
            this.valueGetter = valueGetter;
        }

        public virtual INode<TValue> BuildRoot(TData rootData)
        {
            return this.innerBuilder.BuildRoot(rootData, this.childGetter, this.valueGetter);
        }

        public virtual INode<TValue> BuildBranchOrLeaf(TData nodeData)
        {
            return this.innerBuilder.BuildBranchOrLeaf(nodeData, this.childGetter, this.valueGetter);
        }
    }
}