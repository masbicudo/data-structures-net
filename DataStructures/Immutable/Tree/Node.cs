using System.Collections.Generic;
using System.Linq;
#if net40
using DataStructures.net40;
#endif

namespace DataStructures.Immutable.Tree
{
    /// <summary>
    /// Represents a node of an immutable tree.
    /// </summary>
    /// <typeparam name="TValue">Type of the value contained in the node.</typeparam>
    public abstract class Node<TValue> : INode<TValue>, ISpecificNodeFactory
    {
        private static readonly ImmutableCollection<INode<TValue>> EmptyCollection
            = new ImmutableCollection<INode<TValue>>(new Node<TValue>[0]);

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
        protected Node(ImmutableCollection<INode<TValue>> children, TValue value)
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
        protected Node(IEnumerable<INode<TValue>> children, TValue value)
        {
            var childrenArray = children ?? EmptyCollection;
            this.Children = childrenArray.ToImmutable();
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
        /// Gets the collection of child nodes.
        /// </summary>
        public ImmutableCollection<INode<TValue>> Children { get; private set; }

        /// <summary>
        /// Gets the collection of child nodes.
        /// </summary>
        IReadOnlyList<INode<TValue>> INode<TValue>.Children
        {
            get { return this.Children; }
        }

        /// <summary>
        /// Applies the visitor pattern to the current tree node.
        /// </summary>
        /// <typeparam name="TResult">Type of the resulting tree node. Any type is allowed, even non INode&lt;T&gt; objects.</typeparam>
        /// <param name="visitor">
        /// Delegate that will be called when visiting a tree node, to change it,
        /// and to add the already visited children.
        /// </param>
        /// <remarks>
        /// This is suitable for the construction of new trees based on the current tree,
        /// even if the other tree if of a different type.
        /// </remarks>
        /// <returns>
        /// An object of TResult type, that corresponds to the visited node, 
        /// that results from applying the `visitor` delegate to the current node.
        /// </returns>
        public IEnumerable<TResult> Visit<TResult>(Visitor<TValue, TResult> visitor)
        {
            // Creating an enumerable object, that will visit each child object, upon enumeration.
            // The responsability of enumerating these is left to the `visitor` delegate,
            // through the `children` parameter, that is an enumerable.
            var newChildren = this.Children.SelectMany(x => x.Visit(visitor));

            // returns a list of values that correspond to the current node
            var newItems = visitor(this, newChildren);

            return newItems;
        }

        /// <summary>
        /// Creates a new node of the same type of the current node, but with any value type, specified by the `TNewNodeValue` type parameter.
        /// </summary>
        /// <typeparam name="TNewNodeValue">The value type of the new node.</typeparam>
        /// <param name="children">Collection of children to pass to the new node. They must have the same value type as the new node.</param>
        /// <param name="value">The value of the new node.</param>
        /// <returns>A new node of the same type of the current node, but with different value type, containing the passed children and value.</returns>
        protected abstract INode<TNewNodeValue> CreateNew<TNewNodeValue>(ImmutableCollection<INode<TNewNodeValue>> children, TNewNodeValue value);

        INode<TNodeValue> ISpecificNodeFactory.CreateNew<TNodeValue>(IReadOnlyList<INode<TNodeValue>> children, TNodeValue value)
        {
            return this.CreateNew(children.ToImmutable(), value);
        }

        public override string ToString()
        {
            return string.Format("{0} {{ Value = '{2}'; Nodes = {1} }}", this.GetType().Name, this.Children.Count, this.Value);
        }
    }
}
