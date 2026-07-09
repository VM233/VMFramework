#if FISHNET
using FishNet.CodeGenerating;
using FishNet.Serializing;

namespace VMFramework.Network
{
    public interface INetworkSerializer
    {
        /// <summary>
        /// 在网络上如何传输，当在此实例被写进byte流时调用
        /// </summary>
        /// <param name="writer"></param>
        [NotSerializer]
        public void WriteFishnet(Writer writer);

        /// <summary>
        /// 在网络上如何传输，当在此实例被从byte流中读出时调用
        /// </summary>
        /// <param name="reader"></param>
        [NotSerializer]
        public void ReadFishnet(Reader reader);
    }
}
#endif