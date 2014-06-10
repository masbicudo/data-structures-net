using System.Collections.Generic;

namespace DataStructures.Immutable.Tree
{
    public abstract class NonRoot<TValue> : Node<TValue>,
        IRoot
    {
        protected NonRoot(ImmutableCollection<INode<TValue>> children, TValue value)
            : base(children, value)
        {
        }

        protected NonRoot(IEnumerable<INode<TValue>> children, TValue value)
            : base(children, value)
        {
        }

        public override bool IsRoot { get { return false; } }
    }
}