using System;
using System.Collections.Generic;

namespace DataStructures.Immutable
{
    internal static class FuncCached
    {
        public static Func<TIn, TOut> Create<TIn, TOut>(Func<TIn, TOut> func)
        {
            var cache = new Dictionary<TIn, TOut>();
            return x =>
            {
                TOut result;
                if (!cache.TryGetValue(x, out result))
                    cache[x] = result = func(x);

                return result;
            };
        }
    }
}