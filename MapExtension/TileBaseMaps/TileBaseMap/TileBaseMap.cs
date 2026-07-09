using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Tilemaps;
using VMFramework.Core;

namespace VMFramework.Maps
{
    public abstract partial class TileBaseMap
        : SerializedMonoBehaviour, IReadableMap<Vector3Int, TileBase>, ITileFillableMap<Vector3Int, TileBase>,
            ITileReplaceableMap<Vector3Int, TileBase>, ITileDestructibleMap<Vector3Int, TileBase>,
            ITilesCubeFillableGridMap<TileBase>, ITilesCubeReplaceableGridMap<TileBase>, ITilesCubeDestructibleGridMap,
            IClearableMap
    {
        [field: SerializeField]
        public Color Color { get; private set; } = Color.white;
        
        [SerializeField]
        private bool clearMapOnAwake = false;

        [ShowInInspector]
        private readonly Dictionary<Vector3Int, TileBase> allTileBases = new();

        protected virtual void Awake()
        {
            if (clearMapOnAwake)
            {
                ClearMap();
            }
            
            SetColor(Color);
        }
        
        protected abstract void SetTile(Vector3Int pos, TileBase tile);
        
        protected abstract void SetCubeTiles(CubeInteger cube, TileBase tile);

        #region Query Tile

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public IEnumerable<Vector3Int> GetAllPoints()
        {
            return allTileBases.Keys;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public IEnumerable<TileBase> GetAllTiles()
        {
            return allTileBases.Values;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public IEnumerable<(Vector3Int, TileBase)> GetAllTilesWithPoints()
        {
            foreach (var (pos, tile) in allTileBases)
            {
                yield return (pos, tile);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool TryGetTile(Vector3Int pos, out TileBase tile)
        {
            return allTileBases.TryGetValue(pos, out tile);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public TileBase GetTile(Vector3Int pos)
        {
            return allTileBases.GetValueOrDefault(pos);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool ContainsTile(Vector3Int point)
        {
            return allTileBases.ContainsKey(point);
        }
        
        public abstract Sprite GetSprite(Vector3Int pos);

        #endregion

        #region Update

        /// <summary>
        /// 更新瓦片贴图
        /// </summary>
        /// <param name="position"></param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void UpdateTile(Vector3Int position)
        {
            if (allTileBases.TryGetValue(position, out var tileBase))
            {
                SetTile(position, tileBase);
            }
            else
            {
                SetEmpty(position);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void RefreshMap()
        {
            foreach (var pos in allTileBases.Keys)
            {
                UpdateTile(pos);
            }
        }

        #endregion

        #region Fill

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool FillTile(Vector3Int position, [NotNull] TileBase tileBase)
        {
            if (allTileBases.TryAdd(position, tileBase) == false)
            {
                return false;
            }
            
            SetTile(position, tileBase);

            return true;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void FillCubeTiles(CubeInteger cube, [NotNull] TileBase tileBase)
        {
            foreach (var position in cube)
            {
                if (allTileBases.TryAdd(position, tileBase))
                {
                    SetTile(position, tileBase);
                }
            }
        }

        #endregion

        #region Replace

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool ReplaceTile(Vector3Int position, TileBase tileBase)
        {
            if (tileBase == null)
            {
                return DestructTile(position, out _);
            }
            
            allTileBases[position] = tileBase;

            SetTile(position, tileBase);
            
            return true;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void ReplaceCubeTiles(CubeInteger cube, TileBase tileBase)
        {
            if (tileBase == null)
            {
                DestructCubeTiles(cube);
                return;
            }

            foreach (var position in cube)
            {
                allTileBases[position] = tileBase;
            }
            
            SetCubeTiles(cube, tileBase);
        }

        #endregion

        #region Destruct

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool DestructTile(Vector3Int position, out TileBase tileBase)
        {
            if (allTileBases.Remove(position, out tileBase))
            {
                SetEmpty(position);
                return true;
            }
            
            return false;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void DestructCubeTiles(CubeInteger cube)
        {
            foreach (var position in cube)
            {
                allTileBases.Remove(position, out _);
            }
            
            SetCubeEmpty(cube);
        }

        #endregion

        #region Clear

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void SetEmpty(Vector3Int pos)
        {
            SetTile(pos, TileBaseManager.EmptyTileBase);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void SetCubeEmpty(CubeInteger cube)
        {
            SetCubeTiles(cube, TileBaseManager.EmptyTileBase);
        }

        /// <summary>
        /// 清空地图
        /// </summary>
        public virtual void ClearMap()
        {
            allTileBases.Clear();
        }

        #endregion

        public abstract void SetBaseOrder(short order);

        public abstract void SetTileAnchor(Vector3 anchor);
        
        public abstract void SetColor(Color color);

        public abstract void SetColor(Vector3Int position, Color? color);
    }
}