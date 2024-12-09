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
            
            if (mouseEvents.TryGetValue(eventType, out ColliderMouseEvent mouseEvent) == false)
            {
                return;
            }
            
            mouseEvent.Propagate(trigger);
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void AddCallback(MouseEventType eventType, Action<ColliderMouseEventTrigger> callback,
            int priority = GameEventPriority.TINY)
        {
            if (mouseEvents.TryGetValue(eventType, out ColliderMouseEvent gameEvent) == false)
            {
                gameEvent = GameItemManager.Get<ColliderMouseEvent>(ColliderMouseEventConfig.ID);
                mouseEvents.Add(eventType, gameEvent);
            }

            gameEvent.AddCallback(callback, priority);
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void RemoveCallback(MouseEventType eventType, Action<ColliderMouseEventTrigger> callback)
        {
            if (mouseEvents.TryGetValue(eventType, out ColliderMouseEvent gameEvent) == false)
            {
                Debugger.LogWarning($"{nameof(ColliderMouseEvent)} for {eventType} not found.");
                return;
            }

            gameEvent.RemoveCallback(callback);
        }
    }
}