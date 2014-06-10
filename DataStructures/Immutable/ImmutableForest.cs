using System.Collections.Generic;
using DataStructures.Immutable.Tree;

namespace DataStructures.Immutable
{
    public class ImmutableForest<TValue>
    {
        private readonly ImmutableCollection<IVisitableNode<TValue>> nodes;

        public ImmutableForest(ImmutableCollection<IVisitableNode<TValue>> nodes)
        {
            this.nodes = nodes;
        }

        public override string ToString()
        {
            return string.Format("{0} {{ Nodes = {1} }}", this.GetType().Name, this.nodes.Count);
        }

        public ImmutableCollection<IVisitableNode<TValue>> Nodes
        {
            get { return this.nodes; }
        }
    }
}