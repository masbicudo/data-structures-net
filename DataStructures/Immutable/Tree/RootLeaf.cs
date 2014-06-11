using System;
using System.Linq;

namespace DataStructures.Immutable.Tree
{
    /// <summary>
    /// Represents a node that is both the root and leaf of the tree.
    /// </summary>
    /// <typeparam name="TValue"> Type of the value store in this node. </typeparam>
    public class RootLeaf<TValue> : Root<TValue>, ILeaf
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RootLeaf{TValue}"/> class,
        /// passing the value that it will store.
        /// </summary>
        /// <param name="value"> The value contained by the tree node. </param>
        public RootLeaf(TValue value)
            : base(Enumerable.Empty<Node<TValue>>(), value)
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

        protected override INode<TNewNodeValue> CreateNew<TNewNodeValue>(ImmutableCollection<INode<TNewNodeValue>> children, TNewNodeValue value)
        {
            if (children.Count > 0)
                throw new Exception("Cannot create a leaf with children.");

            return new RootLeaf<TNewNodeValue>(value);
        }
    }
}