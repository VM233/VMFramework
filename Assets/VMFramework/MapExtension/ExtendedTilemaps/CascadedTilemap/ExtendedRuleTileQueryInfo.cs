using UnityEngine;

namespace VMFramework.Maps
{
    public readonly struct ExtendedRuleTileQueryInfo
    {
        public readonly Vector3Int position;
        public readonly int layer;
        
        public ExtendedRuleTileQueryInfo(Vector3Int position, int layer)
        {
            this.position = position;
            this.layer = layer;
        }
    }
}