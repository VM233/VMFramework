#if FISHNET
using System;
using System.Runtime.CompilerServices;
using FishNet.Connection;
using FishNet.Object;
using UnityEngine;
using VMFramework.Core;
using VMFramework.Network;

namespace VMFramework.Containers
{
    public partial class ContainerManager
    {
        #region Add Or Swap Item

        [Client]
        public static void AddOrSwapItem(IContainer fromContainer, int fromSlotIndex, IContainer toContainer,
            int toSlotIndex)
        {
            if (fromContainer == null || toContainer == null)
            {
                Debugger.LogWarning($"{nameof(fromContainer)} or {nameof(toContainer)} is Null");
                return;
            }

            fromContainer.AddOrSwapItemTo(fromSlotIndex, toContainer, toSlotIndex);

            if (Instance.IsServerStarted == false)
            {
                Instance.RequestAddOrSwapItem(fromContainer.UUID, fromSlotIndex, toContainer.UUID, toSlotIndex);
            }
        }

        [ServerRpc(RequireOwnership = false)]
        public void RequestAddOrSwapItem(Guid fromContainerUUID, int fromSlotIndex, Guid toContainerUUID,
            int toSlotIndex, NetworkConnection connection = null)
        {
            if (UUIDCoreManager.TryGetOwnerWithWarning(fromContainerUUID, out IContainer fromContainer) == false)
            {
                return;
            }

            if (UUIDCoreManager.TryGetOwnerWithWarning(toContainerUUID, out IContainer toContainer) == false)
            {
                return;
            }

            fromContainer.AddOrSwapItemTo(fromSlotIndex, toContainer, toSlotIndex);
        }

        #endregion

        #region Split Item To

        [Client]
        public static void SplitItemTo(IContainer fromContainer, int fromSlotIndex, int count, IContainer toContainer,
            int toSlotIndex)
        {
            if (fromContainer == null || toContainer == null)
            {
                Debugger.LogWarning($"{nameof(fromContainer)} or {nameof(toContainer)} is Null");
                return;
            }

            if (fromContainer.TrySplitItemTo(fromSlotIndex, count, toContainer, toSlotIndex))
            {
                Instance.RequestSplitItemTo(fromContainer.UUID, fromSlotIndex, count, toContainer.UUID, toSlotIndex);
            }
            else
            {
                RequestReconcile(fromContainer, fromSlotIndex);
                RequestReconcile(toContainer, toSlotIndex);
            }
        }

        [ServerRpc(RequireOwnership = false)]
        private void RequestSplitItemTo(Guid fromContainerUUID, int fromSlotIndex, int count, Guid toContainerUUID,
            int toSlotIndex, NetworkConnection connection = null)
        {
            if (UUIDCoreManager.TryGetOwnerWithWarning(fromContainerUUID, out IContainer fromContainer) == false)
            {
                return;
            }

            if (UUIDCoreManager.TryGetOwnerWithWarning(toContainerUUID, out IContainer toContainer) == false)
            {
                return;
            }

            if (connection.IsHost == false)
            {
                fromContainer.TrySplitItemTo(fromSlotIndex, count, toContainer, toSlotIndex);
            }
        }

        #endregion

        #region Pop Items To

        /// <summary>
        ///     Pop Items to Other Container either on Client or Server
        /// </summary>
        /// <param name="fromContainer"></param>
        /// <param name="count"></param>
        /// <param name="toContainer"></param>
        public static void PopItemsTo(IContainer fromContainer, int count, IContainer toContainer)
        {
            if (fromContainer == null || toContainer == null)
            {
                Debugger.LogWarning($"{nameof(fromContainer)} or {nameof(toContainer)} is Null");
                return;
            }

            fromContainer.PopItemsTo(count, toContainer, out var remainingCount);

            if (remainingCount < count)
            {
                Instance.RequestPopItemsTo(fromContainer.UUID, count, toContainer.UUID);
            }
            else if (Instance.IsClientStarted)
            {
                RequestReconcileAll(fromContainer);
                RequestReconcileAll(toContainer);
            }
        }

        [ServerRpc(RequireOwnership = false)]
        private void RequestPopItemsTo(Guid fromContainerUUID, int count, Guid toContainerUUID,
            NetworkConnection connection = null)
        {
            if (UUIDCoreManager.TryGetOwnerWithWarning(fromContainerUUID, out IContainer fromContainer) == false)
            {
                return;
            }

            if (UUIDCoreManager.TryGetOwnerWithWarning(toContainerUUID, out IContainer toContainer) == false)
            {
                return;
            }

            if (connection.IsHost == false)
            {
                fromContainer.PopItemsTo(count, toContainer, out _);
            }
        }

        #endregion

        #region Pop All Items To

        [Client]
        public static void PopAllItemsTo(IContainer fromContainer, IContainer toContainer)
        {
            if (fromContainer == null || toContainer == null)
            {
                Debugger.LogWarning($"{nameof(fromContainer)} or {nameof(toContainer)} is Null");
                return;
            }

            fromContainer.TryPopAllItemsTo(toContainer);

            Instance.RequestPopAllItemsTo(fromContainer.UUID, toContainer.UUID);
        }

        [ServerRpc(RequireOwnership = false)]
        private void RequestPopAllItemsTo(Guid fromContainerUUID, Guid toContainerUUID,
            NetworkConnection connection = null)
        {
            if (UUIDCoreManager.TryGetOwnerWithWarning(fromContainerUUID, out IContainer fromContainer) == false)
            {
                return;
            }

            if (UUIDCoreManager.TryGetOwnerWithWarning(toContainerUUID, out IContainer toContainer) == false)
            {
                return;
            }

            if (connection.IsHost == false)
            {
                fromContainer.TryPopAllItemsTo(toContainer);
            }
        }

        #endregion

        [Client]
        public static void PopItemByPreferredCountTo(IContainer fromContainer, int slotIndex, int preferredCount,
            IContainer toContainer, int toSlotIndex)
        {
            if (fromContainer == null || toContainer == null)
            {
                Debugger.LogWarning($"{nameof(fromContainer)} or {nameof(toContainer)} is Null");
                return;
            }

            fromContainer.TryPopItemByPreferredCountTo(slotIndex, preferredCount, toContainer, toSlotIndex, out _);

            Instance.RequestPopItemByPreferredCountTo(fromContainer.UUID, slotIndex, preferredCount, toContainer.UUID,
                toSlotIndex);
        }

        [ServerRpc(RequireOwnership = false)]
        private void RequestPopItemByPreferredCountTo(Guid fromContainerUUID, int slotIndex, int preferredCount,
            Guid toContainerUUID, int toSlotIndex, NetworkConnection connection = null)
        {
            if (UUIDCoreManager.TryGetOwnerWithWarning(fromContainerUUID, out IContainer fromContainer) == false)
            {
                return;
            }

            if (UUIDCoreManager.TryGetOwnerWithWarning(toContainerUUID, out IContainer toContainer) == false)
            {
                return;
            }

            if (connection.IsHost)
            {
                return;
            }

            if (fromContainer.TryPopItemByPreferredCountTo(slotIndex, preferredCount, toContainer, toSlotIndex, out _))
            {
                return;
            }

            ReconcileOnTarget(connection, fromContainer, slotIndex);
            ReconcileOnTarget(connection, toContainer, toSlotIndex);
        }

        #region Stack Item

        [Client]
        public static void StackItem(IContainer container)
        {
            if (container == null)
            {
                Debugger.LogWarning($"{nameof(container)} is Null");
                return;
            }

            container.StackItems();

            Instance.StackItemRequest(container.UUID);
        }

        [ServerRpc(RequireOwnership = false)]
        private void StackItemRequest(Guid containerUUID, NetworkConnection connection = null)
        {
            if (UUIDCoreManager.TryGetOwnerWithWarning(containerUUID, out IContainer container) == false)
            {
                return;
            }

            if (connection.IsHost == false)
            {
                container.StackItems();
            }
        }

        #endregion
    }
}
#endif