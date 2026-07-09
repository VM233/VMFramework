#if FISHNET
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using FishNet;
using FishNet.Connection;
using FishNet.Object;
using VMFramework.Core;
using VMFramework.Core.Pools;
using VMFramework.GameLogicArchitecture;
using VMFramework.Network;

namespace VMFramework.Containers
{
    public partial class ContainerManager
    {
        #region Reconcile On Observers

        private static void ReconcileItemOnObservers(IContainer container, UUIDInfo info, int slotIndex)
        {
            foreach (var observer in info.Observers)
            {
                if (InstanceFinder.ServerManager.Clients.TryGetValue(observer, out NetworkConnection observerConn) ==
                    false)
                {
                    UnityEngine.Debug.LogError($"Failed to find observer connection for {observer} of {container}");
                    continue;
                }

                if (observerConn.IsHost)
                {
                    continue;
                }

                Instance.ReconcileOnTarget(observerConn, container.UUIDOwner.UUID, slotIndex,
                    container.GetItem(slotIndex));
            }
        }

        private static void ReconcileSomeItemsOnObservers(IContainer container, UUIDInfo info, HashSet<int> slotIndices)
        {
            var wrapper = new SomeItemsWrapper(container, slotIndices);

            foreach (var observer in info.Observers)
            {
                if (InstanceFinder.ServerManager.Clients.TryGetValue(observer, out NetworkConnection observerConn) ==
                    false)
                {
                    UnityEngine.Debug.LogError($"Failed to find observer connection for {observer} of {container}");
                    continue;
                }

                if (observerConn.IsHost)
                {
                    continue;
                }

                Instance.ReconcileSomeOnTarget(observerConn, container.UUIDOwner.UUID, wrapper);
            }
        }

        private static void ReconcileAllItemsOnObservers(IContainer container, UUIDInfo containerInfo)
        {
            var wrapper = new AllItemsWrapper(container);

            foreach (var observer in containerInfo.Observers)
            {
                if (InstanceFinder.ServerManager.Clients.TryGetValue(observer, out NetworkConnection observerConn) ==
                    false)
                {
                    UnityEngine.Debug.LogError($"Failed to find observer connection for {observer} of {container}");
                    continue;
                }

                if (observerConn.IsHost)
                {
                    continue;
                }

                Instance.ReconcileAllOnTarget(observerConn, containerInfo.Owner.UUID, wrapper);
            }
        }

        #endregion

        #region Reconcile On Target

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void ReconcileOnTarget(NetworkConnection connection, IContainer container, int slotIndex)
        {
            Instance.ReconcileOnTarget(connection, container.UUIDOwner.UUID, slotIndex, container.GetItem(slotIndex));
        }

        [TargetRpc(ExcludeServer = true)]
        [ObserversRpc(ExcludeServer = true)]
        private void ReconcileOnTarget(NetworkConnection connection, Guid containerUUID, int slotIndex,
            IContainerItem item)
        {
            if (UUIDCoreManager.Instance.TryGetComponent(containerUUID, out IContainer container))
            {
                var existingItem = container.GetItem(slotIndex);
                container.SetItem(slotIndex, item);
                if (existingItem != null)
                {
                    GameItemManager.Instance.Return(existingItem);
                }

                return;
            }

            UnityEngine.Debug.LogError($"Failed to find {nameof(container)} with {nameof(containerUUID)}:{containerUUID}");
        }

        [TargetRpc(ExcludeServer = true)]
        [ObserversRpc(ExcludeServer = true)]
        private void ReconcileSomeOnTarget(NetworkConnection connection, Guid containerUUID, SomeItemsWrapper wrapper)
        {
            if (UUIDCoreManager.Instance.TryGetComponent(containerUUID, out IContainer container))
            {
                foreach (var (slotIndex, item) in wrapper.items)
                {
                    var existingItem = container.GetItem(slotIndex);
                    container.SetItem(slotIndex, item);
                    if (existingItem != null)
                    {
                        GameItemManager.Instance.Return(existingItem);
                    }
                }
            }
            else
            {
                UnityEngine.Debug.LogError($"Failed to find {nameof(container)} with {nameof(containerUUID)}:{containerUUID}");
            }

            wrapper.items.ReturnToDefaultPool();
        }

        [TargetRpc(ExcludeServer = true)]
        [ObserversRpc(ExcludeServer = true)]
        private void ReconcileAllOnTarget(NetworkConnection connection, Guid containerUUID, AllItemsWrapper wrapper)
        {
            if (UUIDCoreManager.Instance.TryGetComponent(containerUUID, out IContainer container))
            {
                container.LoadFromItemsList(wrapper.items, autoReturn: true, count: wrapper.count);
            }
            else
            {
                UnityEngine.Debug.LogError($"Failed to find {nameof(container)} with {nameof(containerUUID)}:{containerUUID}");
            }

            wrapper.items.ReturnToDefaultPool();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void ReconcileAllOnTarget(NetworkConnection connection, IContainer container)
        {
            Instance.ReconcileAllOnTarget(connection, container.UUIDOwner.UUID, new(container));
        }

        #endregion

        #region Request Reconcile

        [Client]
        public static void RequestReconcile(IContainer container, int slotIndex)
        {
            container.AssertIsNotNull(nameof(container));

            Instance.RequestReconcile(container.UUIDOwner.UUID, slotIndex);
        }

        [ServerRpc(RequireOwnership = false)]
        private void RequestReconcile(Guid containerUUID, int slotIndex, NetworkConnection connection = null)
        {
            if (UUIDCoreManager.Instance.TryGetComponent(containerUUID, out IContainer container))
            {
                var item = container.GetItem(slotIndex);

                ReconcileOnTarget(connection, containerUUID, slotIndex, item);
            }
            else
            {
                UnityEngine.Debug.LogError($"Failed to find {nameof(container)} with {nameof(containerUUID)}:{containerUUID}");
            }
        }

        [Client]
        public static void RequestReconcileAll(IContainer container)
        {
            container.AssertIsNotNull(nameof(container));

            Instance.RequestReconcileAll(container.UUIDOwner.UUID);
        }

        [ServerRpc(RequireOwnership = false)]
        private void RequestReconcileAll(Guid containerUUID, NetworkConnection connection = null)
        {
            if (UUIDCoreManager.Instance.TryGetComponent(containerUUID, out IContainer container))
            {
                ReconcileAllOnTarget(connection, container);
            }
        }

        #endregion
    }
}
#endif