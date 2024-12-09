using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using Sirenix.OdinInspector;
using UnityEngine;
using VMFramework.Core;

namespace VMFramework.Maps
{
    [RequireComponent(typeof(ExtendedTilemapPrefabController))]
    public partial class CascadedTilemap
        : SerializedMonoBehaviour, IReadableMap<ExtendedRuleTileQueryInfo, ExtendedRuleTile>,
            ITileFillableMap<Vector3Int, ExtendedRuleTilePlaceInfo>, ITileReplaceableMap<Vector3Int, ExtendedRuleTilePlaceInfo>,
            ITileDestructibleMap<Vector3Int, ExtendedRuleTileDestructInfo, ExtendedRuleTile>, ITilesCubeFillableGridMap<ExtendedRuleTilePlaceInfo>,
            ITilesCubeReplaceableGridMap<ExtendedRuleTilePlaceInfo>, ITilesCubeDestructibleGridMap<ExtendedRuleTileDestructInfo>, IClearableMap
    {
        [SerializeField]
        private CascadedTilemapBaseOrderSettings initialBaseOrderSettings;

        [field: Required]
        [field: SerializeField]
        public Grid Grid { get; private set; }

        [ShowInInspector]
        private short[] baseOrders;

        private ExtendedTilemapPrefabController prefabController;

        [ShowInInspector]
        private readonly Dictionary<int, ExtendedTilemap> tilemaps = new();

        private void Awake()
        {
            prefabController = GetComponent<ExtendedTilemapPrefabController>();

            SetBaseOrderSettings(initialBaseOrderSettings);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SetBaseOrderSettings(CascadedTilemapBaseOrderSettings settings)
        {
            var zCount = settings.maxZ + 1;

            baseOrders = new short[zCount];

            if (settings.enableUniformlySpaced)
            {
                int index = 0;
                foreach (var baseOrderFloat in UniformlySpacedRangeFloat.ExcludeBoundaries(short.MinValue, short.MaxValue,
                             zCount))
                {
                    baseOrders[index] = (short)baseOrderFloat.Round();
                    index++;
                }
            }
            else
            {
                for (int index = 0; index < zCount; index++)
                {
                    baseOrders[index] = (short)(index + settings.baseOrderOffset);
                }
            }
        }

        #region Tilemap

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ExtendedTilemap GetOrCreateTilemap(int z)
        {
            if (tilemaps.TryGetValue(z, out var tilemap))
            {
                return tilemap;
            }
            
            return CreateTilemap(z);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private ExtendedTilemap CreateTilemap(int z)
        {
            var go = Instantiate(prefabController.Prefab.gameObject, transform);
            go.SetActive(true);
            
            var tilemap = go.GetComponent<ExtendedTilemap>();
            
            go.name = $"z={z}";

            tilemap.SetBaseOrder(baseOrders[z]);

            OnCreateTilemap(go, tilemap, z);

            tilemaps.Add(z, tilemap);

            return tilemap;
        }

        protected virtual void OnCreateTilemap(GameObject go, ExtendedTilemap tilemap, int z)
        {

        }

        #endregion

        #region Fill

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool FillTile(Vector3Int position, ExtendedRuleTilePlaceInfo info)
        {
            if (tilemaps.TryGetValue(position.z, out var extendedTilemap) == false)
            {
                extendedTilemap = CreateTilemap(position.z);
            }

            return extendedTilemap.FillTile(position.ReplaceZ(info.layer), info.extendedRuleTile);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void FillCubeTiles(CubeInteger cube, ExtendedRuleTilePlaceInfo info)
        {
            var layerCube = new CubeInteger(cube.min.ReplaceZ(info.layer), cube.max.ReplaceZ(info.layer));

            foreach (var z in cube.ZRange)
            {
                if (tilemaps.TryGetValue(z, out var extendedTilemap) == false)
                {
                    extendedTilemap = CreateTilemap(z);
                }
                
                extendedTilemap.FillCubeTiles(layerCube, info.extendedRuleTile);
            }
        }

        #endregion

        #region Replace

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool ReplaceTile(Vector3Int position, ExtendedRuleTilePlaceInfo info)
        {
            if (tilemaps.TryGetValue(position.z, out var extendedTilemap) == false)
            {
                extendedTilemap = CreateTilemap(position.z);
            }

            extendedTilemap.ReplaceTile(position.ReplaceZ(info.layer), info.extendedRuleTile);
            
            return true;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void ReplaceCubeTiles(CubeInteger cube, ExtendedRuleTilePlaceInfo info)
        {
            var layerCube = new CubeInteger(cube.min.ReplaceZ(info.layer), cube.max.ReplaceZ(info.layer));

            foreach (var z in cube.ZRange)
            {
                if (tilemaps.TryGetValue(z, out var extendedTilemap) == false)
                {
                    extendedTilemap = CreateTilemap(z);
                }

                extendedTilemap.ReplaceCubeTiles(layerCube, info.extendedRuleTile);
            }
        }

        #endregion

        #region Destruct

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool DestructTile(Vector3Int position, ExtendedRuleTileDestructInfo info, out ExtendedRuleTile tile)
        {
            if (tilemaps.TryGetValue(position.z, out var extendedTilemap) == false)
            {
                tile = null;
                return false;
            }

            return extendedTilemap.DestructTile(position.ReplaceZ(info.layer), out tile);
        }

        public void DestructCubeTiles(CubeInteger cube, ExtendedRuleTileDestructInfo info)
        {
            var layerCube = new CubeInteger(cube.min.ReplaceZ(info.layer), cube.max.ReplaceZ(info.layer));
            
            foreach (var z in cube.ZRange)
            {
                if (tilemaps.TryGetValue(z, out var extendedTilemap) == false)
                {
                    extendedTilemap = CreateTilemap(z);
                }
                
                extendedTilemap.DestructCubeTiles(layerCube);
            }
        }

        #endregion

        #region Clear

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void ClearMap()
        {
            foreach (var tilemap in tilemaps.Values)
            {
                tilemap.ClearMap();
            }
        }

        #endregion

        #region Update

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void RefreshMap()
        {
            foreach (var tilemap in tilemaps.Values)
            {
                tilemap.RefreshMap();
            }
        }

        #endregion

        ExtendedRuleTile IMapping<ExtendedRuleTileQueryInfo, ExtendedRuleTile>.MapTo(ExtendedRuleTileQueryInfo info) => GetTile(info);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public IEnumerable<ExtendedRuleTileQueryInfo> GetAllPoints()
        {
            foreach (var (z, tilemap) in tilemaps)
            {
                foreach (var point in tilemap.GetAllPoints())
                {
                    yield return new(point.ReplaceZ(z), point.z);
                }
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public IEnumerable<ExtendedRuleTile> GetAllTiles()
        {
            foreach (var tilemap in tilemaps.Values)
            {
                foreach (var tile in tilemap.GetAllTiles())
                {
                    yield return tile;
                }
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ExtendedRuleTile GetTile(ExtendedRuleTileQueryInfo info)
        {
            if (tilemaps.TryGetValue(info.position.z, out var extendedTilemap) == false)
            {
                return null;
            }

            return extendedTilemap.GetTile(info.position.ReplaceZ(info.layer));
        }
    }
}