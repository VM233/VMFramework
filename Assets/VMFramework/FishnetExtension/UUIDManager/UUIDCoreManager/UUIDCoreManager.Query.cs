#if FISHNET
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using FishNet.Connection;
using UnityEngine;
using VMFramework.Core;

namespace VMFramework.Network
{
    public partial class UUIDCoreManager
    {
        #region Try Get Info

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryGetInfo(IUUIDOwner owner, out UUIDInfo info)
        {
            if (owner == null)
            {
                Debugger.LogWarning($"Try to get {nameof(UUIDInfo)} with null owner");
                info = default;
                return false;
            }
            
            return TryGetInfo(owner.UUID, out info);
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryGetInfo(Guid uuid, out UUIDInfo info)
        {
            if (uuid == Guid.Empty)
            {
                Debugger.LogWarning($"Try to get {nameof(UUIDInfo)} with empty uuid");
                info = default;
                return false;
            }

            return uuidInfos.TryGetValue(uuid, out info);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryGetInfoWithWarning(Guid uuid, out UUIDInfo info)
        {
            if (TryGetInfo(uuid, out info) == false)
            {
                Debugger.LogWarning($"The {nameof(uuid)}:{uuid} does not exist!");
                return false;
            }
            return true;
        }

        #endregion

        #region Try Get Owner

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryGetOwner(Guid uuid, out IUUIDOwner owner)
        {
            if (uuid == Guid.Empty)
            {
                Debugger.LogWarning($"Try to get {typeof(IUUIDOwner)} with empty uuid");
                owner = null;
                return false;
            }

            if (uuidInfos.TryGetValue(uuid, out var info))
            {
                owner = info.owner;
                return true;
            }

            owner = null;
            return false;
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryGetOwner<TUUIDOwner>(Guid uuid, out TUUIDOwner owner)
            where TUUIDOwner : IUUIDOwner
        {
            if (uuid == Guid.Empty)
            {
                Debugger.LogWarning($"Try to get {typeof(TUUIDOwner)} with empty uuid");
                owner = default;
                return false;
            }

            if (uuidInfos.TryGetValue(uuid, out var info))
            {
                if (info.owner is TUUIDOwner tOwner)
                {
                    owner = tOwner;
                    return true;
                }
            }

            owner = default;
            return false;
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryGetOwnerWithWarning<TUUIDOwner>(Guid uuid, out TUUIDOwner owner)
            where TUUIDOwner : IUUIDOwner
        {
            if (TryGetOwner(uuid, out owner) == false)
            {
                Debugger.LogWarning($"The {typeof(TUUIDOwner)} with uuid {uuid} does not exist");
                return false;
            }
            
            return true;
        }

        #endregion

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool HasOwner<TUUIDOwner>(Guid uuid) where TUUIDOwner : IUUIDOwner
        {
            if (uuidInfos.TryGetValue(uuid, out var info))
            {
                return info.owner is TUUIDOwner;
            }
            
            return false;
        }

        #region Has UUID

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool HasUUID(Guid uuid)
        {
            return uuidInfos.ContainsKey(uuid);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool HasUUIDWithWarning(Guid uuid)
        {
            if (uuid == Guid.Empty)
            {
                Debugger.LogWarning($"Try to check if {nameof(uuid)} exists with empty uuid");
                return false;
            }
            
            if (uuidInfos.ContainsKey(uuid) == false)
            {
                Debugger.LogWarning($"The {nameof(uuid)} : {uuid} does not exist");
                return false;
            }
            
            return true;
        }

        #endregion

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IReadOnlyCollection<UUIDInfo> GetAllOwnerInfos()
        {
            return uuidInfos.Values;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IEnumerable<IUUIDOwner> GetAllOwners()
        {
            return GetAllOwnerInfos().Select(info => info.owner);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryGetAllObserversID<TCollection>(Guid uuid, out IReadOnlyCollection<int> observers)
        {
            if (TryGetInfo(uuid, out var info))
            {
                observers = info.observers;
                return true;
            }
            
            observers = null;
            return false;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryGetAllObservers<TCollection>(Guid uuid, TCollection observers)
            where TCollection : ICollection<NetworkConnection>
        {
            if (TryGetInfo(uuid, out var info))
            {
                foreach (var id in info.observers)
                {
                    observers.Add(Instance.ServerManager.Clients[id]);
                }
                return true;
            }
            return false;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IEnumerable<NetworkConnection> GetAllObservers(Guid uuid)
        {
            if (TryGetInfo(uuid, out var info))
            {
                return info.observers.Select(id => Instance.ServerManager.Clients[id]);
            }

            return null;
        }
    }
}
#endif