#if FISHNET
using FishNet.Serializing;
using VMFramework.Network;

namespace VMFramework.Containers
{
    public partial class ContainerItem : INetworkSerializationReceiver
    {
        public virtual void Write(Writer writer)
        {
            writer.WriteInt32(count.Value);
        }

        public virtual void Read(Reader reader)
        {
            count.Value = reader.ReadInt32();
        }
    }
}
#endif