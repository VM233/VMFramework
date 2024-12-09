#if FISHNET
using System;
using FishNet.Object;
using FishNet.Connection;
using UnityEngine;
using VMFramework.GameEvents;
using VMFramework.Network;
using VMFramework.Procedure;

namespace VMFramework.Containers
{
    [ManagerCreationProvider(ManagerType.NetworkCore)]
    public sealed partial class ContainerManager : UUIDManager<ContainerManager, IContainer>
    {
        private static readonly Action<IUUIDOwner, NetworkConnection> onObservedFunc = OnObserved;
        private static readonly Action<IUUIDOwner, NetworkConnection> onUnobservedFunc = OnUnobserved;
        
        protected override void OnBeforeInitStart()
        {
            base.OnBeforeInitStart();
            
            OnRegisterEvent += OnRegister;
            OnUnregisterEvent += OnUnregister;
        }

        #region Register & Unregister

        private static void OnRegister(IContainer container)
        {
            if (Instance.IsServerStarted)
            {
                container.OnItemCountChangedEvent += onContainerItemCountChangedFunc;
                container.ItemAddedEvent.AddCallback(onItemAddedFunc, GameEventPriority.TINY);
                container.ItemRemovedEvent.AddCallback(onItemRemovedFunc, GameEventPriority.TINY);
                container.OnObservedEvent += onObservedFunc;
                container.OnUnobservedEvent += onUnobservedFunc;
            }
        }
        
        private static void OnUnregister(IContainer container)
        {
            if (Instance.IsServerStarted)
            {
                container.OnItemCountChangedEvent -= onContainerItemCountChangedFunc;
                container.ItemAddedEvent?.RemoveCallback(onItemAddedFunc);
                container.ItemRemovedEvent?.RemoveCallback(onItemRemovedFunc);
                container.OnObservedEvent -= onObservedFunc;
                container.OnUnobservedEvent -= onUnobservedFunc;
                
                RemoveContainerDirtySlotsInfo(container);
            }
        }

        #endregion

        #region Observe & Unobserve

        private static void OnObserved(IUUIDOwner container, NetworkConnection connection)
        {
            if (UUIDCoreManager.TryGetInfoWithWarning(container.UUID, out var info))
            {
                if (info.observers.Count == 0)
                {
                    ((IContainer)container).OpenOnServer();
                }
            }
            
            ReconcileAllOnTarget(connection, (IContainer)container);
        }

        private static void OnUnobserved(IUUIDOwner container, NetworkConnection connection)
        {
            if (UUIDCoreManager.TryGetInfo(container.UUID, out var info))
            {
                if (info.observers.Count <= 0)
                {
                    ((IContainer)container).CloseOnServer();
                }
            }
            else
            {
                Debug.LogWarning(
                    $"不存在此{container.UUID}对应的{nameof(UUIDInfo)}");
            }
        }

        #endregion
    }
}

#endif