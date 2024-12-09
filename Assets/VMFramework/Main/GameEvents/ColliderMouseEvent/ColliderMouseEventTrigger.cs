using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using UnityEngine;
using Sirenix.OdinInspector;
using VMFramework.Core;

namespace VMFramework.GameEvents
{
    [DisallowMultipleComponent]
    public sealed partial class ColliderMouseEventTrigger : MonoBehaviour
    {
        public bool draggable = false;

        [ShowIf(nameof(draggable))]
        public MouseButtonType dragButton = MouseButtonType.LeftButton;

        [field: Required]
        [field: SerializeField]
        public Transform owner { get; private set; }

        [ShowInInspector]
        private Dictionary<MouseEventType, HashSet<Action<ColliderMouseEventTrigger, MouseEventType>>> callbacks;

        public void SetOwner(Transform owner)
        {
            if (this.owner != null && owner != null)
            {
                Debugger.LogWarning($"ColliderMouseEventTrigger already has an owner : {owner.name}!");
            }

            this.owner = owner;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void AddCallback(MouseEventType eventType,
            [NotNull] Action<ColliderMouseEventTrigger, MouseEventType> callback)
        {
            callbacks ??= new();

            if (callbacks.TryGetValue(eventType, out var eventCallbacks) == false)
            {
                eventCallbacks = new();
                callbacks.Add(eventType, eventCallbacks);
            }

            eventCallbacks.Add(callback);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void RemoveCallback(MouseEventType eventType,
            [NotNull] Action<ColliderMouseEventTrigger, MouseEventType> callback)
        {
            if (callbacks == null)
            {
                return;
            }

            if (callbacks.TryGetValue(eventType, out var eventCallbacks) == false)
            {
                return;
            }

            eventCallbacks.Remove(callback);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void TriggerEvent(MouseEventType eventType)
        {
            if (callbacks == null)
            {
                return;
            }

            if (callbacks.TryGetValue(eventType, out var eventCallbacks) == false)
            {
                return;
            }

            foreach (var callback in eventCallbacks)
            {
                callback(this, eventType);
            }
        }
    }
}
