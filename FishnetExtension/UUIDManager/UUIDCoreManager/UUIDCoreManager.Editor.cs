#if UNITY_EDITOR && FISHNET
using System;
using Sirenix.OdinInspector;

namespace VMFramework.Network
{
    public partial class UUIDCoreManager
    {
        [Button]
        private UUIDInfo GetInfo(Guid guid)
        {
            if (TryGetInfo(guid, out var info))
            {
                return info;
            }
            
            return default;
        }
    }
}
#endif