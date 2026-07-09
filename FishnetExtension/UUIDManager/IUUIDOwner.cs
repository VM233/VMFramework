#if FISHNET
using System;
using FishNet.Connection;
using VMFramework.Core;

namespace VMFramework.Network
{
    public interface IUUIDOwner
    {
        public Guid UUID { get; protected set; }
        
        /// <summary>
        /// Triggered when the owner is observed. Only triggered on the server.
        /// </summary>
        public event Action<IUUIDOwner, NetworkConnection> OnObservedEvent;

        /// <summary>
        /// Triggered when the owner is unobserved. Only triggered on the server.
        /// </summary>
        public event Action<IUUIDOwner, NetworkConnection> OnUnobservedEvent;

        public void OnObserved(NetworkConnection connection);

        public void OnUnobserved(NetworkConnection connection);

        /// <summary>
        /// Changes the uuid of the owner.
        /// </summary>
        /// <param name="uuid"></param>
        /// <returns></returns>
        public bool SetUUID(Guid uuid)
        {
            UUID = this.SetStructValue(UUID, uuid, Guid.Empty, out var result, nameof(uuid));
            return result;
        }
    }
}
#endif