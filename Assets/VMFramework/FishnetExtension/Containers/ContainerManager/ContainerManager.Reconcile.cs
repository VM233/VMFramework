#if FISHNET
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using FishNet;
using FishNet.Connection;
using FishNet.Object;
using UnityEngine;
using VMFramework.Core;
using VMFramework.Core.Pools;
using VMFramework.Network;

namespace VMFramework.Containers
{
    public partial class ContainerManager
    {
        #region Reconcile On Observers

        private static void ReconcileItemOnObservers(IContainer container, UUIDInfo info, int slotIndex)
        {
            foreach (var observer in info.observers)
            {
                if (InstanceFinder.ServerManager.Clients.TryGetValue(observer, out NetworkConnection observerConn) ==
                    false)
                {
                    Debugger.LogError($"Failed to find observer connection for {observer} of {container}");
                    continue;
                }

                if (observerConn.IsHost)
                {
                    continue;
                }

                Instance.ReconcileOnTarget(observerConn, container.UUID, slotIndex, container.GetItem(slotIndex));
            }
        }

        private static void ReconcileSomeItemsOnObservers(IContainer container, UUIDInfo info, HashSet<int> slotIndices)
        {
            var wrapper = new SomeItemsWrapper(container, slotIndices);

            foreach (var observer in info.observers)
            {
                if (InstanceFinder.ServerManager.Clients.TryGetValue(observer, out NetworkConnection observerConn) ==
                    false)
                {
                    Debugger.LogError($"Failed to find observer connection for {observer} of {container}");
                    continue;
                }

                if (observerConn.IsHost)
                {
                    continue;
                }

                Instance.ReconcileSomeOnTarget(observerConn, container.UUID, wrapper);
            }
        }

        private static void ReconcileAllItemsOnObservers(IContainer container, UUIDInfo containerInfo)
        {
            var wrapper = new AllItemsWrapper(container);

            foreach (var observer in containerInfo.observers)
            {
                if (InstanceFinder.ServerManager.Clients.TryGetValue(observer, out NetworkConnection observerConn) ==
                    false)
                {
                    Debugger.LogError($"Failed to find observer connection for {observer} of {container}");
                    continue;
                }

                if (observerConn.IsHost)
                {
                    continue;
                }

                Instance.ReconcileAllOnTarget(observerConn, containerInfo.owner.UUID, wrapper);
            }
        }

        #endregion

        #region Reconcile On Target

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void ReconcileOnTarget(NetworkConnection connection, IContainer container, int slotIndex)
        {
            Instance.ReconcileOnTarget(connection, container.UUID, slotIndex, container.GetItem(slotIndex));
        }

        [TargetRpc(ExcludeServer = true)]
        [ObserversRpc(ExcludeServer = true)]
        private void ReconcileOnTarget(NetworkConnection connection, Guid containerUUID, int slotIndex,
            IContainerItem item)
        {
            if (UUIDCoreManager.TryGetOwner(containerUUID, out IContainer container))
            {
                container.SetItem(slotIndex, item);
                return;
            }

            Debugger.LogError($"Failed to find {nameof(container)} with {nameof(containerUUID)}:{containerUUID}");
        }

        [TargetRpc(ExcludeServer = true)]
        [ObserversRpc(ExcludeServer = true)]
        private void ReconcileSomeOnTarget(NetworkConnection connection, Guid containerUUID, SomeItemsWrapper wrapper)
        {
            if (UUIDCoreManager.TryGetOwner(containerUUID, out IContainer container))
            {
                foreach (var (slotIndex, item) in wrapper.items)
                {
                    container.SetItem(slotIndex, item);
                }
            }
            else
            {
                Debugger.LogError($"Failed to find {nameof(container)} with {nameof(containerUUID)}:{containerUUID}");
            }

            wrapper.items.ReturnToDefaultPool();
        }

        [TargetRpc(ExcludeServer = true)]
        [ObserversRpc(ExcludeServer = true)]
        private void ReconcileAllOnTarget(NetworkConnection connection, Guid containerUUID, AllItemsWrapper wrapper)
        {
            if (UUIDCoreManager.TryGetOwner(containerUUID, out IContainer container))
            {
                container.LoadFromItemsArray(wrapper.items);
            }
            else
            {
                Debugger.LogError($"Failed to find {nameof(container)} with {nameof(containerUUID)}:{containerUUID}");
            }

            wrapper.items.ReturnToDefaultPool();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void ReconcileAllOnTarget(NetworkConnection connection, IContainer container)
        {
            Instance.ReconcileAllOnTarget(connection, container.UUID, new(container));
        }

        #endregion

        #region Request Reconcile

        [Client]
        public static void RequestReconcile(IContainer container, int slotIndex)
        {
            container.AssertIsNotNull(nameof(container));

            Instance.RequestReconcile(container.UUID, slotIndex);
        }

        [ServerRpc(RequireOwnership = false)]
        private void RequestReconcile(Guid containerUUID, int slotIndex, NetworkConnection connection = null)
        {
            if (UUIDCoreManager.TryGetOwner(containerUUID, out IContainer container))
            {
                var item = container.GetItem(slotIndex);

                ReconcileOnTarget(connection, containerUUID, slotIndex, item);
            }
            else
            {
                Debugger.LogError($"Failed to find {nameof(container)} with {nameof(containerUUID)}:{containerUUID}");
            }
        }

        [Client]
        public static void RequestReconcileAll(IContainer container)
        {
            container.AssertIsNotNull(nameof(container));

            Instance.RequestReconcileAll(container.UUID);
        }

        [ServerRpc(RequireOwnership = false)]
        private void RequestReconcileAll(Guid containerUUID, NetworkConnection connection = null)
        {
            if (UUIDCoreManager.TryGetOwner(containerUUID, out IContainer container))
            {
                ReconcileAllOnTarget(connection, container);
            }
        }

        #endregion
    }
}
#endif