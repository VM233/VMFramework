using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace VMFramework.Procedure
{
    /// <summary>
    /// 非线程安全的管理器基类，用于实现单例。
    /// Non-thread-safe manager base class used for singleton implementation.
    /// </summary>
    public class ManagerBehaviour<TInstance> : MonoBehaviour, IManagerBehaviour
        where TInstance : class
    {
        [ShowInInspector]
        [ReadOnly]
        [HideInEditorMode]
        public static TInstance Instance { get; set; }

        protected virtual void Awake()
        {
            if (Instance != null)
            {
                if (Instance.GetType() != GetType())
                {
                    Debug.LogError($"Instance of {typeof(TInstance)} already exists!" +
                                   $"Existing instance: {Instance}");
                    return;
                }
            }
            
            Instance = this as TInstance;

            if (Instance == null)
            {
                Debug.LogError($"Failed to set instance : Type {typeof(TInstance)} for {GetType().Name}!");
            }
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