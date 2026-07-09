using UnityEngine;
using VMFramework.Properties;

namespace VMFramework.Maps
{
    public interface IReadOnlyGridChunk
        : IVector3IntPositionProvider, ICubeIntegerPositionsProvider, IReadableMap<Vector3Int, IGridTile>
    {

    }
}