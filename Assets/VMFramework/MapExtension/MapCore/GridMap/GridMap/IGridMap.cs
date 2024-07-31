﻿using UnityEngine;

namespace VMFramework.Maps
{
    public interface IGridMap
    {
        public Vector3Int ChunkSize { get; }

        public void CreateChunk(Vector3Int position);

        public void DestroyChunk(Vector3Int position);

        public bool HasValidChunk(Vector3Int position);
        
        public bool TryGetChunk(Vector3Int chunkPosition, out IGridChunk chunk);

        public IGridChunk GetChunk(Vector3Int chunkPosition);
    }
}