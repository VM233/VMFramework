#if FISHNET
using System;
using FishNet.Connection;
using VMFramework.Core;
using VMFramework.Network;
using VMFramework.Procedure;

namespace VMFramework.Containers
{
    /// <summary>
    /// 提供容器的自动标记Dirty的功能；提供一些容器的带客户端预测（Client Side Prediction, CSP）的操作
    /// </summary>
    [ManagerCreationProvider(ManagerType.NetworkCore)]
    public partial class ContainerManager : UUIDManager<ContainerManager, IContainer>
    {
        private Action<IUUIDOwner, NetworkConnection> onObservedFunc;
        private Action<IUUIDOwner, NetworkConnection> onUnobservedFunc;

        protected override void Awake()
        {
            base.Awake();
            
            onObservedFunc = OnObserved;
            onUnobservedFunc = OnUnobserved;
        }

        protected override void OnBeforeInitStart()
        {
            base.OnBeforeInitStart();
            
            OnRegisterEvent += OnRegister;
            OnUnregisterEvent += OnUnregister;
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            
            OnRegisterEvent -= OnRegister;
            OnUnregisterEvent -= OnUnregister;
        }

        #region Register & Unregister

        protected virtual void OnRegister(IContainer container)
        {
            if (Instance.IsServerStarted)
            {
                container.OnItemAddedEvent += onItemAddedFunc;
                container.OnItemRemovedEvent += onItemRemovedFunc;
                container.OnItemDirtyEvent += onItemDirtyFunc;
                container.UUIDOwner.OnObservedEvent += onObservedFunc;
                container.UUIDOwner.OnUnobservedEvent += onUnobservedFunc;
            }
        }

        protected virtual void OnUnregister(IContainer container)
        {
            if (Instance.IsServerStarted)
            {
                container.OnItemAddedEvent -= onItemAddedFunc;
                container.OnItemRemovedEvent -= onItemRemovedFunc;
                container.OnItemDirtyEvent -= onItemDirtyFunc;
                container.UUIDOwner.OnObservedEvent -= onObservedFunc;
                container.UUIDOwner.OnUnobservedEvent -= onUnobservedFunc;
                container.IsOpen.SetValue(false, initial: false);

                RemoveContainerDirtySlotsInfo(container);
            }
        }

        #endregion

        #region Observe & Unobserve

        protected virtual void OnObserved(IUUIDOwner owner, NetworkConnection connection)
        {
            if (owner is not IController controller || controller.TryGetComponent(out IContainer container) == false)
            {
                return;
            }
            
            container.IsOpen.SetValue(true, initial: false);
            
            ReconcileAllOnTarget(connection, container);
        }

        protected virtual void OnUnobserved(IUUIDOwner owner, NetworkConnection connection)
        {
            if (owner is not IController controller || controller.TryGetComponent(out IContainer container) == false)
            {
                return;
            }
            
            if (UUIDCoreManager.Instance.TryGetInfoWithWarning(owner.UUID, out var info) == false)
            {
                return;
            }
            
            if (info.Observers.Count <= 0)
            {
                container.IsOpen.SetValue(false, initial: false);
            }
        }

        #endregion
    }
}

#endif