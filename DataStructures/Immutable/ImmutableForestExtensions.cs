using DataStructures.Immutable.Tree;

namespace DataStructures.Immutable
{
    public static class ImmutableForestExtensions
    {
        public static INode<TValue> GetNodeByIdOrNull<TValue, TKey>(ImmutableForest<TKey, TValue> forest, TKey id)
        {
            INode<TValue> outNode;
            forest.TryGetNodeById(id, out outNode);
            return outNode;
        }
    }
}