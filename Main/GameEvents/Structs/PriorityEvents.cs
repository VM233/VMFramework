using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;

namespace VMFramework.GameEvents
{
    public class PriorityEvents<TDelegate> : IReadOnlyCollection<TDelegate>, IReadOnlyPriorityEvents<TDelegate>
        where TDelegate : Delegate
    {
        public int Count => priorityLookup.Count;

        [ShowInInspector]
        protected readonly SortedDictionary<int, CallbackGroup> callbacks = new();

        [ShowInInspector]
        protected readonly Dictionary<TDelegate, int> priorityLookup = new();

        private readonly CombinedCallbackCollection combinedCallbacks;

        public PriorityEvents()
        {
            combinedCallbacks = new CombinedCallbackCollection(this);
        }

        public IReadOnlyCollection<TDelegate> GetCombinedCallbacks()
        {
            return combinedCallbacks;
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
                throw new ArgumentNullException(nameof(callback));
            }

            if (priorityLookup.TryAdd(callback, priority) == false)
            {
                return;
            }

            if (callbacks.TryGetValue(priority, out var group) == false)
            {
                group = new CallbackGroup();
            }

            group.Add(callback);
            callbacks[priority] = group;
        }

        public void Remove(TDelegate callback)
        {
            if (callback == null)
            {
                throw new ArgumentNullException(nameof(callback));
            }

            if (priorityLookup.Remove(callback, out var priority) == false)
            {
                return;
            }

            var group = callbacks[priority];
            group.Remove(callback);

            if (group.Count == 0)
            {
                callbacks.Remove(priority);
                return;
            }

            callbacks[priority] = group;
        }

        public IEnumerator<TDelegate> GetEnumerator()
        {
            return priorityLookup.Keys.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        protected sealed class CallbackGroup
        {
            private readonly List<TDelegate> registrations = new();
            private TDelegate combined;

            public int Count => registrations.Count;

            public void Add(TDelegate callback)
            {
                registrations.Add(callback);
                combined = null;
            }

            public void Remove(TDelegate callback)
            {
                registrations.Remove(callback);
                combined = null;
            }

            public TDelegate GetCombinedCallback()
            {
                if (combined == null)
                {
                    combined = Compose(0, registrations.Count);
                }
                return combined;
            }

            private TDelegate Compose(int start, int count)
            {
                if (count == 1)
                {
                    return registrations[start];
                }
                int leftCount = count / 2;
                return (TDelegate)Delegate.Combine(Compose(start, leftCount),
                    Compose(start + leftCount, count - leftCount));
            }
        }

        private sealed class CombinedCallbackCollection : IReadOnlyCollection<TDelegate>
        {
            private readonly PriorityEvents<TDelegate> owner;

            public CombinedCallbackCollection(PriorityEvents<TDelegate> owner)
            {
                this.owner = owner;
            }

            public int Count => owner.callbacks.Count;

            public IEnumerator<TDelegate> GetEnumerator()
            {
                foreach (var pair in owner.callbacks)
                {
                    yield return pair.Value.GetCombinedCallback();
                }
            }

            IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        }
    }
}
