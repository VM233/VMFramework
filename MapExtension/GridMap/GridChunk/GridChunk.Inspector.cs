#if UNITY_EDITOR
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;
using VMFramework.Core.Linq;

namespace VMFramework.Maps
{
    public partial class GridChunk
    {
        [HideLabel]
        [ShowInInspector]
        private Vector3Int PositionDebug => Position;

        [ShowInInspector]
        private List<IGridTile> TilesDebug => tiles.Cast<IGridTile>().WhereNotNull().ToList();
    }
}
#endif