#if FISHNET
using VMFramework.Network;

namespace VMFramework.GameLogicArchitecture
{
    public partial interface IGameItem : IUUIDOwnerProvider
    {
        public INetworkSerializer NetworkSerializer { get; }
    }
}
#endif