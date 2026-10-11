using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.ExceptionServices;
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
            private readonly IGameItemManager source;

            public GameItemManagerEventsReceiver() => source = Instance;

            public event Action<IGameItem> OnGameItemCreated
            {
                add => source.OnGameItemCreated += value;
                remove => source.OnGameItemCreated -= value;
            }

            public event Action<IGameItem> OnGameItemDestroyed
            {
                add => source.OnGameItemDestroyed += value;
                remove => source.OnGameItemDestroyed -= value;
            }
        }

        public event Action<IGameItem> OnGameItemCreated;
        public event Action<IGameItem> OnGameItemDestroyed;

        [ShowInInspector]
        protected readonly Dictionary<string, CreatablePoolItemsPool<IGameItem, string>> pools = new();
        protected Func<string, IGameItem> createGameItemHandler;
        private Action<IGameItem> publishRetirementHandler;

        private readonly GameItemMaterialization materialization = new();

        public GameItemInitializationKind CurrentInitializationKind => materialization.CurrentKind;

        protected override void Awake()
        {
            base.Awake();

            pools.Clear();
            createGameItemHandler = CreateGameItem;
            publishRetirementHandler = PublishRetirement;
        }

        private IGameItem CreateGameItem(string id)
        {
            var gamePrefab = GamePrefabManager.GetGamePrefabStrictly(id);

            Profiler.BeginSample($"Create GameItem:{id}");
            try { return gamePrefab.GenerateGameItem(); }
            finally { Profiler.EndSample(); }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private CreatablePoolItemsPool<IGameItem, string> CreatePool(string id)
        {
            var pool = new CreatablePoolItemsPool<IGameItem, string>(id, createGameItemHandler, 20000);
            pools.Add(id, pool);
            return pool;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private IGameItem Rent(string id, GameItemInitializationKind kind,
            out CreatablePoolItemsPool<IGameItem, string> pool)
        {
            if (pools.TryGetValue(id, out pool) == false)
            {
                pool = CreatePool(id);
            }

            return materialization.Rent(pool, kind);
        }

        private void RetireUnpublished(CreatablePoolItemsPool<IGameItem, string> pool,
            IGameItem rental, Exception primary, string phase)
        {
            try { pool.Return(rental, publishRetirementHandler); }
            catch (Exception retirement)
            {
                throw new PoolItemAcquisitionException(pool, rental, phase, primary, retirement);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public IGameItem Get(string id)
        {
            var gameItem = Rent(id, GameItemInitializationKind.AuthoredDefaults, out var pool);
            try
            {
                OnGameItemCreated?.Invoke(gameItem);
                return gameItem;
            }
            catch (Exception primary)
            {
                RetireUnpublished(pool, gameItem, primary, "game-item.get.return");
                throw;
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public TGameItem Get<TGameItem>(string id) where TGameItem : IGameItem
        {
            var gameItem = Rent(id, GameItemInitializationKind.AuthoredDefaults, out var pool);
            try
            {
                OnGameItemCreated?.Invoke(gameItem);
                if (gameItem is TGameItem typedGameItem)
                {
                    return typedGameItem;
                }
                throw new InvalidCastException($"GameItem {gameItem} is not of type {typeof(TGameItem)}");
            }
            catch (Exception primary)
            {
                RetireUnpublished(pool, gameItem, primary, "game-item.get.return");
                throw;
            }
        }

        public TGameItem Clone<TGameItem>(TGameItem source, StateCloneContext context)
            where TGameItem : IGameItem
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (source.IsDestroyed)
                throw new InvalidOperationException($"GameItem '{source.id}' cannot enter phase 'game-item.clone.source' after destruction or Return.");

            var rental = Rent(source.id, GameItemInitializationKind.ClonedState, out var pool);
            try
            {
                var clone = (TGameItem)rental;
                if (clone.StateCloner == null)
                {
                    throw new InvalidOperationException($"GameItem {clone.id} has no state cloner.");
                }

                clone.StateCloner.CloneFrom(source.StateCloner, context);
                OnGameItemCreated?.Invoke(clone);
                return clone;
            }
            catch (Exception primary)
            {
                RetireUnpublished(pool, rental, primary, "game-item.clone.return");
                throw;
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Return(IGameItem gameItem)
        {
            if (gameItem == null)
            {
                return;
            }

            if (gameItem.IsDestroyed)
                throw new InvalidOperationException($"GameItem '{gameItem.id}' cannot enter phase 'game-item.return' after destruction or Return.");

            if (pools.TryGetValue(gameItem.id, out var pool) == false)
            {
                pool = CreatePool(gameItem.id);
            }

            pool.Return(gameItem, publishRetirementHandler);
        }

        private void PublishRetirement(IGameItem gameItem)
        {
            if (!gameItem.IsDestroyed)
                throw new GameItemCleanupException(gameItem, "manager.destroyed.publication",
                    new Exception[] { new InvalidOperationException(
                        $"GameItem '{gameItem.id}' did not end its lease before manager retirement publication.") });
            var failures = new GameItemCleanupFailures(gameItem, "manager.destroyed.notification");
            var subscribers = OnGameItemDestroyed;
            if (subscribers != null)
                foreach (Action<IGameItem> subscriber in subscribers.GetInvocationList())
                    try { subscriber(gameItem); }
                    catch (Exception failure) { failures.Capture(failure); }
            failures.ThrowIfFailed();
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
            List<Exception> failures = null;
            try
            {
                try
                {
                    for (int i = 0; i < count; i++)
                    {
                        var gameItem = materialization.Rent(pool, GameItemInitializationKind.AuthoredDefaults);
                        try { gameItems.Add(gameItem); }
                        catch (Exception primary)
                        {
                            RetireUnpublished(pool, gameItem, primary, "prewarm.buffer-adoption.return");
                            throw;
                        }
                        OnGameItemCreated?.Invoke(gameItem);
                    }
                }
                catch (Exception primary) { (failures ??= new()).Add(primary); }

                foreach (var gameItem in gameItems)
                {
                    try { pool.Return(gameItem, publishRetirementHandler); }
                    catch (Exception retirement)
                    {
                        (failures ??= new()).Add(new PoolItemAcquisitionException(pool, gameItem,
                            "prewarm.return", retirement));
                    }
                }
            }
            finally
            {
                gameItems.Clear();
                try { gameItems.ReturnToDefaultPool(); }
                catch (Exception retirement) { (failures ??= new()).Add(retirement); }
            }

            if (failures?.Count == 1) ExceptionDispatchInfo.Capture(failures[0]).Throw();
            if (failures?.Count > 1)
                throw new AggregateException("Game item prewarming and its rental retirement failed.", failures);
        }
    }
}
