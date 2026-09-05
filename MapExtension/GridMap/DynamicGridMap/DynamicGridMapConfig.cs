using UnityEngine;
using VMFramework.Core;

namespace VMFramework.Maps
{
    [System.Serializable]
    public struct DynamicGridMapConfig
    {
        public Vector3Int chunkSize;

        public CubeInteger chunkBounds;

        public DynamicGridMapConfig(Vector3Int chunkSize, CubeInteger chunkBounds)
        {
            this.chunkSize = chunkSize;
            this.chunkBounds = chunkBounds;
        }
    }
}
