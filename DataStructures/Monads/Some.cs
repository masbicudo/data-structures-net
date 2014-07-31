using System;
using System.Collections;
using System.Collections.Generic;

namespace DataStructures.Monads
{
    public sealed class Some<T> : IOption<T>
    {
        public Some(T value)
        {
            if (value == null)
                throw new Exception("Cannot create a Some with null inside");

            this.Value = value;
        }

        public T Value { get; private set; }

        public bool HasValue
        {
            get { return true; }
        }

        public IEnumerator<T> GetEnumerator()
        {
            yield return this.Value;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }
}