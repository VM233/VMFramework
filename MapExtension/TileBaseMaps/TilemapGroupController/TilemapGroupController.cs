using VMFramework.Core;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace VMFramework.Maps
{
    public sealed partial class TilemapGroupController : SerializedMonoBehaviour, IClearableMap
    {
        #region Base Order

        [field: SerializeField]
        public short BaseOrder { get; private set; } = 0;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SetBaseOrder(short order)
        {
            BaseOrder = order;
        }

        #endregion

        #region Tile Anchor

        [field: SerializeField]
        public Vector3 TileAnchor { get; private set; } = new(0.5f, 0.5f, 0);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SetTileAnchor(Vector3 anchor)
        {
            TileAnchor = anchor;
        }

        #endregion

        #region Color

        private Color color = Color.white;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SetColor(Color color)
        {
            this.color = color;
            
            foreach (var tilemap in allTilemaps.Values)
            {
                tilemap.color = color;
            }
        }

        #endregion
        
        [ShowInInspector]
        private readonly Dictionary<int, Tilemap> allTilemaps = new();

        [Required]
        public TilemapPrefabController tilemapPrefabController;

        [Required]
        public Grid grid;
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SetTilemapPrefabController(TilemapPrefabController controller)
        {
            if (tilemapPrefabController != null)
            {
                UnityEngine.Debug.LogWarning($"{nameof(TilemapPrefabController)} has already been set.");
            }
            
            tilemapPrefabController = controller;
        }

        #region Get Tilemap

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SetTilemap(int layer, Tilemap tilemap)
        {
            tilemap.SetActive(true);
            tilemap.enabled = true;
            
            tilemap.tileAnchor = TileAnchor;
            tilemap.color = color;

            var tilemapRenderer = tilemap.GetComponent<TilemapRenderer>();
            tilemapRenderer.enabled = true;
            tilemapRenderer.sortingOrder = BaseOrder + layer;

            allTilemaps.Add(layer, tilemap);
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Tilemap GetTilemap(int layer)
        {
            if (allTilemaps.TryGetValue(layer, out var tilemap))
            {
                return tilemap;
            }

            var go = Instantiate(tilemapPrefabController.Prefab.gameObject, grid.transform);
            tilemap = go.GetComponent<Tilemap>();
            SetTilemap(layer, tilemap);
            tilemap.name = $"Tilemap_{layer}";

            return tilemap;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool TryGetTilemap(int layer, out Tilemap tilemap)
        {
            return allTilemaps.TryGetValue(layer, out tilemap);
        }

        public IEnumerable<Tilemap> GetAllTilemaps()
        {
            return allTilemaps.Values;
        }

        #endregion

        public void ClearAllTilemaps()
        {
            allTilemaps.Clear();
        }

        public Sprite GetSprite(int layerIndex, Vector2Int pos)
        {
            return GetTilemap(layerIndex).GetSprite(pos.As3DXY());
        }

        public Vector3 GetRealPosition(Vector2Int pos)
        {
            return tilemapPrefabController.Prefab.CellToWorld(pos.As3DXY());
        }

        public Vector2Int GetTilePosition(Vector3 realPos)
        {
            return tilemapPrefabController.Prefab.WorldToCell(realPos).XY();
        }

        #region Cell Size

        public Vector2 GetCellSize()
        {
            return tilemapPrefabController.Prefab.cellSize.XY();
        }

        // public void SetCellSize(Vector2 cellSize)
        // {
        //     tilemapPrefabController.tilemapPrefab.layoutGrid.cellSize = cellSize.As3DXY();
        //
        //     foreach (var tilemap in allTilemaps.Values)
        //     {
        //         tilemap.layoutGrid.cellSize = cellSize.As3DXY();
        //     }
        // }

        #endregion

        public void ClearMap()
        {
            foreach (var tilemap in allTilemaps.Values)
            {
                tilemap.ClearAllTiles();
            }
        }
    }
}
