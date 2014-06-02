namespace DataStructures.Immutable.Tree
{
    public class Branch<TKey, TItem> : Node<TKey, TItem>,
        IBranch
    {
        public override bool IsRoot { get { return false; } }
        public override bool IsBranch { get { return true; } }
        public override bool IsLeaf { get { return false; } }
        public override bool IsInternalNode { get { return true; } }
    }
}