using DataStructures.Immutable.Tree;
using System;
using System.Collections.Generic;

namespace DataStructures.Immutable
{
    public interface IImmutableTreeBuilder
    {
        INode<TValue> BuildRoot<TData, TValue>(
            TData rootData,
            Func<TData, IEnumerable<TData>> childGetter,
            Func<TData, TValue> valueGetter);

        INode<TValue> BuildBranchOrLeaf<TData, TValue>(
            TData nodeData,
            Func<TData, IEnumerable<TData>> childGetter,
            Func<TData, TValue> valueGetter);
    }
}