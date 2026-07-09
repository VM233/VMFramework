#if FISHNET
using System;
using FishNet.Connection;
using FishNet.Object;
using VMFramework.Core;
using VMFramework.Network;

namespace VMFramework.Containers
{
    public partial class ContainerManager
    {
        #region Swap Item

        [Client]
        public void SwapItem(IContainer fromContainer, int fromSlotIndex, IContainer toContainer, int toSlotIndex,
            string intention)
        {
            if (fromContainer == null || toContainer == null)
            {
                UnityEngine.Debug.LogWarning($"{nameof(fromContainer)} or {nameof(toContainer)} is Null");
                return;
            }

            fromContainer.SwapItem(fromSlotIndex, toContainer, toSlotIndex, intention);

            if (IsServerStarted == false)
            {
                RequestSwapItem(fromContainer.UUIDOwner.UUID, fromSlotIndex, toContainer.UUIDOwner.UUID, toSlotIndex,
                    intention);
            }
        }

        [ServerRpc(RequireOwnership = false)]
        public void RequestSwapItem(Guid fromContainerUUID, int fromSlotIndex, Guid toContainerUUID, int toSlotIndex,
            string intention, NetworkConnection connection = null)
        {
            if (UUIDCoreManager.Instance.TryGetComponentWithWarning(fromContainerUUID, out IContainer fromContainer) ==
                false)
            {
                return;
            }

            if (UUIDCoreManager.Instance.TryGetComponentWithWarning(toContainerUUID, out IContainer toContainer) ==
                false)
            {
                return;
            }

            fromContainer.SwapItem(fromSlotIndex, toContainer, toSlotIndex, intention);
        }

        #endregion

        #region Add Or Swap Item

        [Client]
        public void AddOrSwapItem(IContainer fromContainer, int fromSlotIndex, IContainer toContainer, int toSlotIndex,
            string intention)
        {
            if (fromContainer == null || toContainer == null)
            {
                UnityEngine.Debug.LogWarning($"{nameof(fromContainer)} or {nameof(toContainer)} is Null");
                return;
            }

            fromContainer.AddOrSwapItemTo(fromSlotIndex, toContainer, toSlotIndex, intention);

            if (IsServerStarted == false)
            {
                RequestAddOrSwapItem(fromContainer.UUIDOwner.UUID, fromSlotIndex, toContainer.UUIDOwner.UUID,
                    toSlotIndex, intention);
            }
        }

        [ServerRpc(RequireOwnership = false)]
        public void RequestAddOrSwapItem(Guid fromContainerUUID, int fromSlotIndex, Guid toContainerUUID,
            int toSlotIndex, string intention, NetworkConnection connection = null)
        {
            if (UUIDCoreManager.Instance.TryGetComponentWithWarning(fromContainerUUID, out IContainer fromContainer) ==
                false)
            {
                return;
            }

            if (UUIDCoreManager.Instance.TryGetComponentWithWarning(toContainerUUID, out IContainer toContainer) ==
                false)
            {
                return;
            }

            fromContainer.AddOrSwapItemTo(fromSlotIndex, toContainer, toSlotIndex, intention);
        }

        #endregion

        #region Pop All Items To

        [Client]
        public void PopAllItemsTo(IContainer fromContainer, IContainer toContainer)
        {
            if (fromContainer == null || toContainer == null)
            {
                UnityEngine.Debug.LogWarning($"{nameof(fromContainer)} or {nameof(toContainer)} is Null");
                return;
            }

            fromContainer.PopAllItemsTo(toContainer, new());

            RequestPopAllItemsTo(fromContainer.UUIDOwner.UUID, toContainer.UUIDOwner.UUID);
        }

        [ServerRpc(RequireOwnership = false)]
        public void RequestPopAllItemsTo(Guid fromContainerUUID, Guid toContainerUUID,
            NetworkConnection connection = null)
        {
            if (UUIDCoreManager.Instance.TryGetComponentWithWarning(fromContainerUUID, out IContainer fromContainer) ==
                false)
            {
                return;
            }

            if (UUIDCoreManager.Instance.TryGetComponentWithWarning(toContainerUUID, out IContainer toContainer) ==
                false)
            {
                return;
            }

            // ReSharper disable once PossibleNullReferenceException
            if (connection.IsHost == false)
            {
                fromContainer.PopAllItemsTo(toContainer, new());
            }
        }

        #endregion

        #region Pop Item By Slot Index To

        [Client]
        public void PopItemBySlotIndexTo(IContainer fromContainer, int slotIndex, IContainer toContainer,
            ContainerAddArguments arguments)
        {
            if (fromContainer == null || toContainer == null)
            {
                UnityEngine.Debug.LogWarning($"{nameof(fromContainer)} or {nameof(toContainer)} is Null");
                return;
            }

            fromContainer.PopItemBySlotIndexTo(slotIndex, toContainer, arguments);

            RequestPopItemBySlotIndexTo(fromContainer.UUIDOwner.UUID, slotIndex, toContainer.UUIDOwner.UUID, arguments);
        }

        [ServerRpc(RequireOwnership = false)]
        public void RequestPopItemBySlotIndexTo(Guid fromContainerUUID, int slotIndex, Guid toContainerUUID,
            ContainerAddArguments arguments, NetworkConnection connection = null)
        {
            if (UUIDCoreManager.Instance.TryGetComponentWithWarning(fromContainerUUID, out IContainer fromContainer) ==
                false)
            {
                return;
            }

            if (UUIDCoreManager.Instance.TryGetComponentWithWarning(toContainerUUID, out IContainer toContainer) ==
                false)
            {
                return;
            }

            // ReSharper disable once PossibleNullReferenceException
            if (connection.IsHost == false)
            {
                fromContainer.PopItemBySlotIndexTo(slotIndex, toContainer, arguments);
            }
        }

        #endregion
    }
}
#endif