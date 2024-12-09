using System.Collections.Generic;
using UnityEngine;

namespace VMFramework.Properties
{
    public interface IVector3IntPositionsProvider
    {
        public void GetPositions(ICollection<Vector3Int> positions);
    }
}