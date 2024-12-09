using System.Diagnostics.CodeAnalysis;

namespace VMFramework.Maps
{
    public readonly struct ExtendedRuleTilePlaceInfo
    {
        public readonly int layer;
        public readonly ExtendedRuleTile extendedRuleTile;
        
        public ExtendedRuleTilePlaceInfo(int layer, [NotNull] ExtendedRuleTile extendedRuleTile)
        {
            this.layer = layer;
            this.extendedRuleTile = extendedRuleTile;
        }
    }
}