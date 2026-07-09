using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Tilemaps;
using VMFramework.Core;
using VMFramework.Core.Pools;

namespace VMFramework.Maps
{
    public partial class MultipleTileBaseMap : TileBaseMap
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
                if (TilemapGroupController.TryGetTilemap(z, out var tilemap) == false)
                {
                    continue;
                }

                var tileBases = ArrayDefaultPool<TileBase>.Get(count);

                for (int i = 0; i < count; i++)
                {
                    tileBases[i] = tile;
                }

                tilemap.SetTilesBlock(rectangle.InsertAsZ(0), tileBases);

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

        public override void SetColor(Color color)
        {
            TilemapGroupController.SetColor(color);
        }

        public override void SetColor(Vector3Int position, Color? color)
        {
            if (TilemapGroupController.TryGetTilemap(position.z, out var tilemap) == false)
            {
                return;
            }

            position.z = 0;

            if (color.HasValue)
            {
                tilemap.SetColor(position, color.Value);
            }
            else
            {
                tilemap.SetColor(position, Color.white);
            }
        }

        public override void ClearMap()
        {
            base.ClearMap();

            TilemapGroupController.ClearMap();
        }

        public override Sprite GetSprite(Vector3Int pos)
        {
            if (TilemapGroupController.TryGetTilemap(pos.z, out var tilemap) == false)
            {
                return null;
            }

            return tilemap.GetSprite(pos.ReplaceZ(0));
        }
    }
}