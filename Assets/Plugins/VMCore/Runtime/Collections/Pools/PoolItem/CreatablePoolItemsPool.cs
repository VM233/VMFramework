using System;
using System.Collections.Generic;

namespace VMFramework.Core.Pools
{
    /// <inheritdoc cref="PoolItemsPool{TPoolItem}"/>
    public sealed class CreatablePoolItemsPool<TPoolItem> : PoolItemsPool<TPoolItem>
        where TPoolItem : ICreatablePoolItem
    {
        private readonly Func<TPoolItem> onCreateFunc;

        public CreatablePoolItemsPool(Func<TPoolItem> onCreateFunc, int capacity) : base(capacity)
        {
            onCreateFunc.AssertIsNotNull(nameof(onCreateFunc));
            this.onCreateFunc = onCreateFunc;
        }

        protected override TPoolItem CreateItem()
        {
            var item = onCreateFunc();
            item.OnCreate();
            return item;
        }
    }

    /// <inheritdoc cref="PoolItemsPool{TPoolItem}"/>
    public sealed class CreatablePoolItemsPool<TItem, TArgument> : PoolItemsPool<TItem>
        where TItem : ICreatablePoolItem<TArgument>
    {
        private readonly TArgument argument;
        private readonly Func<TArgument, TItem> onCreateFunc;

        public CreatablePoolItemsPool(TArgument argument, Func<TArgument, TItem> onCreateFunc, int capacity) :
            base(capacity)
        {
            onCreateFunc.AssertIsNotNull(nameof(onCreateFunc));
            capacity.AssertIsAbove(0, nameof(capacity));

            this.argument = argument;
            this.onCreateFunc = onCreateFunc;
        }

        protected override TItem CreateItem()
        {
            var item = onCreateFunc(argument);
            item.OnCreate(argument);
            return item;
        }
    }
}