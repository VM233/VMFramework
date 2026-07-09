using UnityEngine;
using VMFramework.Core;

namespace VMFramework.Maps
{
    public readonly struct DynamicGridMapConfig
    {
        public readonly Vector3Int chunkSize;
        
        public readonly CubeInteger chunkBounds;
        
        public DynamicGridMapConfig(Vector3Int chunkSize, CubeInteger chunkBounds)
        {
            this.chunkSize = chunkSize;
            this.chunkBounds = chunkBounds;
        }
    }
}