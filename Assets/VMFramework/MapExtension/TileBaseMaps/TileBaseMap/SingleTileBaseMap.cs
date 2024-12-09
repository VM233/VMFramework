using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Tilemaps;
using VMFramework.Core;
using VMFramework.Core.Pools;

namespace VMFramework.Maps
{
    public sealed partial class SingleTileBaseMap : TileBaseMap
    {
        [field: SerializeField]
        public Tilemap Tilemap { get; private set; }
        
        [field: Required]
        [field: SerializeField]
        public Grid Grid { get; private set; }
        
        [MinValue(0)]
        [SerializeField]
        private int maxLayerCount;

        protected override void SetTile(Vector3Int pos, TileBase tile)
        {
            Tilemap.SetTile(pos, tile);
        }

        protected override void SetCubeTiles(CubeInteger cube, TileBase tile)
        {
            var tileBases = ArrayDefaultPool<TileBase>.Get(cube.Count);
            for (int i = 0; i < cube.Count; i++)
            {
                tileBases[i] = tile;
            }
            
            Tilemap.SetTilesBlock(cube, tileBases);
            tileBases.ReturnToDefaultPool();
        }

        public override void ClearMap()
        {
            base.ClearMap();
            
            Tilemap.ClearAllTiles();
        }

        public override void SetBaseOrder(short order)
        {
            Tilemap.GetComponent<TilemapRenderer>().sortingOrder = order;
        }

        public override void SetTileAnchor(Vector3 anchor)
        {
            Tilemap.tileAnchor = anchor;
        }
    }
}