using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Tilemaps;
using VMFramework.Core;

namespace VMFramework.Maps
{
    [RequireComponent(typeof(TilemapGroupController))]
    public sealed class MultipleExtendedTilemap : ExtendedTilemap
    {
        [ShowInInspector]
        [ReadOnly]
        public TilemapGroupController tilemapGroupController { get; private set; }

        protected override void Awake()
        {
            tilemapGroupController = GetComponent<TilemapGroupController>();
            
            base.Awake();
        }

        protected override void SetTile(Vector3Int pos, TileBase tile)
        {
            var tilemap = tilemapGroupController.GetTilemap(pos.z);
            
            tilemap.SetTile(pos.ReplaceZ(0), tile);
        }

        protected override void SetEmpty(Vector3Int pos)
        {
            var tilemap = tilemapGroupController.GetTilemap(pos.z);
            
            tilemap.SetTile(pos.ReplaceZ(0), TileBaseManager.EmptyTileBase);
        }

        // protected override void SetEmpty(Vector2Int pos)
        // {
        //     foreach (var tilemap in tilemapGroupController.GetAllTilemaps())
        //     {
        //         tilemap.SetTile(pos.As3DXY(), TileBaseManager.EmptyTileBase);
        //     }
        // }

        public override void ClearMap()
        {
            base.ClearMap();
            
            tilemapGroupController.ClearAllTilemaps();
        }
        
        #region RealPosition

        public Vector3 GetRealPosition(Vector2Int pos)
        {
            return tilemapGroupController.GetRealPosition(pos);
        }

        public Vector2Int GetTilePosition(Vector3 realPos)
        {
            return tilemapGroupController.GetTilePosition(realPos);
        }

        #endregion

        #region Cell Size

        public Vector2 GetCellSize()
        {
            return tilemapGroupController.GetCellSize();
        }

        #endregion
        
        public Sprite GetSprite(int layerIndex, Vector2Int pos)
        {
            return tilemapGroupController.GetSprite(layerIndex, pos);
        }

        public override void SetBaseOrder(short order)
        {
            tilemapGroupController.SetBaseOrder(order);
        }

        public override void SetTileAnchor(Vector3 anchor)
        {
            tilemapGroupController.SetTileAnchor(anchor);
        }
    }
}