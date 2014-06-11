using DataStructures.Immutable.Tree;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DataStructures.Immutable
{
    public class ImmutableTreeBuilder : IImmutableTreeBuilder
    {
        public virtual INode<TValue> BuildRoot<TData, TValue>(
            TData rootData,
            Func<TData, IEnumerable<TData>> childGetter,
            Func<TData, TValue> valueGetter)
        {
            var children = childGetter(rootData)
                .Select(x => this.BuildBranchOrLeaf(x, childGetter, valueGetter));

            var value = valueGetter(rootData);

            children = children.ToImmutable(null);

            if (children != null)
                return new RootBranch<TValue>(children, value);

            return new RootLeaf<TValue>(value);
        }

        public virtual INode<TValue> BuildBranchOrLeaf<TData, TValue>(
            TData nodeData,
            Func<TData, IEnumerable<TData>> childGetter,
            Func<TData, TValue> valueGetter)
        {
            var children = childGetter(nodeData)
                .Select(x => this.BuildBranchOrLeaf(x, childGetter, valueGetter));

            var value = valueGetter(nodeData);

            children = children.ToImmutable(null);

            if (children != null)
                return new Branch<TValue>(children, value);

            return new Leaf<TValue>(value);
        }
    }
}