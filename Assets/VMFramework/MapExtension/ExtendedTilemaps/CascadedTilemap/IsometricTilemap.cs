using UnityEngine;
using VMFramework.Core;

namespace VMFramework.Maps
{
    public sealed class IsometricTilemap : CascadedTilemap
    {
        protected override void OnCreateTilemap(GameObject go, ExtendedTilemap tilemap, int z)
        {
            base.OnCreateTilemap(go, tilemap, z);
            
            go.transform.localPosition = go.transform.localPosition.AddY(Grid.cellSize.y * z);
        }
    }
}