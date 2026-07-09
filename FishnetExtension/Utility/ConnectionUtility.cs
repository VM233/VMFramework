#if FISHNET
using System.Runtime.CompilerServices;
using FishNet;
using FishNet.Connection;
using VMFramework.Core;

namespace VMFramework.Network
{
    public static class ConnectionUtility
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryGetConnection(this int connectionId, out NetworkConnection connection)
        {
            return InstanceFinder.ServerManager.Clients.TryGetValue(connectionId, out connection);
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryGetConnectionWithWarning(this int connectionId, out NetworkConnection connection)
        {
            if (InstanceFinder.ServerManager.Clients.TryGetValue(connectionId, out connection) == false)
            {
                UnityEngine.Debug.LogWarning($"Connection with ID {connectionId} not found.");
                return false;
            }
            
            return true;
        }
    }
}
#endif