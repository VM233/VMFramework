#if UNITY_EDITOR
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;

namespace VMFramework.Maps
{
    public partial class DynamicGridMap
    {
        [ShowInInspector]
        private List<IGridChunk> ChunksDebug => chunks.Values.ToList();

        [Button]
        private void _CreateChunk(Vector3Int chunkPos)
        {
            TryCreateChunk(chunkPos, out _);
        }
        
        [Button]
        private void _DestructChunk(Vector3Int chunkPos)
        {
            DestructChunk(chunkPos);
        }
    }
}
#endif