#if FISHNET
using FishNet.Serializing;

namespace VMFramework.Network
{
    public interface INetworkSerializationReceiver
    {
        public void Write(Writer writer);
        
        public void Read(Reader reader);
    }
}
#endif