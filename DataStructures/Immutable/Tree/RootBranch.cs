using System.Collections.Generic;

namespace DataStructures.Immutable.Tree
{
    /// <summary>
    /// Represents a node that is both the root and a branch of the tree.
    /// </summary>
    /// <typeparam name="TValue"> Type of the value store in this node. </typeparam>
    public class RootBranch<TValue> : Root<TValue>,
        IRoot, IBranch
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RootBranch{TValue}"/> class,
        /// passing the child nodes, and the value stored in this node.
        /// </summary>
        /// <param name="children"> The child nodes of the node. </param>
        /// <param name="value"> The value contained by the tree node. </param>
        public RootBranch(ImmutableCollection<IVisitableNode<TValue>> children, TValue value)
            : base(children, value)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RootBranch{TValue}"/> class,
        /// passing the child nodes, and the value stored in this node.
        /// </summary>
        /// <param name="children"> The child nodes of the node. </param>
        /// <param name="value"> The value contained by the tree node. </param>
        public RootBranch(IEnumerable<IVisitableNode<TValue>> children, TValue value)
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
    }
}