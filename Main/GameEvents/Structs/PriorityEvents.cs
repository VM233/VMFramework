using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using VMFramework.Core;

namespace VMFramework.GameEvents
{
    public class PriorityEvents<TDelegate> : IReadOnlyCollection<TDelegate>, IReadOnlyPriorityEvents<TDelegate>
        where TDelegate : Delegate
    {
        public int Count => priorityLookup.Count;

        [ShowInInspector]
        protected readonly SortedDictionary<int, TDelegate> callbacks = new();

        [ShowInInspector]
        protected readonly Dictionary<TDelegate, int> priorityLookup = new();

        public IReadOnlyCollection<TDelegate> GetCombinedCallbacks()
        {
            return callbacks.Values;
        }

        public void Clear()
        {
            callbacks.Clear();
            priorityLookup.Clear();
        }

        public void Add(int priority, TDelegate callback)
        {
            if (callback == null)
            {
                UnityEngine.Debug.LogError(
                    $"[{nameof(PriorityEvents<TDelegate>)}] Cannot add null callback with priority {priority}.");
                return;
            }
            
            if (priorityLookup.TryAdd(callback, priority) == false)
            {
                return;
            }

            if (callbacks.TryAdd(priority, callback))
            {
                return;
            }

            var existingCallback = callbacks[priority];
            callbacks[priority] = (TDelegate)Delegate.Combine(existingCallback, callback);
        }

        public void Remove(TDelegate callback)
        {
            if (callback == null)
            {
                UnityEngine.Debug.LogError(
                    $"[{nameof(PriorityEvents<TDelegate>)}] Cannot remove null callback.");
                return;
            }
            
            if (priorityLookup.Remove(callback, out var priority) == false)
            {
                return;
            }

            var existingCallback = callbacks[priority];

            if (existingCallback == callback)
            {
                callbacks.Remove(priority);
                return;
            }

            existingCallback = (TDelegate)Delegate.Remove(existingCallback, callback);
            callbacks[priority] = existingCallback;
        }

        public IEnumerator<TDelegate> GetEnumerator()
        {
            return priorityLookup.Keys.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}