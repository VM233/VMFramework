#if FISHNET
using System.Runtime.CompilerServices;
using FishNet;
using FishNet.Managing.Client;
using FishNet.Managing.Server;
using VMFramework.Network;

namespace VMFramework.GameLogicArchitecture
{
    public partial class ControllerGameItem
    {
        private bool isInitialized;
        private IUUIDOwner uuidOwner;
        private INetworkSerializer networkSerializer;
        
        public IUUIDOwner UUIDOwner
        {
            get
            {
                if (isInitialized == false)
                {
                    Initialize();
                }
                
                return uuidOwner;
            }
        }

        public INetworkSerializer NetworkSerializer
        {
            get
            {
                if (isInitialized == false)
                {
                    Initialize();
                }
                
                return networkSerializer;
            }
        }

        private void Initialize()
        {
            uuidOwner = GetComponent<IUUIDOwner>();
            networkSerializer = GetComponent<INetworkSerializer>();
            isInitialized = true;
        }

        #region Properties

        public static bool IsServer
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => InstanceFinder.IsServerStarted;
        }

        public static bool IsClient
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => InstanceFinder.IsClientStarted;
        }

        public static bool IsHost
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => InstanceFinder.IsHostStarted;
        }

        public static bool IsServerOnly
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => InstanceFinder.IsServerOnlyStarted;
        }

        public static bool IsClientOnly
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => InstanceFinder.IsClientOnlyStarted;
        }

        public static ServerManager ServerManager => InstanceFinder.ServerManager;
        public static ClientManager ClientManager => InstanceFinder.ClientManager;

        #endregion
    }
}
#endif