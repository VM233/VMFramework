#if FISHNET
using FishNet.Serializing;
using Sirenix.OdinInspector;
using UnityEngine;

namespace VMFramework.Network
{
    public class NetworkSerializer : MonoBehaviour, INetworkSerializer
    {
        [ListDrawerSettings(ShowFoldout = false)]
        [ShowInInspector]
        private INetworkSerializationReceiver[] serializationReceivers;
        
        protected virtual void Awake()
        {
            serializationReceivers = GetComponentsInChildren<INetworkSerializationReceiver>();
        }

        public virtual void WriteFishnet(Writer writer)
        {
            foreach (var receiver in serializationReceivers)
            {
                receiver.Write(writer);
            }
        }

        public virtual void ReadFishnet(Reader reader)
        {
            foreach (var receiver in serializationReceivers)
            {
                receiver.Read(reader);
            }
        }
    }
}
#endif