using JetBrains.Annotations;
using System;
using System.Collections.Generic;

namespace DataStructures.Immutable
{
    public class TreeBuildingContext<TData, TValue>
    {
        private readonly Func<TreeBuildingContext<TData, TValue>, IEnumerable<TValue>> contextChildrenGetter;
        private readonly TreeBuildingContext<TData, TValue> parentContext;
        private readonly TreeBuildingContext<TData, TValue> rootContext;
        private readonly TData data;

        private TValue value;
        private bool hasValue;
        private Action postProcessor;

        public TreeBuildingContext(
            [NotNull] TreeBuildingContext<TData, TValue> parentContext,
            TData data,
            Func<TreeBuildingContext<TData, TValue>, IEnumerable<TValue>> contextChildrenGetter)
        {
            if (parentContext == null)
                throw new ArgumentNullException("parentContext");

            this.rootContext = parentContext.rootContext;
            this.data = data;
            this.contextChildrenGetter = contextChildrenGetter;
            this.parentContext = parentContext;
        }

        public TreeBuildingContext(
            TData data,
            Func<TreeBuildingContext<TData, TValue>, IEnumerable<TValue>> contextChildrenGetter)
        {
            this.rootContext = this;
            this.parentContext = null;
            this.data = data;
            this.contextChildrenGetter = contextChildrenGetter;
        }

        public TreeBuildingContext<TData, TValue> ParentContext
        {
            get { return this.parentContext; }
        }

        public TreeBuildingContext<TData, TValue> RootContext
        {
            get { return this.rootContext; }
        }

        public TData Data
        {
            get { return this.data; }
        }

        public bool HasValue
        {
            get { return this.hasValue; }
        }

        public TValue Value
        {
            get
            {
                if (!this.hasValue)
                    throw new Exception("Value is not yet defined");

                return this.value;
            }

            set
            {
                if (this.hasValue)
                    throw new Exception("Cannot set Value more than once");

                this.hasValue = true;
                this.value = value;
            }
        }

        public TValue GetParentOrDefault(Func<TValue> defaultParent = null)
        {
            return this.parentContext != null ? this.parentContext.Value
                : defaultParent != null ? defaultParent()
                : default(TValue);
        }

        [CanBeNull]
        public IEnumerable<TValue> GetChildren()
        {
            return this.contextChildrenGetter(this);
        }

        public void RegisterPostProcessing(Action postProcessor)
        {
            this.postProcessor += postProcessor;
        }

        public void ExecutePostProcessing()
        {
            if (this.postProcessor != null)
                this.postProcessor();
        }
    }
}