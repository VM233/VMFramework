using System.Collections.Generic;
using UnityEngine;
using VMFramework.Core;
using VMFramework.Core.Pools;

namespace VMFramework.Maps
{
    public interface IGridChunk
        : IReadOnlyGridChunk, ITileFillableMap<Vector3Int, IGridTile>,
            ITileFillableMap<Vector3Int, IGridTile, IGridTile>, ITileReplaceableMap<Vector3Int, IGridTile>,
            ITileDestructibleMap<Vector3Int, IGridTile>, IClearableMap, ICreatablePoolItem<IGridMap>
    {
        public Vector3Int MinTilePosition { get; }

        public CubeInteger TilePositions { get; }

        public IGridMap Map { get; }

        public Vector3Int Size { get; }

        public void Place(GridChunkPlaceInfo info);

        IEnumerable<Vector3Int> IReadableMap<Vector3Int>.GetAllPoints() => Positions;
    }
}