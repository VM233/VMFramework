using System;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace VMFramework.Maps
{
    public sealed class SingleExtendedTilemap : ExtendedTilemap
    {
        [field: SerializeField]
        public TilemapPrefabController TilemapPrefabController { get; private set; }
        
        [field: Required]
        [field: SerializeField]
        public Grid Grid { get; private set; }
        
        [MinValue(0)]
        [SerializeField]
        private int maxLayerCount;
        
        private Tilemap tilemap;

        protected override void Awake()
        {
            base.Awake();
            
            tilemap = Instantiate(TilemapPrefabController.Prefab, Grid.transform);
            tilemap.transform.localPosition = Vector3.zero;
        }

        private void Start()
        {
            tilemap.name = name;
        }

        protected override void SetTile(Vector3Int pos, TileBase tile)
        {
            tilemap.SetTile(pos, tile);
        }

        protected override void SetEmpty(Vector3Int pos)
        {
            tilemap.SetTile(pos, TileBaseManager.EmptyTileBase);
        }

        // protected override void SetEmpty(Vector2Int pos)
        // {
        //     for (int i = 0; i < maxLayerCount; i++)
        //     {
        //         var pos3D = new Vector3Int(pos.x, pos.y, i);
        //         tilemap.SetTile(pos3D, TileBaseManager.EmptyTileBase);
        //     }
        // }

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
    }
}