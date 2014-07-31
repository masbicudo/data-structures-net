using DataStructures.Monads;
using JetBrains.Annotations;
using System.Collections.Generic;

namespace DataStructures.Immutable
{
    /// <summary>
    /// Represents the context of building an immutable tree.
    /// </summary>
    /// <typeparam name="TSourceData"> Type of the data used to get a value for each node in the tree. </typeparam>
    /// <typeparam name="TNodeValue"> Type of the value of nodes in the tree. </typeparam>
    public abstract class TreeBuildingContextBase<TSourceData, TNodeValue>
    {
        private readonly TSourceData sourceData;

        protected TreeBuildingContextBase(TSourceData sourceData)
        {
            this.sourceData = sourceData;
        }

        /// <summary>
        /// Gets the source data that is used to get the value of the node being built.
        /// </summary>
        public TSourceData SourceData
        {
            get { return this.sourceData; }
        }

        /// <summary>
        /// Gets the value that will be used to build the parent of the node being built in the current context.
        /// </summary>
        public abstract IOption<TNodeValue> ParentNodeValue { get; }

        /// <summary>
        /// Gets the values that were used to build the children of the node being built in the current context.
        /// </summary>
        [CanBeNull]
        public abstract IEnumerable<TNodeValue> ChildNodesValues { get; }
    }
}