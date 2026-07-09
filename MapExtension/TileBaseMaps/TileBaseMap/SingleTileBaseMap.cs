using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Tilemaps;
using VMFramework.Core;
using VMFramework.Core.Pools;

namespace VMFramework.Maps
{
    public partial class SingleTileBaseMap : TileBaseMap
    {
        [Required]
        public Tilemap tilemap;

        [Required]
        public Grid grid;
        
        [MinValue(0)]
        public int maxLayerCount;

        private TilemapRenderer tilemapRenderer;

        protected override void Awake()
        {
            base.Awake();

            tilemap.enabled = true;
            tilemapRenderer = tilemap.GetComponent<TilemapRenderer>();
        }

        protected virtual void OnEnable()
        {
            tilemapRenderer.enabled = true;
        }

        protected override void SetTile(Vector3Int pos, TileBase tile)
        {
            tilemap.SetTile(pos, tile);
        }

        protected override void SetCubeTiles(CubeInteger cube, TileBase tile)
        {
            var tileBases = ArrayDefaultPool<TileBase>.Get(cube.Count);
            for (int i = 0; i < cube.Count; i++)
            {
                tileBases[i] = tile;
            }
            
            tilemap.SetTilesBlock(cube, tileBases);
            tileBases.ReturnToDefaultPool();
        }

        public override void ClearMap()
        {
            base.ClearMap();
            
            tilemap.ClearAllTiles();
        }

        public override void SetBaseOrder(short order)
        {
            tilemap.GetComponent<TilemapRenderer>().sortingOrder = order;
        }

        public override void SetTileAnchor(Vector3 anchor)
        {
            tilemap.tileAnchor = anchor;
        }

        public override void SetColor(Color color)
        {
            tilemap.color = color;
        }

        public override void SetColor(Vector3Int position, Color? color)
        {
            if (color.HasValue)
            {
                tilemap.SetColor(position, color.Value);
            }
            else
            {
                tilemap.SetColor(position, Color.white);
            }
        }

        public override Sprite GetSprite(Vector3Int pos)
        {
            return tilemap.GetSprite(pos);
        }
    }
}