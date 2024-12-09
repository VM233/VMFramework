using VMFramework.Core;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace VMFramework.Maps
{
    public sealed class TilemapGroupController : SerializedMonoBehaviour
    {
        #region Base Order

        [field: SerializeField]
        public short baseOrder { get; private set; } = 0;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SetBaseOrder(short order)
        {
            baseOrder = order;
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
        
        [ShowInInspector]
        private readonly Dictionary<int, Tilemap> allTilemaps = new();
        
        [SerializeField]
        private List<InitialTilemapConfig> initialTilemapConfigs = new();
        
        [field: SerializeField]
        public TilemapPrefabController TilemapPrefabController { get; private set; }
        
        [field: Required]
        [field: SerializeField]
        public Grid Grid { get; private set; }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SetTilemapPrefabController(TilemapPrefabController controller)
        {
            if (TilemapPrefabController != null)
            {
                Debugger.LogWarning($"{nameof(Maps.TilemapPrefabController)} has already been set.");
            }
            
            TilemapPrefabController = controller;
        }

        #region Get Tilemap

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SetTilemap(int layer, Tilemap tilemap)
        {
            tilemap.SetActive(true);
            
            tilemap.tileAnchor = TileAnchor;

            var tilemapRenderer = tilemap.GetComponent<TilemapRenderer>();
            
            tilemapRenderer.sortingOrder = baseOrder + layer;

            allTilemaps.Add(layer, tilemap);
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Tilemap GetTilemap(int layer)
        {
            if (allTilemaps.TryGetValue(layer, out var tilemap))
            {
                return tilemap;
            }

            var go = Instantiate(TilemapPrefabController.Prefab.gameObject, Grid.transform);

            tilemap = go.GetComponent<Tilemap>();
            
            SetTilemap(layer, tilemap);
            
            tilemap.name = $"Tilemap_{layer}";

            return tilemap;
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
            return TilemapPrefabController.Prefab.CellToWorld(pos.As3DXY());
        }

        public Vector2Int GetTilePosition(Vector3 realPos)
        {
            return TilemapPrefabController.Prefab.WorldToCell(realPos).XY();
        }

        #region Cell Size

        public Vector2 GetCellSize()
        {
            return TilemapPrefabController.Prefab.cellSize.XY();
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
    }
}
