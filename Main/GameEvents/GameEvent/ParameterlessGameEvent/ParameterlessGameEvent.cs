using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using VMFramework.Core;
using VMFramework.Core.Pools;
using VMFramework.GameLogicArchitecture;
using VMFramework.Properties;

namespace VMFramework.GameEvents
{
    public class ParameterlessGameEvent : GameItem, IParameterlessGameEvent
    {
        [ShowInInspector]
        protected PriorityEvents<Action> callbacks = new();

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
                if (callbacks.Count > 1)
                {
                    hasExtraCallbacks = true;
                }
            }
            else
            {
                if (callbacks.Count > 0)
                {
                    hasExtraCallbacks = true;
                }
            }

            if (hasExtraCallbacks)
            {
                UnityEngine.Debug.LogWarning($"{this} has extra callbacks. Callbacks Count : {callbacks.Count}");
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

        private void DebugLog()
        {
            UnityEngine.Debug.LogWarning($"{this} was triggered!");
        }
        
        public void Reset()
        {
            callbacks.Clear();
            isEnabledProperty.Clear();
        }

        #region Callbacks

        public void AddCallback(Action callback, int priority)
        {
            callbacks.Add(priority, callback);
        }

        public void RemoveCallback(Action callback)
        {
            callbacks.Remove(callback);
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

        private readonly List<Action> tempCallbacks = new();

        public void Propagate()
        {
            if (CanPropagate() == false)
            {
                return;
            }

            tempCallbacks.Clear();

            foreach (var callback in callbacks.GetCombinedCallbacks())
            {
                tempCallbacks.Add(callback);
            }

            foreach (var callback in tempCallbacks)
            {
                callback();
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