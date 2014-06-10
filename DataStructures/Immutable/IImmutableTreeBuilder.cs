using DataStructures.Immutable.Tree;
using System;
using System.Collections.Generic;

namespace DataStructures.Immutable
{
    public interface IImmutableTreeBuilder
    {
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