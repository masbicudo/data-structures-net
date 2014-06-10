using System.Collections.Generic;
using System.Linq;

namespace DataStructures.Immutable.Tree
{
    /// <summary>
    /// Represents a node of a tree structure.
    /// </summary>
    /// <typeparam name="TValue">Type of value contained in each tree node.</typeparam>
    public interface INode<out TValue> : IImmutable
    {
        /// <summary>
        /// Gets a value indicating whether the node is the root of a tree.
        /// </summary>
        bool IsRoot { get; }

        /// <summary>
        /// Gets a value indicating whether the node is a brach,
        /// that is, have one or more children.
        /// </summary>
        bool IsBranch { get; }

        /// <summary>
        /// Gets a value indicating whether the node is a leaf.
        /// </summary>
        bool IsLeaf { get; }

        /// <summary>
        /// Gets a value indicating whether the node is internal,
        /// that is: neither root nor leaf.
        /// </summary>
        bool IsInternalNode { get; }

        /// <summary>
        /// Gets the value held by the current node.
        /// </summary>
        TValue Value { get; }

        /// <summary>
        /// Gets the set of child nodes of the current node.
        /// </summary>
        IReadOnlyCollection<INode<TValue>> Children { get; }
    }

    public static class NodeExtensions
    {
        /// <summary>
        /// Gets an enumerable for all descendant leaves, or the current node itself when it is a leaf.
        /// </summary>
        /// <param name="node"> The node to enumerate itself (if its a leaf) or all descendant leaves. </param>
        /// <typeparam name="TValue"> Type of the value contained in the node. </typeparam>
        /// <returns> An `IEnumerable` that returns all leaf nodes. </returns>
        public static IEnumerable<INode<TValue>> GetAllLeavesEnum<TValue>(this INode<TValue> node)
        {
            return node.IsLeaf ? SingleItemSet(node) : node.Children.SelectMany(x => x.GetAllLeavesEnum());
        }

        /// <summary>
        /// Gets an enumerable returning the current node,
        /// and all of it's descendants in order of height in the tree.
        /// This requires the creation of an intermediate queue,
        /// with memory consumption of O(max-width).
        /// </summary>
        /// <param name="node"> The node to enumerate itself and all descendant nodes. </param>
        /// <typeparam name="TValue"> Type of the value contained in the node. </typeparam>
        /// <returns> An `IEnumerable` that returns all the nodes, in height order. </returns>
        public static IEnumerable<INode<TValue>> GetAllNodesByHeightEnum<TValue>(this INode<TValue> node)
        {
            var queue = new Queue<INode<TValue>>();
            queue.Enqueue(node);
            while (queue.Count > 0)
            {
                var item = queue.Dequeue();
                yield return item;

                foreach (var childItem in item.Children)
                    queue.Enqueue(childItem);
            }
        }

        /// <summary>
        /// Gets an enumerable returning the current node, and all of it's descendants in recursive descent order.
        /// </summary>
        /// <param name="node"> The node to enumerate itself and all descendant nodes. </param>
        /// <typeparam name="TValue"> Type of the value contained in the node. </typeparam>
        /// <returns> An `IEnumerable` that returns all the nodes, in recursive descent order. </returns>
        public static IEnumerable<INode<TValue>> GetAllNodesEnum<TValue>(this INode<TValue> node)
        {
            return SingleItemSet(node).Concat(node.Children.SelectMany(x => x.GetAllNodesEnum()));
        }

        private static IEnumerable<T> SingleItemSet<T>(T item)
        {
            yield return item;
        }
    }
}