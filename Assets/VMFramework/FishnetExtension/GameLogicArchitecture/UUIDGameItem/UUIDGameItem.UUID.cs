#if FISHNET
using System;
using System.Collections.Generic;
using FishNet.Connection;
using FishNet.Serializing;
using Sirenix.OdinInspector;

namespace VMFramework.Network
{
    public partial class UUIDGameItem : IUUIDOwner
    {
        [ShowInInspector]
        public Guid uuid { get; private set; }

        Guid IUUIDOwner.UUID
        {
            get => uuid;
            set => uuid = value;
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

        #region Network Serialization

        protected override void OnWrite(Writer writer)
        {
            base.OnWrite(writer);

            writer.WriteGuidAllocated(uuid);
        }

        protected override void OnRead(Reader reader)
        {
            base.OnRead(reader);

            uuid = reader.ReadGuid();
        }

        #endregion

        protected override void OnGetStringProperties(
            ICollection<(string propertyID, string propertyContent)> collection)
        {
            base.OnGetStringProperties(collection);

            collection.Add(("UUID", uuid.ToString()));
        }
    }
}
#endif