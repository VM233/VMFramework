#if UNITY_EDITOR
using System;
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
                Tilemap = tilemap;
            }

            var grid = transform.QueryFirstComponentInParents<Grid>(true);

            if (grid != null)
            {
                Grid = grid;
            }
        }
    }
}
#endif