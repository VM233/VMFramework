#if UNITY_EDITOR
using UnityEngine;

namespace VMFramework.Maps
{
    public sealed partial class TilemapGroupController
    {
        private void Reset()
        {
            tilemapPrefabController = GetComponent<TilemapPrefabController>();
            grid = GetComponent<Grid>();
        }
    }
}
#endif