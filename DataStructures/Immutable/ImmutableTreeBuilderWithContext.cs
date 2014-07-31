using DataStructures.Immutable.Tree;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DataStructures.Immutable
{
    public class ImmutableTreeBuilderWithContext : IImmutableTreeBuilderWithContext
    {
        public virtual INode<TValue> BuildRoot<TData, TValue>(
            TData rootData,
            Func<TData, IEnumerable<TData>> childGetter,
            Func<TreeBuildingContext<TData, TValue>, TValue> valueGetter,
            Action<TreeBuildingContext<TData, TValue>> postProcessing = null)
        {
            var funcCached = FuncCached.Create<TreeBuildingContext<TData, TValue>, IEnumerable<INode<TValue>>>(
                ctx => childGetter == null
                    ? null
                    : childGetter(rootData)
                        .Select(x => this.BuildBranchOrLeaf(ctx, x, childGetter, valueGetter, postProcessing))
                        .ToImmutable(null));

            var context = new TreeBuildingContext<TData, TValue>(
                rootData,
                ctx =>
                {
                    var childValues = funcCached(ctx);
                    return childValues == null ? null : childValues.Select(n => n.Value);
                });

            context.NodeValue = valueGetter(context);

            if (postProcessing != null)
                context.RootContext.RegisterPostProcessing(() => postProcessing(context));

            var children = funcCached(context);

            if (context.ParentContext == null)
                context.ExecutePostProcessing();

            if (children != null)
                return new RootBranch<TValue>(children, context.NodeValue);

            return new RootLeaf<TValue>(context.NodeValue);
        }

        public virtual INode<TValue> BuildBranchOrLeaf<TData, TValue>(
            TreeBuildingContext<TData, TValue> context,
            TData nodeData,
            Func<TData, IEnumerable<TData>> childGetter,
            Func<TreeBuildingContext<TData, TValue>, TValue> valueGetter,
            Action<TreeBuildingContext<TData, TValue>> postProcessing = null)
        {
            var funcCached = FuncCached.Create<TreeBuildingContext<TData, TValue>, IEnumerable<INode<TValue>>>(
                ctx => childGetter == null
                    ? null
                    : childGetter(nodeData)
                        .Select(x => this.BuildBranchOrLeaf(ctx, x, childGetter, valueGetter, postProcessing))
                        .ToImmutable(null));

            context = new TreeBuildingContext<TData, TValue>(
                context,
                nodeData,
                ctx =>
                {
                    var childValues = funcCached(ctx);
                    return childValues == null ? null : childValues.Select(n => n.Value);
                });

            context.NodeValue = valueGetter(context);

            if (postProcessing != null)
                context.RootContext.RegisterPostProcessing(() => postProcessing(context));

            var children = funcCached(context);

            if (context.ParentContext == null)
                context.ExecutePostProcessing();

            if (children != null)
                return new Branch<TValue>(children, context.NodeValue);

            return new Leaf<TValue>(context.NodeValue);
        }
    }
}