#if FISHNET

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using FishNet;
using VMFramework.Core;
using FishNet.Connection;
using FishNet.Object;
using Sirenix.OdinInspector;
using VMFramework.GameLogicArchitecture;
using VMFramework.Procedure;

namespace VMFramework.Network
{
    [ManagerCreationProvider(ManagerType.NetworkCore)]
    public class UUIDCoreManager : NetworkManagerBehaviour<UUIDCoreManager>
    {
        [ShowInInspector]
        [DictionaryDrawerSettings(DisplayMode = DictionaryDisplayOptions.ExpandedFoldout)]
        private readonly Dictionary<Guid, UUIDInfo> uuidInfos = new();

        private GameItemEvents serverItems;
        private GameItemEvents clientItems;

        public event Action<IUUIDOwner> OnUUIDOwnerRegistered;
        public event Action<IUUIDOwner> OnUUIDOwnerUnregistered;

        public event Action<IUUIDOwner, NetworkConnection> OnUUIDOwnerObserved;
        public event Action<IUUIDOwner, NetworkConnection> OnUUIDOwnerUnobserved;

        protected override void Awake()
        {
            base.Awake();

            uuidInfos.Clear();
            OnUUIDOwnerRegistered = null;
            OnUUIDOwnerUnregistered = null;
            OnUUIDOwnerObserved = null;
            OnUUIDOwnerUnobserved = null;
        }

        public override void OnDespawnServer(NetworkConnection connection)
        {
            base.OnDespawnServer(connection);

            foreach (var info in uuidInfos.Values)
            {
                info.Observers.Remove(connection.ClientId);
            }
        }

        public override void OnStartServer()
        {
            base.OnStartServer();

            serverItems = GameItemEvents.Instance;
            serverItems.OnGameItemDestroyed += OnGameItemDestroyedOnServer;
        }

        public override void OnStopServer()
        {
            base.OnStopServer();

            var source = serverItems;
            serverItems = null;
            if (!ReferenceEquals(source, null))
                source.OnGameItemDestroyed -= OnGameItemDestroyedOnServer;
            uuidInfos.Clear();
        }

        public override void OnStartClient()
        {
            base.OnStartClient();

            if (InstanceFinder.IsServerStarted == false)
            {
                clientItems = GameItemEvents.Instance;
                clientItems.OnGameItemDestroyed += OnGameItemDestroyedOnClient;
            }
        }

        public override void OnStopClient()
        {
            base.OnStopClient();

            var source = clientItems;
            clientItems = null;
            if (!ReferenceEquals(source, null))
                source.OnGameItemDestroyed -= OnGameItemDestroyedOnClient;

            if (InstanceFinder.IsServerStarted == false)
            {
                uuidInfos.Clear();
            }
        }

        private void OnGameItemDestroyedOnServer(IGameItem gameItem)
        {
            var owner = gameItem.UUIDOwner;

            if (owner == null || owner.UUID == Guid.Empty)
            {
                return;
            }

            Unregister(owner);

            owner.SetUUID(Guid.Empty);
        }

        private void OnGameItemDestroyedOnClient(IGameItem gameItem)
        {
            var owner = gameItem.UUIDOwner;

            if (owner == null)
            {
                return;
            }

            Unregister(owner);

            owner.SetUUID(Guid.Empty);
        }

        #region Register & Unregister

        public bool Register(IUUIDOwner owner)
        {
            if (owner == null)
            {
                UnityEngine.Debug.LogWarning($"Failed to register a null {nameof(IUUIDOwner)}");
                return false;
            }

            var uuid = owner.UUID;

            if (uuid == Guid.Empty)
            {
                UnityEngine.Debug.LogWarning($"Failed to register a {owner.GetType()} with an empty uuid");
                return false;
            }

            if (uuidInfos.TryAdd(uuid, new UUIDInfo(owner, Instance.IsServerInitialized)) == false)
            {
                var oldOwner = uuidInfos[uuid].Owner;

                UnityEngine.Debug.LogWarning($"Registering a {owner} with an existing uuid: {uuid}." +
                                 $"The old owner : {oldOwner} will be overridden.");

                Unregister(uuid);

                uuidInfos[uuid] = new UUIDInfo(owner, Instance.IsServerInitialized);
            }

            OnUUIDOwnerRegistered?.Invoke(owner);

            return true;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Unregister(IUUIDOwner owner)
        {
            if (owner == null)
            {
                UnityEngine.Debug.LogWarning($"Failed to unregister a null {nameof(IUUIDOwner)}");
                return false;
            }

            if (Unregister(owner.UUID, out var existingOwner) == false)
            {
                return false;
            }

            if (owner != existingOwner)
            {
                UnityEngine.Debug.LogWarning($"Failed to unregister. " +
                                 $"The owner {owner} does not match the existing owner {existingOwner}." +
                                 $"They have the same uuid but are not the same object.");
                return false;
            }

            return true;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Unregister(Guid uuid)
        {
            return Unregister(uuid, out _);
        }

        public bool Unregister(Guid uuid, out IUUIDOwner owner)
        {
            if (uuid == Guid.Empty)
            {
                UnityEngine.Debug.LogWarning($"Failed to unregister a {nameof(IUUIDOwner)} with an empty uuid");
                owner = null;
                return false;
            }

            if (uuidInfos.Remove(uuid, out var info) == false)
            {
                UnityEngine.Debug.LogWarning($"Failed to unregister a {nameof(IUUIDOwner)} with uuid {uuid}. It does not exist.");
                owner = null;
                return false;
            }

            owner = info.Owner;

            OnUUIDOwnerUnregistered?.Invoke(info.Owner);

            return true;
        }

        #endregion

        #region Observe

        [ServerRpc(RequireOwnership = false)]
        private void ObserveRPC(Guid uuid, NetworkConnection connection = null)
        {
            if (TryGetInfoWithWarning(uuid, out var info))
            {
                ObserveInstantly(info, connection);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void ObserveInstantly(UUIDInfo info, NetworkConnection connection)
        {
            info.Owner.OnObserved(connection);

            info.Observers.Add(connection.ClientId);

            OnUUIDOwnerObserved?.Invoke(info.Owner, connection);
        }

        [Server]
        public void Observe(Guid uuid, NetworkConnection connection)
        {
            if (uuid == Guid.Empty)
            {
                UnityEngine.Debug.LogWarning($"{nameof(uuid)} is empty");
                return;
            }

            if (TryGetInfoWithWarning(uuid, out var info) == false)
            {
                return;
            }

            ObserveInstantly(info, connection);
        }

        public void Observe(Guid uuid)
        {
            if (uuid == Guid.Empty)
            {
                UnityEngine.Debug.LogWarning($"{nameof(uuid)} is empty");
                return;
            }

            if (Instance.IsClientStarted == false)
            {
                UnityEngine.Debug.LogWarning($"The client is not started yet, cannot observe {uuid}");
                return;
            }

            if (TryGetInfoWithWarning(uuid, out var info) == false)
            {
                return;
            }

            if (Instance.IsHostStarted)
            {
                ObserveInstantly(info, InstanceFinder.ClientManager.Connection);
            }
            else
            {
                Instance.ObserveRPC(uuid);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Observe(IUUIDOwner owner)
        {
            if (owner == null)
            {
                UnityEngine.Debug.LogWarning($"Failed to observe a null {nameof(IUUIDOwner)}");
                return;
            }

            Observe(owner.UUID);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Observe(IUUIDOwner owner, NetworkConnection connection)
        {
            if (owner == null)
            {
                UnityEngine.Debug.LogWarning($"Failed to observe a null {nameof(IUUIDOwner)}");
                return;
            }

            Observe(owner.UUID, connection);
        }

        #endregion

        #region Unobserve

        [ServerRpc(RequireOwnership = false)]
        private void _Unobserve(Guid uuid, NetworkConnection connection = null)
        {
            if (TryGetInfoWithWarning(uuid, out var info))
            {
                UnobserveInstantly(info, connection);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void UnobserveInstantly(UUIDInfo info, NetworkConnection connection)
        {
            info.Observers.Remove(connection.ClientId);

            info.Owner.OnUnobserved(connection);

            OnUUIDOwnerUnobserved?.Invoke(info.Owner, connection);
        }

        public void Unobserve(Guid uuid)
        {
            if (uuid == Guid.Empty)
            {
                UnityEngine.Debug.LogWarning($"{nameof(uuid)} is null or empty");
                return;
            }

            if (Instance.IsClientStarted == false)
            {
                UnityEngine.Debug.LogWarning($"The client is not started yet, cannot unobserve {uuid}");
                return;
            }

            if (TryGetInfo(uuid, out var info) == false)
            {
                return;
            }

            if (Instance.IsHostStarted)
            {
                UnobserveInstantly(info, InstanceFinder.ClientManager.Connection);
            }
            else
            {
                Instance._Unobserve(uuid);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Unobserve(IUUIDOwner owner)
        {
            if (owner == null)
            {
                UnityEngine.Debug.LogWarning($"Failed to unobserve a null {nameof(IUUIDOwner)}");
                return;
            }

            Unobserve(owner.UUID);
        }

        #endregion
        /// <summary>
        /// 检查一致性
        /// </summary>
        /// <returns></returns>
        public bool CheckConsistency<TUUIDOwner>(TUUIDOwner owner)
            where TUUIDOwner : IUUIDOwner
        {
            if (owner == null)
            {
                UnityEngine.Debug.LogWarning($"检查一致性失败，{nameof(owner)}为空");
                return false;
            }

            if (TryGetOwnerWithWarning(owner.UUID, out TUUIDOwner existedOwner) ==
                false)
            {
                UnityEngine.Debug.LogWarning($"不存在此{owner.UUID}对应的{typeof(TUUIDOwner)}");
                return false;
            }

            if (existedOwner.Equals(owner) == false)
            {
                UnityEngine.Debug.LogWarning($"此{owner}的UUID对应的是{existedOwner}，一致性检查失败");
                return false;
            }

            return true;
        }

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

#if UNITY_EDITOR
        [Button]
        private UUIDInfo GetInfo(Guid guid)
        {
            if (TryGetInfo(guid, out var info))
            {
                return info;
            }

            return default;
        }
#endif
    }
}

#endif
