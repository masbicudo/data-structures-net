using DataStructures.Monads;
using JetBrains.Annotations;
using System;
using System.Collections.Generic;

namespace DataStructures.Immutable
{
    /// <summary>
    /// Represents the context of building an immutable tree.
    /// </summary>
    /// <typeparam name="TSourceData"> Type of the data used to get a value for each node in the tree. </typeparam>
    /// <typeparam name="TNodeValue"> Type of the value of nodes in the tree. </typeparam>
    public class TreeBuildingContext<TSourceData, TNodeValue> :
        TreeBuildingContextBase<TSourceData, TNodeValue>
    {
        [NotNull]
        private readonly Func<TreeBuildingContext<TSourceData, TNodeValue>, IEnumerable<TNodeValue>> contextChildrenGetter;

        private readonly TreeBuildingContext<TSourceData, TNodeValue> parentContext;

        [NotNull]
        private readonly TreeBuildingContext<TSourceData, TNodeValue> rootContext;

        private TNodeValue nodeValue;
        private bool hasNodeValue;
        private Action postProcessor;

        /// <summary>
        /// Initializes a new instance of the <see cref="TreeBuildingContext{TSourceData, TNodeValue}"/> class,
        /// that is a sub-context of a tree building process.
        /// </summary>
        /// <param name="parentContext"> The parent context of this context. </param>
        /// <param name="sourceData"> Source data that is used to create the value of the current node. </param>
        /// <param name="contextChildrenGetter">
        /// A delegate that can get the values of child nodes of the node represented by this context.
        /// </param>
        /// <exception cref="ArgumentNullException"> <paramref name="contextChildrenGetter"/> and <paramref name="contextChildrenGetter"/> cannot be null. </exception>
        public TreeBuildingContext(
            [NotNull] TreeBuildingContext<TSourceData, TNodeValue> parentContext,
            TSourceData sourceData,
            [NotNull] Func<TreeBuildingContext<TSourceData, TNodeValue>, IEnumerable<TNodeValue>> contextChildrenGetter)
            : base(sourceData)
        {
            if (parentContext == null)
                throw new ArgumentNullException("parentContext");
            if (contextChildrenGetter == null)
                throw new ArgumentNullException("contextChildrenGetter");

            this.rootContext = parentContext.rootContext;
            this.contextChildrenGetter = contextChildrenGetter;
            this.parentContext = parentContext;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TreeBuildingContext{TSourceData, TNodeValue}"/> class,
        /// that is the root context of a tree building process.
        /// </summary>
        /// <param name="sourceData"> Source data that is used to create the value of the current node. </param>
        /// <param name="contextChildrenGetter">
        /// A delegate that can get the values of child nodes of the node represented by this context.
        /// </param>
        /// <exception cref="ArgumentNullException"> <paramref name="contextChildrenGetter"/> cannot be null. </exception>
        public TreeBuildingContext(
            TSourceData sourceData,
            [NotNull] Func<TreeBuildingContext<TSourceData, TNodeValue>, IEnumerable<TNodeValue>> contextChildrenGetter)
            : base(sourceData)
        {
            if (contextChildrenGetter == null)
                throw new ArgumentNullException("contextChildrenGetter");

            this.rootContext = this;
            this.parentContext = null;
            this.contextChildrenGetter = contextChildrenGetter;
        }

        /// <summary>
        /// Gets the parent context. If this is the root context then the return is null.
        /// </summary>
        [CanBeNull]
        public TreeBuildingContext<TSourceData, TNodeValue> ParentContext
        {
            get { return this.parentContext; }
        }

        /// <summary>
        /// Gets the context that is the root of the current tree building operation.
        /// </summary>
        [NotNull]
        public TreeBuildingContext<TSourceData, TNodeValue> RootContext
        {
            get { return this.rootContext; }
        }

        /// <summary>
        /// Gets a value indicating whether a node value has been defined.
        /// A value is required to build the final node, so this is eventually going to be true.
        /// </summary>
        public bool HasNodeValue
        {
            get { return this.hasNodeValue; }
        }

        /// <summary>
        /// Gets or sets the value of the node being built in this context.
        /// </summary>
        public TNodeValue NodeValue
        {
            get
            {
                if (!this.hasNodeValue)
                    throw new Exception("NodeValue is not yet defined");

                return this.nodeValue;
            }

            set
            {
                if (this.hasNodeValue)
                    throw new Exception("Cannot set NodeValue more than once");

                this.hasNodeValue = true;
                this.nodeValue = value;
            }
        }

        /// <summary>
        /// Gets the value that will be used to build the parent of the node being built in the current context.
        /// </summary>
        public override IOption<TNodeValue> ParentNodeValue
        {
            get
            {
                if (this.parentContext == null)
                    return None<TNodeValue>.Instance;

                return new Some<TNodeValue>(this.parentContext.NodeValue);
            }
        }

        /// <summary>
        /// Gets the values that were used to build the children of the node being built in the current context.
        /// </summary>
        public override IEnumerable<TNodeValue> ChildNodesValues
        {
            get { return this.contextChildrenGetter(this); }
        }

        /// <summary>
        /// Register a post processing method, that will run after all the tree is ready.
        /// </summary>
        /// <param name="action">Action that is going to be executed.</param>
        public void RegisterPostProcessing(Action action)
        {
            this.postProcessor += action;
        }

        /// <summary>
        /// Executes all the registered post processing actions.
        /// </summary>
        public void ExecutePostProcessing()
        {
            if (this.postProcessor != null)
                this.postProcessor();
        }
    }
}