namespace DataStructures.Immutable.Tree
{
    public class Leaf<TKey, TItem> : Node<TKey, TItem>,
        ILeaf
    {
        public override bool IsRoot { get { return false; } }
        public override bool IsBranch { get { return false; } }
        public override bool IsLeaf { get { return true; } }
        public override bool IsInternalNode { get { return false; } }
    }
}