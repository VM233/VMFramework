#if FISHNET
using System;
using System.Runtime.CompilerServices;
using FishNet.Connection;
using FishNet.Object;
using VMFramework.Procedure;

namespace VMFramework.Network
{
    [ManagerCreationProvider(ManagerType.NetworkCore)]
    public sealed class CooldownNetworkManager : NetworkManagerBehaviour<CooldownNetworkManager>
    {
        protected override void OnBeforeInitStart()
        {
            base.OnBeforeInitStart();
            
            UUIDCoreManager.OnUUIDOwnerObserved += OnUUIDOwnerObserved;
        }

        private void OnUUIDOwnerObserved(IUUIDOwner owner, NetworkConnection connection)
        {
            if (owner is IUUIDCooldownProvider cooldownProvider)
            {
                ReconcileCooldown(connection, owner.UUID, cooldownProvider.cooldown);
            }
        }
        
        [Server]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ReconcileCooldownOnObservers(IUUIDCooldownProvider cooldownProvider)
        {
            foreach (var observer in cooldownProvider.GetObservers())
            {
                ReconcileCooldownOnTargetObserve(observer, cooldownProvider);
            }
        }

        [Server]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void ReconcileCooldownOnTargetObserve(int observer, IUUIDCooldownProvider cooldownProvider)
        {
            if (observer.TryGetConnectionWithWarning(out var connection) == false)
            {
                return;
            }

            if (connection.IsHost)
            {
                return;
            }

            Instance.ReconcileCooldown(connection, cooldownProvider.UUID, cooldownProvider.cooldown);
        }

        [TargetRpc(ExcludeServer = true)]
        private void ReconcileCooldown(NetworkConnection connection, Guid uuid, float cooldown)
        {
            if (UUIDCoreManager.TryGetOwnerWithWarning(uuid, out IUUIDCooldownProvider cooldownProvider))
            {
                cooldownProvider.cooldown = cooldown;
            }
        }
    }
}
#endif