#if FISHNET
using System;
using System.Runtime.CompilerServices;
using FishNet.Connection;
using FishNet.Object;
using VMFramework.Core;
using VMFramework.Procedure;
using VMFramework.Properties;

namespace VMFramework.Network
{
    [ManagerCreationProvider(ManagerType.NetworkCore)]
    public sealed class CooldownNetworkManager : NetworkManagerBehaviour<CooldownNetworkManager>
    {
        protected override void OnBeforeInitStart()
        {
            base.OnBeforeInitStart();

            UUIDCoreManager.Instance.OnUUIDOwnerObserved += OnUUIDOwnerObserved;
        }

        private void OnUUIDOwnerObserved(IUUIDOwner owner, NetworkConnection connection)
        {
            if (owner is not IController controller ||
                controller.TryGetComponent(out ICooldownProvider cooldownProvider) == false)
            {
                return;
            }

            ReconcileCooldown(connection, owner.UUID, cooldownProvider.Cooldown);
        }

        [Server]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ReconcileCooldownOnObservers(IUUIDOwner owner, ICooldownProvider cooldownProvider)
        {
            foreach (var observer in owner.GetObservers())
            {
                ReconcileCooldownOnTargetObserve(observer, owner, cooldownProvider);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void ReconcileCooldownOnTargetObserve(int observer, IUUIDOwner owner,
            ICooldownProvider cooldownProvider)
        {
            if (observer.TryGetConnectionWithWarning(out var connection) == false)
            {
                return;
            }

            if (connection.IsHost)
            {
                return;
            }

            Instance.ReconcileCooldown(connection, owner.UUID, cooldownProvider.Cooldown);
        }

        [TargetRpc(ExcludeServer = true)]
        private void ReconcileCooldown(NetworkConnection connection, Guid uuid, float cooldown)
        {
            if (UUIDCoreManager.Instance.TryGetComponentWithWarning(uuid, out ICooldownProvider cooldownProvider))
            {
                cooldownProvider.Cooldown = cooldown;
            }
        }
    }
}
#endif