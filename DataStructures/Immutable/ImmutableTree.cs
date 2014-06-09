using System;
using System.Collections.Generic;
using DataStructures.Immutable.Tree;

namespace DataStructures.Immutable
{
    public class ImmutableTree<TValue>
    {
        private IRoot rootNode;

        public ImmutableTree(IEnumerable<INode<TValue>> children)
        {
            // TODO: Complete member initialization
            this.children = children;
        }
    }
}