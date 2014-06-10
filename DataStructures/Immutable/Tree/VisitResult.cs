namespace DataStructures.Immutable.Tree
{
    public struct VisitResult<TItem>
    {
        public INode<TItem> node;
    }

    public struct VisitResult<TValue, TData>
    {
        public INode<TValue> node;
        public TData data;
    }
}