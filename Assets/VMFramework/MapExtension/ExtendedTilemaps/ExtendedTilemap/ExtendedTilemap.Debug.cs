#if UNITY_EDITOR
using Sirenix.OdinInspector;
using UnityEngine;
using VMFramework.Core;
using VMFramework.GameLogicArchitecture;
using VMFramework.OdinExtensions;

namespace VMFramework.Maps
{
    public partial class ExtendedTilemap
    {
        [Button(ButtonStyle.Box)]
        private void _RefreshMap()
        {
            RefreshMap();
        }

        [Button]
        private void _ClearMap()
        {
            ClearMap();
        }

        [Button(ButtonStyle.Box)]
        private void _FillTile([HideLabel] Vector3Int position,
            [GamePrefabID(typeof(ExtendedRuleTile))] [HideLabel] string id)
        {
            this.FillTile(position, id);
        }

        [Button(ButtonStyle.Box)]
        private void _FillCubeTiles(Vector3Int start, Vector3Int end,
            [GamePrefabID(typeof(ExtendedRuleTile))] [HideLabel] string id)
        {
            this.FillCubeTiles(new CubeInteger(start, end), id);
        }

        [Button(ButtonStyle.Box)]
        private void _ReplaceTile([HideLabel] Vector3Int position,
            [GamePrefabID(typeof(ExtendedRuleTile))] [HideLabel] string id)
        {
            this.ReplaceTile(position, id);
        }

        [Button(ButtonStyle.Box)]
        private void _ReplaceCubeTiles(Vector3Int start, Vector3Int end,
            [GamePrefabID(typeof(ExtendedRuleTile))] [HideLabel] string id)
        {
            this.ReplaceCubeTiles(new CubeInteger(start, end), id);
        }

        [Button(ButtonStyle.Box)]
        private void _DestructTile([HideLabel] Vector3Int position)
        {
            this.DestructTile(position);
        }

        [Button(ButtonStyle.Box)]
        private void _DestructCubeTiles(Vector3Int start, Vector3Int end)
        {
            DestructCubeTiles(new CubeInteger(start, end));
        }

        [Button(ButtonStyle.Box)]
        private void FillRandomTiles([HideLabel] CubeInteger cube,
            [GamePrefabID(typeof(ExtendedRuleTile))] [HideLabel] string id, [PropertyRange(0, 1)] float density)
        {
            var extendedRuleTile = GamePrefabManager.GetGamePrefabStrictly<ExtendedRuleTile>(id);
            this.FillRandomCubeTiles(cube, extendedRuleTile, density, GlobalRandom.Default);
        }
    }
}
#endif