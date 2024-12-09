#if FISHNET
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;
using VMFramework.Core;
using VMFramework.Network;

namespace VMFramework.Containers
{
    public partial class ContainerManager
    {
        private static readonly ItemCountChangedHandler onContainerItemCountChangedFunc = OnContainerItemCountChanged;
        private static readonly Action<ContainerItemChangedParameter> onItemAddedFunc = OnItemAdded;
        private static readonly Action<ContainerItemChangedParameter> onItemRemovedFunc = OnItemRemoved;
        
        private static readonly Dictionary<IContainer, HashSet<int>> allDirtySlots = new();
        
        private static readonly List<IContainer> dirtyContainersRemovalList = new();

        private static void RemoveContainerDirtySlotsInfo(IContainer container)
        {
            allDirtySlots.Remove(container);
        }
        
        #region Container Changed

        private void Update()
        {
            if (IsServerStarted == false)
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

                if (UUIDCoreManager.TryGetInfo(container.UUID, out var info) == false)
                {
                    Debugger.LogWarning($"Container with UUID {container.UUID} not found.");
                    dirtyContainersRemovalList.Add(container);
                    continue;
                }

                if (info.observers.Count == 0)
                {
                    dirtySlots.Clear();
                    continue;
                }

                if (info.observers.Count == 1 && info.observers.Contains(hostClientID))
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

        private static void OnContainerItemCountChanged(IContainer container, 
            int slotIndex, IContainerItem item, int previous, int current)
        {
            if (current != previous)
            {
                SetSlotDirty(container, slotIndex);
            }
        }
        
        private static void OnItemAdded(ContainerItemChangedParameter parameter)
        {
            SetSlotDirty(parameter.container, parameter.slotIndex);
        }

        private static void OnItemRemoved(ContainerItemChangedParameter parameter)
        {
            SetSlotDirty(parameter.container, parameter.slotIndex);
        }

        #endregion

        #region Set Slot Dirty

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void SetSlotDirty(IContainer container, int slotIndex)
        {
            if (container.UUID == Guid.Empty)
            {
                Debugger.LogWarning($"{container}'s UUID is null or empty.");
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