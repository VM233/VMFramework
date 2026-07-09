#if FISHNET
using System;
using FishNet.Connection;
using FishNet.Object;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Sirenix.OdinInspector;
using UnityEngine;
using VMFramework.Core.JSON;

namespace VMFramework.Network
{
    [DisallowMultipleComponent]
    public class UUIDNetworkObject : NetworkBehaviour, IUUIDOwner, IJSONSerializationReceiver
    {
        public bool enableSerialization = true;
        
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

        public override void OnStartServer()
        {
            base.OnStartServer();

            if (UUID == Guid.Empty)
            {
                this.TrySetUUIDAndRegister(Guid.NewGuid());
            }
            else
            {
                UUIDCoreManager.Instance.Register(this);
            }
        }

        public override void OnStopNetwork()
        {
            base.OnStopNetwork();
            
            UUIDCoreManager.Instance.Unregister(this);
            UUID = Guid.Empty;
        }

        public override void OnSpawnServer(NetworkConnection connection)
        {
            base.OnSpawnServer(connection);
            
            SendGuid(connection, UUID);
        }

        [TargetRpc(ExcludeServer = true)]
        private void SendGuid(NetworkConnection connection, Guid guid)
        {
            UUID = guid;
            UUIDCoreManager.Instance.Register(this);
        }

        public virtual void SerializeTo(JObject o, JsonSerializer serializer)
        {
            if (enableSerialization == false)
            {
                return;
            }
            
            if (UUID != Guid.Empty)
            {
                o.Add("uuid", UUID.ToString());
            }
        }

        public virtual void DeserializeFrom(JObject o, JsonSerializer serializer)
        {
            if (enableSerialization == false)
            {
                return;
            }
            
            if (o.TryGetValue("uuid", out JToken uuidToken))
            {
                UUID = Guid.Parse(uuidToken.ToString());
            }
        }
    }
}
#endif