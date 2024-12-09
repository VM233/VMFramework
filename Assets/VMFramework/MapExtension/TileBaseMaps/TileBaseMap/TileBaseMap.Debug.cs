using System.Runtime.CompilerServices;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Tilemaps;
using VMFramework.Core;
using VMFramework.GameLogicArchitecture;
using VMFramework.OdinExtensions;

namespace VMFramework.Maps
{
    public partial class TileBaseMap
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private TileBase GetTileBase(string id)
        {
            var config = GamePrefabManager.GetGamePrefab<ITileBaseConfig>(id);
            return config.GetTileBase();
        }
        
        [Button]
        private void _FillTile([HideLabel] Vector3Int position, [GamePrefabID(typeof(ITileBaseConfig))] string id)
        {
            FillTile(position, GetTileBase(id));
        }

        [Button]
        private void _FillCubeTiles(CubeInteger cube, [GamePrefabID(typeof(ITileBaseConfig))] string id)
        {
            FillCubeTiles(cube, GetTileBase(id));
        }

        [Button]
        private void _ReplaceTile(Vector3Int position, [GamePrefabID(typeof(ITileBaseConfig))] string id)
        {
            ReplaceTile(position, GetTileBase(id));
        }
        
        [Button]
        private void _ReplaceCubeTiles(CubeInteger cube, [GamePrefabID(typeof(ITileBaseConfig))] string id)
        {
            ReplaceCubeTiles(cube, GetTileBase(id));
        }

        [Button]
        private void _DestructTile(Vector3Int position)
        {
            this.DestructTile(position);
        }
        
        [Button]
        private void _DestructCubeTiles(CubeInteger cube)
        {
            DestructCubeTiles(cube);
        }
    }
}