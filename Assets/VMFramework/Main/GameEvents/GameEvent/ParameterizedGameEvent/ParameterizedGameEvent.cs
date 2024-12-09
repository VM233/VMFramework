using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using VMFramework.Core;
using VMFramework.Core.Pools;
using VMFramework.GameLogicArchitecture;

namespace VMFramework.GameEvents
{
    public abstract partial class ParameterizedGameEvent<TArgument> : GameItem, IParameterizedGameEvent<TArgument>
    {
        [ShowInInspector]
        private readonly SortedDictionary<int, HashSet<Action<TArgument>>> callbacks = new();
        [ShowInInspector]
        private readonly Dictionary<Action<TArgument>, int> callbacksLookup = new();

        [ShowInInspector]
        private readonly HashSet<IToken> disabledTokens = new();
        
        public bool IsEnabled => disabledTokens.Count <= 0;

        public event EnabledChangedEventHandler OnEnabledChangedEvent;

        #region Pool Events

        protected override void OnCreate()
        {
            base.OnCreate();

            if (IsDebugging)
            {
                AddCallback(DebugLog, GameEventPriority.SUPER);
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
                Debugger.LogWarning($"{this} has extra callbacks. Callbacks Count : {callbacksLookup.Count}");
            }
        }

        #endregion

        private void DebugLog(TArgument argument)
        {
            Debugger.LogWarning($"{this} was triggered with argument {argument}.");
        }

        #region Enable / Disable

        public void Enable(IToken token)
        {
            token.AssertIsNotNull(nameof(token));
            
            if (disabledTokens.Remove(token) == false)
            {
                return;
            }

            if (disabledTokens.Count == 0)
            {
                OnEnabledChangedEvent?.Invoke(false, true);
            }
        }

        public void Disable(IToken token)
        {
            token.AssertIsNotNull(nameof(token));
            
            if (disabledTokens.Add(token) == false)
            {
                return;
            }

            if (disabledTokens.Count == 1)
            {
                OnEnabledChangedEvent?.Invoke(true, false);
            }
        }

        #endregion

        public void Reset()
        {
            callbacks.Clear();
            callbacksLookup.Clear();
            disabledTokens.Clear();
        }

        #region Callbacks

        public void AddCallback(Action<TArgument> callback, int priority)
        {
            if (callback == null)
            {
                Debug.LogError($"Cannot add null callback to {this}");
                return;
            }
            
            if (callbacks.TryGetValue(priority, out var set))
            {
                if (set.Add(callback))
                {
                    callbacksLookup.Add(callback, priority);
                    return;
                }

                var methodName = callback.Method.Name;
                Debugger.LogWarning($"Callback {methodName} already exists in {this} with priority {priority}.");
                
                return;
            }

            set = new() { callback };
            callbacks.Add(priority, set);
            callbacksLookup.Add(callback, priority);
        }
        
        public void RemoveCallback(Action<TArgument> callback)
        {
            if (callback == null)
            {
                Debug.LogError($"Cannot remove null callback from {this}");
                return;
            }

            if (callbacksLookup.Count == 0)
            {
                return;
            }
            
            if (callbacksLookup.TryGetValue(callback, out var priority) == false)
            {
                Debugger.LogWarning($"Callback {callback.Method.Name} does not exist in {this}");
                return;
            }
            
            callbacks[priority].Remove(callback);
            callbacksLookup.Remove(callback);
        }

        #endregion

        #region Propagate

        protected virtual bool CanPropagate()
        {
            if (IsEnabled == false)
            {
                Debugger.LogWarning($"GameEvent:{id} is disabled. Cannot propagate.");
                return false;
            }
            
            return true;
        }

        public void Propagate(TArgument argument)
        {
            if (CanPropagate() == false)
            {
                return;
            }

            var tempCallbacks = ListPool<(int priority, Action<TArgument> callback)>.Default.Get();

            tempCallbacks.Clear();
            
            foreach (var (priority, set) in callbacks)
            {
                foreach (var callback in set)
                {
                    tempCallbacks.Add((priority, callback));
                }
            }

            foreach (var (priority, callback) in tempCallbacks)
            {
                callback(argument);
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