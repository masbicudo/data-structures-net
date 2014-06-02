namespace DataStructures.Immutable.Tree
{
    public struct VisitResult<TItem>
    {
        public INode<TItem> node;
    }

    public struct VisitResult<TItem, TData>
    {
        public INode<TItem> node;
        public TData data;
    }
}