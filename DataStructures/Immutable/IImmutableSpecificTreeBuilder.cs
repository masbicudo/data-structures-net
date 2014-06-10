using DataStructures.Immutable.Tree;

namespace DataStructures.Immutable
{
    public interface IImmutableSpecificTreeBuilder<in TData, TValue>
    {
        INode<TValue> BuildRoot(TData rootData);

        INode<TValue> BuildBranchOrLeaf(TData nodeData);
    }
}