#if FISHNET
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using FishNet;
using FishNet.Connection;
using FishNet.Managing.Client;
using FishNet.Managing.Server;
using FishNet.Serializing;
using Sirenix.OdinInspector;
using VMFramework.GameLogicArchitecture;

namespace VMFramework.Network
{
    public partial class UUIDGameItem : GameItem, IUUIDOwner
    {
        public override IUUIDOwner UUIDOwner => this;

        [ShowInInspector]
        public Guid UUID { get; private set; }

        Guid IUUIDOwner.UUID
        {
            get => UUID;
            set => UUID = value;
        }
        
        public event Action<IUUIDOwner, NetworkConnection> OnObservedEvent;
        public event Action<IUUIDOwner, NetworkConnection> OnUnobservedEvent;

        void IUUIDOwner.OnObserved(NetworkConnection connection)
        {
            OnObservedEvent?.Invoke(this, connection);
        }

        void IUUIDOwner.OnUnobserved(NetworkConnection connection)
        {
            OnUnobservedEvent?.Invoke(this, connection);
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

        #region Network Serialization

        protected override void OnWrite(Writer writer)
        {
            base.OnWrite(writer);
            
            writer.WriteGuidAllocated(UUID);
        }

        protected override void OnRead(Reader reader)
        {
            base.OnRead(reader);
            
            UUID = reader.ReadGuid();
        }

        #endregion

        protected override void OnGetStringProperties(
            ICollection<(string propertyID, string propertyContent)> collection)
        {
            base.OnGetStringProperties(collection);

            collection.Add(("UUID", UUID.ToString()));
        }
    }
}
#endif