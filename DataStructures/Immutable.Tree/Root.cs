namespace DataStructures.Immutable.Tree
{
    public class Root<TKey, TItem> : Node<TKey, TItem>,
        IRoot, IBranch
    {
        public override bool IsRoot { get { return true; } }
        public override bool IsBranch { get { return true; } }
        public override bool IsLeaf { get { return false; } }
        public override bool IsInternalNode { get { return false; } }
    }
}