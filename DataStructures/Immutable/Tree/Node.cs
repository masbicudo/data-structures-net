﻿using System.Collections.Generic;
using System.Linq;

namespace DataStructures.Immutable.Tree
{
    /// <summary>
    /// Represents a node of an immutable tree.
    /// </summary>
    /// <typeparam name="TValue">Type of the value contained in the node.</typeparam>
    public abstract class Node<TValue> : IVisitableNode<TValue>
    {
        private static readonly ImmutableCollection<IVisitableNode<TValue>> EmptyCollection
            = new ImmutableCollection<IVisitableNode<TValue>>(new Node<TValue>[0]);

        /// <summary>
        /// Initializes a new instance of the <see cref="Node{TValue}"/> class,
        /// with the specified children and value.
        /// </summary>
        /// <param name="children">
        /// The children.
        /// </param>
        /// <param name="value">
        /// The value.
        /// </param>
        protected Node(ImmutableCollection<IVisitableNode<TValue>> children, TValue value)
        {
            this.Children = children == null || children.Count == 0 ? EmptyCollection : children;
            this.Value = value;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Node{TValue}"/> class,
        /// with the specified children and value.
        /// </summary>
        /// <param name="children">
        /// The children.
        /// </param>
        /// <param name="value">
        /// The value.
        /// </param>
        protected Node(IEnumerable<IVisitableNode<TValue>> children, TValue value)
        {
            var childrenArray = children.ToArray();
            this.Children = children == null || childrenArray.Length == 0
                ? EmptyCollection
                : new ImmutableCollection<IVisitableNode<TValue>>(childrenArray);

            this.Value = value;
        }

        /// <summary>
        /// Gets a value indicating whether the node is the root of a tree.
        /// </summary>
        public abstract bool IsRoot { get; }

        /// <summary>
        /// Gets a value indicating whether the node is a brach,
        /// that is, have one or more children.
        /// </summary>
        public abstract bool IsBranch { get; }

        /// <summary>
        /// Gets a value indicating whether the node is a leaf.
        /// </summary>
        public abstract bool IsLeaf { get; }

        /// <summary>
        /// Gets a value indicating whether the node is internal,
        /// that is: neither root nor leaf.
        /// </summary>
        public abstract bool IsInternalNode { get; }

        /// <summary>
        /// Gets the value held by the current node.
        /// </summary>
        public TValue Value { get; private set; }

        /// <summary>
        /// Gets all descendant leaves, or this object itself if it is a leaf.
        /// </summary>
        public IEnumerable<INode<TValue>> AllLeaves
        {
            get { return this.Children.SelectMany(x => x.IsLeaf ? SingleItemSet(x) : x.AllLeaves); }
        }

        /// <summary>
        /// Gets the collection of child nodes.
        /// </summary>
        public ImmutableCollection<IVisitableNode<TValue>> Children { get; private set; }

        /// <summary>
        /// Gets the collection of child nodes.
        /// </summary>
        IReadOnlyCollection<INode<TValue>> INode<TValue>.Children
        {
            get { return this.Children; }
        }

        /// <summary>
        /// Applies the visitor pattern to the current tree node,
        /// allowing the return of additional data along with the node in each visit.
        /// </summary>
        /// <typeparam name="TData">Type of the additional data object.</typeparam>
        /// <param name="visitor">Delegate the will be called when visiting a tree node.</param>
        /// <remarks>
        /// This is suitable for the construction of new trees based on the current tree,
        /// even if the other tree if of a different type.
        /// </remarks>
        /// <returns>A VisitResult struct, containing the visited node (or a replacement), and the additional data produced by the visitor.</returns>
        public IEnumerable<VisitResult<TValue, TData>> Visit<TData>(Visitor<TValue, TData> visitor)
        {
            // Creating an enumerable object, that will visit each child object, upon enumeration.
            // The responsability of enumerating these is left to the `visitor` delegate,
            // through the `children` parameter, that is an enumerable.
            var newChildren = this.Children.SelectMany(x => x.Visit(visitor));

            // replace current node value
            // change list of child nodes (add/remove)
            var newItem = visitor(this, newChildren);

            return newItem;
        }

        /// <summary>
        /// Applies the visitor pattern to the current tree node.
        /// </summary>
        /// <param name="visitor">Delegate the will be called when visiting a tree node.</param>
        /// <remarks>
        /// This is suitable for the modification of the current tree (which will result in a new immutable tree).
        /// </remarks>
        /// <returns>A VisitResult struct, containing the visited node (or a replacement).</returns>
        public IEnumerable<VisitResult<TValue>> Visit(Visitor<TValue> visitor)
        {
            // Creating an enumerable object, that will visit each child object, upon enumeration.
            // The responsability of enumerating these is left to the `visitor` delegate,
            // through the `children` parameter, that is an enumerable.
            var newChildren = this.Children.SelectMany(x => x.Visit(visitor));

            // replace current node value
            // change list of child nodes (add/remove)
            var newItem = visitor(this, newChildren);

            return newItem;
        }

        private static IEnumerable<T> SingleItemSet<T>(T item)
        {
            yield return item;
        }
    }
}