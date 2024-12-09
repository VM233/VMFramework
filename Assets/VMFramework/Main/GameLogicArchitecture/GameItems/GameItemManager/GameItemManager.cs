using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using VMFramework.Core;
using VMFramework.Core.Pools;

namespace VMFramework.GameLogicArchitecture
{
    public static partial class GameItemManager
    {
        public sealed class GameItemManagerEventsReceiver : IGameItemEventsReceiver
        {
            public event Action<IGameItem> OnGameItemCreated
            {
                add => GameItemManager.OnGameItemCreated += value;
                remove => GameItemManager.OnGameItemCreated -= value;
            }
            public event Action<IGameItem> OnGameItemDestroyed
            {
                add => GameItemManager.OnGameItemDestroyed += value;
                remove => GameItemManager.OnGameItemDestroyed -= value;
            }
        }
        
        private static event Action<IGameItem> OnGameItemCreated;
        private static event Action<IGameItem> OnGameItemDestroyed;
        
        private static readonly Dictionary<string, CreatablePoolItemsPool<IGameItem, string>> pools = new();
        private static readonly Func<string, IGameItem> createGameItemHandler = CreateGameItem;

        private static IGameItem CreateGameItem(string id)
        {
            var gamePrefab = GamePrefabManager.GetGamePrefabStrictly(id);
            
            var gameItem = gamePrefab.GenerateGameItem();
            
            return gameItem;
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static CreatablePoolItemsPool<IGameItem, string> CreatePool(string id)
        {
            var pool = new CreatablePoolItemsPool<IGameItem, string>(id, createGameItemHandler, 20000);
            pools.Add(id, pool);
            return pool;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IGameItem Get(string id)
        {
            if (pools.TryGetValue(id, out var pool) == false)
            {
                pool = CreatePool(id);
            }
            
            var gameItem = pool.Get(out _);
            
            OnGameItemCreated?.Invoke(gameItem);
            
            return gameItem;
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static TGameItem Get<TGameItem>(string id) where TGameItem : IGameItem
        {
            if (pools.TryGetValue(id, out var pool) == false)
            {
                pool = CreatePool(id);
            }
            
            var gameItem = pool.Get(out _);
            
            OnGameItemCreated?.Invoke(gameItem);
            
            return (TGameItem)gameItem;
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Return(IGameItem gameItem)
        {
            if (pools.TryGetValue(gameItem.id, out var pool) == false)
            {
                pool = CreatePool(gameItem.id);
            }
            
            pool.Return(gameItem);
            
            OnGameItemDestroyed?.Invoke(gameItem);
        }
        
        /// <summary>
        /// Clones the game item and returns the clone.
        /// </summary>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static TGameItem GetClone<TGameItem>(this TGameItem instance) where TGameItem : class, IGameItem
        {
            instance.AssertIsNotNull(nameof(instance));
            
            var clone = Get(instance.id);

            clone.CloneTo(instance);

            return (TGameItem)clone;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void PrewarmUntil(string id, int count)
        {
            if (count <= 0)
            {
                return;
            }
            
            if (pools.TryGetValue(id, out var pool) == false)
            {
                pool = CreatePool(id);
            }

            if (pool.Count >= count)
            {
                return;
            }

            var gameItems = ListPool<IGameItem>.Default.Get();
            gameItems.Clear();

            for (int i = 0; i < count; i++)
            {
                var gameItem = pool.Get(out _);
                
                OnGameItemCreated?.Invoke(gameItem);
                
                gameItems.Add(gameItem);
            }

            foreach (var gameItem in gameItems)
            {
                pool.Return(gameItem);
            
                OnGameItemDestroyed?.Invoke(gameItem);
            }
            
            gameItems.ReturnToDefaultPool();
        }
    }
}