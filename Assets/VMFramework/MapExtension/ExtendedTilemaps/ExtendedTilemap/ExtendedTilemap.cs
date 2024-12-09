using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using VMFramework.Core;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Tilemaps;
using VMFramework.GameLogicArchitecture;

namespace VMFramework.Maps
{
    public abstract partial class ExtendedTilemap
        : SerializedMonoBehaviour, IReadableMap<Vector3Int, ExtendedRuleTile>, ITileFillableMap<Vector3Int, ExtendedRuleTile>,
            ITileReplaceableMap<Vector3Int, ExtendedRuleTile>, ITileDestructibleMap<Vector3Int, ExtendedRuleTile>,
            ITilesCubeFillableGridMap<ExtendedRuleTile>, ITilesCubeReplaceableGridMap<ExtendedRuleTile>,
            ITilesCubeDestructibleGridMap, IClearableMap
    {
        private ExtendedRuleTileGeneralSetting Setting => BuiltInModulesSetting.ExtendedRuleTileGeneralSetting;

        [SerializeField]
        private bool clearMapOnAwake = false;

        [ShowInInspector]
        private readonly Dictionary<Vector3Int, ExtendedRuleTile> allRuleTiles = new();

        protected virtual void Awake()
        {
            if (clearMapOnAwake)
            {
                ClearMap();
            }
        }
        
        protected abstract void SetTile(Vector3Int pos, TileBase tile);

        #region Query Tile

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public IEnumerable<Vector3Int> GetAllPoints()
        {
            return allRuleTiles.Keys;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public IEnumerable<ExtendedRuleTile> GetAllTiles()
        {
            return allRuleTiles.Values;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool TryGetTile(Vector3Int pos, out ExtendedRuleTile tile)
        {
            return allRuleTiles.TryGetValue(pos, out tile);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ExtendedRuleTile GetTile(Vector3Int pos)
        {
            return allRuleTiles.GetValueOrDefault(pos);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool ContainsTile(Vector3Int point)
        {
            return allRuleTiles.ContainsKey(point);
        }

        #endregion

        #region Update

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void ForcedUpdate(Vector3Int pos)
        {
            if (TryGetTile(pos, out var extendedRuleTile))
            {
                ForcedUpdate(pos, extendedRuleTile);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void ForcedUpdate(Vector3Int pos, [NotNull] ExtendedRuleTile extendedRuleTile)
        {
            SetEmpty(pos);

            var neighbor = this.GetEightDirectionsNeighborsOnPlane(pos, PlaneType.XY);

            var spriteConfig = extendedRuleTile.GetSpriteConfig(neighbor);

            if (spriteConfig == null)
            {
                return;
            }
            
            var tileBase = TileBaseManager.GetTileBase(spriteConfig.sprite.GetRandomItem());
            
            SetTile(pos, tileBase);
        }

        /// <summary>
        /// 更新瓦片贴图
        /// </summary>
        /// <param name="position"></param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void UpdateTile(Vector3Int position)
        {
            if (allRuleTiles.TryGetValue(position, out var extendedRuleTile))
            {
                if (extendedRuleTile.enableUpdate)
                {
                    ForcedUpdate(position, extendedRuleTile);
                }
            }
            else
            {
                SetEmpty(position);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void UpdateNeighboringTiles(Vector3Int position)
        {
            foreach (var neighborPos in position.GetEightDirectionsNeighborsOnPlane(PlaneType.XY))
            {
                UpdateTile(neighborPos);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void UpdateNeighboringTiles(CubeInteger cube)
        {
            var minXY = cube.min.XY();
            var maxXY = cube.max.XY();
            foreach (var layer in cube.ZRange)
            {
                var rectangle = new RectangleInteger(minXY, maxXY);

                foreach (var positionXY in rectangle.GetOuterRectangle().GetBoundary())
                {
                    ForcedUpdate(positionXY.InsertAsZ(layer));
                }
            }
        }

        /// <summary>
        /// 更新所有瓦片的贴图
        /// </summary>
        public void RefreshMap()
        {
            foreach (var pos in allRuleTiles.Keys)
            {
                UpdateTile(pos);
            }
        }

        #endregion

        #region Fill

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private bool FillTileWithoutUpdate(Vector3Int position, [NotNull] ExtendedRuleTile extendedRuleTile)
        {
            extendedRuleTile.AssertIsNotNull(nameof(extendedRuleTile));

            return allRuleTiles.TryAdd(position, extendedRuleTile);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool FillTile(Vector3Int position, [NotNull] ExtendedRuleTile extendedRuleTile)
        {
            if (FillTileWithoutUpdate(position, extendedRuleTile) == false)
            {
                return false;
            }

            ForcedUpdate(position, extendedRuleTile);
            UpdateNeighboringTiles(position);
            
            return true;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void FillCubeTiles(CubeInteger cube, [NotNull] ExtendedRuleTile extendedRuleTile)
        {
            foreach (var position in cube)
            {
                FillTileWithoutUpdate(position, extendedRuleTile);
            }

            foreach (var position in cube)
            {
                ForcedUpdate(position, extendedRuleTile);
            }

            UpdateNeighboringTiles(cube);
        }

        #endregion

        #region Replace

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void ReplaceTileWithoutUpdate(Vector3Int position, [NotNull] ExtendedRuleTile extendedRuleTile)
        {
            allRuleTiles[position] = extendedRuleTile;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool ReplaceTile(Vector3Int position, ExtendedRuleTile extendedRuleTile)
        {
            if (extendedRuleTile == null)
            {
                return DestructTile(position, out _);
            }

            ReplaceTileWithoutUpdate(position, extendedRuleTile);

            ForcedUpdate(position, extendedRuleTile);
            
            UpdateNeighboringTiles(position);
            
            return true;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void ReplaceCubeTiles(CubeInteger cube, ExtendedRuleTile extendedRuleTile)
        {
            if (extendedRuleTile == null)
            {
                DestructCubeTiles(cube);
                return;
            }

            foreach (var position in cube)
            {
                ReplaceTileWithoutUpdate(position, extendedRuleTile);
            }

            foreach (var position in cube)
            {
                ForcedUpdate(position, extendedRuleTile);
            }
            
            UpdateNeighboringTiles(cube);
        }

        #endregion

        #region Destruct

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool DestructTileWithoutUpdate(Vector3Int position, out ExtendedRuleTile tile)
        {
            if (allRuleTiles.Remove(position, out tile))
            {
                SetEmpty(position);
                return true;
            }
            
            return false;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool DestructTile(Vector3Int position, out ExtendedRuleTile tile)
        {
            if (DestructTileWithoutUpdate(position, out tile))
            {
                UpdateNeighboringTiles(position);
                
                return true;
            }

            return false;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void DestructCubeTiles(CubeInteger cube)
        {
            foreach (var position in cube)
            {
                DestructTileWithoutUpdate(position, out _);
            }
            
            UpdateNeighboringTiles(cube);
        }

        #endregion

        #region Clear

        protected abstract void SetEmpty(Vector3Int pos);

        /// <summary>
        /// 清空地图
        /// </summary>
        public virtual void ClearMap()
        {
            allRuleTiles.Clear();
        }

        #endregion

        public abstract void SetBaseOrder(short order);

        public abstract void SetTileAnchor(Vector3 anchor);
    }
}