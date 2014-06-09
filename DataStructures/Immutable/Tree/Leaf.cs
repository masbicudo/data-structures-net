using System.Linq;

namespace DataStructures.Immutable.Tree
{
    public class Leaf<TItem> : Node<TItem>,
        ILeaf
    {
        public Leaf(TItem value)
            : base(Enumerable.Empty<IVisitableNode<TItem>>(), value)
        {
        }

        public override bool IsRoot
        {
            get { return false; }
        }

        public override bool IsBranch
        {
            get { return false; }
        }

        public override bool IsLeaf
        {
            get { return true; }
        }

        public override bool IsInternalNode
        {
            get { return false; }
        }
    }
}