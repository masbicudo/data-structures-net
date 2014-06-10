using DataStructures.Immutable.Tree;

namespace DataStructures.Immutable
{
    public interface IImmutableSpecificTreeBuilder<in TData, TValue>
    {
        IVisitableNode<TValue> BuildRoot(TData rootData);

        IVisitableNode<TValue> BuildBranchOrLeaf(TData nodeData);
    }
}