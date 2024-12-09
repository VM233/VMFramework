using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using VMFramework.Core;

namespace VMFramework.Procedure
{
    /// <summary>
    /// 非线程安全的管理器基类，用于实现单例。
    /// Non-thread-safe manager base class used for singleton implementation.
    /// </summary>
    /// <typeparam name="TInstance"></typeparam>
    public class ManagerBehaviour<TInstance> : MonoBehaviour, IManagerBehaviour
        where TInstance : ManagerBehaviour<TInstance>
    {
        [ShowInInspector]
        [ReadOnly]
        [HideInEditorMode]
        protected static TInstance Instance { get; private set; }

        void IManagerBehaviour.SetInstance()
        {
            if (Instance != null)
            {
                Debug.LogError($"Instance of {typeof(TInstance)} already exists!");
                return;
            }
            
            Instance = (TInstance)this;
            Instance.AssertIsNotNull(nameof(Instance));
        }

        protected virtual void OnBeforeInitStart()
        {
            
        }
        
        protected virtual void GetInitializationActions(ICollection<InitializationAction> actions)
        {
            actions.Add(new(InitializationOrder.BeforeInitStart, OnBeforeInitStartInternal, this));
        }

        void IInitializer.GetInitializationActions(ICollection<InitializationAction> actions)
        {
            GetInitializationActions(actions);
        }

        private void OnBeforeInitStartInternal(Action onDone)
        {
            OnBeforeInitStart();
            onDone();
        }
    }
}
