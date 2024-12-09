#if FISHNET
using System;
using System.Collections.Generic;
using FishNet.Object;
using Sirenix.OdinInspector;
using UnityEngine;
using VMFramework.Core;

namespace VMFramework.Procedure
{
    [RequireComponent(typeof(NetworkObject))]
    public class NetworkManagerBehaviour<TInstance> : NetworkBehaviour, IManagerBehaviour
        where TInstance : NetworkManagerBehaviour<TInstance>
    {
        [ShowInInspector]
        [ReadOnly]
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
#endif
