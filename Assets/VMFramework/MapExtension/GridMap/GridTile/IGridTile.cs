using UnityEngine;
using VMFramework.Properties;

namespace VMFramework.Maps
{
    public interface IGridTile : IVector3IntPositionProvider
    {
        public IGridChunk Chunk { get; }
        
        public Vector3Int PositionInChunk { get; }

        public void InitGridTileInfo(GridTilePlaceInfo info);
    }
}