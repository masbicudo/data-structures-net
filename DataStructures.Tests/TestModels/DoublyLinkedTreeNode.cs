using System;
using System.Diagnostics;

namespace DataStructures.Tests.TestModels
{
    [DebuggerDisplay("DoublyLinkedTreeNode: {Value}")]
    internal class DoublyLinkedTreeNode<TValue>
    {
        private DoublyLinkedTreeNode<TValue> parent;
        private DoublyLinkedTreeNode<TValue>[] children;

        public DoublyLinkedTreeNode(TValue value, DoublyLinkedTreeNode<TValue> parent)
        {
            this.Value = value;
            this.Parent = parent;
        }

        public DoublyLinkedTreeNode(TValue value, DoublyLinkedTreeNode<TValue>[] children)
        {
            this.Value = value;
            this.Children = children;
        }

        public TValue Value { get; private set; }

        public DoublyLinkedTreeNode<TValue> Parent
        {
            get
            {
                return this.parent;
            }
            set
            {
                if (this.parent != null)
                    throw new Exception("Cannot set Parent more than once");

                this.parent = value;
            }
        }

        public DoublyLinkedTreeNode<TValue>[] Children
        {
            get
            {
                return this.children;
            }
            set
            {
                if (this.children != null)
                    throw new Exception("Cannot set Children more than once");

                this.children = value;
            }
        }
    }
}