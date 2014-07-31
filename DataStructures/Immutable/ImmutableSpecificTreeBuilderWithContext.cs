using System;
using System.Collections.Generic;
using DataStructures.Immutable.Tree;

namespace DataStructures.Immutable
{
    public class ImmutableSpecificTreeBuilderWithContext<TData, TValue> :
        IImmutableSpecificTreeBuilderWithContext<TData, TValue>
    {
        private readonly IImmutableTreeBuilderWithContext innerBuilder;
        private readonly Func<TData, IEnumerable<TData>> childGetter;
        private readonly Func<TreeBuildingContext<TData, TValue>, TValue> valueGetter;
        private readonly Action<TreeBuildingContext<TData, TValue>> postProcessing;

        public ImmutableSpecificTreeBuilderWithContext(
            IImmutableTreeBuilderWithContext innerBuilder,
            Func<TData, IEnumerable<TData>> childGetter,
            Func<TreeBuildingContext<TData, TValue>, TValue> valueGetter,
            Action<TreeBuildingContext<TData, TValue>> postProcessing = null)
        {
            this.innerBuilder = innerBuilder;
            this.childGetter = childGetter;
            this.valueGetter = valueGetter;
            this.postProcessing = postProcessing;
        }

        public virtual INode<TValue> BuildRoot(TData rootData)
        {
            return this.innerBuilder.BuildRoot(rootData, this.childGetter, this.valueGetter, this.postProcessing);
        }

        public virtual INode<TValue> BuildBranchOrLeaf(TreeBuildingContext<TData, TValue> context, TData nodeData)
        {
            return this.innerBuilder.BuildBranchOrLeaf(context, nodeData, this.childGetter, this.valueGetter, this.postProcessing);
        }
    }
}