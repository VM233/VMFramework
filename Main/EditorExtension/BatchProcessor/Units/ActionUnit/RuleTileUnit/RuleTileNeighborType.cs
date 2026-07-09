#if UNITY_EDITOR && ENABLE_TILEMAP
using UnityEngine;

namespace VMFramework.Editor.BatchProcessor
{
    public enum RuleTileNeighborType
    {
        None = 0,
        This = RuleTile.TilingRuleOutput.Neighbor.This,
        NotThis = RuleTile.TilingRuleOutput.Neighbor.NotThis,
    }
}
#endif