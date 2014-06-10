using System;
using System.Linq;

namespace DataStructures.Immutable.Tree
{
    /// <summary>
    /// Represents a leaf in an immutable tree structure.
    /// </summary>
    /// <typeparam name="TValue"> Type of data stored in this node. </typeparam>
    public class Leaf<TValue> : NonRoot<TValue>,
        ILeaf, INonRoot
    {
        public Leaf(TValue value)
            : base(Enumerable.Empty<INode<TValue>>(), value)
        {
        }

        /// <summary>
        /// Gets a value indicating that this node is not a branch.
        /// </summary>
        public override bool IsBranch
        {
            get { return false; }
        }

        /// <summary>
        /// Gets a value indicating that this node is a leaf.
        /// </summary>
        public override bool IsLeaf
        {
            get { return true; }
        }

        /// <summary>
        /// Gets a value indicating that this node is not an internal tree node.
        /// </summary>
        public override bool IsInternalNode
        {
            get { return false; }
        }

        protected override INode<TValue> CreateNew(ImmutableCollection<INode<TValue>> children, TValue value)
        {
            if (children.Count > 0)
                throw new Exception("Cannot create a leaf with children.");

            return new Leaf<TValue>(value);
        }
    }
}