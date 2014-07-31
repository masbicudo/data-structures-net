using DataStructures.Immutable.Tree;

namespace DataStructures.Immutable
{
    public interface IImmutableSpecificTreeBuilderWithContext<TData, TValue>
    {
        INode<TValue> BuildRoot(TData rootData);

        INode<TValue> BuildBranchOrLeaf(TreeBuildingContext<TData, TValue> context, TData nodeData);
    }
}