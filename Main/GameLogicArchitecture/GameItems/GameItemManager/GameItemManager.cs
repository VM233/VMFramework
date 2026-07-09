using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Sirenix.OdinInspector;
using UnityEngine.Profiling;
using VMFramework.Core;
using VMFramework.Core.Pools;
using VMFramework.Procedure;

namespace VMFramework.GameLogicArchitecture
{
    [ManagerCreationProvider(ManagerType.GameItemCore)]
    public class GameItemManager : ManagerBehaviour<IGameItemManager>, IGameItemManager
    {
        public sealed class GameItemManagerEventsReceiver : IGameItemEventsReceiver
        {
            public event Action<IGameItem> OnGameItemCreated
            {
                add => Instance.OnGameItemCreated += value;
                remove => Instance.OnGameItemCreated -= value;
            }
            
            public event Action<IGameItem> OnGameItemDestroyed
            {
                add => Instance.OnGameItemDestroyed += value;
                remove => Instance.OnGameItemDestroyed -= value;
            }
        }
        
        public event Action<IGameItem> OnGameItemCreated;
        public event Action<IGameItem> OnGameItemDestroyed;
        
        [ShowInInspector]
        protected readonly Dictionary<string, CreatablePoolItemsPool<IGameItem, string>> pools = new();
        protected Func<string, IGameItem> createGameItemHandler;

        protected override void Awake()
        {
            base.Awake();
            
            pools.Clear();
            createGameItemHandler = CreateGameItem;
        }

        private IGameItem CreateGameItem(string id)
        {
            var gamePrefab = GamePrefabManager.GetGamePrefabStrictly(id);
            
            Profiler.BeginSample($"Create GameItem:{id}");
            var gameItem = gamePrefab.GenerateGameItem();
            Profiler.EndSample();
            
            return gameItem;
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private CreatablePoolItemsPool<IGameItem, string> CreatePool(string id)
        {
            var pool = new CreatablePoolItemsPool<IGameItem, string>(id, createGameItemHandler, 20000);
            pools.Add(id, pool);
            return pool;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public IGameItem Get(string id)
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
        public TGameItem Get<TGameItem>(string id) where TGameItem : IGameItem
        {
            if (pools.TryGetValue(id, out var pool) == false)
            {
                pool = CreatePool(id);
            }
            
            var gameItem = pool.Get(out _);
            
            OnGameItemCreated?.Invoke(gameItem);

            if (gameItem is TGameItem typedGameItem)
            {
                return typedGameItem;
            }

            throw new InvalidCastException($"GameItem {gameItem} is not of type {typeof(TGameItem)}");
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Return(IGameItem gameItem)
        {
            if (gameItem == null)
            {
                return;
            }

            if (gameItem.IsDestroyed)
            {
                UnityEngine.Debug.LogError($"Attempting to return a destroyed game item: {gameItem}");
                return;
            }
            
            if (pools.TryGetValue(gameItem.id, out var pool) == false)
            {
                pool = CreatePool(gameItem.id);
            }
            
            pool.Return(gameItem);
            
            OnGameItemDestroyed?.Invoke(gameItem);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void PrewarmUntil(string id, int count)
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