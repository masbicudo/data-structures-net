using System.Collections.Generic;

namespace DataStructures.Immutable.Tree
{
    public delegate IEnumerable<VisitResult<TItem>> Visitor<TItem>(INode<TItem> node, IEnumerable<VisitResult<TItem>> children);

    public delegate IEnumerable<VisitResult<TItem, TData>> Visitor<TItem, TData>(INode<TItem> node, IEnumerable<VisitResult<TItem, TData>> children);
}