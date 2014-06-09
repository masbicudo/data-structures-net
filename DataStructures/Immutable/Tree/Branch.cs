using System.Collections.Generic;

namespace DataStructures.Immutable.Tree
{
    public class Branch<TItem> : Node<TItem>,
        IBranch
    {
        public Branch(ImmutableCollection<IVisitableNode<TItem>> children, TItem value)
            : base(children, value)
        {
        }

        public Branch(IEnumerable<IVisitableNode<TItem>> children, TItem value)
            : base(children, value)
        {
        }

        public override bool IsRoot
        {
            get { return false; }
        }

        public override bool IsBranch
        {
            get { return true; }
        }

        public override bool IsLeaf
        {
            get { return false; }
        }

        public override bool IsInternalNode
        {
            get { return true; }
        }
    }
}