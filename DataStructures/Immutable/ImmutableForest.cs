using System;
using System.Collections.Generic;
using System.Linq;
using DataStructures.Immutable.Tree;

namespace DataStructures.Immutable
{
    public class ImmutableForest<TValue>
    {
        private IEnumerable<ImmutableTree<TValue>> trees;

        public ImmutableForest(IEnumerable<ImmutableTree<TValue>> trees)
        {
            this.trees = trees;
        }
    }

    public class ImmutableTreeBuilder : IImmutableTreeBuilder
    {

        public ImmutableForest<TValue> BuildForest<TData, TValue>(IEnumerable<TData> roots, Func<TData, IEnumerable<TData>> childGetter, Func<TData, TValue> valueGetter)
        {
            var trees = roots.Select(x => this.BuildTree(x, childGetter, valueGetter));
            return new ImmutableForest<TValue>(trees);
        }

        public ImmutableTree<TValue> BuildTree<TData, TValue>(TData root, Func<TData, IEnumerable<TData>> childGetter, Func<TData, TValue> valueGetter)
        {
            throw new NotImplementedException();
        }

        public virtual IVisitableNode<TValue> BuildRoot<TData, TValue>(
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

        public virtual IVisitableNode<TValue> BuildBranchOrLeaf<TData, TValue>(
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