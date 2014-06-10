using System;
using System.Collections.Generic;
using System.Linq;
using DataStructures.Immutable.Tree;

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

            if (children.Any())
            {
                return new RootBranch<TValue>(children, value);
            }

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

            if (children.Any())
            {
                return new Branch<TValue>(children, value);
            }

            return new Leaf<TValue>(value);
        }
    }
}