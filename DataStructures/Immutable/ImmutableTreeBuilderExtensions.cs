using System;
using System.Collections.Generic;
using System.Linq;
using DataStructures.Immutable.Tree;

namespace DataStructures.Immutable
{
    public static class ImmutableTreeBuilderExtensions
    {
        /// <summary>
        /// Creates a specific tree builder wrapping the current tree builder,
        /// and using the given delegates to access the properties from the source data.
        /// </summary>
        /// <typeparam name="TData">Type of data that is used to construct the tree.</typeparam>
        /// <typeparam name="TValue">Type of value contained, </typeparam>
        /// <param name="treeBuilder"> The generic tree builder that will be used by the specific one with the given delegates. </param>
        /// <param name="childGetter"> The child getter delegate is used to get the children of a TData object. </param>
        /// <param name="valueGetter"> The value getter delegate is used to get the value of a TData object. This value will be stored in the tree node. </param>
        /// <returns>A specific tree builder.</returns>
        public static IImmutableSpecificTreeBuilder<TData, TValue> ToSpecificTreeBuilder<TData, TValue>(
            this IImmutableTreeBuilder treeBuilder,
            Func<TData, IEnumerable<TData>> childGetter,
            Func<TData, TValue> valueGetter)
        {
            return new ImmutableSpecificTreeBuilder<TData, TValue>(treeBuilder, childGetter, valueGetter);
        }

        /// <summary>
        /// Creates a forest given the root nodes data,
        /// and delegates to get children of a node data and to build a tree node.
        /// </summary>
        /// <typeparam name="TData">Type of data that is used to construct the tree.</typeparam>
        /// <typeparam name="TValue">Type of value contained, </typeparam>
        /// <param name="treeBuilder">The tree builder used to build the forest.</param>
        /// <param name="childGetter"> The child getter delegate is used to get the children of a TData object. </param>
        /// <param name="valueGetter">
        /// The value getter delegate is used to get the value of a TData object.
        /// This value will be stored in the tree node.
        /// </param>
        /// <param name="rootsData"> Data for each of the root nodes in the forest to be built. </param>
        /// <param name="nonRootsData">
        /// Data for each of the non-root nodes in the forest to be built.
        /// These are branches and leaves that are not connected to the forest.
        /// </param>
        /// <returns>An immutable forest created from the passed data.</returns>
        public static ImmutableForest<TValue> BuildForest<TData, TValue>(
            this IImmutableTreeBuilder treeBuilder,
            Func<TData, IEnumerable<TData>> childGetter,
            Func<TData, TValue> valueGetter,
            IEnumerable<TData> rootsData,
            IEnumerable<TData> nonRootsData = null)
        {
            if (treeBuilder == null)
                throw new ArgumentNullException("treeBuilder");
            if (childGetter == null)
                throw new ArgumentNullException("childGetter");
            if (valueGetter == null)
                throw new ArgumentNullException("valueGetter");
            if (rootsData == null)
                throw new ArgumentNullException("rootsData");

            nonRootsData = nonRootsData ?? Enumerable.Empty<TData>();
            IEnumerable<INode<TValue>> roots = rootsData.Select(x => treeBuilder.BuildRoot(x, childGetter, valueGetter));
            IEnumerable<INode<TValue>> nonRoots = nonRootsData.Select(x => treeBuilder.BuildRoot(x, childGetter, valueGetter));
            return new ImmutableForest<TValue>(roots.Concat(nonRoots).ToImmutable());
        }

        /// <summary>
        /// Creates a forest given the root nodes data.
        /// </summary>
        /// <typeparam name="TData">Type of data that is used to construct the tree.</typeparam>
        /// <typeparam name="TValue">Type of value contained, </typeparam>
        /// <param name="treeBuilder">The tree builder used to build the forest.</param>
        /// <param name="rootsData"> Data for each of the root nodes in the forest to be built. </param>
        /// <param name="nonRootsData">
        /// Data for each of the non-root nodes in the forest to be built.
        /// These are branches and leaves that are not connected to the forest.
        /// </param>
        /// <returns>An immutable forest created from the passed data.</returns>
        public static ImmutableForest<TValue> BuildForest<TData, TValue>(
            this IImmutableSpecificTreeBuilder<TData, TValue> treeBuilder,
            IEnumerable<TData> rootsData,
            IEnumerable<TData> nonRootsData = null)
        {
            if (treeBuilder == null)
                throw new ArgumentNullException("treeBuilder");
            if (rootsData == null)
                throw new ArgumentNullException("rootsData");

            nonRootsData = nonRootsData ?? Enumerable.Empty<TData>();
            IEnumerable<INode<TValue>> roots = rootsData.Select(treeBuilder.BuildRoot);
            IEnumerable<INode<TValue>> nonRoots = nonRootsData.Select(treeBuilder.BuildRoot);
            return new ImmutableForest<TValue>(roots.Concat(nonRoots).ToImmutable());
        }

        /// <summary>
        /// Creates a collection of nodes given the nodes data,
        /// and delegates to get children of a node data and to build a tree node.
        /// </summary>
        /// <typeparam name="TData">Type of data that is used to construct the tree nodes.</typeparam>
        /// <typeparam name="TValue">Type of value contained in each tree node. </typeparam>
        /// <param name="treeBuilder">The tree builder used to build the nodes.</param>
        /// <param name="childGetter"> The child getter delegate is used to get the children of a TData object. </param>
        /// <param name="valueGetter">
        /// The value getter delegate is used to get the value of a TData object.
        /// This value will be stored in the tree node.
        /// </param>
        /// <param name="data"> Data for each of the nodes to be built. </param>
        /// <returns>The group of nodes created from the data.</returns>
        public static IEnumerable<INode<TValue>> BuildBranchesOrLeaves<TData, TValue>(
            this IImmutableTreeBuilder treeBuilder,
            Func<TData, IEnumerable<TData>> childGetter,
            Func<TData, TValue> valueGetter,
            IEnumerable<TData> data)
        {
            if (treeBuilder == null)
                throw new ArgumentNullException("treeBuilder");
            if (childGetter == null)
                throw new ArgumentNullException("childGetter");
            if (valueGetter == null)
                throw new ArgumentNullException("valueGetter");
            if (data == null)
                throw new ArgumentNullException("data");

            var nodes = data.Select(x => treeBuilder.BuildBranchOrLeaf(x, childGetter, valueGetter));
            return nodes;
        }

        /// <summary>
        /// Creates a collection of nodes given the nodes data.
        /// </summary>
        /// <typeparam name="TData">Type of data that is used to construct the tree nodes.</typeparam>
        /// <typeparam name="TValue">Type of value contained in each tree node. </typeparam>
        /// <param name="treeBuilder">The tree builder used to build the nodes.</param>
        /// <param name="data"> Data for each of the nodes to be built. </param>
        /// <returns>The group of nodes created from the data.</returns>
        public static IEnumerable<INode<TValue>> BuildBranchesOrLeaves<TData, TValue>(
            this IImmutableSpecificTreeBuilder<TData, TValue> treeBuilder,
            IEnumerable<TData> data)
        {
            if (treeBuilder == null)
                throw new ArgumentNullException("treeBuilder");
            if (data == null)
                throw new ArgumentNullException("data");

            var nodes = data.Select(treeBuilder.BuildBranchOrLeaf);
            return nodes;
        }
    }
}