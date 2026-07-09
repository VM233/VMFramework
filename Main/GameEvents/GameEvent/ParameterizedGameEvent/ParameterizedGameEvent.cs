using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using VMFramework.Core;
using VMFramework.Core.Pools;
using VMFramework.GameLogicArchitecture;
using VMFramework.Properties;

namespace VMFramework.GameEvents
{
    public abstract class ParameterizedGameEvent<TArgument> : GameItem, IParameterizedGameEvent<TArgument>
    {
        [ShowInInspector]
        private readonly
            SortedDictionary<int, (HashSet<Action<TArgument>> argumentCallbacks, HashSet<Action> callbacks)> callbacks =
                new();

        [ShowInInspector]
        private readonly Dictionary<Delegate, int> callbacksLookup = new();

        public ITokenProperty<IToken> IsEnabled => isEnabledProperty;

        [ShowInInspector]
        protected readonly TokenProperty<IToken> isEnabledProperty = new()
        {
            TrueIfEmptyTokens = true
        };

        #region Pool Events

        protected override void OnCreate()
        {
            base.OnCreate();

            isEnabledProperty.SetOwner(this);

            if (IsDebugging)
            {
                AddCallback(DebugLog, PriorityDefines.SUPER);
            }
        }

        protected override void OnReturn()
        {
            base.OnReturn();

            bool hasExtraCallbacks = false;
            if (IsDebugging)
            {
                if (callbacksLookup.Count > 1)
                {
                    hasExtraCallbacks = true;
                }
            }
            else
            {
                if (callbacksLookup.Count > 0)
                {
                    hasExtraCallbacks = true;
                }
            }

            if (hasExtraCallbacks)
            {
                UnityEngine.Debug.LogWarning($"{this} has extra callbacks. Callbacks Count : {callbacksLookup.Count}");
            }
        }

        #endregion
        
        #region Valid

        public event IValidCheckDispatcher.ValidCheckHandler OnCheckValid;

        public virtual bool IsValid()
        {
            bool isValid = false;
            OnCheckValid?.Invoke(this, ref isValid);
            return isValid;
        }

        #endregion

        private void DebugLog(TArgument argument)
        {
            UnityEngine.Debug.LogWarning($"{this} was triggered with argument {argument}.");
        }

        public void Reset()
        {
            foreach (var tuple in callbacks.Values)
            {
                tuple.argumentCallbacks.Clear();
                tuple.callbacks.Clear();
            }

            callbacksLookup.Clear();
            isEnabledProperty.Clear();
        }

        #region Callbacks

        public void AddCallback(Action callback, int priority)
        {
            if (callback == null)
            {
                Debug.LogError($"Cannot add null {nameof(callback)} to {this}");
                return;
            }

            if (callbacks.TryGetValue(priority, out var tuple) == false)
            {
                tuple.argumentCallbacks = new();
                tuple.callbacks = new() { callback };
                callbacks.Add(priority, tuple);
                callbacksLookup.Add(callback, priority);
                return;
            }

            if (tuple.callbacks.Add(callback))
            {
                callbacksLookup.Add(callback, priority);
                return;
            }

            var methodName = callback.Method.Name;
            UnityEngine.Debug.LogWarning($"Callback {methodName} already exists in {this} with priority {priority}.");
        }

        public void RemoveCallback(Action callback)
        {
            if (callback == null)
            {
                Debug.LogError($"Cannot remove null {nameof(callback)} from {this}");
                return;
            }

            if (callbacksLookup.Count == 0)
            {
                return;
            }

            if (callbacksLookup.TryGetValue(callback, out var priority) == false)
            {
                UnityEngine.Debug.LogWarning($"Callback {callback.Method.Name} does not exist in {this}");
                return;
            }

            callbacks[priority].callbacks.Remove(callback);
            callbacksLookup.Remove(callback);
        }

        public void AddCallback(Action<TArgument> callback, int priority)
        {
            if (callback == null)
            {
                Debug.LogError($"Cannot add null {nameof(callback)} to {this}");
                return;
            }

            if (callbacks.TryGetValue(priority, out var tuple) == false)
            {
                tuple.argumentCallbacks = new() { callback };
                tuple.callbacks = new();
                callbacks.Add(priority, tuple);
                callbacksLookup.Add(callback, priority);
                return;
            }

            if (tuple.argumentCallbacks.Add(callback))
            {
                callbacksLookup.Add(callback, priority);
                return;
            }

            var methodName = callback.Method.Name;
            UnityEngine.Debug.LogWarning($"Callback {methodName} already exists in {this} with priority {priority}.");
        }

        public void RemoveCallback(Action<TArgument> callback)
        {
            if (callback == null)
            {
                Debug.LogError($"Cannot remove null {nameof(callback)} from {this}");
                return;
            }

            if (callbacksLookup.Count == 0)
            {
                return;
            }

            if (callbacksLookup.TryGetValue(callback, out var priority) == false)
            {
                return;
            }

            callbacks[priority].argumentCallbacks.Remove(callback);
            callbacksLookup.Remove(callback);
        }

        #endregion

        #region Propagate

        protected virtual bool CanPropagate()
        {
            if (isEnabledProperty.GetValue() == false)
            {
                UnityEngine.Debug.LogWarning($"GameEvent:{id} is disabled. Cannot propagate.");
                return false;
            }

            return true;
        }

        private readonly List<(Action<TArgument> argumentCallback, Action callback)> tempCallbacks = new();

        public void Propagate(TArgument argument)
        {
            Propagate(argument, true);
        }

        public void Propagate(TArgument argument, bool propagateAction)
        {
            if (CanPropagate() == false)
            {
                return;
            }

            tempCallbacks.Clear();

            foreach (var (_, set) in callbacks)
            {
                foreach (var argumentCallback in set.argumentCallbacks)
                {
                    tempCallbacks.Add((argumentCallback, null));
                }

                if (propagateAction)
                {
                    foreach (var callback in set.callbacks)
                    {
                        tempCallbacks.Add((null, callback));
                    }
                }
            }

            foreach (var (argumentCallback, callback) in tempCallbacks)
            {
                if (argumentCallback != null)
                {
                    argumentCallback(argument);
                    continue;
                }

                if (propagateAction)
                {
                    callback();
                }
            }

            tempCallbacks.ReturnToDefaultPool();

            OnPropagationStopped();
        }

        protected virtual void OnPropagationStopped()
        {

        }

        #endregion
    }
}