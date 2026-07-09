#if FISHNET
namespace VMFramework.Network
{
    public interface IUUIDOwnerProvider
    {
        public IUUIDOwner UUIDOwner { get; }
    }
}
#endif