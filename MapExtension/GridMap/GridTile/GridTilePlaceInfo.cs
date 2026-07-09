using UnityEngine;

namespace VMFramework.Maps
{
    public readonly struct GridTilePlaceInfo
    {
        public readonly IGridChunk chunk;
        public readonly Vector3Int position;
        public readonly Vector3Int positionInChunk;

        public GridTilePlaceInfo(IGridChunk chunk, Vector3Int position, Vector3Int positionInChunk)
        {
            this.chunk = chunk;
            this.position = position;
            this.positionInChunk = positionInChunk;
        }
    }
}