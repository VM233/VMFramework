﻿#if FISHNET
using FishNet.Serializing;
using UnityEngine.Scripting;
using VMFramework.GameLogicArchitecture;

namespace VMFramework.Containers
{
    [Preserve]
    public static class ContainerItemSerializer
    {
        public static void WriteIContainerItem(this Writer writer, ContainerItem containerItem)
        {
            GameItem.WriteGameItem(writer, containerItem);
        }
        
        public static ContainerItem ReadIContainerItem(this Reader reader)
        {
            return GameItem.ReadGameItem<ContainerItem>(reader);
        }
    }
}
#endif