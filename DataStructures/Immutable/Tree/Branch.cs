using System.Collections.Generic;

namespace DataStructures.Immutable.Tree
{
    /// <summary>
    /// Represents a branch in an immutable tree structure.
    /// </summary>
    /// <typeparam name="TValue"> Type of data stored in this node. </typeparam>
    public class Branch<TValue> : NonRoot<TValue>,
        IBranch, INonRoot
    {
        public Branch(ImmutableCollection<INode<TValue>> children, TValue value)
            : base(children, value)
        {
        }

        public Branch(IEnumerable<INode<TValue>> children, TValue value)
            : base(children, value)
        {
        }

        /// <summary>
        /// Gets a value indicating that this node is a branch.
        /// </summary>
        public override bool IsBranch
        {
            get { return true; }
        }

        /// <summary>
        /// Gets a value indicating that this node is not a leaf.
        /// </summary>
        public override bool IsLeaf
        {
            get { return false; }
        }

        /// <summary>
        /// Gets a value indicating that this node is an internal tree node.
        /// </summary>
        public override bool IsInternalNode
        {
            get { return true; }
        }

        protected override INode<TValue> CreateNew(ImmutableCollection<INode<TValue>> children, TValue value)
        {
            return new Branch<TValue>(children, value);
        }
    }
}