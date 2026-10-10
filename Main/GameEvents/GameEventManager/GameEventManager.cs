using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.InputSystem;
using VMFramework.Core;
using VMFramework.GameLogicArchitecture;
using VMFramework.Procedure;
using VMFramework.Properties;

namespace VMFramework.GameEvents
{
    [ManagerCreationProvider(ManagerType.EventCore)]
    public class GameEventManager : ManagerBehaviour<GameEventManager>
    {
        internal static bool IsRecyclingRegisteredEvents { get; private set; }

        public event Action<IGameEvent> OnGameEventRegistered;
        public event Action<IGameEvent> OnGameEventUnregistered;

        [ShowInInspector]
        private readonly Dictionary<string, (IGameEvent Event, IGameItemManager RentalOwner)> allGameEvents = new();

        protected override void Awake()
        {
            base.Awake();

            Clear(false);
        }

        protected override void OnDestroy()
        {
            try { Clear(true); }
            finally { base.OnDestroy(); }
        }

        protected virtual void Clear(bool recycle)
        {
            List<Exception> failures = null;
            IsRecyclingRegisteredEvents = recycle;
            try
            {
                var registrations = allGameEvents.Values.ToArray();
                allGameEvents.Clear();
                foreach (var registration in registrations)
                {
                    try { registration.Event.IsEnabled.OnDirty -= OnEnableChanged; }
                    catch (Exception error) { (failures ??= new()).Add(error); }
                    if (recycle && registration.RentalOwner != null)
                    {
                        try { registration.RentalOwner.Return(registration.Event); }
                        catch (Exception error) { (failures ??= new()).Add(error); }
                    }
                }
                if (failures != null) throw new AggregateException(failures);
            }
            finally
            {
                IsRecyclingRegisteredEvents = false;
                OnGameEventRegistered = null;
                OnGameEventUnregistered = null;
                allGameEvents.Clear();
            }
        }

        /// <summary>Registers a rental owned here until unregister or manager retirement.</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Register(string gameEventID)
        {
            var rentalOwner = GameItemManager.Instance;
            var gameEvent = rentalOwner.Get<IGameEvent>(gameEventID);
            Register(gameEvent, rentalOwner);
        }

        /// <summary>Registers a borrowed event; its caller retains lifetime ownership.</summary>
        public virtual void Register(IGameEvent gameEvent) => Register(gameEvent, null);

        private void Register(IGameEvent gameEvent, IGameItemManager rentalOwner)
        {
            if (gameEvent == null) throw new ArgumentNullException(nameof(gameEvent));
            try
            {
                if (IsRecyclingRegisteredEvents)
                    throw new InvalidOperationException("Game event registration rejected during registered event retirement.");
                allGameEvents.Add(gameEvent.id, (gameEvent, rentalOwner));
            }
            catch (Exception primary)
            {
                if (rentalOwner != null)
                {
                    try { rentalOwner.Return(gameEvent); }
                    catch (Exception retirement) { throw new AggregateException(primary, retirement); }
                }
                throw;
            }
            gameEvent.IsEnabled.OnDirty += OnEnableChanged;
            OnGameEventRegistered?.Invoke(gameEvent);
        }

        public virtual void Unregister(string gameEventID)
        {
            if (allGameEvents.Remove(gameEventID, out var registration) == false)
            {
                Debug.LogWarning($"Game Event with ID: {gameEventID} does not exist.");
                return;
            }
            try
            {
                registration.Event.IsEnabled.OnDirty -= OnEnableChanged;
                OnGameEventUnregistered?.Invoke(registration.Event);
            }
            catch (Exception primary)
            {
                if (registration.RentalOwner != null)
                {
                    try { registration.RentalOwner.Return(registration.Event); }
                    catch (Exception retirement) { throw new AggregateException(primary, retirement); }
                }
                throw;
            }
            if (registration.RentalOwner != null) registration.RentalOwner.Return(registration.Event);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Unregister(IGameEvent gameEvent)
        {
            if (gameEvent == null) throw new ArgumentNullException(nameof(gameEvent));
            if (allGameEvents.TryGetValue(gameEvent.id, out var registration) &&
                !ReferenceEquals(registration.Event, gameEvent))
                throw new InvalidOperationException($"Game event unregister rejected for a different registered owner of '{gameEvent.id}'.");
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
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public TGameEvent GetGameEventStrictly<TGameEvent>(string id)
        {
            if (allGameEvents.TryGetValue(id, out var registration) == false)
            {
                throw new KeyNotFoundException($"GameEvent with id {id} not found.");
            }

            if (registration.Event is not TGameEvent typedGameEvent)
            {
                throw new InvalidCastException($"GameEvent with id {id} is not of type {typeof(TGameEvent)}.");
            }

            return typedGameEvent;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public IGameEvent GetGameEventStrictly(string id)
        {
            if (allGameEvents.TryGetValue(id, out var registration) == false)
            {
                throw new KeyNotFoundException($"GameEvent with id {id} not found.");
            }

            return registration.Event;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool TryGetGameEvent<TGameEvent>(string id, out TGameEvent gameEvent)
        {
            if (allGameEvents.TryGetValue(id, out var registration))
            {
                if (registration.Event is TGameEvent typedGameEvent)
                {
                    gameEvent = typedGameEvent;
                    return true;
                }
            }

            gameEvent = default;
            return false;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool TryGetGameEvent(string id, out IGameEvent gameEvent)
        {
            if (allGameEvents.TryGetValue(id, out var registration))
            {
                gameEvent = registration.Event;
                return true;
            }
            gameEvent = null;
            return false;
        }

        /// <summary>
        /// https://docs.unity3d.com/Packages/com.unity.inputsystem@1.14/manual/Migration.html
        /// </summary>
        /// <param name="id"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T GetValue<T>(string id) where T : struct
        {
            var gameEvent = GetGameEventStrictly<InputSystemGameEvent>(id);

            return gameEvent.InputAction.ReadValue<T>();
        }

        /// <summary>
        /// https://docs.unity3d.com/Packages/com.unity.inputsystem@1.14/manual/Migration.html
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool GetBoolValue(string id)
        {
            var gameEvent = GetGameEventStrictly<InputSystemGameEvent>(id);

            return gameEvent.InputAction.IsPressed();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public InputAction GetInputAction(string id)
        {
            var gameEvent = GetGameEventStrictly<InputSystemGameEvent>(id);
            return gameEvent.InputAction;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void AddCallback(string id, Delegate callback, int priority)
        {
            var gameEvent = GetGameEventStrictly(id);

            gameEvent.AddCallback(callback, priority);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void AddCallback(string id, Action callback, int priority)
        {
            var gameEvent = GetGameEventStrictly<IReadOnlyParameterlessGameEvent>(id);

            gameEvent.AddCallback(callback, priority);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void AddCallback<TArgument>(string id, Action<TArgument> callback, int priority)
        {
            var gameEvent = GetGameEventStrictly<IReadOnlyParameterizedGameEvent<TArgument>>(id);

            gameEvent.AddCallback(callback, priority);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void RemoveCallback(string id, Delegate callback)
        {
            var gameEvent = GetGameEventStrictly(id);

            gameEvent.RemoveCallback(callback);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void RemoveCallback(string id, Action callback)
        {
            var gameEvent = GetGameEventStrictly<IReadOnlyParameterlessGameEvent>(id);

            gameEvent.RemoveCallback(callback);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void RemoveCallback<TArgument>(string id, Action<TArgument> callback)
        {
            var gameEvent = GetGameEventStrictly<IReadOnlyParameterizedGameEvent<TArgument>>(id);

            gameEvent.RemoveCallback(callback);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool IsEnabled(string id)
        {
            var gameEvent = GetGameEventStrictly(id);

            var isEnabled = gameEvent.IsEnabled.GetValue();

            return isEnabled;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Enable(string id, IToken token)
        {
            var gameEvent = GetGameEventStrictly(id);

            gameEvent.IsEnabled.RemoveToken(token);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Disable(string id, IToken token)
        {
            var gameEvent = GetGameEventStrictly(id);

            gameEvent.IsEnabled.AddToken(token);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Enable<TEnumerable>(TEnumerable ids, IToken token) where TEnumerable : IEnumerable<string>
        {
            if (ids == null)
            {
                return;
            }

            foreach (var id in ids)
            {
                Enable(id, token);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Disable<TEnumerable>(TEnumerable ids, IToken token) where TEnumerable : IEnumerable<string>
        {
            if (ids == null)
            {
                return;
            }

            foreach (var id in ids)
            {
                Disable(id, token);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Propagate(string id)
        {
            var gameEvent = GetGameEventStrictly<IParameterlessGameEvent>(id);

            gameEvent.Propagate();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Propagate<TArgument>(string id, TArgument argument)
        {
            var gameEvent = GetGameEventStrictly<IParameterizedGameEvent<TArgument>>(id);

            gameEvent.Propagate(argument);
        }
    }
}
