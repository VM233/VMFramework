using VMFramework.Core;

namespace VMFramework.Maps
{
    public interface ITilesCubeDestructibleGridMap
    {
        public void DestructCubeTiles(CubeInteger cube);
    }
    
    public interface ITilesCubeDestructibleGridMap<in TInfo> : ITilesCubeDestructibleGridMap
    {
        public void DestructCubeTiles(CubeInteger cube, TInfo info);

        void ITilesCubeDestructibleGridMap.DestructCubeTiles(CubeInteger cube)
        {
            DestructCubeTiles(cube, default);
        }
    }
}