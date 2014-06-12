using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataStructures.FixedTypeLists
{
    public struct Unit<T>
    {
        private readonly T value1;

        public Unit(T value)
        {
            this.value1 = value;
        }

        public T Value
        {
            get { return this.value1; }
        }

        public Duo<T, T2> Add<T2>(T2 value)
        {
            return new Duo<T, T2>(this.value1, value);
        }
    }

    public struct Duo<T1, T2>
    {
        private readonly T1 first;
        private readonly T2 second;

        public Duo(T1 first, T2 second)
        {
            this.first = first;
            this.second = second;
        }

        public T1 First
        {
            get { return this.first; }
        }

        public T2 Second
        {
            get { return this.second; }
        }
    }

    public struct Option<T>
    {
        private readonly bool hasValue;
        private readonly T value;

        public Option(T value)
            : this()
        {
            if (value != null)
            {
                this.hasValue = true;
                this.value = value;
            }
        }

        public bool IsSomething
        {
            get { return this.hasValue; }
        }

        public bool IsNothing
        {
            get { return !this.hasValue; }
        }

        public T Value
        {
            get
            {
                if (!this.hasValue)
                    throw new InvalidOperationException("Option has got no value.");

                return this.value;
            }
        }

        public TResult Match<TResult>(TResult whenHasValue, TResult whenIsNothing)
        {
            return this.hasValue ? whenHasValue : whenIsNothing;
        }

        public TResult Match<TResult>(Func<T, TResult> whenHasValue, Func<TResult> whenIsNothing)
        {
            return this.hasValue ? whenHasValue(this.value) : whenIsNothing();
        }

        public static implicit operator Option<T>(T value)
        {
            return new Option<T>(value);
        }

        public static explicit operator T(Option<T> value)
        {
            return value.Value;
        }


        public static bool operator ==(Option<T> a, Option<T> b)
        {
            return EqualityComparer<Option<T>>.Default.Equals(a, b);
        }

        public static bool operator !=(Option<T> a, Option<T> b)
        {
            return !(a == b);
        }

        public override string ToString()
        {
            return this.Match(v => string.Format("Some {0}", v), () => "None");
        }

        public static Option<T> Nothing
        {
            get { return new Option<T>(); }
        }
    }

    public static class EnumerableExtensions
    {
        public static IEnumerable<Duo<Option<TFirst>, Option<TSecond>>> OptionZip<TFirst, TSecond>(
            this IEnumerable<TFirst> first,
            IEnumerable<TSecond> second,
            bool overIterateFirst = true,
            bool overIterateSecond = true)
        {
            using (var iterator1 = first.GetEnumerator())
            using (var iterator2 = second.GetEnumerator())
            {
                bool hasNext1 = true;
                var hasNext2 = true;
                while ((hasNext1 = hasNext1 && iterator1.MoveNext()) | (hasNext2 = hasNext2 && iterator2.MoveNext()))
                {
                    if ((!overIterateFirst && !hasNext1) || (!overIterateSecond && !hasNext2))
                    {
                        yield break;
                    }

                    yield return new Duo<Option<TFirst>, Option<TSecond>>(
                        hasNext1 ? iterator1.Current : Option<TFirst>.Nothing,
                        hasNext2 ? iterator2.Current : Option<TSecond>.Nothing);
                }
            }
        }
    }
}
