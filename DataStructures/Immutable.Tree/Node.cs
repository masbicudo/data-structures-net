using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace DataStructures.Immutable.Tree
{
    public abstract class Node<TKey, TItem> : IVisitableNode<TItem>
    {
        private TKey id;
        private TKey parentId;

        protected Node(ReadOnlyCollection<Node<TKey, TItem>> children, TItem value, TKey id, TKey parentId, bool isConnected)
        {
            this.Value = value;
            this.id = id;
            this.parentId = parentId;
            this.IsConnected = isConnected;
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
        public TItem Value { get; private set; }

        public bool IsConnected { get; private set; }

        /// <summary>
        /// Gets all descendant leaves, or this object itself if it is a leaf.
        /// </summary>
        public IEnumerable<INode<TItem>> AllLeaves
        {
            get { return this.Children.SelectMany(x => x.IsLeaf ? SingleItemSet(x) : x.AllLeaves); }
        }

        /// <summary>
        /// Gets the collection of child nodes.
        /// </summary>
        public abstract IReadOnlyCollection<IVisitableNode<TItem>> Children { get; }

        /// <summary>
        /// Gets the collection of child nodes.
        /// </summary>
        IReadOnlyCollection<INode<TItem>> INode<TItem>.Children
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
        public IEnumerable<VisitResult<TItem, TData>> Visit<TData>(Visitor<TItem, TData> visitor)
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
        public IEnumerable<VisitResult<TItem>> Visit(Visitor<TItem> visitor)
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
