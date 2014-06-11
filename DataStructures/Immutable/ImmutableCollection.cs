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
            if (items == null)
                throw new ArgumentNullException("items");

            Debug.Assert(!(items is IImmutablePrototype<ImmutableCollection<TItem>>), "Items collection cannot be an immutable prototype of itself.");
            return items.ToArray();
        }

        private static List<TItem> GetItems(Prototype prototype)
        {
            if (prototype == null)
                throw new ArgumentNullException("prototype");

            Debug.Assert(prototype.IsReadOnly, "Builder must be set to read-only state before creating an immutable from it.");
            return prototype.Inner;
        }

        public ImmutableCollection<TItem> ToImmutable()
        {
            return this;
        }

        /// <summary>
        /// Represents a prototype of an immutable collection.
        /// This collection is mutable until `ToImmutable` is called.
        /// </summary>
        public sealed class Prototype : IList<TItem>, IReadOnlyList<TItem>, IImmutablePrototype<ImmutableCollection<TItem>>, IEnumerable<TItem>, IEnumerable, IMayBeImmutable
        {
            private bool isReadOnly;

            internal readonly List<TItem> Inner;

            public Prototype()
            {
                this.Inner = new List<TItem>();
            }

            public Prototype(IEnumerable<TItem> items)
            {
                this.Inner = items.ToList();
            }

            public Prototype(int capacity)
            {
                this.Inner = new List<TItem>(capacity);
            }

            public IEnumerator<TItem> GetEnumerator()
            {
                return this.Inner.GetEnumerator();
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return this.GetEnumerator();
            }

            public void Add(TItem item)
            {
                if (this.isReadOnly)
                    throw new Exception("This collection is now in read-only state.");

                this.Inner.Add(item);
            }

            public void AddRange(IEnumerable<TItem> items)
            {
                if (this.isReadOnly)
                    throw new Exception("This collection is now in read-only state.");

                this.Inner.AddRange(items);
            }

            public void Clear()
            {
                if (this.isReadOnly)
                    throw new Exception("This collection is now in read-only state.");

                this.Inner.Clear();
            }

            public bool Contains(TItem item)
            {
                return this.Inner.Contains(item);
            }

            public void CopyTo(TItem[] array, int arrayIndex)
            {
                if (this.isReadOnly)
                    throw new Exception("This collection is now in read-only state.");

                this.Inner.CopyTo(array, arrayIndex);
            }

            public bool Remove(TItem item)
            {
                if (this.isReadOnly)
                    throw new Exception("This collection is now in read-only state.");

                return this.Inner.Remove(item);
            }

            public int Count
            {
                get { return this.Inner.Count; }
            }

            public bool IsReadOnly
            {
                get { return this.isReadOnly; }
            }

            public int IndexOf(TItem item)
            {
                return this.Inner.IndexOf(item);
            }

            public void Insert(int index, TItem item)
            {
                if (this.isReadOnly)
                    throw new Exception("This collection is now in read-only state.");

                this.Inner.Insert(index, item);
            }

            public void RemoveAt(int index)
            {
                if (this.isReadOnly)
                    throw new Exception("This collection is now in read-only state.");

                this.Inner.RemoveAt(index);
            }

            public TItem this[int index]
            {
                get
                {
                    return this.Inner[index];
                }

                set
                {
                    if (this.isReadOnly)
                        throw new Exception("This collection is now in read-only state.");

                    this.Inner[index] = value;
                }
            }

            public void SetImmutableFlag()
            {
                this.Inner.TrimExcess();
                this.isReadOnly = true;
            }

            public ImmutableCollection<TItem> ToImmutable()
            {
                this.SetImmutableFlag();
                return new ImmutableCollection<TItem>(this);
            }

            bool IMayBeImmutable.IsImmutable
            {
                get { return this.isReadOnly; }
            }
        }
    }
}
