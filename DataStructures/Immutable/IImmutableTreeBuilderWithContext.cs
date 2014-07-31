using System;
using System.Collections.Generic;
using DataStructures.Immutable.Tree;

namespace DataStructures.Immutable
{
    public interface IImmutableTreeBuilderWithContext
    {
        INode<TValue> BuildRoot<TData, TValue>(
            TData rootData,
            Func<TData, IEnumerable<TData>> childGetter,
            Func<TreeBuildingContext<TData, TValue>, TValue> valueGetter,
            Action<TreeBuildingContext<TData, TValue>> postProcessing = null);

        INode<TValue> BuildBranchOrLeaf<TData, TValue>(
            TreeBuildingContext<TData, TValue> context,
            TData nodeData,
            Func<TData, IEnumerable<TData>> childGetter,
            Func<TreeBuildingContext<TData, TValue>, TValue> valueGetter,
            Action<TreeBuildingContext<TData, TValue>> postProcessing = null);
    }
}