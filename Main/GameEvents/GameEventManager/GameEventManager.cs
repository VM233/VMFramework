using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Sirenix.OdinInspector;
using UnityEngine;
using VMFramework.Core;
using VMFramework.GameLogicArchitecture;
using VMFramework.Procedure;
using VMFramework.Properties;

namespace VMFramework.GameEvents
{
    [ManagerCreationProvider(ManagerType.EventCore)]
    public partial class GameEventManager : ManagerBehaviour<GameEventManager>
    {
        public event Action<IGameEvent> OnGameEventRegistered;
        public event Action<IGameEvent> OnGameEventUnregistered;

        [ShowInInspector]
        private readonly Dictionary<string, IGameEvent> allGameEvents = new();

        protected override void Awake()
        {
            base.Awake();

            Clear(false);
        }

        protected virtual void OnDestroy()
        {
            Clear(true);
        }

        protected virtual void Clear(bool recycle)
        {
            if (recycle)
            {
                foreach (var gameEvent in allGameEvents.Values)
                {
                    GameItemManager.Instance.Return(gameEvent);
                }
            }

            OnGameEventRegistered = null;
            OnGameEventUnregistered = null;

            allGameEvents.Clear();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Register(string gameEventID)
        {
            var gameEvent = GameItemManager.Instance.Get<IGameEvent>(gameEventID);

            if (gameEvent == null)
            {
                Debug.LogError($"GameEventManager: Could not find {nameof(IGameEvent)} with ID {gameEventID}");
                return;
            }

            Register(gameEvent);
        }

        public virtual void Register(IGameEvent gameEvent)
        {
            if (gameEvent == null)
            {
                Debug.LogError("GameEventManager: Cannot register null game event.");
                return;
            }

            if (allGameEvents.TryAdd(gameEvent.id, gameEvent) == false)
            {
                Debug.LogError($"Game Event with ID: {gameEvent.id} already exists.");
                return;
            }

            gameEvent.IsEnabled.OnDirty += OnEnableChanged;

            OnGameEventRegistered?.Invoke(gameEvent);
        }

        public virtual void Unregister(string gameEventID)
        {
            if (allGameEvents.Remove(gameEventID, out var gameEvent) == false)
            {
                UnityEngine.Debug.LogWarning($"Game Event with ID: {gameEventID} does not exist.");
                return;
            }

            gameEvent.IsEnabled.OnDirty -= OnEnableChanged;

            OnGameEventUnregistered?.Invoke(gameEvent);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Unregister(IGameEvent gameEvent)
        {
            if (gameEvent == null)
            {
                Debug.LogError("GameEventManager: Cannot unregister null game event.");
                return;
            }

            Unregister(gameEvent.id);
        }

        protected virtual void OnEnableChanged(IReadOnlyProperty property, bool initial)
        {
            var current = property.GetValue<bool>();
            var owner = property.Owner;

            if (owner is not IGameEvent gameEvent)
            {
                UnityEngine.Debug.LogError(
                    $"[{nameof(GameEventManager)}] Owner: {owner.GetType().Name} is not an {nameof(IGameEvent)}");
                return;
            }

            if (CoreSetting.GameEventGeneralSetting.directDependencies.TryGetValue(gameEvent.id,
                    out var dependencies) == false)
            {
                return;
            }

            if (current == false)
            {
                foreach (var dependency in dependencies)
                {
                    Disable(dependency, gameEvent);
                }
            }
            else
            {
                foreach (var dependency in dependencies)
                {
                    Enable(dependency, gameEvent);
                }
            }
        }
    }
}