#if UNITY_EDITOR
using UnityEngine;
using UnityEngine.Tilemaps;
using VMFramework.Core;

namespace VMFramework.Maps
{
    public partial class SingleTileBaseMap
    {
        private void Reset()
        {
            if (TryGetComponent(out Tilemap tilemap))
            {
                this.tilemap = tilemap;
            }

            var newGrid = transform.QueryFirstComponentInParents<Grid>(true);

            if (newGrid != null)
            {
                grid = newGrid;
            }
        }
    }
}
#endif