#if FISHNET
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using FishNet.Connection;
using VMFramework.Core;

namespace VMFramework.Network
{
    public partial class UUIDCoreManager
    {
        #region Try Get Info

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool TryGetInfo(IUUIDOwner owner, out UUIDInfo info)
        {
            if (owner == null)
            {
                UnityEngine.Debug.LogWarning($"Try to get {nameof(UUIDInfo)} with null owner");
                info = default;
                return false;
            }
            
            return TryGetInfo(owner.UUID, out info);
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool TryGetInfo(Guid uuid, out UUIDInfo info)
        {
            if (uuid == Guid.Empty)
            {
                UnityEngine.Debug.LogWarning($"Try to get {nameof(UUIDInfo)} with empty uuid");
                info = default;
                return false;
            }

            return uuidInfos.TryGetValue(uuid, out info);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool TryGetInfoWithWarning(Guid uuid, out UUIDInfo info)
        {
            if (TryGetInfo(uuid, out info) == false)
            {
                UnityEngine.Debug.LogWarning($"The {nameof(uuid)}:{uuid} does not exist!");
                return false;
            }
            return true;
        }

        #endregion

        #region Try Get Component

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool TryGetComponent<TComponent>(Guid uuid, out TComponent component)
        {
            if (TryGetOwner(uuid, out var owner) == false)
            {
                component = default;
                return false;
            }

            var controller = (IController)owner;
            return controller.TryGetComponent(out component);
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool TryGetComponentWithWarning<TComponent>(Guid uuid, out TComponent component)
        {
            if (TryGetOwnerWithWarning(uuid, out var owner) == false)
            {
                component = default;
                return false;
            }

            var controller = (IController)owner;
            return controller.TryGetComponent(out component);
        }

        #endregion

        #region Try Get Owner

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool TryGetOwner(Guid uuid, out IUUIDOwner owner)
        {
            if (uuid == Guid.Empty)
            {
                UnityEngine.Debug.LogWarning($"Try to get {typeof(IUUIDOwner)} with empty uuid");
                owner = null;
                return false;
            }

            if (uuidInfos.TryGetValue(uuid, out var info))
            {
                owner = info.Owner;
                return true;
            }

            owner = null;
            return false;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool TryGetOwnerWithWarning(Guid uuid, out IUUIDOwner owner)
        {
            if (TryGetOwner(uuid, out owner) == false)
            {
                UnityEngine.Debug.LogWarning($"The {typeof(IUUIDOwner)} with uuid {uuid} does not exist");
                return false;
            }
            
            return true;
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool TryGetOwner<TUUIDOwner>(Guid uuid, out TUUIDOwner owner)
            where TUUIDOwner : IUUIDOwner
        {
            if (uuid == Guid.Empty)
            {
                UnityEngine.Debug.LogWarning($"Try to get {typeof(TUUIDOwner)} with empty uuid");
                owner = default;
                return false;
            }

            if (uuidInfos.TryGetValue(uuid, out var info))
            {
                if (info.Owner is TUUIDOwner tOwner)
                {
                    owner = tOwner;
                    return true;
                }
            }

            owner = default;
            return false;
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool TryGetOwnerWithWarning<TUUIDOwner>(Guid uuid, out TUUIDOwner owner)
            where TUUIDOwner : IUUIDOwner
        {
            if (TryGetOwner(uuid, out owner) == false)
            {
                UnityEngine.Debug.LogWarning($"The {typeof(TUUIDOwner)} with uuid {uuid} does not exist");
                return false;
            }
            
            return true;
        }

        #endregion

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool HasOwner<TUUIDOwner>(Guid uuid) where TUUIDOwner : IUUIDOwner
        {
            if (uuidInfos.TryGetValue(uuid, out var info))
            {
                return info.Owner is TUUIDOwner;
            }
            
            return false;
        }

        #region Has UUID

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool HasUUID(Guid uuid)
        {
            return uuidInfos.ContainsKey(uuid);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool HasUUIDWithWarning(Guid uuid)
        {
            if (uuid == Guid.Empty)
            {
                UnityEngine.Debug.LogWarning($"Try to check if {nameof(uuid)} exists with empty uuid");
                return false;
            }
            
            if (uuidInfos.ContainsKey(uuid) == false)
            {
                UnityEngine.Debug.LogWarning($"The {nameof(uuid)} : {uuid} does not exist");
                return false;
            }
            
            return true;
        }

        #endregion

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public IReadOnlyCollection<UUIDInfo> GetAllOwnerInfos()
        {
            return uuidInfos.Values;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public IEnumerable<IUUIDOwner> GetAllOwners()
        {
            return GetAllOwnerInfos().Select(info => info.Owner);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool TryGetAllObserversID<TCollection>(Guid uuid, out IReadOnlyCollection<int> observers)
        {
            if (TryGetInfo(uuid, out var info))
            {
                observers = info.Observers;
                return true;
            }
            
            observers = null;
            return false;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool TryGetAllObservers<TCollection>(Guid uuid, TCollection observers)
            where TCollection : ICollection<NetworkConnection>
        {
            if (TryGetInfo(uuid, out var info))
            {
                foreach (var id in info.Observers)
                {
                    observers.Add(Instance.ServerManager.Clients[id]);
                }
                return true;
            }
            return false;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public IEnumerable<NetworkConnection> GetAllObservers(Guid uuid)
        {
            if (TryGetInfo(uuid, out var info))
            {
                return info.Observers.Select(id => Instance.ServerManager.Clients[id]);
            }

            return null;
        }
    }
}
#endif