using System;
using VMFramework.Core.Pools;

namespace VMFramework.Maps
{
    /// <inheritdoc cref="PoolItemsPool{TPoolItem}"/>
    public sealed class GridChunkPool : PoolItemsPool<IGridChunk>
    {
        private readonly IGridMap gridMap;
        private readonly Func<IGridChunk> onCreateFunc;

        public GridChunkPool(IGridMap gridMap, Func<IGridChunk> onCreateFunc, int capacity) : base(capacity)
        {
            this.gridMap = gridMap;
            this.onCreateFunc = onCreateFunc;
        }

        protected override IGridChunk CreateItem()
        {
            var item = onCreateFunc();
            item.OnCreate(gridMap);
            return item;
        }
    }
}