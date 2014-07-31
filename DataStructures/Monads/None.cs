using System;
using System.Collections;
using System.Collections.Generic;

namespace DataStructures.Monads
{
    public sealed class None<T> : IOption<T>
    {
        private static readonly None<T> instance = new None<T>();

        private None()
        {
        }

        public T Value
        {
            get { throw new InvalidOperationException("This is a Null object, with a non existing value."); }
        }

        public bool HasValue
        {
            get { return false; }
        }

        public static None<T> Instance
        {
            get { return instance; }
        }

        public IEnumerator<T> GetEnumerator()
        {
            yield break;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }
}