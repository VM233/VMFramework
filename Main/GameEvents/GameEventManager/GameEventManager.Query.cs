using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine.InputSystem;

namespace VMFramework.GameEvents
{
    public partial class GameEventManager
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public TGameEvent GetGameEventStrictly<TGameEvent>(string id)
        {
            if (allGameEvents.TryGetValue(id, out IGameEvent gameEventInterface) == false)
            {
                throw new KeyNotFoundException($"GameEvent with id {id} not found.");
            }
            
            if (gameEventInterface is not TGameEvent typedGameEvent)
            {
                throw new InvalidCastException($"GameEvent with id {id} is not of type {typeof(TGameEvent)}.");
            }
            
            return typedGameEvent;
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public IGameEvent GetGameEventStrictly(string id)
        {
            if (allGameEvents.TryGetValue(id, out var gameEvent) == false)
            {
                throw new KeyNotFoundException($"GameEvent with id {id} not found.");
            }
            
            return gameEvent;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool TryGetGameEvent<TGameEvent>(string id, out TGameEvent gameEvent)
        {
            if (allGameEvents.TryGetValue(id, out IGameEvent gameEventInterface))
            {
                if (gameEventInterface is TGameEvent typedGameEvent)
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
            return allGameEvents.TryGetValue(id, out gameEvent);
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
    }
}