using System.Collections.Generic;

namespace DataStructures.Immutable.Tree
{
    public abstract class Root<TValue> : Node<TValue>,
        IRoot
    {
        protected Root(ImmutableCollection<INode<TValue>> children, TValue value)
            : base(children, value)
        {
        }

        protected Root(IEnumerable<INode<TValue>> children, TValue value)
            : base(children, value)
        {
        }

        public override bool IsRoot
        {
            get { return true; }
        }

        public override bool IsInternalNode
        {
            get { return false; }
        }
    }
}