using System.Diagnostics.CodeAnalysis;

namespace VMFramework.Maps
{
    public interface ITileFillableMap<in TPoint, in TTileInfo>
    {
        public bool FillTile(TPoint position, [DisallowNull] TTileInfo info);
    }

    public interface ITileFillableMap<in TPoint, in TTileInfo, TTile>
    {
        public bool FillTile(TPoint position, [DisallowNull] TTileInfo info, [NotNull] out TTile existingTile);
    }
}