#if FISHNET
using FishNet.Serializing;

namespace VMFramework.Containers
{
    public partial class ContainerItem
    {
        protected override void OnWrite(Writer writer)
        {
            base.OnWrite(writer);

            writer.WriteInt32(count.value);
        }

        protected override void OnRead(Reader reader)
        {
            base.OnRead(reader);

            count.value = reader.ReadInt32();
        }
    }
}
#endif