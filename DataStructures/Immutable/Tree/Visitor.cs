using System.Collections.Generic;

namespace DataStructures.Immutable.Tree
{
    public delegate IEnumerable<TResult> Visitor<in TValue, TResult>(INode<TValue> node, IEnumerable<TResult> children);
}