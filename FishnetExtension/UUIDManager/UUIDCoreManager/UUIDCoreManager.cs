#if FISHNET

using System;
using System.Collections.Generic;
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
    public partial class UUIDCoreManager : NetworkManagerBehaviour<UUIDCoreManager>
    {
        [ShowInInspector]
        [DictionaryDrawerSettings(DisplayMode = DictionaryDisplayOptions.ExpandedFoldout)]
        private readonly Dictionary<Guid, UUIDInfo> uuidInfos = new();

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
            
            GameItemEvents.Instance.OnGameItemDestroyed += OnGameItemDestroyedOnServer;
        }

        public override void OnStopServer()
        {
            base.OnStopServer();
            
            GameItemEvents.Instance.OnGameItemDestroyed -= OnGameItemDestroyedOnServer;
            uuidInfos.Clear();
        }

        public override void OnStartClient()
        {
            base.OnStartClient();

            if (InstanceFinder.IsServerStarted == false)
            {
                GameItemEvents.Instance.OnGameItemDestroyed += OnGameItemDestroyedOnClient;
            }
        }

        public override void OnStopClient()
        {
            base.OnStopClient();
            
            GameItemEvents.Instance.OnGameItemDestroyed -= OnGameItemDestroyedOnClient;

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
    }
}

#endif