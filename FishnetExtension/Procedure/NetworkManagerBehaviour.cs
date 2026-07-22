#if FISHNET
using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using FishNet.Object;
using Sirenix.OdinInspector;
using UnityEngine;

namespace VMFramework.Procedure
{
    /// <inheritdoc cref="ManagerBehaviour{TInstance}"/>
    [RequireComponent(typeof(NetworkObject))]
    public class NetworkManagerBehaviour<TInstance> : NetworkBehaviour, IManagerBehaviour
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

        private UniTask OnBeforeInitStartInternal(CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            OnBeforeInitStart();
            return UniTask.CompletedTask;
        }
    }
}
#endif
