using System.Diagnostics.CodeAnalysis;
using VMFramework.Core;

namespace VMFramework.Maps
{
    public interface ITilesCubeFillableGridMap<in TTileInfo>
    {
        public void FillCubeTiles(CubeInteger cube, [NotNull] TTileInfo tileInfo);
    }
}