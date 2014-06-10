using System;
using System.Collections.Generic;
using DataStructures.Immutable.Tree;

namespace DataStructures.Immutable
{
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

        public virtual IVisitableNode<TValue> BuildRoot(TData rootData)
        {
            return this.innerBuilder.BuildRoot(rootData, this.childGetter, this.valueGetter);
        }

        public virtual IVisitableNode<TValue> BuildBranchOrLeaf(TData nodeData)
        {
            return this.innerBuilder.BuildBranchOrLeaf(nodeData, this.childGetter, this.valueGetter);
        }

    }
}