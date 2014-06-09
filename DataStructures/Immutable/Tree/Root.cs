using System.Collections.Generic;

namespace DataStructures.Immutable.Tree
{
    public abstract class Root<TValue> : Node<TValue>,
        IRoot
    {
        protected Root(ImmutableCollection<IVisitableNode<TValue>> children, TValue value)
            : base(children, value)
        {
        }

        protected Root(IEnumerable<IVisitableNode<TValue>> children, TValue value)
            : base(children, value)
        {
        }

        public override bool IsRoot { get { return true; } }
        public override bool IsInternalNode { get { return false; } }
    }
}