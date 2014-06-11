using System.Collections.Generic;
using System.Linq;
using DataStructures.SystemExtensions;

namespace DataStructures.Immutable.Tree
{
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
            return node.IsLeaf ? node.ToUnitSet() : node.Children.SelectMany(GetAllLeavesEnum);
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
            return node.ToUnitSet().Concat(node.Children.SelectMany(x => x.GetAllNodesEnum()));
        }

        /// <summary>
        /// Recreates a node only if changes are detected by comparing the passed children and value,
        /// with the Children and Value properties of the node to recreate.
        /// When no reconstruction is needed, the node itself is returned.
        /// </summary>
        /// <typeparam name="TNodeValue">Type of the value of the new node and it's child nodes.</typeparam>
        /// <param name="node">The node to be reconstructed or not, depending on the detected changes.</param>
        /// <param name="children">
        /// The children of the reconstructed node. They will be compared with the base node, to detect changes.
        /// </param>
        /// <param name="value">
        /// Desired value of the node. If it is different from the base node value, a reconstruction is required.
        /// </param>
        /// <returns>
        /// The reconstructed node, when a reconstruction is required either by changes in children or in value, or both. 
        /// Otherwise, the base node is returned, without changes.
        /// </returns>
        public static IEnumerable<INode<TNodeValue>> RecreateNodeIfNeeded<TNodeValue>(this INode<TNodeValue> node, IEnumerable<INode<TNodeValue>> children, TNodeValue value)
        {
            return RecreateNodeIfNeeded(node, node as ISpecificNodeFactory, children, value, keepChildren: false, keepValue: false);
        }

        /// <summary>
        /// Recreates a node only if changes are detected by comparing the passed children,
        /// with the Children property of the node to recreate.
        /// When no reconstruction is needed, the node itself is returned.
        /// </summary>
        /// <typeparam name="TNodeValue">Type of the value of the children of the new node.</typeparam>
        /// <param name="node">The node to be reconstructed or not, depending on the detected changes.</param>
        /// <param name="children">
        /// The children of the reconstructed node. They will be compared with the base node, to detect changes.
        /// </param>
        /// <returns>
        /// The reconstructed node, when a reconstruction is required by changes in children. 
        /// Otherwise, the base node is returned, without changes.
        /// </returns>
        public static IEnumerable<INode<TNodeValue>> RecreateNodeIfNeeded<TNodeValue>(this INode<TNodeValue> node, IEnumerable<INode<TNodeValue>> children)
        {
            return RecreateNodeIfNeeded(node, node as ISpecificNodeFactory, children, default(TNodeValue), keepChildren: false, keepValue: true);
        }

        /// <summary>
        /// Recreates a node only if changes are detected by comparing the passed value,
        /// with the Value property of the node to recreate.
        /// When no reconstruction is needed, the node itself is returned.
        /// </summary>
        /// <typeparam name="TNodeValue">Type of the value of the new node.</typeparam>
        /// <param name="node">The node to be reconstructed or not, depending on the detected changes.</param>
        /// <param name="value">
        /// Desired value of the node. If it is different from the base node value, a reconstruction is required.
        /// </param>
        /// <returns>
        /// The reconstructed node, when a reconstruction is required by a value change. 
        /// Otherwise, the base node is returned, without changes.
        /// </returns>
        public static IEnumerable<INode<TNodeValue>> RecreateNodeIfNeeded<TNodeValue>(this INode<TNodeValue> node, TNodeValue value)
        {
            return RecreateNodeIfNeeded(node, node as ISpecificNodeFactory, null, value, keepChildren: true, keepValue: false);
        }

        private static IEnumerable<INode<TNodeValue>> RecreateNodeIfNeeded<TNodeValue>(this INode<TNodeValue> node, ISpecificNodeFactory nodeReconstructionFactory, IEnumerable<INode<TNodeValue>> children, TNodeValue value, bool keepChildren, bool keepValue)
        {
            ImmutableCollection<INode<TNodeValue>>.Prototype newChildren = null;

            if (!keepChildren)
            {
                int index = 0;

                using (var iterator1 = node.Children.GetEnumerator())
                using (var iterator2 = children.GetEnumerator())
                {
                    bool hasNext1;
                    bool hasNext2;
                    while ((hasNext1 = iterator1.MoveNext()) & (hasNext2 = iterator2.MoveNext()))
                    {
                        if (iterator1.Current != iterator2.Current)
                            break;

                        index++;
                    }

                    if (!hasNext1)
                    {
                        if (hasNext2)
                        {
                            newChildren = new ImmutableCollection<INode<TNodeValue>>.Prototype(node.Children);
                            newChildren.Add(iterator2.Current);
                            while (iterator2.MoveNext())
                                newChildren.Add(iterator2.Current);
                        }
                    }
                    else if (!hasNext2)
                    {
                        newChildren = new ImmutableCollection<INode<TNodeValue>>.Prototype(index);
                        newChildren.AddRange(node.Children.Take(index));
                    }
                    else
                    {
                        newChildren = new ImmutableCollection<INode<TNodeValue>>.Prototype(node.Children.Count);
                        newChildren.AddRange(node.Children.Take(index));
                        newChildren.Add(iterator2.Current);
                        while (iterator2.MoveNext())
                            newChildren.Add(iterator2.Current);
                    }
                }
            }

            if (newChildren != null)
                return nodeReconstructionFactory.CreateNew(newChildren.ToImmutable(), keepValue ? node.Value : value).ToUnitSet();

            if (!keepValue && !EqualityComparer<TNodeValue>.Default.Equals(node.Value, value))
                return nodeReconstructionFactory.CreateNew(node.Children, value).ToUnitSet();

            return node.ToUnitSet();
        }
    }
}