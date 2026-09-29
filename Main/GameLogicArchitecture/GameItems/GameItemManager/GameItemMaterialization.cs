using System;
using VMFramework.Core.Pools;

namespace VMFramework.GameLogicArchitecture
{
    public sealed class GameItemMaterialization
    {
        private GameItemInitializationKind? currentKind;

        public GameItemInitializationKind CurrentKind => currentKind ??
            throw new InvalidOperationException("No native game-item initialization is active.");

        public TItem Rent<TItem>(PoolItemsPool<TItem> pool, GameItemInitializationKind kind)
            where TItem : IPoolItem
        {
            var previousKind = currentKind;
            currentKind = kind;
            try
            {
                return pool.Get(out _);
            }
            finally
            {
                currentKind = previousKind;
            }
        }
    }
}
