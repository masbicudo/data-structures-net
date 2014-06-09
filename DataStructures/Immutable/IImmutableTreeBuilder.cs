using DataStructures.Immutable.Tree;
using System;
using System.Collections.Generic;

namespace DataStructures.Immutable
{
    public interface IImmutableTreeBuilder
    {
        ImmutableForest<TValue> BuildForest<TData, TValue>(
            IEnumerable<TData> roots,
            Func<TData, IEnumerable<TData>> childGetter,
            Func<TData, TValue> valueGetter);

        ImmutableTree<TValue> BuildTree<TData, TValue>(
            TData root,
            Func<TData, IEnumerable<TData>> childGetter,
            Func<TData, TValue> valueGetter);

        IVisitableNode<TValue> BuildRoot<TData, TValue>(
            TData rootData,
            Func<TData, IEnumerable<TData>> childGetter,
            Func<TData, TValue> valueGetter);

        IVisitableNode<TValue> BuildBranchOrLeaf<TData, TValue>(
            TData nodeData,
            Func<TData, IEnumerable<TData>> childGetter,
            Func<TData, TValue> valueGetter);
    }
}
