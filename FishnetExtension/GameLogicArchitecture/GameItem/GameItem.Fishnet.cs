#if FISHNET
using FishNet.CodeGenerating;
using FishNet.Serializing;
using VMFramework.Network;

namespace VMFramework.GameLogicArchitecture
{
    public partial class GameItem : INetworkSerializer
    {
        public virtual IUUIDOwner UUIDOwner => null;

        public virtual INetworkSerializer NetworkSerializer => this;
        
        #region Network Serialization

        /// <summary>
        /// 在网络上如何传输，当在此实例被写进byte流时调用
        /// </summary>
        /// <param name="writer"></param>
        [NotSerializer]
        protected virtual void OnWrite(Writer writer)
        {
            
        }

        /// <summary>
        /// 在网络上如何传输，当在此实例被从byte流中读出时调用
        /// </summary>
        /// <param name="reader"></param>
        [NotSerializer]
        protected virtual void OnRead(Reader reader)
        {
            
        }

        void INetworkSerializer.WriteFishnet(Writer writer)
        {
            OnWrite(writer);
        }

        void INetworkSerializer.ReadFishnet(Reader reader)
        {
            OnRead(reader);
        }

        #endregion
    }
}
#endif