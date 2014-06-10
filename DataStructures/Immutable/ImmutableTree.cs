using System;
using System.Linq;
using DataStructures.Immutable.Tree;
using System.Collections.Generic;

namespace DataStructures.Immutable
{
    public class ImmutableTree<TValue, TId>
    {
        private Root<TValue> root;

        public ImmutableTree(Root<TValue> root, Func<TValue, TId> idGetter)
        {
            this.root = root;

            var allNodes = root.GetAllNodesEnum().ToArray();
        }
    }
}