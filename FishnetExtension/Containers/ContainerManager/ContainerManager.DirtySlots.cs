#if FISHNET
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using VMFramework.Core;
using VMFramework.Network;

namespace VMFramework.Containers
{
    public partial class ContainerManager
    {
        private static readonly IContainer.ItemAddedHandler onItemAddedFunc = OnItemAdded;
        private static readonly IContainer.ItemRemovedHandler onItemRemovedFunc = OnItemRemoved;
        private static readonly ItemDirtyHandler onItemDirtyFunc = OnItemDirty;

        private static readonly Dictionary<IContainer, HashSet<int>> allDirtySlots = new();
        
        private static readonly List<IContainer> dirtyContainersRemovalList = new();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void RemoveContainerDirtySlotsInfo(IContainer container)
        {
            allDirtySlots.Remove(container);
        }
        
        #region Container Changed

        protected virtual void Update()
        {
            if (IsServerStarted == false)
            {
                return;
            }

            if (allDirtySlots.Count <= 0)
            {
                return;
            }

            var isHost = IsHostStarted;
            var hostClientID = isHost ? ClientManager.Connection.ClientId : -1;

            foreach (var (container, dirtySlots) in allDirtySlots)
            {
                if (dirtySlots.Count == 0)
                {
                    continue;
                }

                if (UUIDCoreManager.Instance.TryGetInfo(container.UUIDOwner.UUID, out var info) == false)
                {
                    UnityEngine.Debug.LogWarning($"Container with UUID {container.UUIDOwner.UUID} not found.");
                    dirtyContainersRemovalList.Add(container);
                    continue;
                }

                if (info.Observers.Count == 0)
                {
                    dirtySlots.Clear();
                    continue;
                }

                if (info.Observers.Count == 1 && info.Observers.Contains(hostClientID))
                {
                    dirtySlots.Clear();
                    continue;
                }

                if (dirtySlots.Count == 1)
                {
                    var slotIndex = dirtySlots.First();
                    ReconcileItemOnObservers(container, info, slotIndex);
                }
                else
                {
                    var ratio = dirtySlots.Count / (float)container.Count;

                    if (ratio > 0.5f)
                    {
                        ReconcileAllItemsOnObservers(container, info);
                    }
                    else
                    {
                        ReconcileSomeItemsOnObservers(container, info, dirtySlots);
                    }
                }

                dirtySlots.Clear();
            }

            if (dirtyContainersRemovalList.Count > 0)
            {
                foreach (var container in dirtyContainersRemovalList)
                {
                    RemoveContainerDirtySlotsInfo(container);
                }
            }
        }
        
        private static void OnItemAdded(IContainer container, int index, IContainerItem item)
        {
            SetSlotDirty(container, index);
        }

        private static void OnItemRemoved(IContainer container, int index, IContainerItem item)
        {
            SetSlotDirty(container, index);
        }
        
        private static void OnItemDirty(IContainer container, int index, IContainerItem item, bool local)
        {
            if (local)
            {
                return;
            }
            
            SetSlotDirty(container, index);
        }

        #endregion

        #region Set Slot Dirty

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void SetSlotDirty(IContainer container, int slotIndex)
        {
            if (container == null)
            {
                return;
            }
            
            if (container.UUIDOwner.UUID == Guid.Empty)
            {
                UnityEngine.Debug.LogWarning($"{container}'s UUID is null or empty.");
                return;
            }
            
            if (allDirtySlots.TryGetValue(container, out var dirtySlots) == false)
            {
                dirtySlots = new HashSet<int>();
                allDirtySlots.Add(container, dirtySlots);
            }
            
            dirtySlots.Add(slotIndex);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void SetSlotDirty(IContainerItem item)
        {
            var container = item.SourceContainer;
            SetSlotDirty(container, item.SlotIndex);
        }

        #endregion
    }
}
#endif