using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace DataStructures.Immutable.Tree
{
    public delegate IEnumerable<TItem> Visitor<TItem>(INode<TItem> item, IEnumerable<INode<TItem>> children);

    public interface INode<out TItem>
    {
        bool IsRoot { get; }
        bool IsBranch { get; }
        bool IsLeaf { get; }

        TItem Value { get; }
    }

    public abstract class Node<TKey, TItem> : INode<TItem>
    {
        private TKey id;
        private TKey parentId;

        protected Node(ReadOnlyCollection<Node<TKey, TItem>> children, TItem value, TKey id, TKey parentId, bool isConnected)
        {
            this.Children = children;
            this.Value = value;
            this.id = id;
            this.parentId = parentId;
            this.IsConnected = isConnected;
        }

        public abstract bool IsRoot { get; }
        public abstract bool IsBranch { get; }
        public abstract bool IsLeaf { get; }

        public TItem Value { get; private set; }

        public bool IsConnected { get; private set; }

        public ReadOnlyCollection<Node<TKey, TItem>> Children { get; private set; }

        public IEnumerable<Node<TKey, TItem>> Visit(Visitor<TItem> visitor)
        {
            // visiting each child node, and then checking which ones have changed
            var newChildren = this.Children.SelectMany(x => x.Visit(visitor));

            // replace current node, with a list of nodes
            // change list of child nodes (add/remove)


            var newItem = visitor(this, newChildren);

            return newItem;
        }
    }

    public class Forest<TItem>
    {
    }

    public class Tree<TItem>
    {
    }

    public interface IRoot
    {
    }

    public interface IBranch
    {
    }

    public interface ILeaf
    {
    }

    public class Root<TKey, TItem> : Node<TKey, TItem>,
        IRoot
    {
        public override bool IsRoot
        {
            get { return true; }
        }

        public override bool IsBranch
        {
            get { return false; }
        }

        public override bool IsLeaf
        {
            get { return false; }
        }
    }

    public class Branch<TKey, TItem> : Node<TKey, TItem>,
        IBranch
    {
        public override bool IsRoot
        {
            get { return false; }
        }

        public override bool IsBranch
        {
            get { return true; }
        }

        public override bool IsLeaf
        {
            get { return false; }
        }
    }

    public class Leaf<TKey, TItem> : Node<TKey, TItem>,
        ILeaf
    {
        public override bool IsRoot
        {
            get { return false; }
        }

        public override bool IsBranch
        {
            get { return false; }
        }

        public override bool IsLeaf
        {
            get { return true; }
        }
    }
}
