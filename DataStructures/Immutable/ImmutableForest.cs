using System;
using System.Collections.Generic;
using System.Linq;
using DataStructures.Immutable.Tree;

namespace DataStructures.Immutable
{
    /// <summary>
    /// Represents an immutable forest, composed of rooted trees and disconnected branches and leaves.
    /// </summary>
    /// <typeparam name="TValue">Type of the value contained in each tree node.</typeparam>
    public class ImmutableForest<TValue> : IReadableForest<TValue>, IImmutable
    {
        private readonly ImmutableCollection<INode<TValue>> nodes;

        public ImmutableForest(ImmutableCollection<INode<TValue>> nodes)
        {
            this.nodes = nodes;
        }

        public ImmutableCollection<INode<TValue>> Nodes
        {
            get { return this.nodes; }
        }

        public IEnumerable<Root<TValue>> RootsEnum
        {
            get { return this.Nodes.OfType<Root<TValue>>(); }
        }

        public IEnumerable<NonRoot<TValue>> DisconnectedNodesEnum
        {
            get { return this.Nodes.OfType<NonRoot<TValue>>(); }
        }

        public override string ToString()
        {
            return string.Format("{0} {{ Nodes = {1} }}", this.GetType().Name, this.nodes.Count);
        }

        public IReadableForest<TResultingNodeValue> Visit<TResultingNodeValue>(Visitor<TValue, INode<TResultingNodeValue>> visitor)
        {
            var newNodes = this.nodes.SelectMany(x => x.Visit(visitor)).ToImmutable();

            // Comparing each node to see if anything changed.
            // When comparing immutable objects, a reference comparison is enough.
            var equals = this.nodes.Count == newNodes.Count && Enumerable.SequenceEqual(this.nodes, newNodes, EqualityComparer<object>.Default);

            // If all children are equal and in the same order, then we return the current forest as the result.
            if (equals && this is IReadableForest<TResultingNodeValue>)
                return (IReadableForest<TResultingNodeValue>)this;

            return new ImmutableForest<TResultingNodeValue>(newNodes);
        }

        IEnumerable<INode<TValue>> IReadableForest<TValue>.RootNodes
        {
            get { return this.RootsEnum; }
        }

        IEnumerable<INode<TValue>> IReadableForest<TValue>.DisconnectedNodes
        {
            get { return this.DisconnectedNodesEnum; }
        }

        IReadOnlyList<INode<TValue>> IReadableForest<TValue>.Nodes
        {
            get { return this.Nodes; }
        }
    }

    /// <summary>
    /// Represents an immutable forest containing uniquely identifiable node values.
    /// </summary>
    /// <typeparam name="TKey">Type of the identity object.</typeparam>
    /// <typeparam name="TValue">Type of the value of each node.</typeparam>
    public class ImmutableForest<TKey, TValue> : ImmutableForest<TValue>
    {
        private readonly Dictionary<TKey, INode<TValue>> mapOfIdsToConnectedNodes;
        private readonly Dictionary<TKey, INode<TValue>> mapOfIdsToDisconnectedNodes;

        public ImmutableForest(ImmutableCollection<INode<TValue>> nodes, Func<TValue, TKey> idGetter)
            : base(nodes)
        {
            this.mapOfIdsToConnectedNodes = nodes
                .OfType<Root<TValue>>()
                .SelectMany(NodeExtensions.GetAllNodesEnum)
                .ToDictionary(n => idGetter(n.Value));

            this.mapOfIdsToDisconnectedNodes = nodes
                .OfType<NonRoot<TValue>>()
                .SelectMany(NodeExtensions.GetAllNodesEnum)
                .ToDictionary(n => idGetter(n.Value));
        }

        public bool TryGetNodeById(TKey id, out INode<TValue> outNode)
        {
            if (this.mapOfIdsToConnectedNodes.TryGetValue(id, out outNode))
                return true;

            var result = this.mapOfIdsToDisconnectedNodes.TryGetValue(id, out outNode);
            return result;
        }

        public override string ToString()
        {
            return string.Format("{0} {{ Nodes = {1} }}", this.GetType().Name, this.Nodes.Count);
        }
    }
}