using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;

namespace DataStructures.Immutable
{
    /// <summary>
    /// Represents an immutable collection. This is different from a `ReadOnlyCollection`
    /// because it is impossible to have a reference to the inner collection, even for the creator of the collection.
    /// (It is possible only using reflection)
    /// </summary>
    /// <typeparam name="TItem"> Type of the items inside this collection. These may be mutable or not. </typeparam>
    public sealed class ImmutableCollection<TItem> : ReadOnlyCollection<TItem>, IImmutablePrototype<ImmutableCollection<TItem>>, IImmutable, IEnumerable<TItem>, IEnumerable
    {
        public static readonly ImmutableCollection<TItem> Empty = new ImmutableCollection<TItem>(Enumerable.Empty<TItem>());

        /// <summary>
        /// Initializes a new instance of the <see cref="ImmutableCollection{TItem}"/> class.
        /// </summary>
        /// <param name="items"> Items to be inserted in this immutable collection. </param>
        internal ImmutableCollection(IEnumerable<TItem> items)
            : base(GetItems(items))
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ImmutableCollection{TItem}"/> class.
        /// </summary>
        /// <param name="prototype"> Items to be inserted in this immutable collection. </param>
        internal ImmutableCollection(Prototype prototype)
            : base(GetItems(prototype))
        {
        }

        private static TItem[] GetItems(IEnumerable<TItem> items)
        {
            Debug.Assert(!(items is IImmutablePrototype<ImmutableCollection<TItem>>), "Items collection cannot be an immutable prototype of itself.");
            return items.ToArray();
        }

        private static List<TItem> GetItems(Prototype items)
        {
            Debug.Assert(items.IsReadOnly, "Builder must be set to read-only state before creating an immutable from it.");
            return items.inner;
        }

        public ImmutableCollection<TItem> ToImmutable()
        {
            return this;
        }

        public sealed class Prototype : IList<TItem>, IReadOnlyList<TItem>, IImmutablePrototype<ImmutableCollection<TItem>>, IEnumerable<TItem>, IEnumerable
        {
            private bool isReadOnly;

            internal readonly List<TItem> inner = new List<TItem>();

            public IEnumerator<TItem> GetEnumerator()
            {
                return this.inner.GetEnumerator();
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return this.GetEnumerator();
            }

            public void Add(TItem item)
            {
                if (this.isReadOnly)
                    throw new Exception("This collection is now in read-only state.");

                this.inner.Add(item);
            }

            public void Clear()
            {
                if (this.isReadOnly)
                    throw new Exception("This collection is now in read-only state.");

                this.inner.Clear();
            }

            public bool Contains(TItem item)
            {
                return this.inner.Contains(item);
            }

            public void CopyTo(TItem[] array, int arrayIndex)
            {
                if (this.isReadOnly)
                    throw new Exception("This builder has already been used to build an immutable object.");

                this.inner.CopyTo(array, arrayIndex);
            }

            public bool Remove(TItem item)
            {
                if (this.isReadOnly)
                    throw new Exception("This builder has already been used to build an immutable object.");

                return this.inner.Remove(item);
            }

            public int Count
            {
                get { return this.inner.Count; }
            }

            public bool IsReadOnly { get { return this.isReadOnly; } }
            public int IndexOf(TItem item)
            {
                throw new System.NotImplementedException();
            }

            public void Insert(int index, TItem item)
            {
                throw new System.NotImplementedException();
            }

            public void RemoveAt(int index)
            {
                throw new System.NotImplementedException();
            }

            public TItem this[int index]
            {
                get { throw new System.NotImplementedException(); }
                set { throw new System.NotImplementedException(); }
            }

            public void SetReadOnlyFlag()
            {
                this.isReadOnly = true;
            }

            public ImmutableCollection<TItem> ToImmutable()
            {
                return new ImmutableCollection<TItem>(this);
            }
        }
    }
}
