using DataStructures.Immutable.Tree;
using DataStructures.SystemExtensions;
using System;
using System.Linq;

namespace DataStructures.Immutable
{
    public class ImmutableTree<TKey, TValue> : ImmutableForest<TKey, TValue>
    {
        private Root<TValue> root;

        public ImmutableTree(Root<TValue> root, Func<TValue, TKey> idGetter)
            : base(root.ToUnitSet().Cast<INode<TValue>>().ToImmutable(), idGetter)
        {
            this.root = root;
        }

        public Root<TValue> Root
        {
            get { return this.root; }
        }
    }
}