using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using UnityEngine;
using Sirenix.OdinInspector;
using VMFramework.Core;
using VMFramework.Core.Pools;
using VMFramework.OdinExtensions;

namespace VMFramework.GameEvents
{
    [DisallowMultipleComponent]
    public class ColliderMouseEventTrigger : MonoBehaviour
    {
        [CommonPreset(ColliderMouseEventGeneralSetting.TRIGGER_PRIORITY_PRESET_KEY)]
        public int priority = 0;

        public bool draggable = false;

        [ShowIf(nameof(draggable))]
        public MouseButtonType dragButton = MouseButtonType.LeftButton;

        [field: Required]
        [field: SerializeField]
        public Transform Owner { get; set; }

#if UNITY_EDITOR
        private void Reset()
        {
            if (Owner == null)
            {
                Owner = transform;
            }
        }
#endif

        [ShowInInspector]
        protected readonly Dictionary<MouseEventType, HashSet<MouseEventHandler>> callbacks = new();
        
        public virtual void SetOwner(Transform owner)
        {
            if (Owner != null && owner != null)
            {
                UnityEngine.Debug.LogWarning($"ColliderMouseEventTrigger already has an owner : {owner.name}!");
            }

            Owner = owner;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void AddCallback(MouseEventType eventType, [NotNull] MouseEventHandler callback)
        {
            if (callbacks.TryGetValue(eventType, out var eventCallbacks) == false)
            {
                eventCallbacks = new();
                callbacks.Add(eventType, eventCallbacks);
            }

            eventCallbacks.Add(callback);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void RemoveCallback(MouseEventType eventType, [NotNull] MouseEventHandler callback)
        {
            if (callbacks.TryGetValue(eventType, out var eventCallbacks) == false)
            {
                return;
            }

            eventCallbacks.Remove(callback);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void TriggerEvent(MouseEventType eventType)
        {
            if (callbacks.TryGetValue(eventType, out var eventCallbacks) == false)
            {
                return;
            }
            
            var snapshot = ListPool<MouseEventHandler>.Default.Get();
            try
            {
                snapshot.Clear();
                snapshot.AddRange(eventCallbacks);
                foreach (var callback in snapshot)
                {
                    callback(this, eventType);
                }
            }
            finally
            {
                snapshot.Clear();
                snapshot.ReturnToDefaultPool();
            }
        }
    }
}
