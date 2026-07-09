using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Sirenix.OdinInspector;
using VMFramework.Core;
using VMFramework.GameLogicArchitecture;

namespace VMFramework.GameEvents
{
    public partial class ColliderMouseEventManager
    {
        [ShowInInspector]
        private static readonly Dictionary<MouseEventType, ColliderMouseEvent> mouseEvents = new();

        [Button]
        private static void Invoke(MouseEventType eventType, ColliderMouseEventTrigger trigger)
        {
            trigger.TriggerEvent(eventType);
            
            if (mouseEvents.TryGetValue(eventType, out var mouseEvent) == false)
            {
                return;
            }
            
            mouseEvent.Propagate(trigger);
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void AddCallback(MouseEventType eventType, Action<ColliderMouseEventTrigger> callback,
            int priority = PriorityDefines.TINY)
        {
            if (mouseEvents.TryGetValue(eventType, out var gameEvent) == false)
            {
                gameEvent = GameItemManager.Instance.Get<ColliderMouseEvent>(ColliderMouseEventConfig.ID);
                mouseEvents.Add(eventType, gameEvent);
            }

            gameEvent.AddCallback(callback, priority);
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void RemoveCallback(MouseEventType eventType, Action<ColliderMouseEventTrigger> callback)
        {
            if (mouseEvents.TryGetValue(eventType, out var gameEvent) == false)
            {
                UnityEngine.Debug.LogWarning($"{nameof(ColliderMouseEvent)} for {eventType} not found.");
                return;
            }

            gameEvent.RemoveCallback(callback);
        }
    }
}