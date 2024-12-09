using System.Collections.Generic;
using UnityEngine;

namespace VMFramework.Maps
{
    public interface IGridMap : IClearableMap
    {
        public Vector3Int ChunkSize { get; }
        
        public bool TryCreateChunk(Vector3Int position, out IGridChunk chunk);

        public void DestructChunk(Vector3Int position);

        public bool HasValidChunk(Vector3Int position);
        
        public bool TryGetChunk(Vector3Int chunkPosition, out IGridChunk chunk);

        public IGridChunk GetChunk(Vector3Int chunkPosition);
        
        public IEnumerable<IGridChunk> GetAllChunks();
    }
}