using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.Tilemaps;
using VMFramework.Core;
using VMFramework.Core.Pools;

namespace VMFramework.Maps
{
    public sealed partial class MultipleTileBaseMap : TileBaseMap
    {
        [field: Required]
        [field: SerializeField]
        public TilemapGroupController TilemapGroupController { get; private set; }

        protected override void SetTile(Vector3Int pos, TileBase tile)
        {
            var tilemap = TilemapGroupController.GetTilemap(pos.z);
            tilemap.SetTile(pos.ReplaceZ(0), tile);
        }

        protected override void SetCubeTiles(CubeInteger cube, TileBase tile)
        {
            int count = cube.Size.x * cube.Size.y;
            var rectangle = cube.XYRectangle;
            for (int z = cube.min.z; z <= cube.max.z; z++)
            {
                var tilemap = TilemapGroupController.GetTilemap(z);
                
                var tileBases = ArrayDefaultPool<TileBase>.Get(count);
                
                for (int i = 0; i < count; i++)
                {
                    tileBases[i] = tile;
                }

                tilemap.SetTilesBlock(rectangle.InsertAsZ(z), tileBases);
                
                tileBases.ReturnToDefaultPool();
            }
        }

        public override void SetBaseOrder(short order)
        {
            TilemapGroupController.SetBaseOrder(order);
        }

        public override void SetTileAnchor(Vector3 anchor)
        {
            TilemapGroupController.SetTileAnchor(anchor);
        }
    }
}