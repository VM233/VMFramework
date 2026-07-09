using VMFramework.Core;

namespace VMFramework.Maps
{
    public interface ITilesCubeReplaceableGridMap<in TTileInfo>
    {
        public void ReplaceCubeTiles(CubeInteger cube, TTileInfo tileInfo);
    }
}