#if UNITY_EDITOR
using Sirenix.OdinInspector;
using UnityEngine;
using VMFramework.GameLogicArchitecture;
using VMFramework.OdinExtensions;

namespace VMFramework.Maps
{
    public partial class CascadedTilemap
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
            [GamePrefabID(typeof(ExtendedRuleTile))] [HideLabel] string id, int layer = 0)
        {
            FillTile(position, new(layer, id));
        }

        [Button(ButtonStyle.Box)]
        private void _FillCubeTiles(Vector3Int start, Vector3Int end,
            [GamePrefabID(typeof(ExtendedRuleTile))] [HideLabel] string id, int layer = 0)
        {
            FillCubeTiles(new(start, end), new(layer, id));
        }

        [Button(ButtonStyle.Box)]
        private void _ReplaceTile([HideLabel] Vector3Int position,
            [GamePrefabID(typeof(ExtendedRuleTile))] [HideLabel] string id, int layer = 0)
        {
            ReplaceTile(position, new(layer, id));
        }

        [Button(ButtonStyle.Box)]
        private void _ReplaceCubeTiles(Vector3Int start, Vector3Int end,
            [GamePrefabID(typeof(ExtendedRuleTile))] [HideLabel] string tile, int layer = 0)
        {
            ReplaceCubeTiles(new(start, end), new(layer, tile));
        }

        [Button(ButtonStyle.Box)]
        private void _DestructTile([HideLabel] Vector3Int position, int layer = 0)
        {
            this.DestructTile(position, new(layer));
        }

        [Button(ButtonStyle.Box)]
        private void _DestructCubeTiles(Vector3Int start, Vector3Int end, int layer = 0)
        {
            DestructCubeTiles(new(start, end), new(layer));
        }
    }
}
#endif