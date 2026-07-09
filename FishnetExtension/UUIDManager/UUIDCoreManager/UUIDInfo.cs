#if FISHNET
using System.Collections.Generic;
using Sirenix.OdinInspector;

namespace VMFramework.Network
{
    public struct UUIDInfo
    {
        [ShowInInspector]
        public IUUIDOwner Owner { get; }

        [ShowInInspector]
        public HashSet<int> Observers { get; }

        public UUIDInfo(IUUIDOwner owner, bool asServer)
        {
            this.Owner = owner;
            if (asServer)
            {
                Observers = new();
            }
            else
            {
                Observers = null;
            }
        }
    }
}
#endif