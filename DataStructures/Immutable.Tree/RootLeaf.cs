namespace DataStructures.Immutable.Tree
{
    /// <summary>
    /// Represents a node that is both the root and leaf of the tree.
    /// </summary>
    /// <typeparam name="TKey">
    /// </typeparam>
    /// <typeparam name="TItem">
    /// </typeparam>
    public class RootLeaf<TKey, TItem> : Node<TKey, TItem>,
        IRoot, ILeaf
    {
        public override bool IsRoot { get { return true; } }
        public override bool IsBranch { get { return false; } }
        public override bool IsLeaf { get { return true; } }
        public override bool IsInternalNode { get { return false; } }
    }
}