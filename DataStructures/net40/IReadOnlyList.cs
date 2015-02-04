using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

#if net40
namespace DataStructures.net40
{
    public interface IReadOnlyList<out T> :
        IEnumerable<T>
    {
        T this[int index] { get; }
        int Count { get; }
    }
}
#endif