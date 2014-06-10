using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace DataStructures.Immutable
{
    /// <summary>
    /// Represents an immutable collection. This is different from a `ReadOnlyCollection`
    /// because it is impossible to refer to the inner collection.
    /// </summary>
    /// <typeparam name="TItem"> Type of the items inside this collection. These may be mutable or not. </typeparam>
    public class ImmutableCollection<TItem> : ReadOnlyCollection<TItem>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ImmutableCollection{TItem}"/> class.
        /// </summary>
        /// <param name="items"> Items to be inserted in this immutable collection. </param>
        public ImmutableCollection(IEnumerable<TItem> items)
            : base(items.ToArray())
        {
        }
    }
}
