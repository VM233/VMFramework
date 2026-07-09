using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using UnityEngine;
using VMFramework.Core;

namespace VMFramework.Maps
{
    public partial class GridChunk : IGridChunk
    {
        public Vector3Int Position { get; private set; }
        
        public CubeInteger Positions { get; private set; }
        
        public Vector3Int MinTilePosition { get; private set; }
        
        public CubeInteger TilePositions { get; private set; }
        
        public Vector3Int Size { get; private set; }
        
        public IGridMap Map { get; private set; }

        protected IGridTile[,,] tiles;

        public void OnCreate(IGridMap map)
        {
            Map = map;
            Size = Map.ChunkSize;
            Size.CreateArray(ref tiles);
        }

        public virtual void Place(GridChunkPlaceInfo info)
        {
            Position = info.position;
            Positions = new(Size);
            
            MinTilePosition = Map.ChunkSize * Position;
            TilePositions = new(MinTilePosition, MinTilePosition + Map.ChunkSize - Vector3Int.one);
        }

        public IEnumerable<IGridTile> GetAllTiles()
        {
            foreach (var tile in tiles)
            {
                if (tile != null)
                {
                    yield return tile;
                }
            }
        }

        public IEnumerable<(Vector3Int, IGridTile)> GetAllTilesWithPoints()
        {
            return tiles.Enumerate();
        }

        public IGridTile GetTile(Vector3Int relativePosition)
        {
            relativePosition.AssertContainsBy(Positions, nameof(relativePosition), nameof(Positions));

            return tiles.Get(relativePosition);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private bool DestructTileWithoutChecking(Vector3Int relativePosition, out IGridTile tile)
        {
            if (tiles.Remove(relativePosition, out tile) == false)
            {
                return false;
            }
                
            return true;
        }

        public bool FillTile(Vector3Int relativePosition, [DisallowNull] IGridTile tile)
        {
            tile.AssertIsNotNull(nameof(tile));
            relativePosition.AssertContainsBy(Positions, nameof(relativePosition), nameof(Positions));

            if (tiles.TrySet(relativePosition, tile) == false)
            {
                return false;
            }
            
            return true;
        }

        public bool FillTile(Vector3Int relativePosition, IGridTile tile, out IGridTile existingTile)
        {
            tile.AssertIsNotNull(nameof(tile));
            relativePosition.AssertContainsBy(Positions, nameof(relativePosition), nameof(Positions));

            if (tiles.TrySet(relativePosition, tile, out existingTile) == false)
            {
                return false;
            }
            
            return true;
        }

        public bool ReplaceTile(Vector3Int relativePosition, IGridTile tile)
        {
            relativePosition.AssertContainsBy(Positions, nameof(relativePosition), nameof(Positions));
            
            if (tile == null)
            {
                return DestructTileWithoutChecking(relativePosition, out _);
            }

            bool destructed = DestructTileWithoutChecking(relativePosition, out _);
            
            tiles.Set(relativePosition, tile);
            return destructed;
        }

        public bool DestructTile(Vector3Int relativePosition, out IGridTile tile)
        {
            relativePosition.AssertContainsBy(Positions, nameof(relativePosition), nameof(Positions));
            
            return DestructTileWithoutChecking(relativePosition, out tile);
        }

        public void ClearMap()
        {
            foreach (var position in Positions)
            {
                DestructTileWithoutChecking(position, out _);
            }
        }
    }
}